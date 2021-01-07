CREATE VIEW [dbo].[vwPortfolioPhases]
	AS SELECT
	po.IDPrefix as Portfolio,
	pph.[Order],
	pph.ViewKey,
	pph.Name,
	CASE 
		WHEN pph.Id = pc.CompletedPhase_Id THEN 'Archived'
		WHEN pph.Id = pc.ArchivePhase_Id THEN 'Archivable'
		ELSE 'Normal'
	END AS PhaseType
  FROM [dbo].[Portfolios] po
  LEFT JOIN [dbo].[PortfolioConfigurations] pc ON po.Id = pc.Portfolio_Id
  LEFT JOIN [dbo].[ProjectPhases] pph ON pph.Configuration_Id = pc.Portfolio_Id
