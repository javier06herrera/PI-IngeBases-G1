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
											--on edition: El autor est� en proceso de edici�n
											--pending collaboration: El autor env�a el art�culo y el autor no ha solicitado colaboraci�n
											--pending assignation: El art�culo est� en proceso de assignaci�n de revisores
											--not checked: Faltan miembros revisores de concluir, 
											--checked: Todos los revisores concluyeron su revisi�n
											--published: Fue Aceptado por el coordinador
);



insert into Article
values ('Robinson Crusoe','short','Wreck of an english man and survival in a desert island','12-02-1999','Nac� en 1632, en la ciudad de York, de una buena familia, aunque no de la regi�n, pues mi padre era un extranjero de Brema1 que, inicialmente, se asent� en Hull2. All� consigui� hacerse con una considerable fortuna como comerciante y, m�s tarde, abandon� sus negocios y se fue a vivir a York, donde se cas� con mi madre, que pertenec�a a la familia Robinson, una de las buenas familias del condado de la cual obtuve mi nombre, Robinson Kreutznaer. Mas, por la habitual alteraci�n de las palabras que se hace en Inglaterra, ahora nos llaman y nosotros tambi�n nos llamamos y escribimos nuestro nombre Crusoe; y as� me han llamado siempre mis compa�eros.   Ten�a dos hermanos mayores, uno de ellos fue coronel de un regimiento de infanter�a inglesa en Flandes, que antes hab�a estado bajo el mando del c�lebre coronel Lockhart, y muri� en la batalla de Dunkerque3 contra los espa�oles.    Lo que fue de mi segundo hermano, nunca lo he sabido al igual que mi padre y mi madre tampoco supieron lo que fue de m�.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('I, Robot','long','Elaborate philosophic implications concerning the three laws of robotics','12-02-1999','Un  robot  no  debe  da�ar  a  un  ser  humano  o,  por  su  inacci�n,dejar que un ser humano sufra da�o.2.  Un  robot  debe  obedecer  las  �rde  nes  que  le  son  dadas  por  unser  hu  mano,  excepto  cuando  estas  �rdenes  est�n  en  oposici�ncon la primera Ley.3.  Un  robot  debe  proteger  su  propia  existencia,  hasta  donde  estaprotec ci�n no est� en conflicto con la primera o segunda Leyes.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'not checked');

insert into Article
values ('Mamita Yunai','long','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT);

insert into Article
values ('Morgan Salgari','long','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

insert into Article
values ('Animal Farm','long','Metaphor about the rotting structure of a raising empire','12-02-1999','One day, Squealer takes the sheep off to a remote spot to teach them a new chant. Not long afterward, the animals have just finished their day�s work when they hear the terrified neighing of a horse. It is Clover, and she summons the others hastily to the yard. There, the animals gaze in amazement at Squealer walking toward them on his hind legs. Napoleon soon appears as well, walking upright; worse, he carries a whip. Before the other animals have a chance to react to the change, the sheep begin to chant, as if on cue: �Four legs good, two legs better!� Clover, whose eyes are failing in her old age, asks Benjamin to read the writing on the barn wall where the Seven Commandments were originally inscribed. Only the last commandment remains: �all animals are equal.� However, it now carries an addition: �but some animals are more equal than others.� In the days that follow, Napoleon openly begins smoking a pipe, and the other pigs subscribe to human magazines, listen to the radio, and begin to install a telephone, also wearing human clothes that they have salvaged from Mr. Jones�s wardrobe.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending collaboration');

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

INSERT INTO	Topic
VALUES ('Satire', 'Politics')

INSERT INTO	Topic
VALUES ('Story', 'Fiction')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE INVOLVES(
articleId INT,
category  VARCHAR(100),
topicName VARCHAR(100),
PRIMARY KEY(articleId, category, topicName),
CONSTRAINT FK_Article_INVOLVES FOREIGN KEY (articleId) REFERENCES Article(articleID) ON DELETE CASCADE ON UPDATE CASCADE, --En caso de eliminar un art�culo no tiene mucho sentido conservar la relaci�n que ten�a con los temas.
CONSTRAINT FK_TopicCat_INVOLVES FOREIGN KEY (category, topicName) REFERENCES Topic(subjectCategory, subjectTopicName) ON DELETE NO ACTION ON UPDATE CASCADE--No se quiere permitir borrar temas y que un art�culo quede sin temas asociados.  
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

INSERT INTO INVOLVES
VALUES	(5,'Satire', 'Politics')

INSERT INTO INVOLVES
VALUES	(5,'Story', 'Fiction')


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
VALUES ( 'barrKev@puchimail.com','Kevin', 'Barrantes','10-10-1990', 30, 'Rio Segundo','Costa Rica','Read, Listen good music','Spanish, English','188888','Intel','core',5,'Versatility',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'glorymoravi@gmail.com','Gloriana', 'Mora','10-10-1990', 30,'San Sebasti�n','Costa Rica','Read, travel','Spanish, English','288888','Intel','core', 5, 'Creativity',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'alvAnt@puchimail.com','Antonio', '�lvares', '10-10-1990',30,'Barva','Costa Rica','Play video games','Spanish, English','388888','Intel','active',3,'Math-skills',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'dbarrantescr@gmail.com','Daniel', 'Barrante','10-10-1990', 30,'Alajuela','Costa Rica','Play the piano, eat caldosas, beign a "bicho"','Spanish, English','48888','Intel','active',3,'Adaptability',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'herrJav@puchimail.com','Javier', 'Herrera','10-10-1990', 30, 'Santa Ana','Costa Rica','Play the guitar','Spanish, English','588888','Intel','peripheral',0,'Organized',DEFAULT)

INSERT INTO CommunityMember 
VALUES('ant.alvarez.chavarria@hotmail.es','coordinator','cordino','1990-10-10',50,'Jac�','Costa Rica','Coordinar','Java','24888','Coordinar', 'coordinator', 5,'coodinar',DEFAULT);
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
email VARCHAR(100),
category  VARCHAR(100),
skillName VARCHAR(100),
PRIMARY KEY(email, category, skillName),
CONSTRAINT FK_Article_HAS_SKILL FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE, -- En caso de eliminar un miembro no tiene mucho sentido conservar la relaci�n que ten�a con las habilidades.
CONSTRAINT FK_SkillCat_HAS_SKILL FOREIGN KEY (category, skillName) REFERENCES Skill(subjectCategory, subjectSkillName) ON DELETE CASCADE ON UPDATE CASCADE --Si una habilidad se elimina no tiene mucho sentido seguir asoci�ndola a alg�n miembro. 
)

INSERT INTO HAS_SKILL
VALUES ('barrKev@puchimail.com', 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES ('glorymoravi@gmail.com', 'Dise�o', 'Adobe')

INSERT INTO HAS_SKILL
VALUES ('alvAnt@puchimail.com', 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES ('herrJav@puchimail.com', 'Dise�o', 'Adobe')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Question (
email		VARCHAR(100),
questionId  INT           IDENTITY (1, 1) PRIMARY KEY,
question    VARCHAR (MAX) NOT NULL,
faq		    VARCHAR(10)   NOT NULL DEFAULT 'not posted', -- posted, not posted
answer      VARCHAR (MAX) DEFAULT 'no answer',
status		VARCHAR (15)  NOT NULL DEFAULT 'not checked', --checked, not checked
CONSTRAINT FK_CommunityMember_Question FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE NO ACTION ON UPDATE CASCADE, --No se desea perder preguntas a causa de que un miembro se elimine.

);

INSERT INTO	Question
VALUES ('barrKev@puchimail.com', 'How to exit VIM?','posted', 'If you are in edit mode, first press the <Esc> key. Then enter :wq + <Enter> to save and exit. And by the way this does not work', DEFAULT)

INSERT INTO	Question
VALUES ('glorymoravi@gmail.com','How to create an article?',DEFAULT, DEFAULT, DEFAULT)

INSERT INTO	Question
VALUES ('alvAnt@puchimail.com', 'Will vaccinations make me autistic?', DEFAULT , 'No', 'checked')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE WRITES(
email	VARCHAR(100),
articleId	INT
PRIMARY KEY(email,articleId),
CONSTRAINT FK_CommunityMember_WRITES FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE NO ACTION ON UPDATE CASCADE,--No se desea que se eliminen art�culos ya posteados.
CONSTRAINT FK_Article_WRITES FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE --No se desea que existan art�culos sin autor asociado.
 )

 INSERT INTO WRITES
 VALUES	('barrKev@puchimail.com',1)

 INSERT INTO WRITES
 VALUES	('barrKev@puchimail.com',2)

 INSERT INTO WRITES
 VALUES	('glorymoravi@gmail.com',1)

 INSERT INTO WRITES
 VALUES	('alvAnt@puchimail.com',2)

 INSERT INTO WRITES
 VALUES	('dbarrantescr@gmail.com',2)

 INSERT INTO WRITES
 VALUES	('herrJav@puchimail.com',3)

 INSERT INTO WRITES
 VALUES	('barrKev@puchimail.com',4)

 INSERT INTO WRITES
 VALUES	('herrJav@puchimail.com',5)
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
CONSTRAINT FK_CommunityMember_REVIEW FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE, --No se desea que existan personas que no son miembros revisando art�culos.
CONSTRAINT FK_Article_REVIEW FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE --No se desea tener miembros asignados a revisar art�culos que ya no existen. 
 )

INSERT INTO REVIEWS
 VALUES	(1,'alvAnt@puchimail.com','Sin comentarios',5,5,5,100,'reviewed')

INSERT INTO REVIEWS
 VALUES	(1,'barrKev@puchimail.com','Sin comentarios',5,5,5,75,DEFAULT)

INSERT INTO REVIEWS
 VALUES	(2,'glorymoravi@gmail.com','Sin comentarios',5,5,5,25,'reviewed')

INSERT INTO REVIEWS
 VALUES	(2,'dbarrantescr@gmail.com','Sin comentarios',5,5,5,30,'reviewed')

INSERT INTO REVIEWS
 VALUES	(3,'glorymoravi@gmail.com','Sin comentarios',5,5,5,60,DEFAULT)


CREATE TABLE IS_NOMINATED(
answer VARCHAR(100) DEFAULT 'pending',
comments VARCHAR(MAX)  DEFAULT 'not apply',
email VARCHAR(100),
articleId	INT,
PRIMARY KEY(email,ArticleId),
CONSTRAINT FK_CommunityMember_IS_NOMINATED FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE,-- No se desea que existan nominaciones de personas que no son miembros. 
CONSTRAINT FK_Article_IS_NOMINATED FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE -- No se desea tener miembros nominados a revisar art�culos que ya no existen. 
)

INSERT INTO IS_NOMINATED
VALUES ('pending','no comment','barrKev@puchimail.com', 5)