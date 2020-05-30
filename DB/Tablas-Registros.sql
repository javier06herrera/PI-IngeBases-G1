DROP TABLE WRITES;
DROP TABLE REVIEWS;
DROP TABLE Question;
DROP TABLE CommunityMember;
DROP TABLE INVOLVES;
DROP TABLE Article;
DROP TABLE Topic;

CREATE TABLE Article( 
articleId		INT IDENTITY(1,1) PRIMARY KEY,
name			VARCHAR(200) NOT NULL UNIQUE,
type			VARCHAR(5) NOT NULL, --long, short
abstract		VARCHAR(MAX) NOT NULL,
publishDate		DATE NOT NULL,
content			VARCHAR(MAX) NOT NULL,
baseGrade		FLOAT NOT NULL DEFAULT 0.0,
accessCount		INT NOT NULL DEFAULT 0,
likesCount		INT NOT NULL DEFAULT 0, 
dislikesCount	INT NOT NULL DEFAULT 0,
likeBalance		INT NOT NULL DEFAULT 0,
);


insert into Article
values ('Robinson Crusoe','short','Wreck of an english man and survival in a desert island','12-02-1999','C://Daniel/Defoe',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('I, Robot','long','Elaborate philosophic implications concerning the three laws of robotics','12-02-1999','C://Isaac/Asimov',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('Mamita Yunai','long','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Topic(
subject_category  VARCHAR(100),
subject_topicName VARCHAR(100),
PRIMARY KEY(subject_category, subject_topicName)
)

INSERT INTO	Topic
VALUES ('Science Fiction', 'Robotics')

INSERT INTO	Topic
VALUES ('Science Fiction', 'Space')

INSERT INTO	Topic
VALUES ('Novel', 'Survival')

INSERT INTO	Topic
VALUES ('Novel', 'Banana Republic')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE INVOLVES(
articleId INT,
category  VARCHAR(100),
topicName VARCHAR(100),
PRIMARY KEY(articleId, category, topicName),
CONSTRAINT FK_Article_INVOLVES FOREIGN KEY (articleId) REFERENCES Article(articleID),
CONSTRAINT FK_TopicCat_INVOLVES FOREIGN KEY (category, topicName) REFERENCES Topic(subject_category, subject_topicName)
)

INSERT INTO INVOLVES
VALUES	(1,'Novel', 'Survival')

INSERT INTO INVOLVES
VALUES	(2,'Science Fiction', 'Robotics')

INSERT INTO INVOLVES
VALUES	(2,'Science Fiction', 'Space')

INSERT INTO INVOLVES
VALUES	(3,'Novel', 'Banana Republic')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE CommunityMember(
memberId			INT IDENTITY(1,1) PRIMARY KEY,
name				VARCHAR(75) NOT NULL,
lastName			VARCHAR(75) NOT NULL,
address_city		VARCHAR(150) NOT NULL,
address_country		VARCHAR(150) NOT NULL,
hobbies				VARCHAR(300) NOT NULL,
languages			VARCHAR(300) NOT NULL,
email				VARCHAR(100) NOT NULL,
typeOfMember		VARCHAR(100) NOT NULL,
totalQualification	INT	NOT NULL DEFAULT 0,
)

INSERT INTO CommunityMember
VALUES ( 'Kevin', 'Barrantes', 'Rio Segundo','Costa Rica','Read, Listen good music','Spanish, English','barrKev@puchimail.com','Generic',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Gloriana', 'Mora', 'San Sebasti�n','Costa Rica','Read, travel','Spanish, English','moraGlo@puchimail.com','Generic', DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Antonio', '�lvares', 'Barva','Costa Rica','Play video games','Spanish, English','alvAnt@puchimail.com','Generic',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Daniel', 'Barrante', 'Alajuela','Costa Rica','Play the piano, eat caldosas, beign a "bicho"','Spanish, English','barrDan@puchimail.com','Generic',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Javier', 'Herrera', 'Santa Ana','Costa Rica','Play the guitar','Spanish, English','herrJav@puchimail.com','Generic',DEFAULT)

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Question (
questionId  INT           IDENTITY (1, 1) PRIMARY KEY,
askedBy		INT NOT NULL, 
question    VARCHAR (MAX) NOT NULL,
faq		    VARCHAR(10)   NOT NULL DEFAULT 'not posted', -- posted, not posted
answer      VARCHAR (MAX) DEFAULT 'NO ANSWER',
CONSTRAINT FK_CommunityMember_Question FOREIGN KEY (askedBy) REFERENCES CommunityMember(memberId)
);

INSERT INTO	Question
VALUES (5, 'Why are we here?','posted', DEFAULT)

INSERT INTO	Question
VALUES (5, 'What happened to Jimmy Hoffa?',DEFAULT, DEFAULT)

INSERT INTO	Question
VALUES (5, 'Will vaccinations make me autistic?', DEFAULT , 'No')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE WRITES(
memberId	INT,
articleId	INT
PRIMARY KEY(memberId,articleId),
CONSTRAINT FK_CommunityMember_WRITES FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId),
CONSTRAINT FK_Article_WRITES FOREIGN KEY (articleId) REFERENCES Article(articleId)
 )

 INSERT INTO WRITES
 VALUES	(1,1)

 INSERT INTO WRITES
 VALUES	(2,1)

 INSERT INTO WRITES
 VALUES	(3,2)

 INSERT INTO WRITES
 VALUES	(4,2)

INSERT INTO WRITES
 VALUES	(5,3)
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE REVIEWS(
memberId	INT,
articleId	INT,
status		VARCHAR(11) NOT NULL DEFAULT 'not checked',--checked, not checked 
PRIMARY KEY(memberId,articleId),
CONSTRAINT FK_CommunityMember_REVIEW FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId),
CONSTRAINT FK_Article_REVIEW FOREIGN KEY (articleId) REFERENCES Article(articleId)
 )

 INSERT INTO REVIEWS
 VALUES	(3,1,'checked')

 INSERT INTO REVIEWS
 VALUES	(4,1,DEFAULT)

 INSERT INTO REVIEWS
 VALUES	(2,2,DEFAULT)

 INSERT INTO REVIEWS
 VALUES	(1,2,DEFAULT)

INSERT INTO REVIEWS
 VALUES	(1,3,DEFAULT)


-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------
-------------------------------------FIRST ITERATION SCRIPT--------------------------------------------------------------------------------------------

--DROP TABLE Creates;
--DROP TABLE Member;
--DROP TABLE ForeignParticipant;
--DROP TABLE CommunityUser;
--DROP TABLE ArticleTopic;
--DROP TABLE Article;
--DROP TABLE Faq;

--CREATE TABLE Article
--( 
--articleId   INT IDENTITY(1,1) PRIMARY KEY,
--name		VARCHAR(50) NOT NULL UNIQUE,
--type        BIT NOT NULL, --LONG(0) SHORT(1)
--abstract	VARCHAR(MAX) NOT NULL,
--publishDate DATE NOT NULL,
--content		VARCHAR(MAX) NOT NULL,

--);


--insert into Article
--values ('Robinson Crusoe',1,'Naufragio de un ingl�s del siglo 17','12-02-1999','C://Daniel/Defoe');

--insert into Article
--values ('Yo Robot',1,'Problem�ticas filos�ficas en la aplicaci�n de las leyes de la rob�tica','12-02-1999','C://Isaac/Asimov');
---------------------------------------------------------------------------------------------------------------------------------
--CREATE TABLE ArticleTopic
--(
--articleId INT,
--topic VARCHAR(50),
--PRIMARY KEY(articleId, topic),
--CONSTRAINT FK_ArticleTopic_Articles FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE NO ACTION ON UPDATE CASCADE 
--)

--INSERT INTO ArticleTopic
--values (1,'Naufragio')

--INSERT INTO ArticleTopic
--values (1,'Novela Francesa')

--INSERT INTO ArticleTopic
--values (2,'Filosof�a')

--INSERT INTO ArticleTopic
--values (2,'Ciencia Ficci�n')
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
--CREATE TABLE Faq (
--    questionId  INT           IDENTITY (1, 1) NOT NULL,
--    question    VARCHAR (MAX) NOT NULL,
--    status      BIT           NOT NULL,
--    answer      VARCHAR (MAX) NULL,
--    PRIMARY KEY CLUSTERED (questionId ASC)
--);

