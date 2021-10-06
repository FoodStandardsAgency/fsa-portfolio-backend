projectid=DV2103009
curl -s "$apiUrl/SearchIndex/rebuild" -X GET -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" 
