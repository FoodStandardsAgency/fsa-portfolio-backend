CREATE TABLE [dbo].[Milestones] (
    [Id]                            INT            IDENTITY (1, 1) NOT NULL,
    [Project_ProjectReservation_Id] INT            NOT NULL,
    [Name]                          NVARCHAR (250) NULL,
    [Deadline_Date]                 DATETIME       NULL,
    [Deadline_Flags]                INT            NOT NULL,
    [Order]                         INT            NOT NULL,
    CONSTRAINT [PK_dbo.Milestones] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Milestones_dbo.Projects_Project_ProjectReservation_Id] FOREIGN KEY ([Project_ProjectReservation_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Project_ProjectReservation_Id]
    ON [dbo].[Milestones]([Project_ProjectReservation_Id] ASC);

