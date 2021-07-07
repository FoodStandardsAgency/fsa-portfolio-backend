projectid=DV2103009
#curl -s "$apiUrl/Projects/$projectid/edit" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" 
curl -s "$apiUrl/Projects/$projectid/edit" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" > "${outputDir}/${apiPortfolioKey}_${projectid}.json"
