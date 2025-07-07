import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profissionais',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profissionais.html',
  styleUrls: ['./profissionais.css']
})
export class Profissionais {
  profissionais = [
    { id: 1, nome: 'Dr. João', especialidade: 'Cardiologista' },
    { id: 2, nome: 'Dra. Maria', especialidade: 'Pediatra' },
    { id: 3, nome: 'Dr. Pedro', especialidade: 'Ortopedista' }
  ];
}