projectid=DV2103009
curl -s "$apiUrl/SearchIndex/$projectid/index" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" 
