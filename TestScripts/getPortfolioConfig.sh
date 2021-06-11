#curl -s $apiUrl/PortfolioConfiguration?portfolio=$apiPortfolioKey -X GET -H "PowerBIAPIKey: $apiKeyPowerBI" > "${outputDir}/${apiPortfolioKey}_config.json"
curl $apiUrl/PortfolioConfiguration?portfolio=$apiPortfolioKey -X GET -H "PowerBIAPIKey: $apiKeyPowerBI"
