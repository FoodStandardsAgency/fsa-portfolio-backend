﻿UPDATE [dbo].[PortfolioLabelConfigs] SET FieldOptions = '0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20' WHERE FieldName = 'priority_main'

UPDATE [dbo].[PortfolioLabelConfigs] SET FieldOptions = '0, 1, 2, 3, 4' 
WHERE FieldName IN ('funded', 'confidence', 'priorities', 'benefits', 'criticality')
	AND FieldOptions = ''