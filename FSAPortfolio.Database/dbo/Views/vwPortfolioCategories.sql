CREATE VIEW [dbo].[vwPortfolioCategories]
	AS SELECT
	po.IDPrefix as Portfolio,
	pcat.[Order],
	pcat.ViewKey,
	pcat.Name
  FROM [dbo].[Portfolios] po
  INNER JOIN [dbo].[PortfolioConfigurations] pc ON po.Id = pc.Portfolio_Id
  LEFT JOIN [dbo].[ProjectCategories] pcat ON pcat.Configuration_Id = pc.Portfolio_Id
  
