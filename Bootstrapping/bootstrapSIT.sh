echo Initialising access groups...
curl -s $apiUrl/AccessGroups/init -H "TestAPIKey: $apiKey"

echo Creating SIT superuser...
curl -s $apiUrl/Users/create -X POST -d userName=$apiUser -d password=$apiUserPassword -d accessgroup=$apiUserAccessGroup -H "TestAPIKey: $apiKey" 
source getToken.sh

echo Creating SIT editor...
curl -s $apiUrl/Users/create -X POST -d userName=editor -d password=$apiUserPassword -d accessgroup=fsa -H "TestAPIKey: $apiKey" 

echo Creating SIT reader...
curl -s $apiUrl/Users/create -X POST -d userName=reader -d password=$apiUserPassword -d accessgroup=fsa -H "TestAPIKey: $apiKey" 

echo Creating a new portfolio...
curl -s $apiUrl/NewPortfolio -X POST --data "{ \"viewkey\": \"$apiPortfolioKey\", \"abbr\": \"$apiPortfolioKey\", \"name\": \"$apiPortfolioName\", \"description\": \"$apiPortfolioName\" }" -H "TestAPIKey: $apiKey" -H "Authorization: Bearer $access_token" -H "Content-Type: application/json" 

echo Giving user permission on the portfolio...
curl -s $apiUrl/Portfolios/$apiPortfolioKey/permission -X POST --data "{ \"userName\": \"$apiUser\", \"permission\": \"$apiUserPortfolioRole\" }" -H "TestAPIKey: $apiKey" -H "Authorization: Bearer $access_token" -H "Content-Type: application/json"
curl -s $apiUrl/Portfolios/$apiPortfolioKey/permission -X POST --data "{ \"userName\": \"editor\", \"permission\": \"Editor\" }" -H "TestAPIKey: $apiKey" -H "Authorization: Bearer $access_token" -H "Content-Type: application/json"
curl -s $apiUrl/Portfolios/$apiPortfolioKey/permission -X POST --data "{ \"userName\": \"reader\", \"permission\": \"Read\" }" -H "TestAPIKey: $apiKey" -H "Authorization: Bearer $access_token" -H "Content-Type: application/json"
