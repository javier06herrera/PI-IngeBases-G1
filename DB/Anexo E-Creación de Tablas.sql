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
CONSTRAINT FK_Article_INVOLVES FOREIGN KEY (articleId) REFERENCES Article(articleID) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_TopicCat_INVOLVES FOREIGN KEY (category, topicName) REFERENCES Topic(subject_category, subject_topicName) ON DELETE NO ACTION ON UPDATE CASCADE
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
birthDate			DATE	NOT NULL,
age					INT		NOT NULL,	
addressCity		VARCHAR(150) NOT NULL,
addressCountry		VARCHAR(150) NOT NULL,
hobbies				VARCHAR(300) NOT NULL,
languages			VARCHAR(300) NOT NULL,
email				VARCHAR(100) NOT NULL,
phoneNumber			VARCHAR(15) NOT NULL UNIQUE,
workInformation		VARCHAR(MAX) NOT NULL,
typeOfMember		VARCHAR(100) NOT NULL,
totalQualification	INT	NOT NULL DEFAULT 0,
)

INSERT INTO CommunityMember
VALUES ( 'Kevin', 'Barrantes','10-10-1990', 30, 'Rio Segundo','Costa Rica','Read, Listen good music','Spanish, English','barrKev@puchimail.com','1','Intel','Generic',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Gloriana', 'Mora','10-10-1990', 30,'San Sebastián','Costa Rica','Read, travel','Spanish, English','moraGlo@puchimail.com','2','Intel','Generic', DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Antonio', 'Álvares', '10-10-1990',30,'Barva','Costa Rica','Play video games','Spanish, English','alvAnt@puchimail.com','3','Intel','Generic',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Daniel', 'Barrante','10-10-1990', 30,'Alajuela','Costa Rica','Play the piano, eat caldosas, beign a "bicho"','Spanish, English','barrDan@puchimail.com','4','Intel','Generic',DEFAULT)

INSERT INTO CommunityMember
VALUES ( 'Javier', 'Herrera','10-10-1990', 30, 'Santa Ana','Costa Rica','Play the guitar','Spanish, English','herrJav@puchimail.com','5','Intel','Generic',DEFAULT)

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE Skill(
subject_category  VARCHAR(100),
subject_skillName VARCHAR(100),
PRIMARY KEY(subject_category, subject_skillName)
)

INSERT INTO Skill
VALUES ('Musica', 'Tocar piano')

INSERT INTO Skill
VALUES ('Diseño', 'Adobe')

---------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE HAS_SKILL(
memberId INT,
category  VARCHAR(100),
skillName VARCHAR(100),
PRIMARY KEY(memberId, category, skillName),
CONSTRAINT FK_Article_HAS_SKILL FOREIGN KEY (memberId) REFERENCES CommunityMember(memberId) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_SkillCat_HAS_SKILL FOREIGN KEY (category, skillName) REFERENCES Skill(subject_category, subject_skillName) ON DELETE CASCADE ON UPDATE CASCADE
)

INSERT INTO HAS_SKILL
VALUES (1, 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES (2, 'Diseño', 'Adobe')

INSERT INTO HAS_SKILL
VALUES (3, 'Musica', 'Tocar piano')

INSERT INTO HAS_SKILL
VALUES (4, 'Diseño', 'Adobe')

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
VALUES (1, 'Why are we here?','posted', DEFAULT, DEFAULT)

INSERT INTO	Question
VALUES ( 2,'What happened to Jimmy Hoffa?',DEFAULT, DEFAULT, DEFAULT)

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
