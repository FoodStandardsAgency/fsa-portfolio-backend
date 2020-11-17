CREATE VIEW [dbo].[vwProjectUpdates] AS
SELECT
	po.ShortName as Portfolio,
	rv.ProjectId,
	pr.[Name],
	up.[Timestamp],
	CASE 
		WHEN up.Id = pr.LatestUpdate_Id THEN 'Latest'
		WHEN up.Id = pr.FirstUpdate_Id THEN 'First'
		ELSE 'Update'
	END as [Description],
	up.Budget,
	up.Spent,
	up.PercentageComplete,
	up.ExpectedCurrentPhaseEnd_Date,
	up.Text as [Update],
	uprs.[Name] as RAG,
	upph.[Name] as Phase,
	upoh.[Name] as [Status]
FROM [dbo].[ProjectReservations] rv
	JOIN [dbo].[Portfolios] po ON rv.Portfolio_Id = po.Id
	JOIN [dbo].[Projects] pr ON pr.ProjectReservation_Id = rv.Id
	LEFT JOIN [dbo].[ProjectUpdateItems] up ON pr.ProjectReservation_Id = up.Project_Id
		LEFT JOIN [dbo].[ProjectRAGStatus] uprs ON up.RAGStatus_Id = uprs.Id
		LEFT JOIN [dbo].[ProjectPhases] upph ON up.Phase_Id = upph.Id
		LEFT JOIN [dbo].[ProjectOnHoldStatus] upoh ON up.OnHoldStatus_Id = upoh.Id
