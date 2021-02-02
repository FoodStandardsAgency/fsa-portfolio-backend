IF OBJECT_ID(N'tempdb..#teamMap') IS NOT NULL
BEGIN
	DROP TABLE #teamMap
END
GO
;

 WITH newteams AS (
	SELECT [Id]
		  ,[ViewKey]
		  ,[Name]
		  ,[Order]
		  ,rownum = ROW_NUMBER() OVER(PARTITION BY ViewKey ORDER BY Id)
	  FROM [dbo].[Teams]
  )
  SELECT
	t.ViewKey,
	t.Id as oldId,
	nt.Id as newId,
	nt.rownum
  INTO #teamMap
  FROM [dbo].[Teams] t
	LEFT JOIN newteams nt ON t.ViewKey = nt.ViewKey
	WHERE nt.rownum = 1 AND t.Id != nt.Id

-- Check
SELECT * FROM [dbo].[People] p JOIN #teamMap tm ON p.Team_Id = tm.oldId
SELECT * FROM [dbo].[PortfolioTeams] pt JOIN #teamMap tm ON pt.Team_Id = tm.oldId
SELECT * FROM [dbo].[Teams] t JOIN #teamMap tm ON t.Id = tm.oldId

------------------ DO THE UPDATES ------------------
-- Update people
UPDATE p SET p.Team_Id = tm.newId FROM [dbo].[People] p JOIN #teamMap tm ON p.Team_Id = tm.oldId

-- Remove Portfolio teams
DELETE pt FROM [dbo].[PortfolioTeams] pt JOIN #teamMap tm ON pt.Team_Id = tm.oldId

-- Remove teams
DELETE t FROM [dbo].[Teams] t JOIN #teamMap tm ON t.Id = tm.oldId
------------------------------------------------------

-- Check
SELECT * FROM [dbo].[People] p JOIN #teamMap tm ON p.Team_Id = tm.oldId
SELECT * FROM [dbo].[PortfolioTeams] pt JOIN #teamMap tm ON pt.Team_Id = tm.oldId
SELECT * FROM [dbo].[Teams] t JOIN #teamMap tm ON t.Id = tm.oldId

