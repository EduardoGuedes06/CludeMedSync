import { z } from 'zod';

// Definimos as regras de validação para o formulário de agendamento.
export const appointmentFormSchema = z.object({
  patientId: z.string()
    .min(1, { message: 'Por favor, selecione um paciente.' }),
  
  professionalId: z.string()
    .min(1, { message: 'Por favor, selecione um profissional.' }),
  
  // Usaremos string para a data e depois combinaremos com a hora.
  date: z.string()
    .min(1, { message: 'Por favor, selecione uma data.' }),

  time: z.string()
    .regex(/^([01]\d|2[0-3]):([0-5]\d)$/, { message: 'Formato de hora inválido (HH:MM).' }),

  notes: z.string().optional(), // Notas são opcionais
});

// Criamos o tipo TypeScript a partir do schema.
export type AppointmentFormData = z.infer<typeof appointmentFormSchema>;