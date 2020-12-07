CREATE TABLE [dbo].[Users] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Timestamp]     DATETIME       NOT NULL,
    [UserName]      NVARCHAR (50)  NULL,
    [PasswordHash]  NVARCHAR (300) NULL,
    [AccessGroupId] INT            NOT NULL,
    [RoleList]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Users_dbo.AccessGroups_AccessGroupId] FOREIGN KEY ([AccessGroupId]) REFERENCES [dbo].[AccessGroups] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_AccessGroupId]
    ON [dbo].[Users]([AccessGroupId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserName]
    ON [dbo].[Users]([UserName] ASC);

