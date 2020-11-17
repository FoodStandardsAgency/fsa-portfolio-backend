CREATE TABLE [dbo].[ProjectUpdateItems] (
    [Id]                            INT             IDENTITY (1, 1) NOT NULL,
    [Project_Id]                    INT             NOT NULL,
    [Timestamp]                     DATETIME        NOT NULL,
    [Text]                          NVARCHAR (MAX)  NULL,
    [PercentageComplete]            REAL            NULL,
    [Budget]                        DECIMAL (18, 2) NOT NULL,
    [Spent]                         DECIMAL (18, 2) NOT NULL,
    [ExpectedCurrentPhaseEnd_Date]  DATETIME        NULL,
    [ExpectedCurrentPhaseEnd_Flags] INT             NOT NULL,
    [SyncId]                        INT             NOT NULL,
    [OnHoldStatus_Id]               INT             NULL,
    [Person_Id]                     INT             NULL,
    [Phase_Id]                      INT             NULL,
    [RAGStatus_Id]                  INT             NULL,
    CONSTRAINT [PK_dbo.ProjectUpdateItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.People_Person_Id] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectOnHoldStatus_OnHoldStatus_Id] FOREIGN KEY ([OnHoldStatus_Id]) REFERENCES [dbo].[ProjectOnHoldStatus] ([Id]),
    CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectPhases_Phase_Id] FOREIGN KEY ([Phase_Id]) REFERENCES [dbo].[ProjectPhases] ([Id]),
    CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.ProjectRAGStatus_RAGStatus_Id] FOREIGN KEY ([RAGStatus_Id]) REFERENCES [dbo].[ProjectRAGStatus] ([Id]),
    CONSTRAINT [FK_dbo.ProjectUpdateItems_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_RAGStatus_Id]
    ON [dbo].[ProjectUpdateItems]([RAGStatus_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Phase_Id]
    ON [dbo].[ProjectUpdateItems]([Phase_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Person_Id]
    ON [dbo].[ProjectUpdateItems]([Person_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OnHoldStatus_Id]
    ON [dbo].[ProjectUpdateItems]([OnHoldStatus_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[ProjectUpdateItems]([Project_Id] ASC);

