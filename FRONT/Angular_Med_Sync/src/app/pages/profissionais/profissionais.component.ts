import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenericModalComponent } from '../modals/modal.component'; 
import { ProfissionalModalComponent } from '../modals/profissional/profissional.modal.component';

@Component({
  selector: 'app-profissionais',
  standalone: true,
  imports: [CommonModule, GenericModalComponent, ProfissionalModalComponent],
  templateUrl: './profissionais.component.html',
  styleUrls: ['./profissionais.component.css']
})

export class Profissionais {
  modalAberto = false;

  abrirModalProfissional() {
    this.modalAberto = true;
  }

  fecharModalProfissional() {
    this.modalAberto = false;
  }

  salvarProfissional(paciente: any) {
    this.fecharModalProfissional();
  }
}