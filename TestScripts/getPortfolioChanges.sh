curl -s "$apiUrl/Projects/changes?portfolio=$apiPortfolioKey" -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" > "${outputDir}/${apiPortfolioKey}_changes.json"
#curl -s "$apiUrl/Projects/changes?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" > "${outputDir}/${apiPortfolioKey}_changes.json"
