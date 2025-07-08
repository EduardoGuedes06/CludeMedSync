import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenericModalComponent } from '../modals/modal.component'; 
import { PacienteModalComponent } from '../modals/paciente/paciente.modal.component';

@Component({
  selector: 'app-pacientes',
  standalone: true,
  imports: [CommonModule, GenericModalComponent, PacienteModalComponent],
  templateUrl: './pacientes.component.html',
  styleUrls: ['./pacientes.component.css']
})
export class Pacientes {

  modalAberto = false;

  abrirModalPaciente() {
    this.modalAberto = true;
  }

  fecharModalPaciente() {
    this.modalAberto = false;
  }

  salvarPaciente(paciente: any) {
    this.fecharModalPaciente();
  }
}
