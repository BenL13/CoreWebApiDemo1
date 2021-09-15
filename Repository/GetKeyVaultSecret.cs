using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiDemo1.IRepository;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using CoreWebApiDemo1.Models;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Services.AppAuthentication;

namespace CoreWebApiDemo1.Repository
{
    public class GetKeyVaultSecret : IGetKeyVaultSecret
    {
        public readonly IOptions<EnvironmentConfig> appSettings;

        public GetKeyVaultSecret()
        {
        }

        public GetKeyVaultSecret(IOptions<EnvironmentConfig> app)
        {
            appSettings = app;
            

        }
        public async Task DBInstance(string secret)
        {
            CosmosClient cosmosClient = new CosmosClient(secret);
            var _createDatabaseAndConatiners = new CreateDBInstance(appSettings, cosmosClient);
            await _createDatabaseAndConatiners.CreateDatabaseAsync();
            await _createDatabaseAndConatiners.CreateContainerAsync();
            
        }
        public async Task DocumentDBInstance(string endPoint,string key)
        {
            DocumentClient documentClient = new DocumentClient(new Uri(endPoint), key);
            var _createDatabaseAndConatiners = new CreateDBUsingDocument(appSettings, documentClient);
            await _createDatabaseAndConatiners.CreateDatabaseAsync();
            await _createDatabaseAndConatiners.CreateCollectionsAsync();

        }
        public string GetVaultValue()
        {
            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            string vaultBaseUrl = appSettings.Value.vaultBaseUrl;
            string secretName = appSettings.Value.secretName;
            var secret = client.GetSecretAsync(vaultBaseUrl, secretName).GetAwaiter().GetResult();
            return secret.Value;

        }

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {

            string clientId = appSettings.Value.ClientID;// Configuration["ClientID"];
            string clientSecret = appSettings.Value.ClientSecret;

            var credential = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, credential).ConfigureAwait(false);
            return result.AccessToken;
        }

        public void GetKeySecret()
        {
            AzureServiceTokenProvider tokenProvider = new AzureServiceTokenProvider();
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
            var credentail = keyVaultClient.GetSecretAsync(appSettings.Value.vaultBaseUrl,appSettings.Value.secretName).Result;
            var secret = credentail.Value.ToString();
        }
    }
}
