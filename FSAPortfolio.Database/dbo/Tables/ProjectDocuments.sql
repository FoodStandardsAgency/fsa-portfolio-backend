CREATE TABLE [dbo].[ProjectDocuments] (
    [Project_Id]  INT NOT NULL,
    [Document_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectDocuments] PRIMARY KEY CLUSTERED ([Project_Id] ASC, [Document_Id] ASC),
    CONSTRAINT [FK_dbo.ProjectDocuments_dbo.Documents_Document_Id] FOREIGN KEY ([Document_Id]) REFERENCES [dbo].[Documents] ([Id]),
    CONSTRAINT [FK_dbo.ProjectDocuments_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Document_Id]
    ON [dbo].[ProjectDocuments]([Document_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[ProjectDocuments]([Project_Id] ASC);

