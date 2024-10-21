--Esse banco foi criado quando o AppDbSeed e as migrações ainda não existiam
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
('Banco de Dados', 'https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/19.png', NULL, 1);

INSERT INTO Prova (Nome, CursoId) VALUES
('Prova de Lógica da Programação', 1),  -- Alice
('Prova de Banco de Dados', 2);  -- Bob

INSERT INTO Modulo (Nome, CursoId) VALUES
('Módulo 1 - Iniciante',  1),
('Módulo 2 - Intermediário', 1),
('Módulo 1 - Iniciante', 2),
('Módulo 2 - Intermediário', 2),
('Módulo 3 - Extras', 2);

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
('O que é uma função na programação?', 1),  -- Prova de Lógica da programação
('Qual a diferença entre um loop while e um loop for?', 1),  -- Prova de Lógica da programação
('O que é um banco de dados?', 2);  -- Prova de Banco de dados

INSERT INTO Alternativa (Texto, Correta, QuestaoId) VALUES
('É um meio de armazenar dados', 1, 1),  -- Questão 1
('É um bloco de código que pode ser chamado várias vezes', 0, 1),  -- Questão 1
('Um é repetido infinitamente, e o outro até que um valor seja verdadeiro', 0, 2),  -- Questão 2
('O loop while se repete até que um valor seja verdadeiro, e o loop for até que a iteração seja concluída uma certa quantidade de vezes.', 1, 2),  -- Questão 2
('É uma sequência de comandos para criar uma lógica', 0, 3),  -- Questão 3
('É uma estrutura de armazenamento de dados', 1, 3);  -- Questão 3

INSERT INTO Resposta (UsuarioId, QuestaoId, AlternativaId) VALUES
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 1, 1),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 3),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 3, 5); 

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
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2, 2);

INSERT INTO UsuarioCurso (UsuarioId, CursoId) VALUES
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 1),
('ddf093a6-6cb5-4ff7-9a64-83da34aee005', 2);
