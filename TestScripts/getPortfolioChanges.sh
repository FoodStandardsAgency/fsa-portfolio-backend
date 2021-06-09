curl -s "$apiUrl/DataDump/changes?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
#curl -s "$apiUrl/DataDump/changes?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
#curl -s "$apiUrl/Projects/changes?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_changes.json"
#curl -s "$apiUrl/Projects/changes?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_changes.json"
