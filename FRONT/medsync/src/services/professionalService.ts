import { api } from '../lib/api';
import type { Professional } from '../types/professional';
import type { ProfessionalFormData } from '../lib/validations/professional';

export const professionalService = {
  /**
   * Busca a lista de todos os profissionais da API.
   */
  getProfessionals: async (): Promise<Professional[]> => {
    try {
      console.log('Buscando lista de profissionais da API real...');
      const response = await api.get<Professional[]>('/professionals');
      return response.data;
    } catch (error: any) {
      console.error('Erro ao buscar profissionais:', error.response?.data || error.message);
      throw new Error('Não foi possível carregar os profissionais.');
    }
  },

  /**
   * Cria um novo profissional na API.
   */
  createProfessional: async (professionalData: ProfessionalFormData): Promise<Professional> => {
    try {
      console.log('Criando novo profissional na API real...', professionalData);
      const response = await api.post<Professional>('/professionals', professionalData);
      return response.data;
    } catch (error: any) {
      console.error('Erro ao criar profissional:', error.response?.data || error.message);
      throw new Error('Não foi possível criar o profissional.');
    }
  },

  /**
   * Atualiza um profissional existente na API.
   */
  updateProfessional: async (professionalId: string, professionalData: ProfessionalFormData): Promise<Professional> => {
    try {
      console.log(`Atualizando profissional ${professionalId} na API real...`, professionalData);
      const response = await api.put<Professional>(`/professionals/${professionalId}`, professionalData);
      return response.data;
    } catch (error: any) {
      console.error('Erro ao atualizar profissional:', error.response?.data || error.message);
      throw new Error('Não foi possível atualizar o profissional.');
    }
  },

  /**
   * Apaga um profissional da API.
   */
  deleteProfessional: async (professionalId: string): Promise<void> => {
    try {
      console.log(`Apagando profissional ${professionalId} da API real...`);
      await api.delete(`/professionals/${professionalId}`);
    } catch (error: any)
    {
      console.error('Erro ao apagar profissional:', error.response?.data || error.message);
      throw new Error('Não foi possível apagar o profissional.');
    }
  },
};
