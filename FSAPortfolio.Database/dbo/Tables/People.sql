CREATE TABLE [dbo].[People] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Surname]   NVARCHAR (250) NULL,
    [Firstname] NVARCHAR (250) NULL,
    [Email]     NVARCHAR (250) NULL,
    [G6team]    NVARCHAR (50)  NULL,
    [Timestamp] DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.People] PRIMARY KEY CLUSTERED ([Id] ASC)
);

