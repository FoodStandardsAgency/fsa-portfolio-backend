#curl -s "$apiUrl/Projects/updates?portfolio=$apiPortfolioKey&id=DV2103008" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_updates.json"
curl -s "$apiUrl/Projects/updates?portfolio=$apiPortfolioKey" -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_updates.json"
