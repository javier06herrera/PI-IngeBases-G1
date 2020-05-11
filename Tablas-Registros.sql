--DROP TABLE Creates;
--DROP TABLE Member;
--DROP TABLE ForeignParticipant;
--DROP TABLE CommunityUser;
DROP TABLE ArticleTopic;
DROP TABLE Article;
DROP TABLE Faq;

CREATE TABLE Article
( 
articleId   INT IDENTITY(1,1) PRIMARY KEY,
name		VARCHAR(50) NOT NULL UNIQUE,
type        BIT NOT NULL, --LONG(0) SHORT(1)
abstract	VARCHAR(MAX) NOT NULL,
publishDate DATE NOT NULL,
content		VARCHAR(MAX) NOT NULL,

);


insert into Article
values ('Robinson Crusoe',1,'Naufragio de un inglés del siglo 17','12-02-1999','C://Daniel/Defoe');

insert into Article
values ('Yo Robot',1,'Problemáticas filosóficas en la aplicación de las leyes de la robótica','12-02-1999','C://Isaac/Asimov');
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE ArticleTopic
(
articleId INT,
topic VARCHAR(50),
PRIMARY KEY(articleId, topic),
CONSTRAINT FK_ArticleTopic_Articles FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE NO ACTION ON UPDATE CASCADE 
)

INSERT INTO ArticleTopic
values (1,'Naufragio')

INSERT INTO ArticleTopic
values (1,'Novela Francesa')

INSERT INTO ArticleTopic
values (2,'Filosofía')

INSERT INTO ArticleTopic
values (2,'Ciencia Ficción')
-------------------------------------------------------------------------------------------------------------------
--CREATE TABLE CommunityUser
--(
--userId		INT  IDENTITY PRIMARY KEY,
--name		VARCHAR(50) NOT NULL,
--lastName	VARCHAR(100) NOT NULL,
--userState   INT NOT NULL DEFAULT(0)
--);

--insert into CommunityUser
--values ('Daniel', 'Barrantes',1);

--insert into CommunityUser
--values ('Antonio', 'Alvarez',1);
--------------------------------------------------------------------------------------------------------------------------
--CREATE TABLE ForeignParticipant
--(
--userId int PRIMARY KEY,
--participantAttributes	VARCHAR(MAX) NOT NULL,
--CONSTRAINT FK_ForeignParticipant_Users FOREIGN KEY (userId) REFERENCES CommunityUser(userId) ON DELETE NO ACTION ON UPDATE CASCADE 
--);

--insert into ForeignParticipant
--values (1, 'Datos_de_Participante');

--insert into ForeignParticipant
--values (2, 'Datos_de_Participante');

----------------------------------------------------------------------------------------------------------------------------
--CREATE TABLE Member
--(
--userId int PRIMARY KEY,
--memberRank				INT  NOT NULL, --1:Coordinador 2:Nucleo 3:Activo
--memberAttributes		VARCHAR(MAX) NOT NULL,
--CONSTRAINT FK_Member_Users FOREIGN KEY (userId) REFERENCES CommunityUser(userId) ON DELETE NO ACTION ON UPDATE CASCADE
--);

--insert into Member
--values (1, 1 , 'Datos_de_Participante');

--insert into Member
--values (2, 2, 'Datos_de_Participante');

--------------------------------------------------------------------------------------------------------------------------------
--CREATE TABLE Creates
--(
--articleId int NOT NULL,
--userId int NOT NULL,
--PRIMARY KEY(articleId,userId),
--CONSTRAINT FK_Creates_Articles FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE NO ACTION ON UPDATE CASCADE,
--CONSTRAINT FK_Creates_Member FOREIGN KEY (userId) REFERENCES Member(userId) ON DELETE NO ACTION ON UPDATE CASCADE
--);

--insert into Creates
--values (1,1);
--insert into Creates
--values (2,2);
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Faq (
    questionId  INT           IDENTITY (1, 1) NOT NULL,
    question    VARCHAR (MAX) NOT NULL,
    status      BIT           NOT NULL,
    answer      VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED (questionId ASC)
);
