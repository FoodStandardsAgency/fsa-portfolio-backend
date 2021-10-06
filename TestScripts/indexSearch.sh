curl -s "$apiUrl/SearchIndex/search" -X POST -d term=$1 -H "AdminAPIKey: $apiKeyAdmin" -H "Authorization: Bearer $access_token" 

