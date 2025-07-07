import { api } from '../lib/api'; // 1. Importamos a nossa instância do Axios
import type { User } from '../types/auth';

// O tipo de resposta que esperamos da API .NET 8
// Supondo que a API retorna um objeto com `token` e `user`
interface LoginResponse {
  token: string;
  user: User;
}

export const authService = {
  /**
   * Realiza uma chamada de API real para o endpoint de login.
   * @param credentials - O email e a senha do usuário.
   * @returns Uma Promise que resolve com o token e os dados do usuário.
   */
  signIn: async (credentials: { email: string; password: string }): Promise<LoginResponse> => {
    console.log('Enviando credenciais para a API real:', credentials);

    try {
      // 2. Usamos `api.post` para fazer a requisição.
      // A URL base ('http://localhost:5000/api') já está configurada.
      // O endpoint aqui seria '/auth/login' ou similar, dependendo da sua API.
      const response = await api.post<{ token: string; user: User }>('/auth/login', credentials);

      // 3. Retornamos os dados diretamente da resposta da API.
      // O Axios coloca a resposta do servidor dentro da propriedade `data`.
      return response.data;
    } catch (error: any) {
      // 4. Tratamento de erros
      // Se a API retornar um erro (ex: 401 Unauthorized), o Axios o lançará.
      // Nós o capturamos e relançamos para que o componente de UI possa lidar com ele.
      console.error('Erro na chamada de API de login:', error.response?.data || error.message);
      throw new Error(error.response?.data?.message || 'Falha ao fazer login. Tente novamente.');
    }
  },
};
