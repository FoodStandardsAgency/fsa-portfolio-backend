-- Clear priority_main from the filter labels (not used).
UPDATE dbo.PortfolioLabelConfigs SET Flags = (Flags & ~128) WHERE FieldName = 'priority_main' -- orig value = 143

-- Set filterable flag
UPDATE dbo.PortfolioLabelConfigs SET Flags = (Flags | 0x800) WHERE FieldName IN (
'risk_rating', 
'project_size', 
'budgettype', 
'budget', 
'spent', 'forecast_spend', 'budget_field1', 'budget_field2',
'budget_option1', 
'budget_option2',
'processes_option1',
'processes_option2'
)