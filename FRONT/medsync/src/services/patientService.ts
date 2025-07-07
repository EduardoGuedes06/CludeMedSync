import { api } from '../lib/api';
import type { Patient } from '../types/patient';
import type { PatientFormData } from '../lib/validations/patient';

export const patientService = {
  /**
   * Busca a lista de todos os pacientes da API.
   */
  getPatients: async (): Promise<Patient[]> => {
    try {
      console.log('Buscando lista de pacientes da API real...');
      const response = await api.get<Patient[]>('/patients');
      return response.data;
    } catch (error: any) {
      console.error('Erro ao buscar pacientes:', error.response?.data || error.message);
      throw new Error('Não foi possível carregar os pacientes.');
    }
  },

  /**
   * Cria um novo paciente na API.
   * @param patientData Os dados do novo paciente, validados pelo formulário.
   */
  createPatient: async (patientData: PatientFormData): Promise<Patient> => {
    try {
      console.log('Criando novo paciente na API real...', patientData);
      const response = await api.post<Patient>('/patients', patientData);
      return response.data;
    } catch (error: any) {
      console.error('Erro ao criar paciente:', error.response?.data || error.message);
      throw new Error('Não foi possível criar o paciente.');
    }
  },

  /**
   * Atualiza um paciente existente na API.
   * @param patientId O ID do paciente a ser atualizado.
   * @param patientData Os novos dados do paciente.
   */
  updatePatient: async (patientId: string, patientData: PatientFormData): Promise<Patient> => {
    try {
      console.log(`Atualizando paciente ${patientId} na API real...`, patientData);
      const response = await api.put<Patient>(`/patients/${patientId}`, patientData);
      return response.data;
    } catch (error: any) {
      console.error('Erro ao atualizar paciente:', error.response?.data || error.message);
      throw new Error('Não foi possível atualizar o paciente.');
    }
  },

  /**
   * Apaga um paciente da API.
   * @param patientId O ID do paciente a ser apagado.
   */
  deletePatient: async (patientId: string): Promise<void> => {
    try {
      console.log(`Apagando paciente ${patientId} da API real...`);
      await api.delete(`/patients/${patientId}`);
    } catch (error: any) {
      console.error('Erro ao apagar paciente:', error.response?.data || error.message);
      throw new Error('Não foi possível apagar o paciente.');
    }
  },
};