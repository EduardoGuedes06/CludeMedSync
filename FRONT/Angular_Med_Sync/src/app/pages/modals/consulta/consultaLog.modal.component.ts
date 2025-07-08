import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-consulta-log-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './consultaLog.modal.component.html',
  styleUrls: ['./consultaLog.modal.component.css']
})
export class ConsultaLogModal {
  @Input() aberto = false;
  @Output() abertoChange = new EventEmitter<boolean>();

  fecharModal() {
    debugger
    this.aberto = false;
    this.abertoChange.emit(this.aberto);
  }
}
