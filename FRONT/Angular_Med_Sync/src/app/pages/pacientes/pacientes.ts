import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pacientes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pacientes.html',
  styleUrls: ['./pacientes.css']
})
export class Pacientes {
  pacientes = [
    { id: 1, nome: 'Ana Silva', idade: 30 },
    { id: 2, nome: 'Carlos Souza', idade: 45 },
    { id: 3, nome: 'Mariana Costa', idade: 27 }
  ];
}