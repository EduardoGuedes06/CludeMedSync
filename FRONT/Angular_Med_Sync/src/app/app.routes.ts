import { Routes } from '@angular/router';
import { Consultas } from './pages/consulta/consulta.component';
import { Pacientes } from './pages/pacientes/pacientes.component';
import { Profissionais } from './pages/profissionais/profissionais.component';
import { HistoricoConsultas } from './pages/historico.consulta/historico.consulta.component';
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
      { path: 'historico-consultas', component: HistoricoConsultas },
      { path: 'consultas', component: Consultas },
      { path: 'pacientes', component: Pacientes },
      { path: 'profissionais', component: Profissionais },
      { path: '', redirectTo: 'consultas', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'home' }
];
