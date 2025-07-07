import type { Appointment } from '../types/appointment';
import type { Patient } from '../types/patient';
import type { Professional } from '../types/professional';

// --- Dados Fictícios (Mocks) ---
// Em um projeto maior, estes mocks poderiam vir de um arquivo separado.

const mockProfessionals: Professional[] = [
  { id: 'prof-1', name: 'Dr. Carlos Andrade', specialty: 'Cardiologia', licenseNumber: 'CRM/SP 12345', isActive: true },
  { id: 'prof-2', name: 'Dra. Sofia Mendes', specialty: 'Clínica Geral', licenseNumber: 'CRM/RJ 67890', isActive: true },
];

const mockPatients: Patient[] = [
  { id: '1', name: 'Ana Silva', email: 'ana.silva@example.com', phone: '(11) 98765-4321', dateOfBirth: '1985-04-12', isActive: true },
  { id: '2', name: 'Bruno Costa', email: 'bruno.costa@example.com', phone: '(21) 91234-5678', dateOfBirth: '1990-11-23', isActive: true },
  { id: '5', name: 'Elisa Ferreira', email: 'elisa.f@example.com', phone: '(41) 98877-6655', dateOfBirth: '1992-08-15', isActive: true },
];

// Gera datas de consulta para o dia de hoje
const today = new Date();
const getTodayAt = (hour: number, minute: number) => new Date(today.getFullYear(), today.getMonth(), today.getDate(), hour, minute).toISOString();

const mockAppointments: Appointment[] = [
  { id: 'apt-1', patient: mockPatients[0], professional: mockProfessionals[0], dateTime: getTodayAt(9, 0), status: 'Confirmed' },
  { id: 'apt-2', patient: mockPatients[1], professional: mockProfessionals[1], dateTime: getTodayAt(9, 30), status: 'Confirmed' },
  { id: 'apt-3', patient: mockPatients[2], professional: mockProfessionals[0], dateTime: getTodayAt(10, 0), status: 'Scheduled' },
  { id: 'apt-4', patient: mockPatients[0], professional: mockProfessionals[1], dateTime: getTodayAt(14, 0), status: 'Completed' },
];

// --- Tipos de Resposta do Serviço ---

export interface DashboardKPIs {
  appointmentsToday: number;
  patientsWaiting: number;
  revenueToday: number; // Exemplo de outro KPI
}

// --- Serviço ---

export const dashboardService = {
  /**
   * Simula a busca dos Indicadores Chave de Desempenho (KPIs) para o dashboard.
   */
  getDashboardKPIs: async (): Promise<DashboardKPIs> => {
    console.log('Buscando KPIs do dashboard (simulado)...');
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({
          appointmentsToday: mockAppointments.length,
          patientsWaiting: mockAppointments.filter(a => a.status === 'Confirmed' || a.status === 'Scheduled').length,
          revenueToday: 580.00,
        });
      }, 700); // Simula latência de rede
    });
  },

  /**
   * Simula a busca das próximas consultas do dia.
   */
  getUpcomingAppointments: async (): Promise<Appointment[]> => {
    console.log('Buscando próximas consultas (simulado)...');
    return new Promise(resolve => {
      setTimeout(() => {
        const upcoming = mockAppointments
          .filter(a => a.status === 'Scheduled' || a.status === 'Confirmed')
          .sort((a, b) => new Date(a.dateTime).getTime() - new Date(b.dateTime).getTime());
        resolve(upcoming);
      }, 900);
    });
  },
};