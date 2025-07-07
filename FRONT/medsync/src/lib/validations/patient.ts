import { z } from 'zod';
export const patientFormSchema = z.object({
  name: z.string()
    .min(3, { message: 'O nome deve ter pelo menos 3 caracteres.' })
    .max(100, { message: 'O nome não pode ter mais de 100 caracteres.' }),
  
  email: z.string()
    .email({ message: 'Por favor, insira um endereço de email válido.' }),
  
  phone: z.string()
    .min(10, { message: 'O número de telefone deve ter pelo menos 10 dígitos.' })
    .max(15, { message: 'O número de telefone é muito longo.' }),
  
  dateOfBirth: z.string()
    .refine((date) => !isNaN(Date.parse(date)), {
      message: 'Por favor, insira uma data de nascimento válida.',
    }),
});
export type PatientFormData = z.infer<typeof patientFormSchema>;