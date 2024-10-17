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
    ModuloId INT NOT NULL,
	StatusId INT NOT NULL
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
ALTER TABLE Video ADD CONSTRAINT FK_Video_Status FOREIGN KEY (StatusId) REFERENCES Status(Id);
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
('Não iniciado', 'rgba(255, 0, 0, 1)');         -- Vermelho

INSERT INTO Usuario (UsuarioId, Nome, DataNascimento, Foto) VALUES
('user_001', 'Alice', '05-11-1995', 'foto_alice.jpg'),
('user_002', 'Bob', '09-09-1995', 'foto_bob.jpg'),
('user_003', 'Carlos', '01-10-2000', 'foto_carlos.jpg');

INSERT INTO Trilha (Nome, Foto) VALUES
('Trilha de Backend', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/CapasTrilha/1.png');

INSERT INTO Curso (Nome, Foto, DataConclusao, TrilhaId, StatusId) VALUES
('Lógica de programação', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/18.png', NULL, 1, 1),
('Banco de Dados', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/19.png', NULL, 1, 2),
('C# Iniciante', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/22.png', NULL, 1, 2),
('ASP.Net MVC - C#', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/23.png', NULL, 1, 2),
('Git e Github (Trilha Backend)', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/45.png', NULL, 1, 2),
('Princípios SOLID', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/48.png', NULL, 1, 2);


INSERT INTO Prova (Nome, CursoId, UsuarioId) VALUES
('Prova de Banco de Dados', 1, 'user_001'),  -- Alice
('Prova de Programação em Python', 2, 'user_002');  -- Bob

INSERT INTO Modulo (Nome, StatusId, UsuarioId, CursoId) VALUES
('Módulo 1 - Iniciante', 3, 'user_001', 1),  -- Alice
('Módulo 2 - Pesquisa de satisfação', 3, 'user_001', 1),  -- Alice
('Módulo 1 - Iniciante', 2, 'user_001', 2),
('Módulo 2 - Intermediário', 2, 'user_001', 2),
('Módulo 3 - Extras', 2, 'user_001', 2);  -- Bob

INSERT INTO Video (Titulo, URL, ModuloId, StatusId) VALUES
('Introdução a Algoritmos', 'https://www.youtube.com/embed/8mei6uVttho?si=gn2VgTONcmRet24o', 1, 2),
('Primeiro algoritmo', 'https://www.youtube.com/embed/M2Af7gkbbro?si=yx5Yy6dgQYy_1Y8f', 1, 3),
('Comando de Entrada e Operadores', 'https://www.youtube.com/embed/RDrfZ-7WE8c?si=JP0LvntY7_cxuWUB', 1, 3),
('Operadores lógicos e relacionais', 'https://www.youtube.com/embed/Ig4QZNpVZYs?si=Eaes88_HwJc28Vp2', 1, 3),
('Introdução ao Scratch', 'https://www.youtube.com/embed/GrPkuk1ezyo?si=QoDgOp2ZVSgM_CTM', 1, 3),
('Exercícios de Algoritmo', 'https://www.youtube.com/embed/v2nCgGSVCeE?si=_-lFdQVYxv_1uJVB', 1, 3),
('Estruturas Condicionais 1', 'https://www.youtube.com/embed/_g05aHdBAEY?si=YHLhKkoo8Cnaieub', 1, 2),
('SQL Server - Instalando no seu computador', 'https://www.youtube.com/embed/OKqpZ6zbZwQ?si=PR8tj46glLT1VUyD', 3, 2),
('Orientações', 'https://www.youtube.com/embed/qEitmEuXG1I?si=71gXL6ykXdoTHoxk', 3, 2),
('Conceitos Essenciais e Modelagem', 'https://www.youtube.com/embed/N_0ujgVRrdI?si=kmYxFk0v6jv0SXSc', 3, 2),
('Relacionamento entre tabelas ', 'https://www.youtube.com/embed/HmFUrlQcCJ0?si=-E4k0khkUdH9ABS3', 4, 2);

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
('user_001', 3),  -- Alice está inscrita no Curso de JavaScript
('user_001', 4),  -- Alice está inscrita no Curso de Banco de Dados
('user_002', 5),  -- Bob está inscrita no Curso de Programação em Python
('user_001', 6);  -- Alice está inscrita no Curso de JavaScript

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


CREATE DATABASE FDEVS;

USE FDEVS;

CREATE TABLE Curso (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Foto VARCHAR(300) NOT NULL,
    DataConclusao DATE NULL,
    TrilhaId INT NOT NULL,
    EstadoId INT NOT NULL
);

CREATE TABLE Questao (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Texto VARCHAR(500) NOT NULL,
    ProvaId INT NOT NULL
);

CREATE TABLE Prova (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(60) NOT NULL,
    CursoId INT NOT NULL
);

CREATE TABLE UsuarioProva (
    UsuarioId VARCHAR(400) NOT NULL,
    ProvaId INT NOT NULL,
    DataRealizacao DATE NULL,
    PRIMARY KEY (UsuarioId, ProvaId),

);

CREATE TABLE Resposta (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    UsuarioId VARCHAR(400) NOT NULL,
    QuestaoId INT NOT NULL,
    AlternativaId INT NOT NULL
);

CREATE TABLE Estado (
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
    Foto VARCHAR(500) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    IsAdmin BIT NOT NULL DEFAULT 0
);

CREATE TABLE Video (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Titulo VARCHAR(50) NOT NULL,
    URL VARCHAR(500) NOT NULL,
    ModuloId INT NOT NULL,
    EstadoId INT NOT NULL
);

CREATE TABLE UsuarioCurso (
    UsuarioId VARCHAR(400) NOT NULL,
    CursoId INT NOT NULL,
    PRIMARY KEY (UsuarioId, CursoId)
);

CREATE TABLE Modulo (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    EstadoId INT NOT NULL,
    UsuarioId VARCHAR(400) NOT NULL,
    CursoId INT NOT NULL
);

CREATE TABLE Alternativa (
    Id INT NOT NULL IDENTITY PRIMARY KEY,
    Texto VARCHAR(200) NOT NULL,
    Correta BIT NOT NULL,
    QuestaoId INT NOT NULL
);

ALTER TABLE Curso ADD CONSTRAINT FK_Curso_Estado FOREIGN KEY (EstadoId) REFERENCES Estado(Id);
ALTER TABLE Curso ADD CONSTRAINT FK_Curso_Trilha FOREIGN KEY (TrilhaId) REFERENCES Trilha(Id);
ALTER TABLE Resposta ADD CONSTRAINT FK_Resposta_Questao FOREIGN KEY (QuestaoId) REFERENCES Questao(Id);
ALTER TABLE Resposta ADD CONSTRAINT FK_Resposta_Alternativa FOREIGN KEY (AlternativaId) REFERENCES Alternativa(Id);
ALTER TABLE UsuarioCurso ADD CONSTRAINT FK_UsuarioCurso_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);
ALTER TABLE UsuarioCurso ADD CONSTRAINT FK_UsuarioCurso_Curso FOREIGN KEY (CursoId) REFERENCES Curso(Id);
ALTER TABLE Modulo ADD CONSTRAINT FK_Modulo_Estado FOREIGN KEY (EstadoId) REFERENCES Estado(Id);
ALTER TABLE Modulo ADD CONSTRAINT FK_Modulo_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);
ALTER TABLE Modulo ADD CONSTRAINT FK_Modulo_Curso FOREIGN KEY (CursoId) REFERENCES Curso(Id);
ALTER TABLE Video ADD CONSTRAINT FK_Video_Modulo FOREIGN KEY (ModuloId) REFERENCES Modulo(Id);
ALTER TABLE Video ADD CONSTRAINT FK_Video_Estado FOREIGN KEY (EstadoId) REFERENCES Estado(Id);
ALTER TABLE Alternativa ADD CONSTRAINT FK_Alternativa_Questao FOREIGN KEY (QuestaoId) REFERENCES Questao(Id);
ALTER TABLE Questao ADD CONSTRAINT FK_Questao_Prova FOREIGN KEY (ProvaId) REFERENCES Prova(Id);
ALTER TABLE Prova ADD CONSTRAINT FK_Prova_Curso FOREIGN KEY (CursoId) REFERENCES Curso(Id);
ALTER TABLE UsuarioProva ADD CONSTRAINT FK_UsuarioProva_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);
ALTER TABLE UsuarioProva ADD CONSTRAINT FK_UsuarioProva_Prova FOREIGN KEY (ProvaId) REFERENCES Prova(Id);
ALTER TABLE Resposta ADD CONSTRAINT FK_Resposta_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId);

INSERT INTO Estado(Nome, Cor) VALUES
('Em andamento', 'rgba(255, 255, 0, 1)'),  -- Amarelo 
('Concluído', 'rgba(0, 255, 0, 1)'),         -- Verde
('Não iniciado', 'rgba(255, 0, 0, 1)');         -- Vermelho

INSERT INTO Trilha (Nome, Foto) VALUES
('Trilha de Backend', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/CapasTrilha/1.png');

INSERT INTO Curso (Nome, Foto, DataConclusao, TrilhaId) VALUES 
('Lógica de programação', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/18.png', NULL, 1),
('Banco de Dados', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/19.png', NULL, 1),
('C# Iniciante', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/22.png', NULL, 1),
('ASP.Net MVC - C#', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/23.png', NULL, 1),
('Git e Github (Trilha Backend)', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/45.png', NULL, 1),
('Princípios SOLID', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/48.png', NULL, 1);

INSERT INTO Prova (Nome, CursoId) VALUES
('Prova de Banco de Dados', 1),  -- Alice
('Prova de Programação em Python', 2);  -- Bob

INSERT INTO Modulo (Nome, CursoId) VALUES
('Módulo 1 - Iniciante',  1),
('Módulo 2 - Pesquisa de satisfação', 1),
('Módulo 1 - Iniciante', 2),
('Módulo 2 - Intermediário', 2),
('Módulo 3 - Extras', 2);


select*from Usuario;

INSERT INTO Video (Titulo, URL, ModuloId) VALUES 
('Introdução a Algoritmos', 'https://www.youtube.com/embed/8mei6uVttho?si=gn2VgTONcmRet24o', 1),
('Primeiro algoritmo', 'https://www.youtube.com/embed/M2Af7gkbbro?si=yx5Yy6dgQYy_1Y8f', 1),
('Comando de Entrada e Operadores', 'https://www.youtube.com/embed/RDrfZ-7WE8c?si=JP0LvntY7_cxuWUB', 1),
('Operadores lógicos e relacionais', 'https://www.youtube.com/embed/Ig4QZNpVZYs?si=Eaes88_HwJc28Vp2', 1),
('Introdução ao Scratch', 'https://www.youtube.com/embed/GrPkuk1ezyo?si=QoDgOp2ZVSgM_CTM', 1),
('Exercícios de Algoritmo', 'https://www.youtube.com/embed/v2nCgGSVCeE?si=_-lFdQVYxv_1uJVB', 1),
('Estruturas Condicionais 1', 'https://www.youtube.com/embed/_g05aHdBAEY?si=YHLhKkoo8Cnaieub', 1),
('SQL Server - Instalando no seu computador', 'https://www.youtube.com/embed/OKqpZ6zbZwQ?si=PR8tj46glLT1VUyD', 3),
('Orientações', 'https://www.youtube.com/embed/qEitmEuXG1I?si=71gXL6ykXdoTHoxk', 3),
('Conceitos Essenciais e Modelagem', 'https://www.youtube.com/embed/N_0ujgVRrdI?si=kmYxFk0v6jv0SXSc', 3),
('Relacionamento entre tabelas ', 'https://www.youtube.com/embed/HmFUrlQcCJ0?si=-E4k0khkUdH9ABS3', 4);

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
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 1, 1),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 3),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 5); 

INSERT INTO UsuarioEstadoVideo (UsuarioId, EstadoId, VideoId) VALUES
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 1), 
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 2), 
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 3),  
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 4), 
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 5),  
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 6), 
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 7),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 8),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 9),  
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 10), 
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 11); 

INSERT INTO UsuarioEstadoModulo (UsuarioId, ModuloId, EstadoId) VALUES
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 1, 2),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 3),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 2),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 4, 2),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 5, 2); 

INSERT INTO UsuarioEstadoCurso (UsuarioId, EstadoId, CursoId) VALUES 
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 1, 1),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 2),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 3),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 4),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 5),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 6);

INSERT INTO UsuarioCurso (UsuarioId, CursoId) VALUES
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 1),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 4),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 5),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 6);




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

