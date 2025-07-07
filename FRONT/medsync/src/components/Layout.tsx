import React from 'react';

// A prop "children" irá representar a página que será renderizada dentro do layout.
interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <div className="flex h-screen bg-gray-50">
      <aside className="w-64 bg-gray-800 p-4 text-white">
        <h2 className="text-xl font-bold">MedSync</h2>
        <nav className="mt-8">
          <p>Link 1</p>
          <p>Link 2</p>
        </nav>
      </aside>

      {/* Conteúdo Principal */}
      <div className="flex flex-1 flex-col">
        {/* Cabeçalho */}
        <header className="bg-white p-4 shadow-md">
          <p>Cabeçalho</p>
        </header>

        {/* Conteúdo da Página */}
        <main className="flex-1 p-8">
          {children}
        </main>
      </div>
    </div>
  );
};