CREATE TABLE [dbo].[PortfolioLabelConfigs] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Configuration_Id] INT            NOT NULL,
    [Flags]            INT            NOT NULL,
    [FieldName]        NVARCHAR (50)  NULL,
    [FieldTitle]       NVARCHAR (50)  NULL,
    [FieldOrder]       INT            NOT NULL,
    [Included]         BIT            NOT NULL,
    [IncludedLock]     BIT            NOT NULL,
    [AdminOnly]        BIT            NOT NULL,
    [AdminOnlyLock]    BIT            NOT NULL,
    [Label]            NVARCHAR (50)  NULL,
    [FieldType]        INT            NOT NULL,
    [FieldTypeLocked]  BIT            NOT NULL,
    [FieldOptions]     NVARCHAR (MAX) NULL,
    [MasterLabel_Id]   INT            NULL,
    [Group_Id]         INT            NULL,
    CONSTRAINT [PK_dbo.PortfolioLabelConfigs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioConfigurations_Configuration_Id] FOREIGN KEY ([Configuration_Id]) REFERENCES [dbo].[PortfolioConfigurations] ([Portfolio_Id]),
    CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioLabelConfigs_MasterLabel_Id] FOREIGN KEY ([MasterLabel_Id]) REFERENCES [dbo].[PortfolioLabelConfigs] ([Id]),
    CONSTRAINT [FK_dbo.PortfolioLabelConfigs_dbo.PortfolioLabelGroups_Group_Id] FOREIGN KEY ([Group_Id]) REFERENCES [dbo].[PortfolioLabelGroups] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Group_Id]
    ON [dbo].[PortfolioLabelConfigs]([Group_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MasterLabel_Id]
    ON [dbo].[PortfolioLabelConfigs]([MasterLabel_Id] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Id_FieldTitle]
    ON [dbo].[PortfolioLabelConfigs]([Configuration_Id] ASC, [FieldTitle] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Id_FieldName]
    ON [dbo].[PortfolioLabelConfigs]([Configuration_Id] ASC, [FieldName] ASC);

