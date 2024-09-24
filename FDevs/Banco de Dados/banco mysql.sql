-- "Conexao": "Server=localhost\\SQLEXPRESS;Database=FDEVS;Trusted_Connection=True;TrustServerCertificate=True;"

CREATE DATABASE FDEVS;

USE FDEVS;

CREATE TABLE Status (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Cor VARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    UsuarioId VARCHAR(400) NOT NULL PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    DataNascimento DATETIME NOT NULL,
    Foto VARCHAR(500) NOT NULL
);

CREATE TABLE Trilha (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL
);

CREATE TABLE Curso (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL,
    DataConclusao DATE NULL,
    TrilhaId INT NOT NULL,
    StatusId INT NOT NULL,
    FOREIGN KEY (StatusId) REFERENCES Status(Id),
    FOREIGN KEY (TrilhaId) REFERENCES Trilha(Id)
);

CREATE TABLE Prova (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    CursoId INT NOT NULL,
    UsuarioId VARCHAR(400) NOT NULL,
    FOREIGN KEY (CursoId) REFERENCES Curso(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId)
);

CREATE TABLE Questao (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Texto VARCHAR(500) NOT NULL,
    ProvaId INT NOT NULL,
    FOREIGN KEY (ProvaId) REFERENCES Prova(Id)
);

CREATE TABLE Alternativa (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Texto VARCHAR(200) NOT NULL,
    Correta TINYINT(1) NOT NULL,
    QuestaoId INT NOT NULL,
    FOREIGN KEY (QuestaoId) REFERENCES Questao(Id)
);

CREATE TABLE Resposta (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UsuarioId VARCHAR(400) NOT NULL,
    QuestaoId INT NOT NULL,
    AlternativaId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (QuestaoId) REFERENCES Questao(Id),
    FOREIGN KEY (AlternativaId) REFERENCES Alternativa(Id)
);

CREATE TABLE Modulo (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    StatusId INT NOT NULL,
    UsuarioId VARCHAR(400) NOT NULL,
    CursoId INT NOT NULL,
    FOREIGN KEY (StatusId) REFERENCES Status(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (CursoId) REFERENCES Curso(Id)
);

CREATE TABLE Video (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(50) NOT NULL,
    URL VARCHAR(500) NOT NULL,
    ModuloId INT NOT NULL,
    FOREIGN KEY (ModuloId) REFERENCES Modulo(Id)
);

CREATE TABLE UsuarioCurso (
    UsuarioId VARCHAR(400) NOT NULL,
    CursoId INT NOT NULL,
    PRIMARY KEY (UsuarioId, CursoId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (CursoId) REFERENCES Curso(Id)
);

-- Inserting initial data
INSERT INTO Status (Nome, Cor) VALUES
('Em andamento', 'rgba(255, 255, 0, 1)'),
('Concluído', 'rgba(0, 255, 0, 1)'),
('Cancelado', 'rgba(255, 0, 0, 1)');

INSERT INTO Usuario (UsuarioId, Nome, DataNascimento, Foto) VALUES
('user_001', 'Alice', '1995-11-05', 'foto_alice.jpg'),
('user_002', 'Bob', '1995-09-09', 'foto_bob.jpg'),
('user_003', 'Carlos', '2000-10-01', 'foto_carlos.jpg');

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
('Prova de Banco de Dados', 1, 'user_001'),
('Prova de Programação em Python', 2, 'user_002');

INSERT INTO Modulo (Nome, StatusId, UsuarioId, CursoId) VALUES
('Módulo 1: Introdução ao Banco de Dados', 1, 'user_001', 1),
('Módulo 2: Modelagem de Dados', 1, 'user_001', 1),
('Módulo 1: Introdução ao Python', 1, 'user_002', 2);

INSERT INTO Video (Titulo, URL, ModuloId) VALUES
('Video 1: O que é Banco de Dados?', 'https://example.com/video1', 1),
('Video 2: Modelagem de Dados 101', 'https://example.com/video2', 2),
('Video 1: Introdução ao Python', 'https://example.com/video3', 3);

INSERT INTO Questao (Texto, ProvaId) VALUES
('O que é um Banco de Dados?', 1),
('Qual a diferença entre SQL e NoSQL?', 1),
('O que é uma função em Python?', 2);

INSERT INTO Alternativa (Texto, Correta, QuestaoId) VALUES
('Um sistema que armazena dados', 1, 1),
('Uma ferramenta de programação', 0, 1),
('É uma linguagem de consulta', 1, 2),
('É uma linguagem de programação', 0, 2),
('Uma sequência de comandos', 1, 3),
('Uma estrutura de dados', 0, 3);

INSERT INTO Resposta (UsuarioId, QuestaoId, AlternativaId) VALUES
('user_001', 1, 1),
('user_001', 2, 3),
('user_002', 2, 5);

INSERT INTO UsuarioCurso (UsuarioId, CursoId) VALUES
('user_001', 1),
('user_002', 2),
('user_001', 3);

-- Selecting user data with courses
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
