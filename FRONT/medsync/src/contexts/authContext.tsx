import React, { createContext, useState, useEffect, useCallback } from 'react';
import { authService } from '../services/authService';
import type { AuthContextType, User } from '../types/auth';

export const AuthContext = createContext<AuthContextType | null>(null);


export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  // Este useEffect é crucial. Ele roda uma única vez quando o app carrega
  // para verificar se já existe uma sessão salva no localStorage.
  useEffect(() => {
    const storedToken = localStorage.getItem('@MedSync:token');
    const storedUser = localStorage.getItem('@MedSync:user');

    if (storedToken && storedUser) {
      // Se encontramos um token e um usuário, consideramos o usuário logado.
      // No mundo real, aqui seria um bom lugar para validar o token com a API.
      setUser(JSON.parse(storedUser));
    }
    // Independentemente do resultado, paramos o loading inicial.
    setLoading(false);
  }, []);

  const signIn = useCallback(async (credentials: { email: string; password: string }) => {
    setLoading(true);
    try {
      const response = await authService.signIn(credentials);
      const { token, user: userData } = response;

      // Persiste os dados no localStorage para manter a sessão
      localStorage.setItem('@MedSync:token', token);
      localStorage.setItem('@MedSync:user', JSON.stringify(userData));

      // Atualiza o estado da aplicação
      setUser(userData);
    } catch (error) {
      console.error('Falha no login:', error);
      // Re-lançamos o erro para que o componente de Login possa tratá-lo
      // (por exemplo, mostrar uma notificação de erro para o usuário).
      throw error;
    } finally {
      setLoading(false);
    }
  }, []);

  const signOut = useCallback(() => {
    setUser(null);
    // Limpa os dados da sessão do localStorage
    localStorage.removeItem('@MedSync:token');
    localStorage.removeItem('@MedSync:user');
    // A navegação para a página de login será tratada no componente que chama o signOut.
  }, []);

  // Um booleano simples para facilitar a verificação de autenticação
  const isAuthenticated = !!user;

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, signIn, signOut, loading }}>
      {/*
        O `loading` inicial é importante. Enquanto for `true`, não renderizamos o resto da app.
        Isso evita um "flash" da tela de login antes de redirecionar para o dashboard.
      */}
      {!loading && children}
    </AuthContext.Provider>
  );
};
