import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-consulta-crud-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upsert.consulta.modal.component.html',
  styleUrl: './upsert.consulta.modal.component.css'
})
export class CrudConsultaModalComponent {
  @Input() aberto = false;
  @Output() abertoChange = new EventEmitter<boolean>();

  @Output() fechar = new EventEmitter<void>();
  @Output() salvar = new EventEmitter<any>();


  fecharModal() {
    this.fechar.emit();
  }

  onSubmit(event: Event) {

  }
}
