import type { Appointment, AppointmentStatus } from '../types/appointment';
import { patientService } from './patientService'; 
import { professionalService } from './professionalService';

const today = new Date();
const getTodayAt = (hour: number, minute: number) => new Date(today.getFullYear(), today.getMonth(), today.getDate(), hour, minute).toISOString();

let mockAppointments: Appointment[] = [];

const initializeMockData = async () => {
  if (mockAppointments.length > 0) return;

  const patients = await patientService.getPatients();
  const professionals = await professionalService.getProfessionals();

  if (patients.length > 0 && professionals.length > 0) {
    mockAppointments = [
      { id: 'apt-1', patient: patients[0], professional: professionals[0], dateTime: getTodayAt(9, 0), status: 'Confirmed', notes: 'Paciente relata dor no peito.' },
      { id: 'apt-2', patient: patients[1], professional: professionals[1], dateTime: getTodayAt(9, 30), status: 'Confirmed' },
      { id: 'apt-3', patient: patients[2], professional: professionals[0], dateTime: getTodayAt(10, 0), status: 'Scheduled' },
      { id: 'apt-4', patient: patients[0], professional: professionals[1], dateTime: getTodayAt(14, 0), status: 'Completed' },
      { id: 'apt-5', patient: patients[3], professional: professionals[1], dateTime: getTodayAt(15, 30), status: 'Canceled', notes: 'Paciente cancelou por telefone.' },
    ];
  }
};

export const appointmentService = {
  getAppointments: async (): Promise<Appointment[]> => {
    await initializeMockData();
    console.log('Buscando lista de agendamentos (simulado)...');
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve([...mockAppointments]);
      }, 400);
    });
  },

  /**
   * Simula a atualização do status de um agendamento.
   * @param appointmentId O ID do agendamento a ser atualizado.
   * @param status O novo status para o agendamento.
   */
  updateAppointmentStatus: async (appointmentId: string, status: AppointmentStatus): Promise<Appointment> => {
    await initializeMockData();
    console.log(`Atualizando status do agendamento ${appointmentId} para ${status} (simulado)...`);
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        const appointmentIndex = mockAppointments.findIndex(a => a.id === appointmentId);
        if (appointmentIndex > -1) {
          mockAppointments[appointmentIndex].status = status;
          resolve(mockAppointments[appointmentIndex]);
        } else {
          reject(new Error('Agendamento não encontrado.'));
        }
      }, 500);
    });
  },

  // ... outras funções (create, update, delete) viriam aqui.
};
