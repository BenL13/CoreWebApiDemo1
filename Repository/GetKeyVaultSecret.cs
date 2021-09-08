using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiDemo1.IRepository;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using CoreWebApiDemo1.Models;
using Microsoft.Extensions.Options;

namespace CoreWebApiDemo1.Repository
{
    public class GetKeyVaultSecret : IGetKeyVaultSecret
    {
        public readonly EnvironmentConfig appSettings;

        public GetKeyVaultSecret()
        {
        }

        public GetKeyVaultSecret(IOptions<EnvironmentConfig> app)
        {
            appSettings = app.Value;
            

        }
        public void DBInstance(string secret)
        {
            var _createDatabaseAndConatiners = new CreateDBInstance(appSettings, secret);
            _createDatabaseAndConatiners.CreateContainerAsync();
            _createDatabaseAndConatiners.CreateDatabaseAsync();
            throw new NotImplementedException();
        }

        public string GetVaultValue()
        {
            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            string vaultBaseUrl = appSettings.vaultBaseUrl;
            string secretName = appSettings.secretName;
            var secret = client.GetSecretAsync(vaultBaseUrl, secretName).GetAwaiter().GetResult();
            return secret.Value;
        }

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {

            string clientId = appSettings.ClientID;// Configuration["ClientID"];
            string clientSecret = appSettings.ClientSecret;

            var credential = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, credential).ConfigureAwait(false);
            return result.AccessToken;
        }
    }
}
