
# 💻 MedSync - Frontend (React + Vite + TypeScript)

Este é o frontend da aplicação **MedSync**, desenvolvido com **React 19**, **TypeScript**, **Vite** e **TailwindCSS**. Ele consome a API RESTful do backend .NET e oferece uma interface amigável para agendamento e gerenciamento de consultas.

---

## 🧰 Tecnologias Utilizadas

- React 19 com TypeScript
- Vite (build tool)
- React Router DOM v6
- TailwindCSS + Lucide Icons
- Axios para chamadas HTTP
- React Hook Form + Zod (validação)
- Radix UI Dialog (modais)
- ESLint para padronização de código

---

## 📁 Estrutura do Projeto

```
/src
  /pages          # Páginas da aplicação (Login, Dashboard, etc.)
  /components     # Componentes reutilizáveis
  /services       # Comunicação com API (axios)
  /contexts       # Contextos globais (auth, tema, etc.)
  /hooks          # Hooks personalizados
  /routes         # Definição das rotas
  /types          # Tipagens globais
  /utils          # Utilitários e helpers
  /styles         # Estilos globais (tailwind)
```

---

## 🔐 Funcionalidades

- Login com persistência de token JWT
- Rotas públicas e privadas com controle de autenticação
- Cadastro e listagem de Pacientes
- Cadastro e listagem de Profissionais
- Agendamento de Consultas
- Alterações de status da consulta:
  - Confirmar, Iniciar, Finalizar, Cancelar, Não comparecimento
- Feedbacks e validações de formulários
- Interface responsiva e acessível

---

## 🚀 Executando o Projeto

### Pré-requisitos

- Node.js 18+
- Backend rodando localmente (`http://localhost:5000`)

### Instalação

```bash
cd frontend
npm install
npm run dev
```

Acesse em: `http://localhost:5173`

---

## 🌐 Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto com:

```
VITE_API_URL=http://localhost:5000/api
```

---

## 🧪 Testes (opcional)

> Planejado para ser estendido com React Testing Library + Jest

---

## 🧼 Boas Práticas Aplicadas

- Componentização clara com separação de responsabilidades
- Formulários fortemente tipados com validação automática
- Navegação protegida por rotas privadas
- Comunicação isolada com API via camada de serviços
- Estilização com TailwindCSS + reutilização via classes utilitárias

---

## 🧪 Exemplo de Stack Atual

```json
"dependencies": {
  "axios": "^1.7.2",
  "lucide-react": "^0.525.0",
  "react": "^19.1.0",
  "react-dom": "^19.1.0",
  "react-hook-form": "^7.52.1",
  "react-router-dom": "^6.24.0",
  "zod": "^3.23.8",
  "@hookform/resolvers": "^3.3.1",
  "@radix-ui/react-dialog": "^1.0.6"
}
```

---

## 👤 Usuário de Teste

```
Email: teste@gmail.com
Senha: Teste@123
```

---

## 📦 Licença

MIT © 2025 — Desenvolvido para o desafio técnico MedSync.
