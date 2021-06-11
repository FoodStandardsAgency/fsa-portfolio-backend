curl -s "$apiUrl/Projects/updates?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" > "${outputDir}/${apiPortfolioKey}_updates.json"
#curl -s "$apiUrl/Projects/updates?portfolio=$apiPortfolioKey" -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" > "${outputDir}/${apiPortfolioKey}_updates.json"
