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


--Estos prueban las nominaciones de los miembros del núcleo 
insert into Article
values ('Robinson Crusoe','short','Wreck of an english man and survival in a desert island','12-02-1999','Nací en 1632, en la ciudad de York, de una buena familia, aunque no de la región, pues mi padre era un extranjero de Brema1 que, inicialmente, se asentó en Hull2. Allí consiguió hacerse con una considerable fortuna como comerciante y, más tarde, abandonó sus negocios y se fue a vivir a York, donde se casó con mi madre, que pertenecía a la familia Robinson, una de las buenas familias del condado de la cual obtuve mi nombre, Robinson Kreutznaer. Mas, por la habitual alteración de las palabras que se hace en Inglaterra, ahora nos llaman y nosotros también nos llamamos y escribimos nuestro nombre Crusoe; y así me han llamado siempre mis compañeros.   Tenía dos hermanos mayores, uno de ellos fue coronel de un regimiento de infantería inglesa en Flandes, que antes había estado bajo el mando del célebre coronel Lockhart, y murió en la batalla de Dunkerque3 contra los españoles.    Lo que fue de mi segundo hermano, nunca lo he sabido al igual que mi padre y mi madre tampoco supieron lo que fue de mí.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending assignation');

insert into Article
values ('I, Robot','short','Elaborate philosophic implications concerning the three laws of robotics','12-02-1999','Un  robot  no  debe  dañar  a  un  ser  humano  o,  por  su  inacción,dejar que un ser humano sufra daño.2.  Un  robot  debe  obedecer  las  órde  nes  que  le  son  dadas  por  unser  hu  mano,  excepto  cuando  estas  órdenes  están  en  oposicióncon la primera Ley.3.  Un  robot  debe  proteger  su  propia  existencia,  hasta  donde  estaprotec ción no esté en conflicto con la primera o segunda Leyes.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending assignation');

insert into Article
values ('Mamita Yunai','short','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending assignation');

--------------------------------------------------------------------------------------
--Estos prueban las revisiones de los miembros del núcleo.
insert into Article
values ('Morgan Salgari','short','Life of a "criollo" politician and his fight against an oppresive fruit company','12-02-1999','C://Carlos/Fallas',25,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'not checked');
insert into Article
values ('Little Women','short','es una novela de la escritora estadounidense Louisa May Alcott publicada el 30 de septiembre de 1868, que trata la vida de cuatro niñas que tras pasar la adolescencia, con la Guerra Civil en los Estados Unidos como fondo, entre 1861 y 1865, se convierten en mujeres.','12-02-1999','Esta obra reproduce, tanto en su estructura como en su tema, la conocidísima novela alegórica de John Bunyan El progreso del peregrino, y de ahí que muchos de los títulos de los capítulos sean alusiones directas a esta obra (Juego de los peregrinos; Cargas; Beth encuentra el Palacio Hermoso; El valle de la humillación de Amy; Jo conoce a Apolión; Meg visita la Feria de las Vanidades; entre otros). A la vez, cada una de las muchachas March está destinada a caracterizar y superar estos defectos',25,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'not checked');

--------------------------------------------------------------------------------------
-- Es para que el coordinador puede pedir colaboración en la revisón del artículo
insert into Article
values ('Animal Farm','long','Metaphor about the rotting structure of a raising empire','12-02-1999','One day, Squealer takes the sheep off to a remote spot to teach them a new chant. Not long afterward, the animals have just finished their day’s work when they hear the terrified neighing of a horse. It is Clover, and she summons the others hastily to the yard. There, the animals gaze in amazement at Squealer walking toward them on his hind legs. Napoleon soon appears as well, walking upright; worse, he carries a whip. Before the other animals have a chance to react to the change, the sheep begin to chant, as if on cue: “Four legs good, two legs better!” Clover, whose eyes are failing in her old age, asks Benjamin to read the writing on the barn wall where the Seven Commandments were originally inscribed. Only the last commandment remains: “all animals are equal.” However, it now carries an addition: “but some animals are more equal than others.” In the days that follow, Napoleon openly begins smoking a pipe, and the other pigs subscribe to human magazines, listen to the radio, and begin to install a telephone, also wearing human clothes that they have salvaged from Mr. Jones’s wardrobe.',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending collaboration');

insert into Article
values ('The Tell-Tale Heart','short','A short story by American writer Edgar Allan Poe, first published in 1843. It is related by an unnamed narrator who endeavors to convince the reader of the narrator’s sanity while simultaneously describing a murder the narrator committed.','12-02-1999','iT’sTRue!  yes,  i  have been ill,  very  ill.  But  why  do  you  say  that  I  have  lost  control  of  my  mind,  why  do  you  say  that  I  am  mad?  Can  you  not  see  that  I  have  full  control of my mind? Is it not clear that  I  am  not  mad?  Indeed,  the  illness  only  made  my  mind,  my  feelings, my senses stronger, more powerful.   My   sense   of   hearing   especially became more powerful. I  could  hear  sounds  I  had  never  heard before. I heard sounds from heaven; and I heard sounds from hell',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending collaboration');

insert into Article
values ('Alice Adventures in Wonderland','long',' It tells of a young girl named Alice, who falls through a rabbit hole into a subterranean fantasy world populated by peculiar, anthropomorphic creatures. It is considered to be one of the best examples of the literary nonsense genre.The tale plays with logic, giving the story lasting popularity with adults as well as with children','12-02-1999','ALICE was   beginning   to   get   very   tired   ofsitting  by  her  sister  on  the  bank,  and  of  havingnothing  to  do :  once  or  twice  she  had  peeped  intothe  book  her  sister  was  reading,  but  it  had  nopictures  or  conversations  in  it,  “and  what  is',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'pending collaboration');
--------------------------------------------------------------------------------------
-- Son para graficar
insert into Article
values ('Cocorí','short','La historia de un niño en la region tropical','12-02-1999','Cocorí es un niño que habita en un pueblo donde tiene contacto con el mar, y a la vez con toda la naturaleza del bosque tropical. Cierto día, un barco se acerca al puerto, y Cocorí tiene la oportunidad de subir en él para conocer a sus tripulantes. Entre ellos, está una niña rubia. Con una curiosidad muy infantil, Cocorí se sorprende por el cabello de la niña, ya que nunca ha visto una como ella, y la niña piensa que la piel de Cocorí se ha llenado de hollín y por eso se ha oscurecido, pues nunca ha visto personas que no sean de tez blanca.',100,15,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

insert into Article
values ('Única mirando al mar','short','Primera novela del escritor costarricense Fernando Contreras Castro, publicada por primera vez en 1993','12-02-1999','El autor comienza con esta obra a explorar la miseria en la cara oculta de Costa Rica, internándose en el botadero de basura de Río Azul, situado en el oeste de la provincia de Cartago.',75,20,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

insert into Article
values ('El mundo de Sofía','short',' Usando como pretexto una trama novelesca, el autor hace una guía básica sobre la historia de la filosofía occidental.','12-02-1999','Sophie, sin que su madre se entere, se convierte en la estudiante de un antiguo filósofo, Alberto Knox. Alberto le enseña sobre la historia de la filosofía. Obtiene una revisión sustantiva y comprensible de los presocráticos hasta Jean-Paul Sartre. Además de esto, Sophie y Alberto reciben postales dirigidas a una niña llamada Hilde de un hombre llamado Albert Knag. A medida que pasa el tiempo, Knag comienza a ocultar los mensajes de cumpleaños a Hilde de maneras cada vez más imposibles, como esconder uno dentro de un plátano sin pelar y hacer hablar al perro de Alberto, Hermes.',85,30,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

insert into Article
values ('Reliquias de la Muerte','short','Tres objetos mágicos altamente poderosos supuestamente creados por la Muerte y dados a tres hermanos en la Familia Peverell.','12-02-1999','De acuerdo a la leyenda, el que posea éstos tres artefactos se convertirá en el Amo de la Muerte. La historia de las Reliquias de la Muerte fue originalmente narrada por Beedle el Bardo, y pasada de familia a familia como un cuento de hadas; la leyenda da los nombres de los tres hermanos como Antioch, Cadmus e Ignotus, poseedores de la Varita, la Piedra y la Capa respectivamente. Harry Potter es la única persona de la que se sabe que haya reunido las tres Reliquias al mismo tiempo (Albus Dumbledore también tuvo las tres Reliquias, pero sólo pudo tener al mismo tiempo la Varita de Saúco y la Capa de Invisibilidad. Más tarde, también tuvo a la vez la Varita de Saúco y la Piedra de la Resurrección).',110,50,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

insert into Article
values ('Orden del Fénix','short','La Orden del Fénix era una organización secreta fundada por Albus Dumbledore para oponerse y luchar contra Lord Voldemort y sus Mortífagos.','12-02-1999','Con la caída de Voldemort, la Orden se disolvió pero en 1995 se reconstituyó cuando Harry Potter le informó a Albus Dumbledore del regreso del Innombrable y el inicio de la Segunda Guerra Mágica. El problema fue que ésta vez el Ministerio no aceptó que el Señor Oscuro regresó, por lo que la Orden actuó solitaria y en secreto para proteger a Harry y la profecía sobre él y lord Voldemort. Luego de la Batalla en el Departamento de Misterios, donde se hallaba la profecía, el Ministerio admitió la verdad.',100,45,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

insert into Article
values ('Breve historia del tiempo','short',' Big Bang, los agujeros negros, los conos de luz y la teoría de supercuerdas.','12-02-1999','¿Cuál es la naturaleza del tiempo? ¿Hubo un principio o habrá un final en el tiempo? ¿Es infinito el universo o tiene límites? A partir de estas preguntas, Stephen Hawking revisa las grandes teorías cosmológicas, desde Aristóteles hasta nuestros días, así como muchos enigmas, paradojas y contradicciones que se plantean como retos para la ciencia actual.Hawking considera que los avances recientes de la física, gracias a las fantásticas nuevas tecnologías, sugieren respuestas a algunas de estas preguntas que desde hace tiempo nos preocupan.',95,60,DEFAULT,DEFAULT,DEFAULT,DEFAULT,'published');

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

INSERT INTO Topic
VALUES ('Drama', 'Fantasy')

INSERT INTO Topic
VALUES ('Novel', 'Fiction')

INSERT INTO Topic
VALUES ('Novel', 'Philosophy')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE INVOLVES(
articleId INT,
category  VARCHAR(100),
topicName VARCHAR(100),
PRIMARY KEY(articleId, category, topicName),
CONSTRAINT FK_Article_INVOLVES FOREIGN KEY (articleId) REFERENCES Article(articleID) ON DELETE CASCADE ON UPDATE CASCADE, --En caso de eliminar un artículo no tiene mucho sentido conservar la relación que tenía con los temas.
CONSTRAINT FK_TopicCat_INVOLVES FOREIGN KEY (category, topicName) REFERENCES Topic(subjectCategory, subjectTopicName) ON DELETE NO ACTION ON UPDATE CASCADE--No se quiere permitir borrar temas y que un artículo quede sin temas asociados.  
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

INSERT INTO INVOLVES
VALUES	(6,'Story', 'Fiction')

INSERT INTO INVOLVES
VALUES	(7,'Story', 'Fiction')

INSERT INTO INVOLVES
VALUES(8,'Novel','Fiction')

INSERT INTO INVOLVES
VALUES(9,'Novel','Fiction')

INSERT INTO INVOLVES
VALUES(10,'Novel','Philosophy')

INSERT INTO INVOLVES
VALUES(11,'Drama','Fantasy')

INSERT INTO INVOLVES
VALUES(12,'Drama','Fantasy')

INSERT INTO INVOLVES
VALUES(13,'Drama','Fantasy')

INSERT INTO INVOLVES
VALUES(14,'Drama','Fantasy')


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
VALUES ( 'glorymoravi@gmail.com','Gloriana', 'Mora','10-10-1990', 30,'San Sebastián','Costa Rica','Read, travel','Spanish, English','288888','Intel','core', 5, 'Creativity',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'antonio.alvarez.chavarria@gmail.com','Antonio', 'Álvarez', '10-10-1990',30,'Barva','Costa Rica','Play video games','Spanish, English','388888','Intel','core',3,'Math-skills',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'dbarrantescr@gmail.com','Daniel', 'Barrante','10-10-1990', 30,'Alajuela','Costa Rica','Play the piano, eat caldosas, beign a "bicho"','Spanish, English','48888','Intel','active',3,'Adaptability',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'herrJav@puchimail.com','Javier', 'Herrera','10-10-1990', 30, 'Santa Ana','Costa Rica','Play the guitar','Spanish, English','588888','Intel','peripheral',0,'Organized',DEFAULT)

INSERT INTO CommunityMember 
VALUES('ant.alvarez.chavarria@hotmail.es','coordinator','cordino','1990-10-10',50,'Jacó','Costa Rica','Coordinar','Java','24888','Coordinar', 'coordinator', 5,'coodinar',DEFAULT);

INSERT INTO CommunityMember 
VALUES('valeriaGM@puchimail.com','Valeria','Gomez Morales','1990-10-10',50,'Jacó','Costa Rica','Read','Spanish','79134','Sykes', 'core', 5,'Speaking',DEFAULT);
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
CONSTRAINT FK_Article_HAS_SKILL FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE, -- En caso de eliminar un miembro no tiene mucho sentido conservar la relación que tenía con las habilidades.
CONSTRAINT FK_SkillCat_HAS_SKILL FOREIGN KEY (category, skillName) REFERENCES Skill(subjectCategory, subjectSkillName) ON DELETE CASCADE ON UPDATE CASCADE --Si una habilidad se elimina no tiene mucho sentido seguir asociándola a algún miembro. 
)

INSERT INTO HAS_SKILL
VALUES ('barrKev@puchimail.com', 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES ('glorymoravi@gmail.com', 'Diseño', 'Adobe')

INSERT INTO HAS_SKILL
VALUES ('antonio.alvarez.chavarria@gmail.com', 'Musica', 'Tocar piano')

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
CONSTRAINT FK_CommunityMember_Question FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE NO ACTION ON UPDATE CASCADE, --No se desea perder preguntas a causa de que un miembro se elimine.

);

INSERT INTO	Question
VALUES ('barrKev@puchimail.com', 'How to exit VIM?','posted', 'If you are in edit mode, first press the <Esc> key. Then enter :wq + <Enter> to save and exit. And by the way this does not work', DEFAULT)

INSERT INTO	Question
VALUES ('glorymoravi@gmail.com','How to create an article?',DEFAULT, DEFAULT, DEFAULT)

INSERT INTO	Question
VALUES ('antonio.alvarez.chavarria@gmail.com', 'Will vaccinations make me autistic?', DEFAULT , 'No', 'checked')
---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE WRITES(
email	VARCHAR(100),
articleId	INT
PRIMARY KEY(email,articleId),
CONSTRAINT FK_CommunityMember_WRITES FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE NO ACTION ON UPDATE CASCADE,--No se desea que se eliminen artículos ya posteados.
CONSTRAINT FK_Article_WRITES FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE --No se desea que existan artículos sin autor asociado.
 )

INSERT INTO WRITES
VALUES	('barrKev@puchimail.com',1)

INSERT INTO WRITES
VALUES	('barrKev@puchimail.com',2)

INSERT INTO WRITES
VALUES	('glorymoravi@gmail.com',1)

INSERT INTO WRITES
VALUES	('antonio.alvarez.chavarria@gmail.com',2)

INSERT INTO WRITES
VALUES	('dbarrantescr@gmail.com',2)

INSERT INTO WRITES
VALUES	('herrJav@puchimail.com',3)

INSERT INTO WRITES
VALUES	('barrKev@puchimail.com',4)

INSERT INTO WRITES
VALUES	('herrJav@puchimail.com',5)

INSERT INTO WRITES
VALUES	('barrKev@puchimail.com',6)

INSERT INTO WRITES
VALUES	('herrJav@puchimail.com',6)

INSERT INTO WRITES
VALUES	('antonio.alvarez.chavarria@gmail.com',7)

INSERT INTO WRITES
VALUES	('dbarrantescr@gmail.com',7)

INSERT INTO WRITES
VALUES	('glorymoravi@gmail.com',7)

 INSERT INTO WRITES
VALUES('barrKev@puchimail.com',8)

INSERT INTO WRITES
VALUES('glorymoravi@gmail.com',9)

INSERT INTO WRITES
VALUES('glorymoravi@gmail.com',10)

INSERT INTO WRITES
VALUES('dbarrantescr@gmail.com',11)

INSERT INTO WRITES
VALUES('herrJav@puchimail.com',12)

INSERT INTO WRITES
VALUES('antonio.alvarez.chavarria@gmail.com',12)

INSERT INTO WRITES
VALUES('antonio.alvarez.chavarria@gmail.com',13)

INSERT INTO WRITES
VALUES('barrKev@puchimail.com',14)

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
CONSTRAINT FK_CommunityMember_REVIEW FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE, --No se desea que existan personas que no son miembros revisando artículos.
CONSTRAINT FK_Article_REVIEW FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE --No se desea tener miembros asignados a revisar artículos que ya no existen. 
 )


 INSERT INTO REVIEWS
 VALUES	(4,'glorymoravi@gmail.com','Sin comentarios',5,5,5,60,DEFAULT)

  INSERT INTO REVIEWS
 VALUES	(5,'glorymoravi@gmail.com','Sin comentarios',5,5,5,60,DEFAULT)

 INSERT INTO REVIEWS
 VALUES	(4,'barrKev@puchimail.com','Sin comentarios',5,5,5,30,DEFAULT)

 INSERT INTO REVIEWS
 VALUES	(5,'barrKev@puchimail.com','Sin comentarios',5,5,5,30,DEFAULT)

  INSERT INTO REVIEWS
 VALUES	(5,'antonio.alvarez.chavarria@gmail.com','Sin comentarios',5,5,5,30,DEFAULT)

 INSERT INTO REVIEWS
 VALUES	(4,'antonio.alvarez.chavarria@gmail.com','Sin comentarios',5,5,5,30,DEFAULT)


 INSERT INTO REVIEWS
 VALUES	(4,'valeriaGM@puchimail.com','Sin comentarios',5,5,5,30,DEFAULT)

 INSERT INTO REVIEWS
 VALUES	(5,'valeriaGM@puchimail.com','Sin comentarios',5,5,5,30,DEFAULT)


CREATE TABLE IS_NOMINATED(
answer VARCHAR(100) DEFAULT 'pending',
comments VARCHAR(MAX)  DEFAULT 'not apply',
email VARCHAR(100),
articleId	INT,
PRIMARY KEY(email,ArticleId),
CONSTRAINT FK_CommunityMember_IS_NOMINATED FOREIGN KEY (email) REFERENCES CommunityMember(email) ON DELETE CASCADE ON UPDATE CASCADE,-- No se desea que existan nominaciones de personas que no son miembros. 
CONSTRAINT FK_Article_IS_NOMINATED FOREIGN KEY (articleId) REFERENCES Article(articleId) ON DELETE CASCADE ON UPDATE CASCADE -- No se desea tener miembros nominados a revisar artículos que ya no existen. 
)

--- Para probar la nominación de los miembros del núcleo.
INSERT INTO IS_NOMINATED
VALUES ('accept','Yes, I want','barrKev@puchimail.com', 1)
INSERT INTO IS_NOMINATED
VALUES ('reject','No, I dont','barrKev@puchimail.com', 2)
INSERT INTO IS_NOMINATED
VALUES ('accept','Yes, I want','barrKev@puchimail.com', 3)

INSERT INTO IS_NOMINATED
VALUES ('accept','Yes, I want','antonio.alvarez.chavarria@gmail.com', 1)
INSERT INTO IS_NOMINATED		
VALUES ('accept','Yes, I want','antonio.alvarez.chavarria@gmail.com', 2)
INSERT INTO IS_NOMINATED		
VALUES ('reject','No, I dont','antonio.alvarez.chavarria@gmail.com', 3)

INSERT INTO IS_NOMINATED
VALUES ('accept','Yes, I want','glorymoravi@gmail.com', 1)
INSERT INTO IS_NOMINATED		
VALUES ('reject','No, I dont','glorymoravi@gmail.com', 2)
INSERT INTO IS_NOMINATED		
VALUES ('accept','Yes, I want','glorymoravi@gmail.com', 3)


---------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------
--Functions
--CREATE FUNCTION PIF_getTopics(@articleId int)
--RETURNS varchar(max)
--AS
--BEGIN
--declare @result varchar(max)
--set @result =''
--select @result = @result + I.topicName + ', '
--from Article A
--join INVOLVES I on A.articleId = I.articleId
--where A.articleId = @articleId
--RETURN @result
--END

----Procedures
--CREATE PROC PISP_getArticles AS 
--SELECT A.*, dbo.PIF_getTopics(A.articleId) as topicName
--FROM Article A
--ORDER BY publishDate DESC

--CREATE PROC PISP_getMemberProfile @email varchar(25) AS
--DECLARE @articleCount int 
--SELECT @articleCount = COUNT(*)
--FROM WRITES W
--WHERE W.email = @email

--SELECT  C.*, @articleCount AS 'articleCount'
--FROM CommunityMember C
--WHERE C.email = @email

--CREATE PROC PISP_getMemeberArticle @email varchar(25) AS 
--SELECT A.*, dbo.PIF_getTopics(A.articleId) AS topicName
--FROM Article A
--JOIN WRITES W ON A.articleId = W.articleId
--WHERE W.email = @email
--ORDER BY publishDate DESC

-------------------------------------------------------------
GO
CREATE TRIGGER PIT_totalCountUpdate
ON Article
FOR  UPDATE
AS

DECLARE @articleId int
DECLARE @newLikeBalance int
DECLARE @oldLikeBalance int
select @articleId=I.articleId, @newLikeBalance = I.likesCount - I.dislikesCount
from inserted I
select @oldLikeBalance = D.likeBalance
from deleted D

IF UPDATE(likesCount) OR UPDATE(dislikesCount)
BEGIN
	UPDATE Article
	SET likeBalance = likesCount-dislikesCount
	WHERE articleId = @articleId 

	UPDATE	CommunityMember
	SET Points = Points + @newLikeBalance - @oldLikeBalance
	WHERE email in (
		SELECT W.email
		FROM WRITES W
		WHERE W.articleId = @articleId)
END
GO
-------------------------------------------------------------------------
GO
CREATE TRIGGER PIT_viewsMeritUpdate
ON Article
FOR  UPDATE
AS
DECLARE @articleId int
SELECT @articleId = I.articleId
FROM inserted I

IF UPDATE(accessCount)
BEGIN
	UPDATE CommunityMember
	SET points = points + 1
	WHERE email IN (
			SELECT W.email
			FROM WRITES W
			WHERE W.articleId = @articleId)
END 
GO