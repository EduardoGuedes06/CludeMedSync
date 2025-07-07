import { BrowserRouter } from 'react-router-dom';
import { AppRoutes } from '../src/router';
import { AuthProvider } from '../src/contexts/authContext';

function App() {
  return (
    // 2. Envolvemos toda a aplicação com o AuthProvider.
    // Agora, qualquer componente dentro de AppRoutes poderá usar o hook useAuth.
    <AuthProvider>
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;