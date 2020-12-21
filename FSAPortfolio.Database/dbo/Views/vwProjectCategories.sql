CREATE VIEW [dbo].[vwProjectCategories] AS
SELECT
	po.ShortName as Portfolio,
	rv.ProjectId,
	pr.[Name],
	pcat.[Name] as Category,
	psubcat.[Name] as SubCategory
FROM [dbo].[ProjectReservations] rv
	JOIN [dbo].[Portfolios] po ON rv.Portfolio_Id = po.Id
	JOIN [dbo].[Projects] pr ON pr.ProjectReservation_Id = rv.Id
	LEFT JOIN [dbo].[ProjectCategories] pcat ON pr.ProjectCategory_Id = pcat.Id
	LEFT JOIN [dbo].[ProjectSubcategories] psubcatlink ON psubcatlink.Project_Id = rv.Id
	LEFT JOIN [dbo].[ProjectCategories] psubcat ON psubcatlink.Subcategory_Id = psubcat.Id
