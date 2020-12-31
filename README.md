# Authenticating to Azure SQL DB by Managed Identity

A customer was concerned about putting account credentials for their Azure SQL DBs and Synapse on the VMs that process data. While there are a number of solutions, one potential is to enable a Managed Identity on the VM and use it to authenticate to the database. This sample verifies that this method works.

## Create a user

You must enable Azure Directory administration on the Azure SQL instances first: <https://docs.microsoft.com/en-us/azure/azure-sql/database/authentication-aad-configure?tabs=azure-powershell>.

Then you can create a user in the database like so...

```sql
CREATE USER [pelasne-sql-vm] FROM EXTERNAL PROVIDER;
EXEC sp_addrolemember 'db_datareader', 'pelasne-sql-vm';
```

The name of the user account must be the name of the Managed Identity. If this is a system-managed-identity, it will typically be the name of the VM.

## Use Metadata Endpoint instead of AzureServiceTokenProvider

The included sample code shows how easy it is to use AzureServiceTokenProvider to get a token and authenticate with it. However, if you are using something other than dotnetcore or cannot use AzureServiceTokenProvider for some other reason, you can also get the token from the local Metadata Endpoint on the VM.

This shows how to use curl to get the token...

```bash
curl 'http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https%3A%2F%2Fdatabase.windows.net' -H Metadata:true
```
