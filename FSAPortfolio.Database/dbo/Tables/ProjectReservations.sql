CREATE TABLE [dbo].[ProjectReservations] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Portfolio_Id] INT           NOT NULL,
    [ProjectId]    NVARCHAR (20) NULL,
    [Year]         INT           NOT NULL,
    [Month]        INT           NOT NULL,
    [Index]        INT           NOT NULL,
    [ReservedAt]   DATETIME      NOT NULL,
    CONSTRAINT [PK_dbo.ProjectReservations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProjectReservations_dbo.Portfolios_Portfolio_Id] FOREIGN KEY ([Portfolio_Id]) REFERENCES [dbo].[Portfolios] ([Id])
);




GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProjectId]
    ON [dbo].[ProjectReservations]([ProjectId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Portfolio_Id_Year_Month_Index]
    ON [dbo].[ProjectReservations]([Portfolio_Id] ASC, [Year] ASC, [Month] ASC, [Index] ASC);

