import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { professionalFormSchema, type ProfessionalFormData } from '../../lib/validations/professional';
import type { Professional } from '../../types/professional';
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

interface ProfessionalFormProps {
  professional?: Professional | null;
  onSuccess: () => void;
}

export const ProfessionalForm = ({ professional, onSuccess }: ProfessionalFormProps) => {
  const isEditMode = !!professional;
  const [isSubmitting, setIsSubmitting] = React.useState(false);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<ProfessionalFormData>({
    resolver: zodResolver(professionalFormSchema),
    defaultValues: {
      name: professional?.name || '',
      email: professional?.email || '',
      phone: professional?.phone || '',
      licenseNumber: professional?.licenseNumber || '',
      specialty: professional?.specialty || '',
    },
  });

  const onSubmit = async (data: ProfessionalFormData) => {
    setIsSubmitting(true);
    console.log('Dados do formulário de profissional válidos:', data);
    
    // Simulação de chamada à API
    await new Promise(resolve => setTimeout(resolve, 1000));

    // Lógica para chamar o serviço de criação ou atualização
    // if (isEditMode) {
    //   await professionalService.updateProfessional(professional.id, data);
    // } else {
    //   await professionalService.createProfessional(data);
    // }
    
    console.log(`Profissional ${isEditMode ? 'atualizado' : 'criado'} com sucesso!`);
    setIsSubmitting(false);
    onSuccess(); // Fecha o modal e recarrega a lista
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <FormField name="name" label="Nome Completo" control={control} errors={errors} />
      <FormField name="email" label="Email" type="email" control={control} errors={errors} />
      <FormField name="phone" label="Telefone" type="tel" control={control} errors={errors} />
      <FormField name="licenseNumber" label="Nº de Licença (Ex: CRM/SP 12345)" control={control} errors={errors} />
      <FormField name="specialty" label="Especialidade" control={control} errors={errors} />

      <div className="pt-4">
        <Button type="submit" isLoading={isSubmitting}>
          {isEditMode ? 'Salvar Alterações' : 'Criar Profissional'}
        </Button>
      </div>
    </form>
  );
};