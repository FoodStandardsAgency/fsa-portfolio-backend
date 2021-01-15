echo Acquiring access token...
response=$(curl "$apiUrl/Token" -d username=$apiUser -d password=$apiUserPasswordHash -d grant_type=password -s)
access_token=$(echo $response | jq -r '.access_token')
echo access_token:
echo $access_token
