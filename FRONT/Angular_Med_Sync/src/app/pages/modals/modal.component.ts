import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-generic-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class GenericModalComponent {

  @Input() aberto: boolean = false;
  @Input() titulo: string = 'Modal'; 
  @Output() fechar = new EventEmitter<void>();
  fecharModal() {
    this.fechar.emit();
  }
  onContentClick(event: MouseEvent) {
    event.stopPropagation();
  }
}