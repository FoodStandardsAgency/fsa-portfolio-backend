WITH recentUpdate AS (
SELECT [Id]
      ,[Project_Id]
      ,[Timestamp]
      ,[Text]
      ,[SyncId]
	  ,row_num = ROW_NUMBER() OVER(PARTITION BY [Project_Id], CONVERT(date, [Timestamp]) ORDER BY [Timestamp] DESC)
  FROM [dbo].[ProjectUpdateItems] WHERE [Text] IS NOT NULL)
SELECT r.ProjectId,  ru.* 
FROM recentUpdate ru
	JOIN [dbo].[ProjectReservations] r ON r.Id = ru.Project_Id
	JOIN [dbo].[ProjectUpdateItems] up ON up.SyncId = ru.SyncId
WHERE ru.row_num > 1
ORDER BY [Timestamp] DESC


WITH recentUpdate AS (
SELECT [Id]
      ,[Project_Id]
      ,[Timestamp]
      ,[Text]
      ,[SyncId]
	  ,row_num = ROW_NUMBER() OVER(PARTITION BY [Project_Id], CONVERT(date, [Timestamp]) ORDER BY [Timestamp] DESC)
  FROM [dbo].[ProjectUpdateItems] WHERE [Text] IS NOT NULL)
UPDATE up SET [Text] = NULL 
FROM recentUpdate ru
	JOIN [dbo].[ProjectReservations] r ON r.Id = ru.Project_Id
	JOIN [dbo].[ProjectUpdateItems] up ON up.SyncId = ru.SyncId
WHERE ru.row_num > 1