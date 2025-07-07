import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { patientFormSchema, type PatientFormData } from '../../lib/validations/patient';
import { patientService } from '../../services/patientService'; // Usaremos o serviço real
import type { Patient } from '../../types/patient';
import { Input } from '../../components/ui/Input';
import { Button } from '../../components/ui/Button';

// Componente auxiliar para renderizar campos do formulário
const FormField = ({ name, label, control, errors, ...props }: any) => (
  <div className="space-y-1">
    <label htmlFor={name} className="block text-sm font-medium text-gray-700">
      {label}
    </label>
    <Controller
      name={name}
      control={control}
      render={({ field }) => <Input id={name} {...field} {...props} />}
    />
    {errors[name] && (
      <p className="text-sm text-red-600">{errors[name].message}</p>
    )}
  </div>
);

interface PatientFormProps {
  patient?: Patient | null;
  onSuccess: () => void;
}

export const PatientForm = ({ patient, onSuccess }: PatientFormProps) => {
  const isEditMode = !!patient;
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [apiError, setApiError] = React.useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<PatientFormData>({
    resolver: zodResolver(patientFormSchema),
    defaultValues: {
      name: patient?.name || '',
      email: patient?.email || '',
      phone: patient?.phone || '',
      dateOfBirth: patient?.dateOfBirth ? new Date(patient.dateOfBirth).toISOString().split('T')[0] : '',
    },
  });

  // Esta função só será chamada se a validação do Zod passar.
  const onSubmit = async (data: PatientFormData) => {
    setIsSubmitting(true);
    setApiError(null);
    
    try {
      if (isEditMode) {
        // 1. Chamada para atualizar um paciente existente
        await patientService.updatePatient(patient.id, data);
      } else {
        // 2. Chamada para criar um novo paciente
        await patientService.createPatient(data);
      }
      
      console.log(`Paciente ${isEditMode ? 'atualizado' : 'criado'} com sucesso!`);
      onSuccess(); // Fecha o modal e recarrega a lista
    } catch (error: any) {
      console.error(`Erro ao ${isEditMode ? 'atualizar' : 'criar'} paciente:`, error);
      setApiError(error.message || `Ocorreu um erro. Por favor, tente novamente.`);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <FormField name="name" label="Nome Completo" control={control} errors={errors} />
      <FormField name="email" label="Email" type="email" control={control} errors={errors} />
      <FormField name="phone" label="Telefone" type="tel" control={control} errors={errors} />
      <FormField name="dateOfBirth" label="Data de Nascimento" type="date" control={control} errors={errors} />

      {apiError && <p className="text-sm text-center text-red-600">{apiError}</p>}

      <div className="pt-4">
        <Button type="submit" isLoading={isSubmitting}>
          {isEditMode ? 'Salvar Alterações' : 'Criar Paciente'}
        </Button>
      </div>
    </form>
  );
};
