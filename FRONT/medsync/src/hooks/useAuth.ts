import { useContext } from 'react';
import { AuthContext } from '../contexts/authContext';
import type { AuthContextType } from '../types/auth';

/**
 * Hook customizado para acessar o contexto de autenticação.
 * Ele simplifica o uso do contexto e adiciona uma verificação de segurança
 * para garantir que o hook só seja usado dentro de um AuthProvider.
 * @returns O valor do contexto de autenticação.
 */
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);

  // Esta verificação é crucial. Se um componente tentar usar este hook
  // sem estar dentro da árvore do AuthProvider, um erro claro será lançado.
  // Isso previne bugs difíceis de rastrear.
  if (!context) {
    throw new Error('useAuth deve ser usado dentro de um AuthProvider');
  }

  return context;
};