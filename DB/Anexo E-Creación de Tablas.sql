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
dislikesCount	INT NOT NULL DEFAULT 0,
likeBalance		INT NOT NULL DEFAULT 0,

);



insert into Article
values ('Robinson Crusoe','short','Wreck of an english man and survival in a desert island','12-02-1999','Nac� en 1632, en la ciudad de York, de una buena familia, aunque no de la regi�n, pues mi padre era un extranjero de Brema1 que, inicialmente, se asent� en Hull2. All� consigui� hacerse con una considerable fortuna como comerciante y, m�s tarde, abandon� sus negocios y se fue a vivir a York, donde se cas� con mi madre, que pertenec�a a la familia Robinson, una de las buenas familias del condado de la cual obtuve mi nombre, Robinson Kreutznaer. Mas, por la habitual alteraci�n de las palabras que se hace en Inglaterra, ahora nos llaman y nosotros tambi�n nos llamamos y escribimos nuestro nombre Crusoe; y as� me han llamado siempre mis compa�eros.   Ten�a dos hermanos mayores, uno de ellos fue coronel de un regimiento de infanter�a inglesa en Flandes, que antes hab�a estado bajo el mando del c�lebre coronel Lockhart, y muri� en la batalla de Dunkerque3 contra los espa�oles.    Lo que fue de mi segundo hermano, nunca lo he sabido al igual que mi padre y mi madre tampoco supieron lo que fue de m�.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('I, Robot','long','Elaborate philosophic implications concerning the three laws of robotics','12-02-1999','Un  robot  no  debe  da�ar  a  un  ser  humano  o,  por  su  inacci�n,dejar que un ser humano sufra da�o.2.  Un  robot  debe  obedecer  las  �rde  nes  que  le  son  dadas  por  unser  hu  mano,  excepto  cuando  estas  �rdenes  est�n  en  oposici�ncon la primera Ley.3.  Un  robot  debe  proteger  su  propia  existencia,  hasta  donde  estaprotec ci�n no est� en conflicto con la primera o segunda Leyes.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('Mamita Yunai','long','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

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

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE CommunityMember(
memberId			INT IDENTITY(1,1) PRIMARY KEY,
name				VARCHAR(75) NOT NULL,
lastName			VARCHAR(75) NOT NULL,
birthDate			DATE			 	,
age					INT					,	
addressCity			VARCHAR(150) NOT NULL,
addressCountry		VARCHAR(150) NOT NULL,
hobbies				VARCHAR(300) 		,
languages			VARCHAR(300) 		,
email				VARCHAR(100) NOT NULL,
mobile				VARCHAR(15) NOT NULL UNIQUE,
job					VARCHAR(MAX) NOT NULL,
memberRank			VARCHAR(100) DEFAULT 'peripheral',--peripheral, active, core, coordinator
points				INT	NOT NULL DEFAULT 0,
skills				VARCHAR(MAX) NOT NULL
)

INSERT INTO CommunityMember
VALUES ( 'Kevin', 'Barrantes','10-10-1990', 30, 'Rio Segundo','Costa Rica','Read, Listen good music','Spanish, English','barrKev@puchimail.com','1','Intel','coordinator',DEFAULT,'Versatility')

INSERT INTO CommunityMember
VALUES ( 'Gloriana', 'Mora','10-10-1990', 30,'San Sebasti�n','Costa Rica','Read, travel','Spanish, English','moraGlo@puchimail.com','2','Intel','coordinator', DEFAULT, 'Creativity')

INSERT INTO CommunityMember
VALUES ( 'Antonio', '�lvares', '10-10-1990',30,'Barva','Costa Rica','Play video games','Spanish, English','alvAnt@puchimail.com','3','Intel','core',DEFAULT,'Math-skills')

INSERT INTO CommunityMember
VALUES ( 'Daniel', 'Barrante','10-10-1990', 30,'Alajuela','Costa Rica','Play the piano, eat caldosas, beign a "bicho"','Spanish, English','barrDan@puchimail.com','4','Intel','core',DEFAULT,'Adaptability')

INSERT INTO CommunityMember
VALUES ( 'Javier', 'Herrera','10-10-1990', 30, 'Santa Ana','Costa Rica','Play the guitar','Spanish, English','herrJav@puchimail.com','5','Intel','core',DEFAULT,'Organized')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Skill(
subjectCategory  VARCHAR(100),
subjectSkillName VARCHAR(100),
PRIMARY KEY(subjectCategory, subjectSkillName)
)

INSERT INTO Skill
VALUES ('Musica', 'Tocar piano')

INSERT INTO Skill
VALUES ('Dise�o', 'Adobe')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE HAS_SKILL(
memberId INT,
category  VARCHAR(100),
skillName VARCHAR(100),
PRIMARY KEY(memberId, category, skillName),
CONSTRAINT FK_Article_HAS_SKILL FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_SkillCat_HAS_SKILL FOREIGN KEY (category, skillName) REFERENCES Skill(subjectCategory, subjectSkillName) ON DELETE CASCADE ON UPDATE CASCADE
)

INSERT INTO HAS_SKILL
VALUES (1, 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES (2, 'Dise�o', 'Adobe')

INSERT INTO HAS_SKILL
VALUES (3, 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES (4, 'Dise�o', 'Adobe')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Question (
memberId	INT,
questionId  INT           IDENTITY (1, 1) PRIMARY KEY,
question    VARCHAR (MAX) NOT NULL,
faq		    VARCHAR(10)   NOT NULL DEFAULT 'not posted', -- posted, not posted
answer      VARCHAR (MAX) DEFAULT 'no answer',
status		VARCHAR (15)  NOT NULL DEFAULT 'not checked', --checked, not checked
CONSTRAINT FK_CommunityMember_Question FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId) ON DELETE NO ACTION ON UPDATE CASCADE,
);

INSERT INTO	Question
VALUES (1, 'How to exit VIM?','posted', 'If you are in edit mode, first press the <Esc> key. Then enter :wq + <Enter> to save and exit. And by the way this does not work', DEFAULT)

INSERT INTO	Question
VALUES ( 2,'How to create an article?',DEFAULT, DEFAULT, DEFAULT)

INSERT INTO	Question
VALUES (3, 'Will vaccinations make me autistic?', DEFAULT , 'No', 'checked')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE WRITES(
memberId	INT,
articleId	INT
PRIMARY KEY(memberId,articleId),
CONSTRAINT FK_CommunityMember_WRITES FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId) ON DELETE NO ACTION ON UPDATE CASCADE,
CONSTRAINT FK_Article_WRITES FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE NO ACTION ON UPDATE CASCADE
 )

 INSERT INTO WRITES
 VALUES	(1,1)

 INSERT INTO WRITES
 VALUES	(1,2)

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
CONSTRAINT FK_CommunityMember_REVIEW FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId) ON DELETE NO ACTION ON UPDATE CASCADE,
CONSTRAINT FK_Article_REVIEW FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE NO ACTION ON UPDATE CASCADE
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