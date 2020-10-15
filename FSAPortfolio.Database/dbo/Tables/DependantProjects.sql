CREATE TABLE [dbo].[DependantProjects] (
    [Project_Id]          INT NOT NULL,
    [DependantProject_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.DependantProjects] PRIMARY KEY CLUSTERED ([Project_Id] ASC, [DependantProject_Id] ASC),
    CONSTRAINT [FK_dbo.DependantProjects_dbo.Projects_DependantProject_Id] FOREIGN KEY ([DependantProject_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id]),
    CONSTRAINT [FK_dbo.DependantProjects_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_DependantProject_Id]
    ON [dbo].[DependantProjects]([DependantProject_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[DependantProjects]([Project_Id] ASC);

