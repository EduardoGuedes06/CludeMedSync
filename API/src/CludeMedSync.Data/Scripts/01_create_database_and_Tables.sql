DROP DATABASE IF EXISTS CludeMedSync;
CREATE DATABASE CludeMedSync;


USE CludeMedSync;

CREATE TABLE Usuario (
    Id VARCHAR(36) NOT NULL PRIMARY KEY,
    UserName VARCHAR(256) NULL,
    NormalizedUserName VARCHAR(256) NULL,
    Email VARCHAR(256) NULL,
    NormalizedEmail VARCHAR(256) NULL,
    EmailConfirmed BOOLEAN NOT NULL DEFAULT FALSE,
    PasswordHash TEXT NULL,
    SecurityStamp TEXT NULL,
    ConcurrencyStamp TEXT NULL,
    PhoneNumber TEXT NULL,
    PhoneNumberConfirmed BOOLEAN NOT NULL DEFAULT FALSE,
    TwoFactorEnabled BOOLEAN NOT NULL DEFAULT FALSE,
    LockoutEnd DATETIME NULL,
    LockoutEnabled BOOLEAN NOT NULL DEFAULT FALSE,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    Role VARCHAR(100) NOT NULL,
    RefreshToken VARCHAR(256) NULL,
    RefreshTokenExpiryTime DATETIME NULL
);

CREATE INDEX IX_Usuario_NormalizedEmail ON Usuario(NormalizedEmail);
CREATE UNIQUE INDEX IX_Usuario_NormalizedUserName ON Usuario(NormalizedUserName);

CREATE TABLE IF NOT EXISTS Paciente (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NomeCompleto VARCHAR(200) NOT NULL,
    DataNascimento DATE NOT NULL,
    CPF VARCHAR(11) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Telefone VARCHAR(15),
    Ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS Profissional (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NomeCompleto VARCHAR(200) NOT NULL,
    Especialidade VARCHAR(100) NOT NULL,
    CRM VARCHAR(20) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Telefone VARCHAR(15),
    Ativo BOOLEAN NOT NULL DEFAULT TRUE
);


CREATE TABLE IF NOT EXISTS Consulta (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId VARCHAR(36) NOT NULL,
    PacienteId INT NOT NULL,
    ProfissionalId INT NOT NULL,
    DataHoraInicio DATETIME NOT NULL,
    DataHoraFim DATETIME NOT NULL,
    Motivo VARCHAR(500),
    Observacao VARCHAR(500),
    Status INT NOT NULL,
    FOREIGN KEY (PacienteId) REFERENCES Paciente(Id),
    FOREIGN KEY (ProfissionalId) REFERENCES Profissional(Id)
);

CREATE TABLE IF NOT EXISTS ConsultaLog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
	UsuarioId VARCHAR(36) NOT NULL,
    ConsultaId INT NOT NULL,
    PacienteId INT NOT NULL,
    ProfissionalId INT NOT NULL,
    NomePaciente VARCHAR(200) NOT NULL,
    NomeProfissional VARCHAR(200) NOT NULL,
    DataHoraInicio DATETIME NOT NULL,
    DataHoraFim DATETIME NOT NULL,
    Motivo VARCHAR(500),
    Observacao VARCHAR(500),
    Status INT NOT NULL,
    DataLog TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IX_Consulta_Paciente_Profissional ON Consulta(PacienteId, ProfissionalId);
CREATE INDEX IX_Consulta_DataHoraInicio ON Consulta(DataHoraInicio);
