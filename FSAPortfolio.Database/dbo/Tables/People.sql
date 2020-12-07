CREATE TABLE [dbo].[People] (
    [Id]                           INT            IDENTITY (1, 1) NOT NULL,
    [Surname]                      NVARCHAR (250) NULL,
    [Firstname]                    NVARCHAR (250) NULL,
    [Email]                        NVARCHAR (250) NULL,
    [Team_Id]                      INT            NULL,
    [ActiveDirectoryPrincipalName] NVARCHAR (150) NULL,
    [ActiveDirectoryId]            NVARCHAR (150) NULL,
    [ActiveDirectoryDisplayName]   NVARCHAR (500) NULL,
    [Department]                   NVARCHAR (150) NULL,
    [Timestamp]                    DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.People] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.People_dbo.Teams_Team_Id] FOREIGN KEY ([Team_Id]) REFERENCES [dbo].[Teams] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_Team_Id]
    ON [dbo].[People]([Team_Id] ASC);

