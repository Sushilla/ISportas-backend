CREATE TABLE Treniruote(

     TreniruotesId varchar(36),

     TrenerioId varchar(36),

     VartotojoId varchar(36),

     Pavadinimas nvarchar(50) COLLATE Lithuanian_CI_AS,

     Aprasymas nvarchar(255) COLLATE Lithuanian_CI_AS,

     SukurimoData datetime
 );