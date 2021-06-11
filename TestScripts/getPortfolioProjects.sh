curl -s "$apiUrl/Projects?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" 
#curl -s "$apiUrl/Projects?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" > "${outputDir}/${apiPortfolioKey}_projects.json"
