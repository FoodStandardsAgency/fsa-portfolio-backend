DELETE rv 
FROM [dbo].[ProjectReservations] rv 
	LEFT JOIN [dbo].[Projects] pr ON pr.ProjectReservation_Id = rv.Id
WHERE pr.ProjectReservation_Id IS NULL AND DATEDIFF(day, rv.ReservedAt, GETDATE()) >= 1