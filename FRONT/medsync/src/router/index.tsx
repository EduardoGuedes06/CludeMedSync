import React, { useEffect, useState } from 'react';
import { useAuth } from '../hooks/useAuth';
import { dashboardService, type DashboardKPIs } from '../services/dashboardService';
import type { Appointment } from '../types/appointment';
import { Card } from '../components/ui/Card';
import { Calendar, Users, DollarSign, Clock } from 'lucide-react';

// Um pequeno componente para formatar a hora
const formatTime = (dateString: string) => {
  return new Date(dateString).toLocaleTimeString('pt-BR', {
    hour: '2-digit',
    minute: '2-digit',
  });
};

const Dashboard = () => {
  const { user } = useAuth();
  const [kpis, setKpis] = useState<DashboardKPIs | null>(null);
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadDashboardData = async () => {
      try {
        setLoading(true);
        // Busca os dados em paralelo para otimizar o carregamento
        const [kpisData, appointmentsData] = await Promise.all([
          dashboardService.getDashboardKPIs(),
          dashboardService.getUpcomingAppointments(),
        ]);
        setKpis(kpisData);
        setAppointments(appointmentsData);
      } catch (err) {
        console.error('Falha ao carregar dados do dashboard:', err);
        setError('Não foi possível carregar as informações do dashboard.');
      } finally {
        setLoading(false);
      }
    };

    loadDashboardData();
  }, []);

  if (loading) {
    return <div>A carregar dashboard...</div>;
  }

  if (error) {
    return <div className="text-red-500">{error}</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-2">Bem-vindo(a), {user?.name}!</h1>
      <p className="text-gray-600 mb-8">Aqui está um resumo da sua clínica hoje.</p>

      {/* Secção de KPIs */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
        <Card title="Consultas do Dia" icon={<Calendar />}>
          <p className="text-3xl font-bold">{kpis?.appointmentsToday}</p>
        </Card>
        <Card title="Pacientes em Espera" icon={<Users />}>
          <p className="text-3xl font-bold">{kpis?.patientsWaiting}</p>
        </Card>
        <Card title="Receita (Hoje)" icon={<DollarSign />}>
          <p className="text-3xl font-bold">{kpis?.revenueToday.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</p>
        </Card>
      </div>

      {/* Secção de Próximas Consultas */}
      <Card title="Próximas Consultas">
        {appointments.length > 0 ? (
          <ul className="space-y-4">
            {appointments.map(apt => (
              <li key={apt.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-md hover:bg-gray-100">
                <div className="flex items-center">
                  <Clock className="h-5 w-5 mr-3 text-brand-secondary" />
                  <div>
                    <p className="font-semibold">{apt.patient.name}</p>
                    <p className="text-sm text-gray-500">com {apt.professional.name}</p>
                  </div>
                </div>
                <div className="text-right">
                   <p className="font-bold text-lg text-brand-primary">{formatTime(apt.dateTime)}</p>
                   <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      apt.status === 'Confirmed' ? 'bg-blue-100 text-blue-800' : 'bg-yellow-100 text-yellow-800'
                    }`}
                  >
                    {apt.status === 'Confirmed' ? 'Confirmada' : 'Agendada'}
                  </span>
                </div>
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500">Nenhuma consulta pendente para hoje.</p>
        )}
      </Card>
    </div>
  );
};

export default Dashboard;
