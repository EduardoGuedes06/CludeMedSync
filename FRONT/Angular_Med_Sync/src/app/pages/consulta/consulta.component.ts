import { Component, OnInit } from '@angular/core'; // Importe OnInit
import { CommonModule, DatePipe } from '@angular/common'; // Importe DatePipe

import { GenericModalComponent } from '../modals/modal.component';
import { ConsultaLogModal } from '../modals/consulta/log/consultaLog.modal.component';
import { CrudConsultaModalComponent } from '../modals/consulta/crud/upsert.consulta.modal.component';

export interface IConsulta {
  id: number;
  pacienteNome: string;
  profissionalNome: string;
  dataHoraInicio: Date;
  dataHoraFim?: Date;
  status: 'Em Andamento' | 'Agendada' | 'Finalizada' | 'Cancelada';
}

@Component({
  selector: 'app-consultas',
  standalone: true,
  imports: [
    CommonModule,
    GenericModalComponent,
    ConsultaLogModal,
    CrudConsultaModalComponent
  ],
  templateUrl: './consulta.componente.html',
  styleUrls: ['./consulta.componente.css']
})
export class Consultas implements OnInit {

  modalLogAberto = false;
  modalCrudAberto = false;

  consultasEmAndamento: IConsulta[] = [];
  consultasAgendadas: IConsulta[] = [];

  ngOnInit(): void {
    this.carregarConsultas();
  }

  carregarConsultas(): void {

  }

  abrirModalLog(): void {
    this.modalLogAberto = true;
  }

  fecharModalLog(): void {
    this.modalLogAberto = false;
  }

  abrirModalCrud(): void {
    this.modalCrudAberto = true;
  }

  fecharModalCrud(): void {
    this.modalCrudAberto = false;
  }


  novaConsulta(): void {

    this.abrirModalCrud();
  }
  
  visualizarHistorico(): void {
    console.log("Ação: Visualizar Histórico");

  }

  exportarExcel(): void {
    console.log("Ação: Exportar para Excel");

  }
}