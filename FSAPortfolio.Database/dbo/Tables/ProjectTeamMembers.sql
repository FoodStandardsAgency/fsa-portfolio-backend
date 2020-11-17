CREATE TABLE [dbo].[ProjectTeamMembers] (
    [Project_Id] INT NOT NULL,
    [Person_Id]  INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectTeamMembers] PRIMARY KEY CLUSTERED ([Project_Id] ASC, [Person_Id] ASC),
    CONSTRAINT [FK_dbo.ProjectTeamMembers_dbo.People_Person_Id] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.ProjectTeamMembers_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_Id]
    ON [dbo].[ProjectTeamMembers]([Person_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[ProjectTeamMembers]([Project_Id] ASC);

