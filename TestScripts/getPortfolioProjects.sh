curl -s "$apiUrl/DataDump/projects?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
#curl -s $apiUrl/Projects?portfolio=$apiPortfolioKey -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_projects.json"
