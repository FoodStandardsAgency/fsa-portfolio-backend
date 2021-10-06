curl -s "$apiUrl/SearchIndex/search" -X POST -d term=test -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" 

