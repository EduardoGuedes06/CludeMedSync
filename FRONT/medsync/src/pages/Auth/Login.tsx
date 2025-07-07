import React, { useState } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { useNavigate } from 'react-router-dom';
import { Button } from '../../components/ui/Button'; // 1. Importamos o nosso Button
import { Input } from '../../components/ui/Input';   // 2. Importamos o nosso Input

const Login = () => {
  const navigate = useNavigate();
  const { signIn } = useAuth();
  const [email, setEmail] = useState('admin@medsync.com');
  const [password, setPassword] = useState('123456');
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      await signIn({ email, password });
      navigate('/dashboard');
    } catch (err) {
      setError('Email ou senha inválidos. Tente novamente.');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center h-screen bg-gray-200">
      <div className="p-8 bg-white rounded-lg shadow-xl w-96">
        <h2 className="text-2xl font-bold text-center text-brand-primary mb-6">MedSync Login</h2>
        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <label className="block text-gray-700 mb-2" htmlFor="email">Email</label>
            {/* 3. Usamos o componente Input */}
            <Input 
              type="email" 
              id="email" 
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              disabled={isLoading}
              placeholder="seuemail@exemplo.com"
            />
          </div>
          <div>
            <label className="block text-gray-700 mb-2" htmlFor="password">Senha</label>
            {/* 4. Usamos o componente Input */}
            <Input 
              type="password" 
              id="password" 
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              disabled={isLoading}
              placeholder="********"
            />
          </div>

          {error && <p className="text-red-500 text-sm text-center">{error}</p>}

          {/* 5. Usamos o componente Button, passando as props `isLoading` e `type` */}
          <Button type="submit" isLoading={isLoading}>
            Entrar
          </Button>
        </form>
      </div>
    </div>
  );
};

export default Login;
