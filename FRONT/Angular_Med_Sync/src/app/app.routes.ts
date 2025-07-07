import { Routes } from '@angular/router';
import { Consultas } from './pages/consultas/consultas';
import { Pacientes } from './pages/pacientes/pacientes';
import { Profissionais } from './pages/profissionais/profissionais';
import { BemVindo } from './pages/bem-vindo/bem-vindo';
import { MainLayout } from './pages/main-layout/main-layout';
import { Login } from './pages/auth/login/login';
import { Registro } from './pages/auth/registro/registro';

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
      { path: 'consultas', component: Consultas },
      { path: 'pacientes', component: Pacientes },
      { path: 'profissionais', component: Profissionais }
    ]
  },
  { path: '**', redirectTo: '' }
];
