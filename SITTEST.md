# Backend Setup

1. Initialise a new local database (e.g. Portfolio.SIT)
	- See the bootstrapping guide for detailed steps
2. Set up a new API hosted in IIS:
	- Publish the API to a new local folder
	- Add an entry to the hosts file for the API (e.g. sitportfolioapi)
	- Add a website with a binding to that hostname
3. Run the bootstrapping script (see [Bootstrapping A Development Environment](BOOTSTRAPPING.md)). Suggestions:
	- Use a different secrets file (e.g. setSecretsSIT.sh) configured for SIT access
	- Configure to create a portfolio with different key to other environments, e.g. "sit"
	- Configure a SIT test user to have `superuser` access group and `Admin` permissions on the portfolio


# Frontend Setup

Set up a new Frontend:
1. Publish the Frontend to a new local folder
	- Set up the env file to point to the SIT backend set up above
2. Add an entry to the hosts file for the API (e.g. sitportfolio)


# Troubleshooting

### Issue: 
***Cypress fails with message:***
`Failed to deserialize the V8 snapshot blob. This can mean that the 
snapshot blob file is corrupted or missing.`

### Solution:
`npx cypress install --force`

