import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-consultas',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './consultas.html',
  styleUrls: ['./consultas.css']
})
export class Consultas {
  consultas = [
    { id: 1, paciente: 'Ana Silva', profissional: 'Dr. João', data: '2025-07-07' },
    { id: 2, paciente: 'Carlos Souza', profissional: 'Dra. Maria', data: '2025-07-08' },
    { id: 3, paciente: 'Mariana Costa', profissional: 'Dr. Pedro', data: '2025-07-09' }
  ];
}
