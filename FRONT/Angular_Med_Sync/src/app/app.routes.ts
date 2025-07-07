import { Routes } from '@angular/router';
import { Consultas } from './pages/consultas/consultas';
import { Pacientes } from './pages/pacientes/pacientes';
import { Profissionais } from './pages/profissionais/profissionais';

export const routes: Routes = [
  { path: '', redirectTo: 'consultas', pathMatch: 'full' },
  { path: 'consultas', component: Consultas },
  { path: 'pacientes', component: Pacientes },
  { path: 'profissionais', component: Profissionais }
];
