CREATE TABLE [dbo].[Projects] (
    [ProjectReservation_Id]            INT             NOT NULL,
    [Name]                             NVARCHAR (250)  NULL,
    [Description]                      NVARCHAR (MAX)  NULL,
    [Theme]                            NVARCHAR (50)   NULL,
    [ProjectType]                      NVARCHAR (50)   NULL,
    [StrategicObjectives]              NVARCHAR (50)   NULL,
    [Programme]                        NVARCHAR (150)  NULL,
    [ProjectCategory_Id]               INT             NULL,
    [ProjectSize_Id]                   INT             NULL,
    [BudgetType_Id]                    INT             NULL,
    [Funded]                           INT             NOT NULL,
    [Confidence]                       INT             NOT NULL,
    [Priorities]                       INT             NOT NULL,
    [Benefits]                         INT             NOT NULL,
    [Criticality]                      INT             NOT NULL,
    [ChannelLink_Name]                 NVARCHAR (MAX)  NULL,
    [ChannelLink_Link]                 NVARCHAR (MAX)  NULL,
    [Lead_Id]                          INT             NULL,
    [LeadRole]                         NVARCHAR (MAX)  NULL,
    [Supplier]                         NVARCHAR (150)  NULL,
    [Priority]                         INT             NULL,
    [StartDate_Date]                   DATETIME        NULL,
    [StartDate_Flags]                  INT             NOT NULL,
    [ActualStartDate_Date]             DATETIME        NULL,
    [ActualStartDate_Flags]            INT             NOT NULL,
    [ExpectedEndDate_Date]             DATETIME        NULL,
    [ExpectedEndDate_Flags]            INT             NOT NULL,
    [HardEndDate_Date]                 DATETIME        NULL,
    [HardEndDate_Flags]                INT             NOT NULL,
    [ActualEndDate_Date]               DATETIME        NULL,
    [ActualEndDate_Flags]              INT             NOT NULL,
    [AssuranceGateCompletedDate_Date]  DATETIME        NULL,
    [AssuranceGateCompletedDate_Flags] INT             NOT NULL,
    [BusinessCaseNumber]               NVARCHAR (50)   NULL,
    [FSNumber]                         NVARCHAR (50)   NULL,
    [RiskRating]                       NVARCHAR (50)   NULL,
    [ProgrammeDescription]             NVARCHAR (MAX)  NULL,
    [LatestUpdate_Id]                  INT             NULL,
    [FirstUpdate_Id]                   INT             NULL,
    [Directorate_Id]                   INT             NULL,
    [KeyContact1_Id]                   INT             NULL,
    [KeyContact2_Id]                   INT             NULL,
    [KeyContact3_Id]                   INT             NULL,
    [TeamSettings_Setting1]            NVARCHAR (MAX)  NULL,
    [TeamSettings_Setting2]            NVARCHAR (MAX)  NULL,
    [TeamSettings_Option1]             NVARCHAR (MAX)  NULL,
    [TeamSettings_Option2]             NVARCHAR (MAX)  NULL,
    [PlanSettings_Setting1]            NVARCHAR (MAX)  NULL,
    [PlanSettings_Setting2]            NVARCHAR (MAX)  NULL,
    [PlanSettings_Option1]             NVARCHAR (MAX)  NULL,
    [PlanSettings_Option2]             NVARCHAR (MAX)  NULL,
    [ProgressSettings_Setting1]        NVARCHAR (MAX)  NULL,
    [ProgressSettings_Setting2]        NVARCHAR (MAX)  NULL,
    [ProgressSettings_Option1]         NVARCHAR (MAX)  NULL,
    [ProgressSettings_Option2]         NVARCHAR (MAX)  NULL,
    [BudgetSettings_Setting1]          DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [BudgetSettings_Setting2]          DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [BudgetSettings_Option1]           NVARCHAR (MAX)  NULL,
    [BudgetSettings_Option2]           NVARCHAR (MAX)  NULL,
    [ProcessSettings_Setting1]         NVARCHAR (MAX)  NULL,
    [ProcessSettings_Setting2]         NVARCHAR (MAX)  NULL,
    [ProcessSettings_Option1]          NVARCHAR (MAX)  NULL,
    [ProcessSettings_Option2]          NVARCHAR (MAX)  NULL,
    [HowToGetToGreen]                  NVARCHAR (MAX)  NULL,
    [ForwardLook]                      NVARCHAR (MAX)  NULL,
    [EmergingIssues]                   NVARCHAR (MAX)  NULL,
    [ForecastSpend]                    INT             DEFAULT ((0)) NOT NULL,
    [CostCentre]                       NVARCHAR (MAX)  NULL,
    [AssuranceGateNumber]              NVARCHAR (MAX)  NULL,
    [NextAssuranceGateNumber]          NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Projects] PRIMARY KEY CLUSTERED ([ProjectReservation_Id] ASC),
    CONSTRAINT [FK_dbo.Projects_dbo.BudgetTypes_BudgetType_Id] FOREIGN KEY ([BudgetType_Id]) REFERENCES [dbo].[BudgetTypes] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.Directorates_Directorate_Id] FOREIGN KEY ([Directorate_Id]) REFERENCES [dbo].[Directorates] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.People_KeyContact1_Id] FOREIGN KEY ([KeyContact1_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.People_KeyContact2_Id] FOREIGN KEY ([KeyContact2_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.People_KeyContact3_Id] FOREIGN KEY ([KeyContact3_Id]) REFERENCES [dbo].[People] ([Id]),
    CONSTRAINT [FK_dbo.Projects_dbo.People_Lead_Id] FOREIGN KEY ([Lead_Id]) REFERENCES [dbo].[People] ([Id]),
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


GO
CREATE NONCLUSTERED INDEX [IX_KeyContact3_Id]
    ON [dbo].[Projects]([KeyContact3_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_KeyContact2_Id]
    ON [dbo].[Projects]([KeyContact2_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_KeyContact1_Id]
    ON [dbo].[Projects]([KeyContact1_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Directorate_Id]
    ON [dbo].[Projects]([Directorate_Id] ASC);

