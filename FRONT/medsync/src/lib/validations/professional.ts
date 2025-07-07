import { z } from 'zod';

// Definimos as regras de validação para o formulário de profissionais.
export const professionalFormSchema = z.object({
  name: z.string()
    .min(3, { message: 'O nome deve ter pelo menos 3 caracteres.' }),
  
  email: z.string()
    .email({ message: 'Por favor, insira um email válido.' }),
  
  phone: z.string()
    .min(10, { message: 'O número de telefone deve ter pelo menos 10 dígitos.' }),

  licenseNumber: z.string()
    .min(5, { message: 'O número da licença parece curto demais.' }),

  specialty: z.string()
    .min(3, { message: 'A especialidade deve ter pelo menos 3 caracteres.' }),
});

export type ProfessionalFormData = z.infer<typeof professionalFormSchema>;