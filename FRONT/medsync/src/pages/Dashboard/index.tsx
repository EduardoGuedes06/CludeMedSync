import { Link } from 'react-router-dom';

export const DashboardPage = () => (
  <div className="flex h-screen flex-col items-center justify-center bg-gray-100">
    <h1 className="text-3xl font-bold">Dashboard</h1>
    <Link to="/login" className="mt-4 text-blue-600 hover:underline">
      Ir para o Login
    </Link>
  </div>
);
