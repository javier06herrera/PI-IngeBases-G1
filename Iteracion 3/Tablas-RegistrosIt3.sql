DROP TABLE IS_NOMINATED;
DROP TABLE HAS_SKILL;
DROP TABLE Skill;
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
baseGrade		INT NOT NULL DEFAULT 0,
accessCount		INT NOT NULL DEFAULT 0,
likesCount		INT NOT NULL DEFAULT 0,
neutralCount	INT NOT NULL DEFAULT 0,
dislikesCount	INT NOT NULL DEFAULT 0,
likeBalance		INT NOT NULL DEFAULT 0,
checkedStatus	VARCHAR(50) NOT NULL DEFAULT 'on edition' 
											--on edition: El autor está en proceso de edición
											--pending collaboration: El autor envía el artículo y el autor no ha solicitado colaboración
											--pending assignation: El artículo está en proceso de assignación de revisores
											--not checked: Faltan miembros revisores de concluir, 
											--checked: Todos los revisores concluyeron su revisión
											--published: Fue Aceptado por el coordinador
);



insert into Article
values ('Robinson Crusoe','short','Wreck of an english man and survival in a desert island','12-02-1999','Nací en 1632, en la ciudad de York, de una buena familia, aunque no de la región, pues mi padre era un extranjero de Brema1 que, inicialmente, se asentó en Hull2. Allí consiguió hacerse con una considerable fortuna como comerciante y, más tarde, abandonó sus negocios y se fue a vivir a York, donde se casó con mi madre, que pertenecía a la familia Robinson, una de las buenas familias del condado de la cual obtuve mi nombre, Robinson Kreutznaer. Mas, por la habitual alteración de las palabras que se hace en Inglaterra, ahora nos llaman y nosotros también nos llamamos y escribimos nuestro nombre Crusoe; y así me han llamado siempre mis compañeros.   Tenía dos hermanos mayores, uno de ellos fue coronel de un regimiento de infantería inglesa en Flandes, que antes había estado bajo el mando del célebre coronel Lockhart, y murió en la batalla de Dunkerque3 contra los españoles.    Lo que fue de mi segundo hermano, nunca lo he sabido al igual que mi padre y mi madre tampoco supieron lo que fue de mí.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('I, Robot','long','Elaborate philosophic implications concerning the three laws of robotics','12-02-1999','Un  robot  no  debe  dañar  a  un  ser  humano  o,  por  su  inacción,dejar que un ser humano sufra daño.2.  Un  robot  debe  obedecer  las  órde  nes  que  le  son  dadas  por  unser  hu  mano,  excepto  cuando  estas  órdenes  están  en  oposicióncon la primera Ley.3.  Un  robot  debe  proteger  su  propia  existencia,  hasta  donde  estaprotec ción no esté en conflicto con la primera o segunda Leyes.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('Mamita Yunai','long','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('Morgan Salgari','long','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');


---------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Topic(
subjectCategory  VARCHAR(100),
subjectTopicName VARCHAR(100),
PRIMARY KEY(subjectCategory, subjectTopicName)
)

INSERT INTO	Topic
VALUES ('Science Fiction', 'Robotics')

INSERT INTO	Topic
VALUES ('Science Fiction', 'Space')

INSERT INTO	Topic
VALUES ('Novel', 'Survival')

INSERT INTO	Topic
VALUES ('Novel', 'Banana Republic')

INSERT INTO	Topic
VALUES ('Novel', 'Pirate Island')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE INVOLVES(
articleId INT,
category  VARCHAR(100),
topicName VARCHAR(100),
PRIMARY KEY(articleId, category, topicName),
CONSTRAINT FK_Article_INVOLVES FOREIGN KEY (articleId) REFERENCES Article(articleID) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_TopicCat_INVOLVES FOREIGN KEY (category, topicName) REFERENCES Topic(subjectCategory, subjectTopicName) ON DELETE NO ACTION ON UPDATE CASCADE
)

INSERT INTO INVOLVES
VALUES	(1,'Novel', 'Survival')

INSERT INTO INVOLVES
VALUES	(2,'Science Fiction', 'Robotics')

INSERT INTO INVOLVES
VALUES	(2,'Science Fiction', 'Space')

INSERT INTO INVOLVES
VALUES	(3,'Novel', 'Banana Republic')

INSERT INTO INVOLVES
VALUES	(4,'Novel', 'Pirate Island')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE CommunityMember(
email				VARCHAR(100) PRIMARY KEY,
name				VARCHAR(75) NOT NULL,
lastName			VARCHAR(75) NOT NULL,
birthDate			DATE			 	,
age					INT					,	
addressCity			VARCHAR(150) NOT NULL,
addressCountry		VARCHAR(150) NOT NULL,
hobbies				VARCHAR(300) 		,
languages			VARCHAR(300) 		,
mobile				VARCHAR(15) NOT NULL UNIQUE,
job					VARCHAR(MAX) NOT NULL,
memberRank			VARCHAR(100) 		,
points				INT	NOT NULL DEFAULT 0,
skills				VARCHAR(MAX) NOT NULL,
password            NVARCHAR(MAX) DEFAULT 123			
)

INSERT INTO CommunityMember
VALUES ( 'barrKev@puchimail.com','Kevin', 'Barrantes','10-10-1990', 30, 'Rio Segundo','Costa Rica','Read, Listen good music','Spanish, English','1','Intel','core',5,'Versatility',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'moraGlo@puchimail.com','Gloriana', 'Mora','10-10-1990', 30,'San Sebastián','Costa Rica','Read, travel','Spanish, English','2','Intel','core', 5, 'Creativity',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'alvAnt@puchimail.com','Antonio', 'Álvares', '10-10-1990',30,'Barva','Costa Rica','Play video games','Spanish, English','3','Intel','active',3,'Math-skills',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'barrDan@puchimail.com','Daniel', 'Barrante','10-10-1990', 30,'Alajuela','Costa Rica','Play the piano, eat caldosas, beign a "bicho"','Spanish, English','4','Intel','active',3,'Adaptability',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'herrJav@puchimail.com','Javier', 'Herrera','10-10-1990', 30, 'Santa Ana','Costa Rica','Play the guitar','Spanish, English','5','Intel','peripheral',0,'Organized',DEFAULT)

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Skill(
subjectCategory  VARCHAR(100),
subjectSkillName VARCHAR(100),
PRIMARY KEY(subjectCategory, subjectSkillName)
)

INSERT INTO Skill
VALUES ('Musica', 'Tocar piano')

INSERT INTO Skill
VALUES ('Diseño', 'Adobe')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE HAS_SKILL(
email VARCHAR(100),
category  VARCHAR(100),
skillName VARCHAR(100),
PRIMARY KEY(email, category, skillName),
CONSTRAINT FK_Article_HAS_SKILL FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_SkillCat_HAS_SKILL FOREIGN KEY (category, skillName) REFERENCES Skill(subjectCategory, subjectSkillName) ON DELETE CASCADE ON UPDATE CASCADE
)

INSERT INTO HAS_SKILL
VALUES ('barrKev@puchimail.com', 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES ('moraGlo@puchimail.com', 'Diseño', 'Adobe')

INSERT INTO HAS_SKILL
VALUES ('alvAnt@puchimail.com', 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES ('herrJav@puchimail.com', 'Diseño', 'Adobe')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Question (
email		VARCHAR(100),
questionId  INT           IDENTITY (1, 1) PRIMARY KEY,
question    VARCHAR (MAX) NOT NULL,
faq		    VARCHAR(10)   NOT NULL DEFAULT 'not posted', -- posted, not posted
answer      VARCHAR (MAX) DEFAULT 'no answer',
status		VARCHAR (15)  NOT NULL DEFAULT 'not checked', --checked, not checked
CONSTRAINT FK_CommunityMember_Question FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE NO ACTION ON UPDATE CASCADE,
);

INSERT INTO	Question
VALUES ('barrKev@puchimail.com', 'How to exit VIM?','posted', 'If you are in edit mode, first press the <Esc> key. Then enter :wq + <Enter> to save and exit. And by the way this does not work', DEFAULT)

INSERT INTO	Question
VALUES ('moraGlo@puchimail.com','How to create an article?',DEFAULT, DEFAULT, DEFAULT)

INSERT INTO	Question
VALUES ('alvAnt@puchimail.com', 'Will vaccinations make me autistic?', DEFAULT , 'No', 'checked')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE WRITES(
email	VARCHAR(100),
articleId	INT
PRIMARY KEY(email,articleId),
CONSTRAINT FK_CommunityMember_WRITES FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE NO ACTION ON UPDATE CASCADE,
CONSTRAINT FK_Article_WRITES FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE
 )

 INSERT INTO WRITES
 VALUES	('barrKev@puchimail.com',1)

 INSERT INTO WRITES
 VALUES	('barrKev@puchimail.com',2)

 INSERT INTO WRITES
 VALUES	('moraGlo@puchimail.com',1)

 INSERT INTO WRITES
 VALUES	('alvAnt@puchimail.com',2)

 INSERT INTO WRITES
 VALUES	('barrDan@puchimail.com',2)

INSERT INTO WRITES
 VALUES	('herrJav@puchimail.com',3)

 INSERT INTO WRITES
 VALUES	('barrKev@puchimail.com',4)
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE REVIEWS(
articleId	INT,
email	VARCHAR(100),
comments	VARCHAR(MAX) NOT NULL,
generalOpinion INT NOT NULL DEFAULT 0,
communityContribution INT NOT NULL DEFAULT 0,
articleStructure INT NOT NULL DEFAULT 0,
totalGrade INT NOT NULL DEFAULT 0,
state		VARCHAR(15) NOT NULL DEFAULT 'not reviewed',--reviewed, not reviewed
PRIMARY KEY(email,articleId),
CONSTRAINT FK_CommunityMember_REVIEW FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_Article_REVIEW FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE
 )

INSERT INTO REVIEWS
 VALUES	(1,'alvAnt@puchimail.com','Sin comentarios',5,5,5,5,'reviewed')

INSERT INTO REVIEWS
 VALUES	(1,'barrKev@puchimail.com','Sin comentarios',5,5,5,5,DEFAULT)

INSERT INTO REVIEWS
 VALUES	(2,'moraGlo@puchimail.com','Sin comentarios',5,5,5,5,DEFAULT)

INSERT INTO REVIEWS
 VALUES	(2,'barrDan@puchimail.com','Sin comentarios',5,5,5,5,DEFAULT)

INSERT INTO REVIEWS
 VALUES	(3,'moraGlo@puchimail.com','Sin comentarios',5,5,5,5,DEFAULT)

INSERT INTO CommunityMember VALUES
('ant.alvarez.chavarria@hotmail.es','coordinator','cordino','1990-10-10',50,'Jacó','Costa Rica',
'Coordinar','Java',24,'Coordinar', 'coordinator', 5,'coodinar',DEFAULT);

CREATE TABLE IS_NOMINATED(
answer VARCHAR(100) DEFAULT 'pending',
comments VARCHAR(100)  DEFAULT 'no aplica',
email VARCHAR(100),
articleId	INT,
PRIMARY KEY(email,ArticleId),
CONSTRAINT FK_CommunityMember_IS_NOMINATED FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_Article_IS_NOMINATED FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE
 )
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
--values ('Robinson Crusoe',1,'Naufragio de un inglés del siglo 17','12-02-1999','C://Daniel/Defoe');

--insert into Article
--values ('Yo Robot',1,'Problemáticas filosóficas en la aplicación de las leyes de la robótica','12-02-1999','C://Isaac/Asimov');
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
--values (2,'Filosofía')

--INSERT INTO ArticleTopic
--values (2,'Ciencia Ficción')
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

