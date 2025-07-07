import axios from 'axios';

// Crie uma instância do Axios com configurações pré-definidas.
export const api = axios.create({
  // Substitua pela URL base da sua API .NET 8
  // Ex: 'https://api.medsync.com/v1'
  baseURL: process.env.VITE_API_URL || 'http://localhost:5000/api', 
});

// Interceptor de Requisições (Request Interceptor)
// Esta função é executada ANTES de cada pedido ser enviado.
api.interceptors.request.use(
  (config) => {
    // Pega o token do localStorage
    const token = localStorage.getItem('@MedSync:token');

    // Se o token existir, anexa-o ao cabeçalho de autorização
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    // Faz algo com o erro da requisição
    return Promise.reject(error);
  }
);