import { Routes } from '@angular/router';
import { Consultas } from './pages/consulta/consulta.component';
import { Pacientes } from './pages/pacientes/pacientes.component';
import { Profissionais } from './pages/profissionais/profissionais.component';
import { Configuracoes } from './pages/configuracoes/configuracoes';
import { BemVindo } from './pages/bem-vindo/bem-vindo';
import { MainLayout } from './pages/main-layout/main-layout';
import { Login } from './pages/auth/login/login';
import { Registro } from './pages/auth/registro/registro';
import { Home } from './pages/home/home';

export const routes: Routes = [
  {
    path: '',
    component: BemVindo
  },
  {
    path: 'auth',
    children: [
      { path: 'login', component: Login },
      { path: 'registro', component: Registro }
    ]
  },
  {
    path: 'app',
    component: MainLayout,
    children: [
      { path: 'home', component: Home },
      { path: 'consultas', component: Consultas },
      { path: 'pacientes', component: Pacientes },
      { path: 'profissionais', component: Profissionais },
      { path: 'configuracoes', component: Configuracoes },
      { path: '', redirectTo: 'home', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'home' }
];
