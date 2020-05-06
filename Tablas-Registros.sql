create table ArticuloReg
( 
Id int PRIMARY KEY,
Name		varchar(MAX) NOT NULL,
Topic		varchar(MAX) NOT NULL,
Abstract	varchar(MAX),
PublishDate date,
Route		varchar(MAX),
);

insert into ArticuloReg
values (1,'Robinson Crusoe','Naufragio','El tom hanks del siglo 17','12-02-1999','c:/Dani/Libros');

insert into ArticuloReg
values (2,'Yo Robot','Filosofia','Todo lo que no es la pelicula','12-02-1999','c:/Isaac/Libros');

create table Users
(
userId int PRIMARY KEY,
name		varchar(MAX) NOT NULL,
lastName	varchar(MAX) NOT NULL,
);

insert into Users
values (207730016, 'Daniel', 'Barrantes');

insert into Users
values (402380024, 'Antonio', 'Alvarez');

create table ForeignParticipant
(
userId int PRIMARY KEY,
name					varchar(MAX) NOT NULL,
lastName				varchar(MAX) NOT NULL,
participantAttributes	varchar(MAX) NOT NULL,
);

insert into ForeignParticipant
values (206430123, 'Cristiano', 'Ronaldo', 'Futbolista');

insert into ForeignParticipant
values (409870041, 'Cristina', 'Caveira', 'Silenciosa');

create table Member
(
userId int PRIMARY KEY,
name					varchar(MAX) NOT NULL,
lastName				varchar(MAX) NOT NULL,
type					varchar(30)  NOT NULL,
memberAttributes		varchar(MAX) NOT NULL,
);

insert into Member
values (123456789, 'Carolina', 'Cabello','Coordinator', 'Pequeña');

insert into Member
values (498765432, 'Eduardo', 'Eduarte','Core', 'Gordo');

create table Crea
(
id int NOT NULL,
userId int NOT NULL,

PRIMARY KEY(id,userId)
);

insert into Crea
values (1,402380024);