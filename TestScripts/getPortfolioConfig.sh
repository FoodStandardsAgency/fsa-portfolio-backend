curl -s "$apiUrl/DataDump/config?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
#curl -s $apiUrl/PortfolioConfiguration?id=$apiPortfolioKey -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_config.json"
