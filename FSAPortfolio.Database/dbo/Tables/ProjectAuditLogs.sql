CREATE TABLE [dbo].[ProjectAuditLogs] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Project_Id] INT            NOT NULL,
    [Timestamp]  DATETIME       NOT NULL,
    [Text]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ProjectAuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProjectAuditLogs_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[ProjectAuditLogs]([Project_Id] ASC);

