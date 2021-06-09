curl -s "$apiUrl/DataDump/config?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
curl -s "$apiUrl/DataDump/projects?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
curl -s "$apiUrl/DataDump/updates?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
curl -s "$apiUrl/DataDump/changes?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" 
