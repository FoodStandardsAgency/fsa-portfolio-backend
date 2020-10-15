CREATE TABLE [dbo].[RelatedProjects] (
    [Project_Id]        INT NOT NULL,
    [RelatedProject_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.RelatedProjects] PRIMARY KEY CLUSTERED ([Project_Id] ASC, [RelatedProject_Id] ASC),
    CONSTRAINT [FK_dbo.RelatedProjects_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id]),
    CONSTRAINT [FK_dbo.RelatedProjects_dbo.Projects_RelatedProject_Id] FOREIGN KEY ([RelatedProject_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RelatedProject_Id]
    ON [dbo].[RelatedProjects]([RelatedProject_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[RelatedProjects]([Project_Id] ASC);

