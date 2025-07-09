import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Necessário para o ngModel nos filtros
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

// Componentes e Modelos
import { GenericModalComponent } from '../modals/modal.component';
import { PacienteModalComponent } from '../modals/paciente/paciente.modal.component';
import { PacienteService } from '../../services/paciente/pacientes.service';
import { Paciente } from '../../models/paciente/paciente.model';
import { PagedResult } from '../../models/generic/paged-result.model';

@Component({
  selector: 'app-pacientes',
  standalone: true,
  imports: [CommonModule, FormsModule, GenericModalComponent, PacienteModalComponent],
  templateUrl: './pacientes.component.html',
  styleUrls: ['./pacientes.component.css']
})
export class Pacientes implements OnInit {

  // --- Estado da Interface do Usuário (UI) ---
  modalAberto = false;
  isLoading = true;
  errorMessage: string | null = null;
  pacienteSelecionado: Paciente | null = null; // Para edição

  // --- Estado dos Dados ---
  pacientes: Paciente[] = [];
  pagedResult: PagedResult<Paciente> | null = null;

  // --- Estado dos Filtros e Paginação ---
  public currentPage = 1;
  public pageSize = 9; // Ideal para um grid 3x3
  public termoBusca = '';
  public statusFiltro = ''; // Valor padrão para "Todos"
  public ordemFiltro = 'Id_desc'; // Valor padrão para "Mais Recentes"

  // Subject para aplicar debounce na busca e evitar chamadas excessivas à API
  private buscaSubject = new Subject<string>();

  constructor(private pacienteService: PacienteService) { }

  ngOnInit(): void {
    // Carrega os dados iniciais ao iniciar o componente
    this.carregarPacientes();

    // Configura o "ouvinte" para a barra de busca
    this.buscaSubject.pipe(
      debounceTime(500),       // Espera 500ms após o usuário parar de digitar
      distinctUntilChanged()   // Só executa se o texto for diferente do anterior
    ).subscribe(() => {
      this.currentPage = 1;    // Reseta para a primeira página ao fazer uma nova busca
      this.carregarPacientes();
    });
  }

  /**
   * Método central que busca os dados da API com base nos filtros e paginação atuais.
   */
  carregarPacientes(): void {
    this.isLoading = true;
    this.errorMessage = null;

    // Constrói o objeto de filtros a ser enviado para o serviço
    const filtros = {
      termoBusca: this.termoBusca,
      ativo: this.statusFiltro,
      orderBy: this.ordemFiltro.split('_')[0],      // Extrai o nome da coluna (ex: "NomeCompleto")
      orderByDesc: this.ordemFiltro.split('_')[1] === 'desc' // Extrai a direção (true ou false)
    };

    this.pacienteService.listarPacientes(this.currentPage, this.pageSize, filtros).subscribe({
      next: (resultado) => {
        this.pagedResult = resultado;
        this.pacientes = resultado.items;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.message;
        this.isLoading = false;
        (window as any).showToast(err.message || 'Falha ao carregar pacientes.', 'danger');
      }
    });
  }

  // --- Métodos de Ação dos Cards ---

  editarPaciente(paciente: Paciente): void {
    this.pacienteSelecionado = paciente; // Guarda o paciente para preencher o formulário do modal
    this.abrirModalPaciente();
    (window as any).showToast(`Editando paciente: ${paciente.nomeCompleto}`, 'info');
  }

  excluirPaciente(paciente: Paciente): void {
    if (confirm(`Tem certeza que deseja excluir o paciente "${paciente.nomeCompleto}"?`)) {
      this.isLoading = true;
      this.pacienteService.excluir(paciente.id).subscribe({
        next: () => {
          (window as any).showToast('Paciente excluído com sucesso!', 'success');
          this.carregarPacientes(); // Recarrega a lista para refletir a mudança
        },
        error: (err) => {
          this.isLoading = false;
          (window as any).showToast(err.message || 'Falha ao excluir paciente.', 'danger');
        }
      });
    }
  }

  // --- Métodos de Filtro e Paginação ---

  onBuscaInput(): void {
    this.buscaSubject.next(this.termoBusca);
  }

  onFiltroChange(): void {
    this.currentPage = 1; // Volta para a primeira página ao mudar um filtro
    this.carregarPacientes();
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
