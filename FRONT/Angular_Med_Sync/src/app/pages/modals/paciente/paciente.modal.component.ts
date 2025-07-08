import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal-paciente',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './paciente.modal.component.html',
  styleUrls: ['./paciente.modal.component.css']
})
export class PacienteModalComponent {
  @Output() fechar = new EventEmitter<void>();
  @Output() salvar = new EventEmitter<any>();

  fecharModal() {
    this.fechar.emit();
  }

  onSubmit(event: Event) {
    event.preventDefault();
    const form = (event.target as HTMLFormElement);

    const paciente = {
      id: (form.querySelector('#paciente-id') as HTMLInputElement)?.value,
      nome: (form.querySelector('#paciente-name') as HTMLInputElement)?.value,
      nascimento: (form.querySelector('#data-Nascimento') as HTMLInputElement)?.value,
      cpf: (form.querySelector('#cpf') as HTMLInputElement)?.value,
      email: (form.querySelector('#email') as HTMLInputElement)?.value,
      telefone: (form.querySelector('#telefone') as HTMLInputElement)?.value
    };

    this.salvar.emit(paciente);
  }
}
