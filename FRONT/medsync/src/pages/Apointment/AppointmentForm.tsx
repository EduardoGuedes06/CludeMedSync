// --- src/pages/Appointments/AppointmentForm.tsx ---
import React, { useEffect, useState } from 'react';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { appointmentFormSchema, type AppointmentFormData } from '../../lib/validations/appointment';
import { patientService } from '../../services/patientService';
import { professionalService } from '../../services/professionalService';
import type { Patient } from '../../types/patient';
import type { Professional } from '../../types/professional';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';

// Componente para campos de seleção (dropdown)
const SelectField = ({ name, label, control, errors, children, ...props }: any) => (
  <div className="space-y-1">
    <label htmlFor={name} className="block text-sm font-medium text-gray-700">{label}</label>
    <Controller
      name={name}
      control={control}
      render={({ field }) => (
        <select
          id={name}
          {...field}
          {...props}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-brand-primary"
        >
          {children}
        </select>
      )}
    />
    {errors[name] && <p className="text-sm text-red-600">{errors[name].message}</p>}
  </div>
);

interface AppointmentFormProps {
  onSuccess: () => void;
}

export const AppointmentForm = ({ onSuccess }: AppointmentFormProps) => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [professionals, setProfessionals] = useState<Professional[]>([]);
  const [isLoadingData, setIsLoadingData] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Carrega os dados de pacientes e profissionais quando o formulário é montado
  useEffect(() => {
    const loadFormData = async () => {
      try {
        setIsLoadingData(true);
        const [patientsData, professionalsData] = await Promise.all([
          patientService.getPatients(),
          professionalService.getProfessionals(),
        ]);
        setPatients(patientsData.filter(p => p.isActive));
        setProfessionals(professionalsData.filter(p => p.isActive));
      } catch (error) {
        console.error("Falha ao carregar dados para o formulário", error);
        // TODO: Mostrar notificação de erro
      } finally {
        setIsLoadingData(false);
      }
    };
    loadFormData();
  }, []);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<AppointmentFormData>({
    resolver: zodResolver(appointmentFormSchema),
    defaultValues: {
      patientId: '',
      professionalId: '',
      date: '',
      time: '',
      notes: '',
    },
  });

  const onSubmit = async (data: AppointmentFormData) => {
    setIsSubmitting(true);
    console.log('Dados do formulário de agendamento válidos:', data);
    
    // Simulação de chamada à API
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    console.log('Agendamento criado com sucesso!');
    setIsSubmitting(false);
    onSuccess();
  };

  if (isLoadingData) {
    return <div>A carregar dados do formulário...</div>;
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <SelectField name="patientId" label="Paciente" control={control} errors={errors}>
        <option value="">Selecione um paciente</option>
        {patients.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
      </SelectField>

      <SelectField name="professionalId" label="Profissional" control={control} errors={errors}>
        <option value="">Selecione um profissional</option>
        {professionals.map(p => <option key={p.id} value={p.id}>{p.name} - {p.specialty}</option>)}
      </SelectField>

      <div className="grid grid-cols-2 gap-4">
        <div className="space-y-1">
          <label htmlFor="date" className="block text-sm font-medium text-gray-700">Data</label>
          <Controller name="date" control={control} render={({ field }) => <Input type="date" id="date" {...field} />} />
          {errors.date && <p className="text-sm text-red-600">{errors.date.message}</p>}
        </div>
        <div className="space-y-1">
          <label htmlFor="time" className="block text-sm font-medium text-gray-700">Hora</label>
          <Controller name="time" control={control} render={({ field }) => <Input type="time" id="time" {...field} />} />
          {errors.time && <p className="text-sm text-red-600">{errors.time.message}</p>}
        </div>
      </div>

      <div className="space-y-1">
        <label htmlFor="notes" className="block text-sm font-medium text-gray-700">Notas (Opcional)</label>
        <Controller name="notes" control={control} render={({ field }) => <textarea id="notes" {...field} rows={3} className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-brand-primary" />} />
      </div>

      <div className="pt-4">
        <Button type="submit" isLoading={isSubmitting}>
          Agendar Consulta
        </Button>
      </div>
    </form>
  );
};
