CREATE TABLE [dbo].[ProjectSubcategories] (
    [Project_Id]     INT NOT NULL,
    [Subcategory_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.ProjectSubcategories] PRIMARY KEY CLUSTERED ([Project_Id] ASC, [Subcategory_Id] ASC),
    CONSTRAINT [FK_dbo.ProjectSubcategories_dbo.ProjectCategories_Subcategory_Id] FOREIGN KEY ([Subcategory_Id]) REFERENCES [dbo].[ProjectCategories] ([Id]),
    CONSTRAINT [FK_dbo.ProjectSubcategories_dbo.Projects_Project_Id] FOREIGN KEY ([Project_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Subcategory_Id]
    ON [dbo].[ProjectSubcategories]([Subcategory_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Project_Id]
    ON [dbo].[ProjectSubcategories]([Project_Id] ASC);

