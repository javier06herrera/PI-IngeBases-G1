create table ArticuloReg
( 
Id int PRIMARY KEY,
Name varchar(MAX) NOT NULL,
Topic varchar(MAX) NOT NULL,
Abstract varchar(MAX),
PublishDate date,
Route varchar(MAX),
);

insert into ArticuloReg
values (1,'Robinson Crusoe','Naufragio','El tom hanks del siglo 17','12-02-1999','c:/Dani/Libros');

insert into ArticuloReg
values (2,'Yo Robot','Filosofia','Todo lo que no es la pelicula','12-02-1999','c:/Isaac/Libros');

