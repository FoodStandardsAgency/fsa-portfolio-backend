curl -s $apiUrl//Portfolios/{$apiPortfolioKey}/summary -X GET -H "APIKey: $apiKey" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_summary_config.json"
