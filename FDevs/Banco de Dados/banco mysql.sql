CREATE DATABASE FDEVS;

USE FDEVS;

CREATE TABLE Curso (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL,
    DataConclusao DATE NULL,
    TrilhaId INT NOT NULL,
    StatusId INT NOT NULL
);

CREATE TABLE Questao (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Texto VARCHAR(500) NOT NULL,
    ProvaId INT NOT NULL
);

CREATE TABLE Prova (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    CursoId INT NOT NULL,
    UsuarioId INT NOT NULL
);

CREATE TABLE Resposta (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    QuestaoId INT NOT NULL,
    AlternativaId INT NOT NULL
);

CREATE TABLE Status (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL
);

CREATE TABLE Trilha (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL
);

CREATE TABLE Usuario (
    UsuarioId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    DataNascimento DATETIME NOT NULL,
    Foto VARCHAR(500) NOT NULL
);

CREATE TABLE Video (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(50) NOT NULL,
    URL VARCHAR(500) NOT NULL,
    ModuloId INT NOT NULL
);

CREATE TABLE UsuarioCurso (
    UsuarioId INT NOT NULL,
    CursoId INT NOT NULL,
    PRIMARY KEY (UsuarioId, CursoId)
);

CREATE TABLE Modulo (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    StatusId INT NOT NULL,
    UsuarioId INT NOT NULL,
    CursoId INT NOT NULL
);

CREATE TABLE Alternativa (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Texto VARCHAR(200) NOT NULL,
    Correta BIT NOT NULL,
    QuestaoId INT NOT NULL
);

ALTER TABLE Curso ADD CONSTRAINT FK_Curso_Status FOREIGN KEY (StatusId) REFERENCES Status(Id);
ALTER TABLE Resposta ADD CONSTRAINT FK_Resposta_Questao FOREIGN KEY (QuestaoId) REFERENCES Questao(Id);
ALTER TABLE Modulo ADD CONSTRAINT FK_Modulo_Curso FOREIGN KEY (CursoId) REFERENCES Curso(Id);
ALTER TABLE Resposta ADD CONSTRAINT FK_Resposta_Alternativa FOREIGN KEY (AlternativaId) REFERENCES Alternativa(Id);
ALTER TABLE Modulo ADD CONSTRAINT FK_Modulo_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);
ALTER TABLE UsuarioCurso ADD CONSTRAINT FK_UsuarioCurso_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);
ALTER TABLE Modulo ADD CONSTRAINT FK_Modulo_Status FOREIGN KEY (StatusId) REFERENCES Status(Id);
ALTER TABLE Video ADD CONSTRAINT FK_Video_Modulo FOREIGN KEY (ModuloId) REFERENCES Modulo(Id);
ALTER TABLE UsuarioCurso ADD CONSTRAINT FK_UsuarioCurso_Curso FOREIGN KEY (CursoId) REFERENCES Curso(Id);
ALTER TABLE Curso ADD CONSTRAINT FK_Curso_Trilha FOREIGN KEY (TrilhaId) REFERENCES Trilha(Id);
ALTER TABLE Alternativa ADD CONSTRAINT FK_Alternativa_Questao FOREIGN KEY (QuestaoId) REFERENCES Questao(Id);
ALTER TABLE Questao ADD CONSTRAINT FK_Questao_Prova FOREIGN KEY (ProvaId) REFERENCES Prova(Id);
ALTER TABLE Prova ADD CONSTRAINT FK_Prova_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);
ALTER TABLE Prova ADD CONSTRAINT FK_Prova_Curso FOREIGN KEY (CursoId) REFERENCES Curso(Id);
ALTER TABLE Resposta ADD CONSTRAINT FK_Resposta_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);

-- Inserindo dados na tabela Status
INSERT INTO Status (Nome) VALUES
('Em andamento'),
('Concluído'),
('Cancelado');

-- Inserindo dados na tabela Usuario
INSERT INTO Usuario (Nome, DataNascimento, Foto) VALUES
('Alice', '1995-05-15', 'foto_alice.jpg'),
('Bob', '1985-09-25', 'foto_bob.jpg'),
('Carlos', '2000-01-10', 'foto_carlos.jpg');

-- Inserindo dados na tabela Trilha
INSERT INTO Trilha (Nome, Foto) VALUES
('Trilha de Backend', 'foto_backend.jpg'),
('Trilha de Frontend', 'foto_frontend.jpg'),
('Trilha de Ciência de Dados', 'foto_ciencia_dados.jpg'),
('Trilha de Desenvolvimento Móvel', 'foto_desenvolvimento_movel.jpg');

-- Inserindo dados na tabela Curso
INSERT INTO Curso (Nome, Foto, DataConclusao, TrilhaId, StatusId) VALUES
('Curso de Banco de Dados', 'foto_banco_dados.jpg', NULL, 1, 1),
('Curso de Programação em Python', 'foto_python.jpg', NULL, 1, 1),
('Curso de JavaScript', 'foto_javascript.jpg', NULL, 2, 1);

-- Inserindo dados na tabela Prova
INSERT INTO Prova (Nome, CursoId, UsuarioId) VALUES
('Prova de Banco de Dados', 1, 1),  -- Alice
('Prova de Programação em Python', 2, 2);  -- Bob

-- Inserindo dados na tabela Modulo
INSERT INTO Modulo (Nome, StatusId, UsuarioId, CursoId) VALUES
('Módulo 1: Introdução ao Banco de Dados', 1, 1, 1),  -- Alice
('Módulo 2: Modelagem de Dados', 1, 1, 1),  -- Alice
('Módulo 1: Introdução ao Python', 1, 2, 2);  -- Bob

-- Inserindo dados na tabela Video
INSERT INTO Video (Titulo, URL, ModuloId) VALUES
('Video 1: O que é Banco de Dados?', 'https://example.com/video1', 1),  -- Módulo 1
('Video 2: Modelagem de Dados 101', 'https://example.com/video2', 2),  -- Módulo 2
('Video 1: Introdução ao Python', 'https://example.com/video3', 3);  -- Módulo 1

-- Inserindo dados na tabela Questao
INSERT INTO Questao (Texto, ProvaId) VALUES
('O que é um Banco de Dados?', 1),  -- Prova de Banco de Dados
('Qual a diferença entre SQL e NoSQL?', 1),  -- Prova de Banco de Dados
('O que é uma função em Python?', 2);  -- Prova de Programação em Python

-- Inserindo dados na tabela Alternativa
INSERT INTO Alternativa (Texto, Correta, QuestaoId) VALUES
('Um sistema que armazena dados', 1, 1),  -- Questão 1
('Uma ferramenta de programação', 0, 1),  -- Questão 1
('É uma linguagem de consulta', 1, 2),  -- Questão 2
('É uma linguagem de programação', 0, 2),  -- Questão 2
('Uma sequência de comandos', 1, 3),  -- Questão 3
('Uma estrutura de dados', 0, 3);  -- Questão 3

-- Inserindo dados na tabela Resposta
INSERT INTO Resposta (UsuarioId, QuestaoId, AlternativaId) VALUES
(1, 1, 1),  -- Alice responde à questão 1
(1, 2, 3),  -- Alice responde à questão 2
(2, 2, 5);  -- Bob responde à questão 2

INSERT INTO UsuarioCurso (UsuarioId, CursoId) VALUES
(1, 1),  -- Alice está inscrita no Curso de Banco de Dados
(2, 2),  -- Bob está inscrito no Curso de Programação em Python
(1, 3);  -- Alice está inscrita no Curso de JavaScript
