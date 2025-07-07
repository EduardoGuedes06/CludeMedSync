import React, { useEffect, useState } from 'react';
import { patientService } from '../../services/patientService';
import type { Patient } from '../../types/patient';
import { Button } from '../../components/ui/Button';
import { Modal } from '../../components/ui/Modal';
import { ConfirmationModal } from '../../components/ui/ConfirmationModal';
import { PatientForm } from './PatientForm';
import { PlusCircle, Edit, Trash2 } from 'lucide-react';

const PatientList = () => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Estados para o modal de formulário
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [editingPatient, setEditingPatient] = useState<Patient | null>(null);

  // Estados para o modal de confirmação de exclusão
  const [isConfirmModalOpen, setIsConfirmModalOpen] = useState(false);
  const [patientToDelete, setPatientToDelete] = useState<Patient | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  const fetchPatients = async () => {
    try {
      setLoading(true);
      const data = await patientService.getPatients();
      setPatients(data);
    } catch (err) {
      setError('Falha ao carregar a lista de pacientes.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPatients();
  }, []);

  // --- Handlers para o Formulário (Criar/Editar) ---
  const handleOpenCreateModal = () => {
    setEditingPatient(null);
    setIsFormModalOpen(true);
  };

  const handleOpenEditModal = (patient: Patient) => {
    setEditingPatient(patient);
    setIsFormModalOpen(true);
  };

  const handleFormSuccess = () => {
    setIsFormModalOpen(false);
    fetchPatients();
  };

  // --- Handlers para a Exclusão ---
  const handleOpenDeleteModal = (patient: Patient) => {
    setPatientToDelete(patient);
    setIsConfirmModalOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!patientToDelete) return;
    setIsDeleting(true);
    try {
      await patientService.deletePatient(patientToDelete.id);
      setIsConfirmModalOpen(false);
      setPatientToDelete(null);
      fetchPatients(); // Recarrega a lista
    } catch (err) {
      // TODO: Mostrar uma notificação de erro para o usuário
      console.error('Falha ao apagar paciente:', err);
    } finally {
      setIsDeleting(false);
    }
  };

  if (loading && patients.length === 0) {
    return <div>A carregar pacientes...</div>;
  }

  if (error) {
    return <div className="text-red-500">{error}</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Pacientes</h1>
        <Button onClick={handleOpenCreateModal}>
          <PlusCircle className="mr-2 h-5 w-5" />
          Novo Paciente
        </Button>
      </div>

      <div className="bg-white shadow-md rounded-lg overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nome</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Ações</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {patients.map((patient) => (
              <tr key={patient.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{patient.name}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{patient.email}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      patient.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                    }`}
                  >
                    {patient.isActive ? 'Ativo' : 'Inativo'}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-4">
                  <button onClick={() => handleOpenEditModal(patient)} className="text-brand-primary hover:text-blue-700 inline-flex items-center">
                    <Edit className="h-4 w-4 mr-1" />
                    Editar
                  </button>
                  <button onClick={() => handleOpenDeleteModal(patient)} className="text-brand-danger hover:text-red-700 inline-flex items-center">
                    <Trash2 className="h-4 w-4 mr-1" />
                    Apagar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Modal para Criar/Editar Paciente */}
      <Modal
        open={isFormModalOpen}
        onOpenChange={setIsFormModalOpen}
        title={editingPatient ? 'Editar Paciente' : 'Novo Paciente'}
      >
        <PatientForm
          patient={editingPatient}
          onSuccess={handleFormSuccess}
        />
      </Modal>

      {/* Modal para Confirmar Exclusão */}
      {patientToDelete && (
        <ConfirmationModal
          open={isConfirmModalOpen}
          onOpenChange={setIsConfirmModalOpen}
          title="Confirmar Exclusão"
          description={`Tem a certeza de que deseja apagar o paciente "${patientToDelete.name}"? Esta ação não pode ser desfeita.`}
          onConfirm={handleDeleteConfirm}
          isConfirming={isDeleting}
        />
      )}
    </div>
  );
};

export default PatientList;
