import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GenericModalComponent } from '../modals/modal.component';
import { PacienteModalComponent } from '../modals/paciente/paciente.modal.component';
import { PacienteService } from '../../services/paciente/pacientes.service';
import { Paciente } from '../../models/paciente/paciente.model';
import { PagedResult } from '../../models/generic/paged-result.model';
import { buildFilterString } from '../../shared/buildFilterString';

interface ActiveFilter {
  field: string;
  display: string;
  value: any;
  displayValue: string;
}

@Component({
  selector: 'app-pacientes',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericModalComponent, PacienteModalComponent],
  templateUrl: './pacientes.component.html',
  styleUrls: ['./pacientes.component.css']
})
export class Pacientes implements OnInit {
  isLoading = true;
  errorMessage: string | null = null;
  pacienteSelecionado: Paciente | null = null;
  modalAberto = false;
  pacientes: Paciente[] = [];
  pagedResult: PagedResult<Paciente> | null = null;
  availableFilters = [
    { value: 'NomeCompleto', display: 'Nome do Paciente', type: 'text' },
    { value: 'CPF', display: 'CPF', type: 'text' },
    { value: 'Email', display: 'E-mail', type: 'text' },
    { value: 'Ativo', display: 'Status', type: 'select', options: [{value: 'true', display: 'Ativo'}, {value: 'false', display: 'Inativo'}] }
  ];
  activeFilters: ActiveFilter[] = [];
  currentFilterField: string = 'NomeCompleto';
  currentFilterValue: any = '';
  public currentPage = 1;
  public pageSize = 9;
  public ordemFiltro = 'Id_desc';

  constructor(private pacienteService: PacienteService) { }

  ngOnInit(): void {
    this.carregarPacientes();
  }

  carregarPacientes(): void {
    this.isLoading = true;
    this.errorMessage = null;
    const dataFilters = this.activeFilters.reduce((acc, filter) => {
      acc[filter.field] = filter.value;
      return acc;
    }, {} as { [key: string]: any });
    const filtroFormatado = buildFilterString(dataFilters);
    const [orderBy, orderDir] = this.ordemFiltro.split('_');
    const orderByDesc = orderDir === 'desc';
    this.pacienteService.listarPacientes(this.currentPage, this.pageSize, filtroFormatado, orderBy, orderByDesc).subscribe({
      next: (resultado) => {
        this.pacientes = resultado.items;
        this.pagedResult = resultado;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Falha ao carregar pacientes. Tente novamente mais tarde.';
        this.isLoading = false;
      }
    });
  }

  addFilter(): void {
    if (!this.currentFilterField || this.currentFilterValue === '' || this.currentFilterValue === null) {
      return;
    }
    const selectedFilterConfig = this.getFilterConfig();
    if (!selectedFilterConfig) return;
    const isDuplicate = this.activeFilters.some(f => f.field === this.currentFilterField);
    if (isDuplicate) {
      return;
    }
    let displayValue = this.currentFilterValue;
    if (selectedFilterConfig.type === 'select') {
      displayValue = selectedFilterConfig.options?.find(opt => opt.value === this.currentFilterValue)?.display || this.currentFilterValue;
    }
    this.activeFilters.push({
      field: this.currentFilterField,
      display: selectedFilterConfig.display,
      value: this.currentFilterValue,
      displayValue: displayValue
    });
    this.currentFilterValue = '';
    this.currentPage = 1;
    this.carregarPacientes();
  }

  removeFilter(index: number): void {
    this.activeFilters.splice(index, 1);
    this.currentPage = 1;
    this.carregarPacientes();
  }
  
  onOrderChange(): void {
    this.currentPage = 1;
    this.carregarPacientes();
  }

  getFilterConfig() {
    return this.availableFilters.find(f => f.value === this.currentFilterField);
  }

  editarPaciente(paciente: Paciente): void {
    this.pacienteSelecionado = { ...paciente };
    this.abrirModalPaciente();
  }

  excluirPaciente(paciente: Paciente): void {
    if (confirm(`Tem certeza que deseja excluir o paciente "${paciente.nomeCompleto}"?`)) {
      this.isLoading = true;
      this.pacienteService.excluir(paciente.id).subscribe({
        next: () => {
          this.carregarPacientes();
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Falha ao excluir o paciente.';
        }
      });
    }
  }

  proximaPagina(): void {
    if (this.pagedResult && this.currentPage < this.pagedResult.totalPages) {
      this.currentPage++;
      this.carregarPacientes();
    }
  }

  paginaAnterior(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.carregarPacientes();
    }
  }

  irParaPagina(page: number): void {
    this.currentPage = page;
    this.carregarPacientes();
  }

  abrirModalPaciente() {
    this.modalAberto = true;
  }

  fecharModalPaciente() {
    this.modalAberto = false;
    this.pacienteSelecionado = null;
  }

  salvarPaciente(paciente: any) {
    this.fecharModalPaciente();
    this.carregarPacientes();
  }

  calcularIdade(dataNascimento: string | Date): number {
    if (!dataNascimento) return 0;
    const hoje = new Date();
    const nascimento = new Date(dataNascimento);
    let idade = hoje.getFullYear() - nascimento.getFullYear();
    const m = hoje.getMonth() - nascimento.getMonth();
    if (m < 0 || (m === 0 && hoje.getDate() < nascimento.getDate())) {
      idade--;
    }
    return idade;
  }
}