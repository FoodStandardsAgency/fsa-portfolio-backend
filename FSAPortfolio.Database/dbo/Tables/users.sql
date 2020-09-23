CREATE TABLE [dbo].[users] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [timestamp]    DATETIME      DEFAULT (getdate()) NOT NULL,
    [username]     VARCHAR (50)  NULL,
    [pass_hash]    VARCHAR (300) NULL,
    [access_group] VARCHAR (3)   NULL
);

