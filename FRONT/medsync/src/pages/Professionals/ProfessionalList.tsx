// --- src/pages/Professionals/ProfessionalList.tsx ---
import React, { useEffect, useState } from 'react';
import { professionalService } from '../../services/professionalService';
import type { Professional } from '../../types/professional';
import { Button } from '../../components/ui/Button';
import { Modal } from '../../components/ui/Modal';
import { ConfirmationModal } from '../../components/ui/ConfirmationModal';
import { ProfessionalForm } from './ProfessionalForm';
import { PlusCircle, Edit, Trash2 } from 'lucide-react';

const ProfessionalList = () => {
  const [professionals, setProfessionals] = useState<Professional[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Estados para gerir os modais
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [editingProfessional, setEditingProfessional] = useState<Professional | null>(null);
  const [isConfirmModalOpen, setIsConfirmModalOpen] = useState(false);
  const [professionalToDelete, setProfessionalToDelete] = useState<Professional | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  const fetchProfessionals = async () => {
    try {
      setLoading(true);
      const data = await professionalService.getProfessionals();
      setProfessionals(data);
    } catch (err) {
      setError('Falha ao carregar a lista de profissionais.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProfessionals();
  }, []);

  // Handlers para o formulário de criar/editar
  const handleOpenCreateModal = () => {
    setEditingProfessional(null);
    setIsFormModalOpen(true);
  };

  const handleOpenEditModal = (professional: Professional) => {
    setEditingProfessional(professional);
    setIsFormModalOpen(true);
  };

  const handleFormSuccess = () => {
    setIsFormModalOpen(false);
    fetchProfessionals();
  };

  // Handlers para a exclusão
  const handleOpenDeleteModal = (professional: Professional) => {
    setProfessionalToDelete(professional);
    setIsConfirmModalOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!professionalToDelete) return;
    setIsDeleting(true);
    try {
      await professionalService.deleteProfessional(professionalToDelete.id);
      setIsConfirmModalOpen(false);
      setProfessionalToDelete(null);
      fetchProfessionals();
    } catch (err) {
      console.error('Falha ao apagar profissional:', err);
    } finally {
      setIsDeleting(false);
    }
  };

  if (loading && professionals.length === 0) {
    return <div>A carregar profissionais...</div>;
  }

  if (error) {
    return <div className="text-red-500">{error}</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Profissionais</h1>
        <Button onClick={handleOpenCreateModal}>
          <PlusCircle className="mr-2 h-5 w-5" />
          Novo Profissional
        </Button>
      </div>

      <div className="bg-white shadow-md rounded-lg overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nome</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Especialidade</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Ações</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {professionals.map((prof) => (
              <tr key={prof.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{prof.name}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{prof.specialty}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      prof.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                    }`}
                  >
                    {prof.isActive ? 'Ativo' : 'Inativo'}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-4">
                  <button onClick={() => handleOpenEditModal(prof)} className="text-brand-primary hover:text-blue-700 inline-flex items-center">
                    <Edit className="h-4 w-4 mr-1" />
                    Editar
                  </button>
                  <button onClick={() => handleOpenDeleteModal(prof)} className="text-brand-danger hover:text-red-700 inline-flex items-center">
                    <Trash2 className="h-4 w-4 mr-1" />
                    Apagar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Modal para Criar/Editar Profissional */}
      <Modal
        open={isFormModalOpen}
        onOpenChange={setIsFormModalOpen}
        title={editingProfessional ? 'Editar Profissional' : 'Novo Profissional'}
      >
        <ProfessionalForm
          professional={editingProfessional}
          onSuccess={handleFormSuccess}
        />
      </Modal>

      {/* Modal para Confirmar Exclusão */}
      {professionalToDelete && (
        <ConfirmationModal
          open={isConfirmModalOpen}
          onOpenChange={setIsConfirmModalOpen}
          title="Confirmar Exclusão"
          description={`Tem a certeza de que deseja apagar o profissional "${professionalToDelete.name}"? Esta ação não pode ser desfeita.`}
          onConfirm={handleDeleteConfirm}
          isConfirming={isDeleting}
        />
      )}
    </div>
  );
};

export default ProfessionalList;
