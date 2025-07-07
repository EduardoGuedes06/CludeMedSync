import React, { useEffect, useState } from 'react';
import { appointmentService } from '../../services/appointmentService';
import type { Appointment, AppointmentStatus } from '../../types/appointment';
import { Button } from '../../components/ui/Button';
import { Modal } from '../../components/ui/Modal';
import { ConfirmationModal } from '../../components/ui/ConfirmationModal';
import { AppointmentForm } from './AppointmentForm';
import { PlusCircle, Clock, CheckCircle, XCircle, User, Stethoscope, Ban, Check, Play, CheckSquare } from 'lucide-react';

const AppointmentCard = ({ appointment, onCancel, onConfirm, onStart, onComplete }: { 
  appointment: Appointment; 
  onCancel: (appointment: Appointment) => void;
  onConfirm: (appointment: Appointment) => void;
  onStart: (appointment: Appointment) => void;
  onComplete: (appointment: Appointment) => void;
}) => {
  const statusConfig: Record<AppointmentStatus, { icon: React.ReactNode; color: string; label: string }> = {
    Scheduled: { icon: <Clock className="h-5 w-5" />, color: 'bg-yellow-100 text-yellow-800 border-yellow-500', label: 'Agendada' },
    Confirmed: { icon: <Clock className="h-5 w-5" />, color: 'bg-blue-100 text-blue-800 border-blue-500', label: 'Confirmada' },
    InProgress: { icon: <Clock className="h-5 w-5 animate-pulse" />, color: 'bg-indigo-100 text-indigo-800 border-indigo-500', label: 'Em Andamento' },
    Completed: { icon: <CheckCircle className="h-5 w-5" />, color: 'bg-green-100 text-green-800 border-green-500', label: 'Concluída' },
    Canceled: { icon: <XCircle className="h-5 w-5" />, color: 'bg-red-100 text-red-800 border-red-500', label: 'Cancelada' },
  };

  const config = statusConfig[appointment.status];
  const appointmentTime = new Date(appointment.dateTime).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
  const canBeCanceled = ['Scheduled', 'Confirmed'].includes(appointment.status);
  const canBeConfirmed = appointment.status === 'Scheduled';
  const canBeStarted = appointment.status === 'Confirmed';
  const canBeCompleted = appointment.status === 'InProgress';

  return (
    <div className={`bg-white p-4 rounded-lg shadow-md border-l-4 ${config.color.split(' ')[2]}`}>
      <div className="flex justify-between items-start">
        <div>
          <p className="text-xl font-bold">{appointmentTime}</p>
          <div className="flex items-center text-sm text-gray-600 mt-2">
            <User className="h-4 w-4 mr-2" />
            <span>{appointment.patient.name}</span>
          </div>
          <div className="flex items-center text-sm text-gray-500 mt-1">
            <Stethoscope className="h-4 w-4 mr-2" />
            <span>{appointment.professional.name}</span>
          </div>
        </div>
        <div className={`flex items-center px-3 py-1 rounded-full text-sm font-semibold ${config.color}`}>
          {config.icon}
          <span className="ml-2">{config.label}</span>
        </div>
      </div>
      {appointment.notes && (
        <div className="mt-4 pt-4 border-t border-gray-200">
          <p className="text-sm text-gray-700 italic">"{appointment.notes}"</p>
        </div>
      )}
      <div className="mt-4 flex justify-end space-x-2">
        {canBeCompleted && (
          <Button variant="primary" onClick={() => onComplete(appointment)} className="w-auto bg-green-600 hover:bg-green-700 px-3 py-1 text-sm flex items-center">
            <CheckSquare className="h-4 w-4 mr-1" />
            Finalizar
          </Button>
        )}
        {canBeStarted && (
           <Button variant="primary" onClick={() => onStart(appointment)} className="w-auto bg-indigo-600 hover:bg-indigo-700 px-3 py-1 text-sm flex items-center">
            <Play className="h-4 w-4 mr-1" />
            Iniciar
          </Button>
        )}
        {canBeConfirmed && (
          <Button variant="primary" onClick={() => onConfirm(appointment)} className="w-auto bg-green-600 hover:bg-green-700 px-3 py-1 text-sm flex items-center">
            <Check className="h-4 w-4 mr-1" />
            Confirmar
          </Button>
        )}
        {canBeCanceled && (
          <Button variant="danger" onClick={() => onCancel(appointment)} className="w-auto px-3 py-1 text-sm flex items-center">
            <Ban className="h-4 w-4 mr-1" />
            Cancelar
          </Button>
        )}
        <Button variant="secondary" className="w-auto px-3 py-1 text-sm">Ver Detalhes</Button>
      </div>
    </div>
  );
};

const AppointmentList = () => {
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);

  const [isCancelModalOpen, setIsCancelModalOpen] = useState(false);
  const [appointmentToCancel, setAppointmentToCancel] = useState<Appointment | null>(null);
  const [isCanceling, setIsCanceling] = useState(false);

  const [isConfirmModalOpen, setIsConfirmModalOpen] = useState(false);
  const [appointmentToConfirm, setAppointmentToConfirm] = useState<Appointment | null>(null);
  const [isConfirming, setIsConfirming] = useState(false);

  const [isStartModalOpen, setIsStartModalOpen] = useState(false);
  const [appointmentToStart, setAppointmentToStart] = useState<Appointment | null>(null);
  const [isStarting, setIsStarting] = useState(false);

  const [isCompleteModalOpen, setIsCompleteModalOpen] = useState(false);
  const [appointmentToComplete, setAppointmentToComplete] = useState<Appointment | null>(null);
  const [isCompleting, setIsCompleting] = useState(false);

  const fetchAppointments = async () => {
    try {
      setLoading(true);
      const data = await appointmentService.getAppointments();
      setAppointments(data.sort((a, b) => new Date(a.dateTime).getTime() - new Date(b.dateTime).getTime()));
    } catch (err) {
      setError('Falha ao carregar a lista de agendamentos.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAppointments();
  }, []);

  const handleFormSuccess = () => {
    setIsFormModalOpen(false);
    fetchAppointments();
  };

  const handleOpenCancelModal = (appointment: Appointment) => {
    setAppointmentToCancel(appointment);
    setIsCancelModalOpen(true);
  };

  const handleCancelConfirm = async () => {
    if (!appointmentToCancel) return;
    setIsCanceling(true);
    try {
      await appointmentService.updateAppointmentStatus(appointmentToCancel.id, 'Canceled');
      setIsCancelModalOpen(false);
      setAppointmentToCancel(null);
      fetchAppointments();
    } catch (err) {
      console.error('Falha ao cancelar agendamento:', err);
    } finally {
      setIsCanceling(false);
    }
  };

  const handleOpenConfirmModal = (appointment: Appointment) => {
    setAppointmentToConfirm(appointment);
    setIsConfirmModalOpen(true);
  };

  const handleConfirmAppointment = async () => {
    if (!appointmentToConfirm) return;
    setIsConfirming(true);
    try {
      await appointmentService.updateAppointmentStatus(appointmentToConfirm.id, 'Confirmed');
      setIsConfirmModalOpen(false);
      setAppointmentToConfirm(null);
      fetchAppointments();
    } catch (err) {
      console.error('Falha ao confirmar agendamento:', err);
    } finally {
      setIsConfirming(false);
    }
  };

  const handleOpenStartModal = (appointment: Appointment) => {
    setAppointmentToStart(appointment);
    setIsStartModalOpen(true);
  };

  const handleStartAppointment = async () => {
    if (!appointmentToStart) return;
    setIsStarting(true);
    try {
      await appointmentService.updateAppointmentStatus(appointmentToStart.id, 'InProgress');
      setIsStartModalOpen(false);
      setAppointmentToStart(null);
      fetchAppointments();
    } catch (err) {
      console.error('Falha ao iniciar agendamento:', err);
    } finally {
      setIsStarting(false);
    }
  };

  const handleOpenCompleteModal = (appointment: Appointment) => {
    setAppointmentToComplete(appointment);
    setIsCompleteModalOpen(true);
  };

  const handleCompleteAppointment = async () => {
    if (!appointmentToComplete) return;
    setIsCompleting(true);
    try {
      await appointmentService.updateAppointmentStatus(appointmentToComplete.id, 'Completed');
      setIsCompleteModalOpen(false);
      setAppointmentToComplete(null);
      fetchAppointments();
    } catch (err) {
      console.error('Falha ao finalizar agendamento:', err);
    } finally {
      setIsCompleting(false);
    }
  };

  const upcomingAppointments = appointments.filter(a => ['Scheduled', 'Confirmed', 'InProgress'].includes(a.status));
  const pastAppointments = appointments.filter(a => ['Completed', 'Canceled'].includes(a.status));

  if (loading) {
    return <div>A carregar agendamentos...</div>;
  }

  if (error) {
    return <div className="text-red-500">{error}</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Agenda do Dia</h1>
        <Button onClick={() => setIsFormModalOpen(true)}>
          <PlusCircle className="mr-2 h-5 w-5" />
          Novo Agendamento
        </Button>
      </div>

      <div className="space-y-8">
        <div>
          <h2 className="text-xl font-semibold mb-4 border-b pb-2">Próximos Agendamentos</h2>
          {upcomingAppointments.length > 0 ? (
            <div className="space-y-4">
              {upcomingAppointments.map(apt => <AppointmentCard key={apt.id} appointment={apt} onCancel={handleOpenCancelModal} onConfirm={handleOpenConfirmModal} onStart={handleOpenStartModal} onComplete={handleOpenCompleteModal} />)}
            </div>
          ) : (
            <p className="text-gray-500">Não há agendamentos pendentes para hoje.</p>
          )}
        </div>
        <div>
          <h2 className="text-xl font-semibold mb-4 border-b pb-2">Histórico do Dia</h2>
          {pastAppointments.length > 0 ? (
            <div className="space-y-4">
              {pastAppointments.map(apt => <AppointmentCard key={apt.id} appointment={apt} onCancel={handleOpenCancelModal} onConfirm={handleOpenConfirmModal} onStart={handleOpenStartModal} onComplete={handleOpenCompleteModal} />)}
            </div>
          ) : (
            <p className="text-gray-500">Nenhum agendamento concluído ou cancelado hoje.</p>
          )}
        </div>
      </div>

      <Modal open={isFormModalOpen} onOpenChange={setIsFormModalOpen} title="Novo Agendamento">
        <AppointmentForm onSuccess={handleFormSuccess} />
      </Modal>

      {appointmentToCancel && (
        <ConfirmationModal
          open={isCancelModalOpen}
          onOpenChange={setIsCancelModalOpen}
          title="Confirmar Cancelamento"
          description={`Tem a certeza de que deseja cancelar a consulta de ${appointmentToCancel.patient.name} com ${appointmentToCancel.professional.name}?`}
          onConfirm={handleCancelConfirm}
          isConfirming={isCanceling}
        />
      )}

      {appointmentToConfirm && (
        <ConfirmationModal
          open={isConfirmModalOpen}
          onOpenChange={setIsConfirmModalOpen}
          title="Confirmar Agendamento"
          description={`Tem a certeza de que deseja confirmar a consulta de ${appointmentToConfirm.patient.name}?`}
          onConfirm={handleConfirmAppointment}
          isConfirming={isConfirming}
        />
      )}

      {appointmentToStart && (
        <ConfirmationModal
          open={isStartModalOpen}
          onOpenChange={setIsStartModalOpen}
          title="Iniciar Consulta"
          description={`Tem a certeza de que deseja iniciar a consulta de ${appointmentToStart.patient.name}?`}
          onConfirm={handleStartAppointment}
          isConfirming={isStarting}
        />
      )}

      {appointmentToComplete && (
        <ConfirmationModal
          open={isCompleteModalOpen}
          onOpenChange={setIsCompleteModalOpen}
          title="Finalizar Consulta"
          description={`Tem a certeza de que deseja finalizar a consulta de ${appointmentToComplete.patient.name}?`}
          onConfirm={handleCompleteAppointment}
          isConfirming={isCompleting}
        />
      )}
    </div>
  );
};

export default AppointmentList;