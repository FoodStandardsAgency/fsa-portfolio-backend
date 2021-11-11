curl -s "$apiUrl/Portfolios/$apiPortfolioKey/summary" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_summary.json"
