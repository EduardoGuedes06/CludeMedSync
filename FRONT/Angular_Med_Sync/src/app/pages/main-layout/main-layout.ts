import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './main-layout.html',
  styleUrls: ['./main-layout.css']
})
export class MainLayout implements OnInit {
  currentSectionId: string = 'painel';
  sidebarActive = window.innerWidth >= 768;

  sections = [
    { id: 'consultas', icon: 'fas fa-calendar-check', label: 'Consultas', route: '/app/consultas' },
    { id: 'pacientes', icon: 'fas fa-user-injured', label: 'Pacientes', route: '/app/pacientes' },
    { id: 'profissionais', icon: 'fas fa-user-md', label: 'Profissionais', route: '/app/profissionais' },
    { id: 'historico-consultas', icon: 'fas fa fa-history', label: 'Historico', route: '/app/historico-consultas' },
  ];

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  logout(event: Event) {
    event.preventDefault();
    this.authService.logout();
    (window as any).showToast('Usuario Deslogado com sucesso', 'info');
    this.router.navigate(['/auth/login']);
  }


  showSection(sectionId: string) {
    this.currentSectionId = sectionId;
  }

  toggleSidebar() {
    this.sidebarActive = !this.sidebarActive;
  }

  onResize(event: any) {
    this.sidebarActive = event.target.innerWidth >= 768;
  }

  ngOnInit() {
    const token = this.authService.getToken();
    if (!token) {
      (window as any).showToast('Você precisa estar autenticado para acessar o painel.', 'warning');
      this.router.navigate(['/auth/login']);
      return;
    }

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      const url = event.urlAfterRedirects;
      const matchedSection = this.sections.find(s => url.startsWith(s.route));
      this.currentSectionId = matchedSection ? matchedSection.id : 'painel';
    });
  }
}
