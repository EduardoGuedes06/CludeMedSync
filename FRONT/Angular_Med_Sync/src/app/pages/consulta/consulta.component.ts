import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GenericModalComponent } from '../modals/modal.component'; 
import { ConsultaLogModal } from '../modals/consulta/consultaLog.modal.component';

@Component({
  selector: 'app-consultas',
  standalone: true,
  imports: [
    CommonModule, 
    GenericModalComponent, 
    ConsultaLogModal
  ],
  templateUrl: './consulta.componente.html',
  styleUrls: ['./consulta.componente.css']
})
export class Consultas {

  modalAberto = false;

  abrirModalConsulta() {
    this.modalAberto = true;
  }

  fecharModalConsulta() {
    this.modalAberto = false;
  }
}