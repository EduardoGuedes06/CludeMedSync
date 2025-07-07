import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-bem-vindo',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './bem-vindo.html',
  styleUrls: ['./bem-vindo.css']
})
export class BemVindo {
  constructor(private router: Router) {}

  acessarPlataforma() {
    this.router.navigate(['/app']);
  }


}
