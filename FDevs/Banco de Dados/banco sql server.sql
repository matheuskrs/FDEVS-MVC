CREATE DATABASE FDEVS;

USE FDEVS;

CREATE TABLE Curso (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL,
    DataConclusao DATE NULL,
    TrilhaId INT NOT NULL,
    StatusId INT NOT NULL
);

CREATE TABLE Questao (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Texto VARCHAR(500) NOT NULL,
    ProvaId INT NOT NULL
);

CREATE TABLE Prova (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    CursoId INT NOT NULL,
    UsuarioId VARCHAR(400) NOT NULL
);

CREATE TABLE Resposta (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    UsuarioId VARCHAR(400) NOT NULL,
    QuestaoId INT NOT NULL,
    AlternativaId INT NOT NULL
);

CREATE TABLE Status (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
	Cor VARCHAR(50) NOT NULL
);

CREATE TABLE Trilha (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL
);

CREATE TABLE Usuario (
    UsuarioId VARCHAR(400) NOT NULL PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    DataNascimento DATETIME NOT NULL,
    Foto VARCHAR(500) NOT NULL
);

CREATE TABLE Video (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Titulo VARCHAR(50) NOT NULL,
    URL VARCHAR(500) NOT NULL,
    ModuloId INT NOT NULL
);

CREATE TABLE UsuarioCurso (
    UsuarioId VARCHAR(400) NOT NULL,
    CursoId INT NOT NULL,
    PRIMARY KEY (UsuarioId, CursoId)
);

CREATE TABLE Modulo (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    StatusId INT NOT NULL,
    UsuarioId VARCHAR(400) NOT NULL,
    CursoId INT NOT NULL
);

CREATE TABLE Alternativa (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
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

INSERT INTO Status (Nome, Cor) VALUES
('Em andamento', 'rgba(255, 255, 0, 1)'),  -- Amarelo com 50% de opacidade
('Concluído', 'rgba(0, 255, 0, 1)'),         -- Verde
('Cancelado', 'rgba(255, 0, 0, 1)');         -- Vermelho

INSERT INTO Usuario (UsuarioId, Nome, DataNascimento, Foto) VALUES
('user_001', 'Alice', '05-11-1995', 'foto_alice.jpg'),
('user_002', 'Bob', '09-09-1995', 'foto_bob.jpg'),
('user_003', 'Carlos', '01-10-2000', 'foto_carlos.jpg');

INSERT INTO Trilha (Nome, Foto) VALUES
('Trilha de Backend', 'foto_backend.jpg'),
('Trilha de Frontend', 'foto_frontend.jpg'),
('Trilha de Ciência de Dados', 'foto_ciencia_dados.jpg'),
('Trilha de Desenvolvimento Móvel', 'foto_desenvolvimento_movel.jpg');

INSERT INTO Curso (Nome, Foto, DataConclusao, TrilhaId, StatusId) VALUES
('Lógica de programação', '/img/Cursos/1.png', NULL, 1, 1),
('Banco de Dados', '/img/Cursos/2.png', NULL, 1, 2),
('JavaScript', '/img/Cursos/3.png', NULL, 2, 2);


INSERT INTO Prova (Nome, CursoId, UsuarioId) VALUES
('Prova de Banco de Dados', 1, 'user_001'),  -- Alice
('Prova de Programação em Python', 2, 'user_002');  -- Bob

INSERT INTO Modulo (Nome, StatusId, UsuarioId, CursoId) VALUES
('Módulo 1: Introdução ao Banco de Dados', 1, 'user_001', 1),  -- Alice
('Módulo 2: Modelagem de Dados', 1, 'user_001', 1),  -- Alice
('Módulo 1: Introdução ao Python', 1, 'user_002', 2);  -- Bob

INSERT INTO Video (Titulo, URL, ModuloId) VALUES
('Video 1: O que é Banco de Dados?', 'https://example.com/video1', 1),
('Video 2: Modelagem de Dados 101', 'https://example.com/video2', 2),
('Video 1: Introdução ao Python', 'https://example.com/video3', 3);

INSERT INTO Questao (Texto, ProvaId) VALUES
('O que é um Banco de Dados?', 1),  -- Prova de Banco de Dados
('Qual a diferença entre SQL e NoSQL?', 1),  -- Prova de Banco de Dados
('O que é uma função em Python?', 2);  -- Prova de Programação em Python

INSERT INTO Alternativa (Texto, Correta, QuestaoId) VALUES
('Um sistema que armazena dados', 1, 1),  -- Questão 1
('Uma ferramenta de programação', 0, 1),  -- Questão 1
('É uma linguagem de consulta', 1, 2),  -- Questão 2
('É uma linguagem de programação', 0, 2),  -- Questão 2
('Uma sequência de comandos', 1, 3),  -- Questão 3
('Uma estrutura de dados', 0, 3);  -- Questão 3

INSERT INTO Resposta (UsuarioId, QuestaoId, AlternativaId) VALUES
('user_001', 1, 1),  -- Alice responde à questão 1
('user_001', 2, 3),  -- Alice responde à questão 2
('user_002', 2, 5);  -- Bob responde à questão 2

INSERT INTO UsuarioCurso (UsuarioId, CursoId) VALUES
('user_001', 1),  -- Alice está inscrita no Curso de Banco de Dados
('user_002', 2),  -- Bob está inscrita no Curso de Programação em Python
('user_001', 3);  -- Alice está inscrita no Curso de JavaScript

SELECT 
    u.UsuarioId,
    u.Nome,
    u.DataNascimento,
    c.Nome AS CursoNome,
    c.Id AS CursoId
FROM 
    Usuario u
JOIN 
    UsuarioCurso uc ON u.UsuarioId = uc.UsuarioId
JOIN 
    Curso c ON uc.CursoId = c.Id
ORDER BY 
    c.Id, u.Nome;