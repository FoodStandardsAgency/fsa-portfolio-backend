CREATE TABLE [dbo].[ProjectDataItems] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Project_Id] INT            NOT NULL,
    [Label_Id]   INT            NOT NULL,
    [Value]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ProjectDataItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProjectDataItems_dbo.PortfolioLabelConfigs_Label_Id] FOREIGN KEY ([Label_Id]) REFERENCES [dbo].[PortfolioLabelConfigs] ([Id]),
    CONSTRAINT [FK_dbo.ProjectDataItems_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Label_Id]
    ON [dbo].[ProjectDataItems]([Label_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[ProjectDataItems]([Project_Id] ASC);

