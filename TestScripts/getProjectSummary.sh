#curl -s "$apiUrl/Portfolios/$apiPortfolioKey/summary" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_summary.json"
curl -v -G "$apiUrl/Portfolios/$apiPortfolioKey/summary" --data-urlencode "user=sally.barber@food.gov.uk" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" 
