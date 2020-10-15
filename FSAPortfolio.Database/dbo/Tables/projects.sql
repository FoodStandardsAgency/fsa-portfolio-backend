CREATE TABLE [dbo].[Projects] (
    [ProjectReservation_Id] INT             NOT NULL,
    [Name]                  NVARCHAR (250)  NULL,
    [Description]           NVARCHAR (1000) NULL,
    [Directorate]           NVARCHAR (150)  NULL,
    [ProjectCategory_Id]    INT             NULL,
    [ProjectSize_Id]        INT             NULL,
    [BudgetType_Id]         INT             NULL,
    [Funded]                INT             NOT NULL,
    [Confidence]            INT             NOT NULL,
    [Priorities]            INT             NOT NULL,
    [Benefits]              INT             NOT NULL,
    [Criticality]           INT             NOT NULL,
    [Lead_Id]               INT             NULL,
    [ServiceLead_Id]        INT             NULL,
    [Team]                  NVARCHAR (500)  NULL,
    [Priority]              INT             NULL,
    [StartDate]             DATETIME        NULL,
    [ActualStartDate]       DATETIME        NULL,
    [ExpectedEndDate]       DATETIME        NULL,
    [HardEndDate]           DATETIME        NULL,
    [LatestUpdate_Id]       INT             NULL,
    [FirstUpdate_Id]        INT             NULL,
    CONSTRAINT [PK_dbo.Projects] PRIMARY KEY CLUSTERED ([ProjectReservation_Id] ASC),
    CONSTRAINT [FK_dbo.Projects_dbo.BudgetTypes_BudgetType_Id] FOREIGN KEY ([BudgetType_Id]) REFERENCES [dbo].[BudgetTypes] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.People_Lead_Id] FOREIGN KEY ([Lead_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.People_ServiceLead_Id] FOREIGN KEY ([ServiceLead_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.ProjectCategories_ProjectCategory_Id] FOREIGN KEY ([ProjectCategory_Id]) REFERENCES [dbo].[ProjectCategories] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.ProjectReservations_ProjectReservation_Id] FOREIGN KEY ([ProjectReservation_Id]) REFERENCES [dbo].[ProjectReservations] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.ProjectSizes_ProjectSize_Id] FOREIGN KEY ([ProjectSize_Id]) REFERENCES [dbo].[ProjectSizes] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.ProjectUpdateItems_FirstUpdate_Id] FOREIGN KEY ([FirstUpdate_Id]) REFERENCES [dbo].[ProjectUpdateItems] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.ProjectUpdateItems_LatestUpdate_Id] FOREIGN KEY ([LatestUpdate_Id]) REFERENCES [dbo].[ProjectUpdateItems] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FirstUpdate_Id]
    ON [dbo].[Projects]([FirstUpdate_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LatestUpdate_Id]
    ON [dbo].[Projects]([LatestUpdate_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ServiceLead_Id]
    ON [dbo].[Projects]([ServiceLead_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Lead_Id]
    ON [dbo].[Projects]([Lead_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BudgetType_Id]
    ON [dbo].[Projects]([BudgetType_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectSize_Id]
    ON [dbo].[Projects]([ProjectSize_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectCategory_Id]
    ON [dbo].[Projects]([ProjectCategory_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProjectReservation_Id]
    ON [dbo].[Projects]([ProjectReservation_Id] ASC);

