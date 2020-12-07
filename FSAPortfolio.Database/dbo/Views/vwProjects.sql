CREATE VIEW [dbo].[vwProjects] AS
SELECT
	po.ShortName as Portfolio,
	rv.ProjectId,
	pr.[Name],
	ps.[Name] as Size,
	pc.[Name] as Category,
	bt.[Name] as BudgetType,
	pr.[Priority],
	pr.[StartDate_Date] as StartDate,
	pr.[ExpectedEndDate_Date] as ExpectedEndDate,
	pr.[HardEndDate_Date] as HardEndDate,
	lu.[Timestamp] as LastUpdated,
	lu.Budget,
	lu.Spent,
	lu.PercentageComplete,
	lu.ExpectedCurrentPhaseEnd_Date as ExpectedCurrentPhaseEnd,
	lurs.[Name] as RAG,
	luph.[Name] as Phase,
	luoh.[Name] as [Status]
FROM [dbo].[ProjectReservations] rv
	JOIN [dbo].[Portfolios] po ON rv.Portfolio_Id = po.Id
	JOIN [dbo].[Projects] pr ON pr.ProjectReservation_Id = rv.Id
	LEFT JOIN [dbo].[ProjectSizes] ps ON pr.ProjectSize_Id = ps.Id
	LEFT JOIN [dbo].[ProjectCategories] pc ON pr.ProjectCategory_Id = pc.Id
	LEFT JOIN [dbo].[BudgetTypes] bt ON pr.BudgetType_Id = bt.Id
	LEFT JOIN [dbo].[ProjectUpdateItems] lu ON pr.LatestUpdate_Id = lu.Id
		LEFT JOIN [dbo].[ProjectRAGStatus] lurs ON lu.RAGStatus_Id = lurs.Id
		LEFT JOIN [dbo].[ProjectPhases] luph ON lu.Phase_Id = luph.Id
		LEFT JOIN [dbo].[ProjectOnHoldStatus] luoh ON lu.OnHoldStatus_Id = luoh.Id
