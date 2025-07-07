import type { Patient } from './patient';
import type { Professional } from './professional';

// Usar um tipo de união para o status da consulta torna o código mais seguro e legível.
// Evita erros de digitação e o autocompletar funciona perfeitamente.
export type AppointmentStatus = 'Scheduled' | 'Confirmed' | 'InProgress' | 'Completed' | 'Canceled';

// Define a estrutura de dados para uma Consulta.
// Note que ela "compõe" os tipos Patient e Professional.
export interface Appointment {
  id: string;
  patient: Patient;
  professional: Professional;
  dateTime: string; // Formato ISO 8601: "2025-07-07T14:30:00.000Z"
  status: AppointmentStatus;
  notes?: string; // Notas são opcionais
}