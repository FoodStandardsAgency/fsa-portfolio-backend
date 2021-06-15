-- Clear priority_main from the filter labels (not used).
UPDATE dbo.PortfolioLabelConfigs SET Flags = (Flags & ~128) WHERE FieldName = 'priority_main' -- orig value = 143

UPDATE dbo.PortfolioLabelConfigs SET Flags = (Flags | 0x800) WHERE FieldName IN (
'risk_rating', 
'project_size', 
'budgettype', 
'cost_centre', 
'budget_option1', 
'budget_option2',
'fsaproc_assurance_gatenumber',
'fsaproc_assurance_nextgate',
'processes_setting1',
'processes_setting2',
'processes_option1',
'processes_option2'
)