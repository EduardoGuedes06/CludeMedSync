DROP DATABASE IF EXISTS CludeMedSync;
CREATE DATABASE CludeMedSync;


USE CludeMedSync;

CREATE TABLE IF NOT EXISTS Usuario (
    Id VARCHAR(36) PRIMARY KEY,
    UserName VARCHAR(256) NOT NULL,
    NormalizedUserName VARCHAR(256) NOT NULL,
    Email VARCHAR(256) NOT NULL,
    NormalizedEmail VARCHAR(256) NOT NULL,
    EmailConfirmed BOOLEAN NOT NULL DEFAULT FALSE,
    PasswordHash TEXT,
    SecurityStamp TEXT,
    ConcurrencyStamp TEXT,
    PhoneNumber TEXT,
    PhoneNumberConfirmed BOOLEAN NOT NULL DEFAULT FALSE,
    TwoFactorEnabled BOOLEAN NOT NULL DEFAULT FALSE,
    LockoutEnd DATETIME,
    LockoutEnabled BOOLEAN NOT NULL DEFAULT FALSE,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    Role VARCHAR(100) NOT NULL
);

CREATE INDEX IX_Usuario_NormalizedEmail ON Usuario(NormalizedEmail);
CREATE UNIQUE INDEX IX_Usuario_NormalizedUserName ON Usuario(NormalizedUserName);

CREATE TABLE IF NOT EXISTS Paciente (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NomeCompleto VARCHAR(200) NOT NULL,
    DataNascimento DATE NOT NULL,
    CPF VARCHAR(11) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Telefone VARCHAR(20),
    Ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE UNIQUE INDEX IX_Paciente_CPF ON Paciente(CPF);
CREATE UNIQUE INDEX IX_Paciente_Email ON Paciente(Email);

CREATE TABLE IF NOT EXISTS Profissional (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NomeCompleto VARCHAR(200) NOT NULL,
    Especialidade VARCHAR(100) NOT NULL,
    CRM VARCHAR(20) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Telefone VARCHAR(20),
    Ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE UNIQUE INDEX IX_Profissional_CRM ON Profissional(CRM);
CREATE UNIQUE INDEX IX_Profissional_Email ON Profissional(Email);


CREATE TABLE IF NOT EXISTS Consulta (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PacienteId INT NOT NULL,
    ProfissionalId INT NOT NULL,
    DataHoraInicio DATETIME NOT NULL,
    DataHoraFim DATETIME NOT NULL,
    Motivo VARCHAR(500),
    Status VARCHAR(50) NOT NULL,
    FOREIGN KEY (PacienteId) REFERENCES Paciente(Id),
    FOREIGN KEY (ProfissionalId) REFERENCES Profissional(Id)
);

CREATE INDEX IX_Consulta_Paciente_Profissional ON Consulta(PacienteId, ProfissionalId);
CREATE INDEX IX_Consulta_DataHoraInicio ON Consulta(DataHoraInicio);
