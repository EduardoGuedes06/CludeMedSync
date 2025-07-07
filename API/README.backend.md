
# 🧠 MedSync - Backend (.NET 8 + Dapper)

Este é o backend da aplicação **MedSync**, construído com **ASP.NET Core 8**, utilizando **DDD (Domain-Driven Design)**, autenticação via **JWT**, e acesso a dados com **Dapper**.

---

## ⚙️ Tecnologias Utilizadas

- .NET 8 SDK
- ASP.NET Core Web API
- Dapper (ORM leve)
- JWT Bearer Authentication
- MySQL (ou SQLite compatível)
- Swagger (Swashbuckle)
- AutoMapper
- xUnit (para testes)
- Docker (para execução local)

---

## 📁 Estrutura de Pastas

```
/src
  /Domain
  /Application
  /Infrastructure
  /WebApi
```

- **Domain**: Entidades, Regras de Negócio, Interfaces
- **Application**: Serviços, DTOs, Validadores
- **Infrastructure**: Implementações (Dapper, Repositórios, Contextos)
- **WebApi**: Controllers, Configurações, Middlewares

---

## 🔐 Autenticação

- Registro: `POST /api/auth/register`
- Login: `POST /api/auth/login`
- Proteção via `[Authorize]` em endpoints privados
- Tokens JWT com refresh token

---

## 📋 Endpoints Principais

### 👤 Usuário
- `POST /api/auth/register` – Registrar
- `POST /api/auth/login` – Login

### 🧑‍⚕️ Profissionais
- `GET /api/profissionais`
- `POST /api/profissionais`
- `PUT /api/profissionais/{id}`
- `DELETE /api/profissionais/{id}`

### 👨‍💼 Pacientes
- `GET /api/pacientes`
- `POST /api/pacientes`
- `PUT /api/pacientes/{id}`
- `DELETE /api/pacientes/{id}`

### 📅 Consultas
- `GET /api/consultas`
- `POST /api/consultas`
- `PATCH /api/consultas/confirmar/{id}`
- `PATCH /api/consultas/iniciar/{id}`
- `PATCH /api/consultas/finalizar/{id}`
- `PATCH /api/consultas/cancelar/{id}`
- `PATCH /api/consultas/paciente-nao-compareceu/{id}`
- `PATCH /api/consultas/profissional-nao-compareceu/{id}`

---

## 🧪 Testes Automatizados

- Testes unitários com `xUnit` em camada de Application e Domain
- Testes de serviços e validações

---

## 🚀 Executando Localmente

### Com .NET CLI

```bash
cd src/WebApi
dotnet restore
dotnet ef database update
dotnet run
```

### Com Docker

```bash
docker-compose up --build
```

- A API estará em: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`

---

## 🛠 Scripts SQL

- Criação das tabelas e índices
- Arquivo: `/scripts/db-init.sql`

---

## 📚 Boas Práticas

- Camadas bem definidas (DDD)
- Repositórios separados usando Dapper
- Respostas padronizadas com DTO de `ResultadoOperacao<T>`
- Tratamento global de erros e logs simples
- Configurações externas via `appsettings.json`

---

## 📦 Licença

MIT © 2025 — Desenvolvido para o desafio técnico MedSync.
