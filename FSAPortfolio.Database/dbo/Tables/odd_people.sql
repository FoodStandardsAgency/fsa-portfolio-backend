CREATE TABLE [dbo].[odd_people] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [timestamp] DATETIME      DEFAULT (getdate()) NOT NULL,
    [surname]   VARCHAR (250) NULL,
    [firstname] VARCHAR (250) NULL,
    [email]     VARCHAR (250) NULL,
    [g6team]    VARCHAR (50)  NULL
);

