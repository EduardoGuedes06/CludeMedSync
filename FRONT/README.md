
# 💻 MedSync - Frontend (Angular 17 + TypeScript)

Este é o frontend da aplicação **MedSync**, desenvolvido com **Angular 17**, **TypeScript**, e **TailwindCSS**. Ele consome a API RESTful do backend .NET e oferece uma interface moderna e responsiva para agendamento e gerenciamento de consultas.

---

## 🧰 Tecnologias Utilizadas

- Angular 17 com TypeScript (Standalone Components)
- Angular Router para navegação
- HttpClient para comunicação com API RESTful
- TailwindCSS para estilização
- RXJS para gerenciamento reativo
- Angular Reactive Forms com validações customizadas
- Serviços para autenticação JWT e manipulação de tokens
- Toasts personalizados para feedback do usuário

---

## 📁 Estrutura do Projeto

```
/src/app
  /components       # Componentes reutilizáveis e UI
  /pages            # Páginas e views (Login, Registro, Dashboard, etc.)
  /services         # Serviços para comunicação com API e lógica de negócio
  /validations      # Validadores customizados para formulários
  /models           # Modelos e interfaces TypeScript
  /shared           # Utilitários, interceptors, e constantes
  /styles           # Estilos globais (tailwind.css configurado)
```

---

## 🔐 Funcionalidades

- Login com persistência e renovação automática de token JWT
- Registro de usuário com validação avançada (senha forte, confirmação)
- Validações de formulário em tempo real (nome, e-mail, senha, etc)
- Navegação com rotas públicas e privadas protegidas
- Cadastro e gerenciamento de Pacientes e Profissionais
- Agendamento e gerenciamento completo de Consultas
- Alterações de status da consulta (Confirmar, Iniciar, Finalizar, Cancelar, Não comparecimento)
- Feedback visual e via toasts
- Interface responsiva e acessível

---

## 🚀 Executando o Projeto

### Pré-requisitos

- Node.js 18+
- Angular CLI 17+ instalado
- Backend rodando localmente (`http://localhost:5000`)

### Instalação

```bash
npm install
npm run start
```

Acesse em: `http://localhost:4200`

---

## 🌐 Variáveis de Ambiente

Crie um arquivo `src/environments/environment.ts` com:

```ts
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000/api'
};
```

---

## 🧼 Boas Práticas Aplicadas

- Componentização clara com Standalone Components
- Formulários reativos com validações customizadas em tempo real
- Navegação protegida por guards e rotas privadas
- Comunicação isolada com API via serviços Angular
- Estilização com TailwindCSS

---

## 👤 Usuário de Teste

```
Email: teste@gmail.com
Senha: Teste@123
```

---

## 📦 Licença

MIT © 2025 — Desenvolvido para o desafio técnico MedSync.
