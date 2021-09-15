using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace CoreWebApiDemo1
{
    public class Program
    {
        private static IUnityContainer _container;

        public static void Main(string[] args)
        {
            _container = new UnityContainer();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseUnityServiceProvider(_container)
            .ConfigureAppConfiguration((context, builder) =>
            {

                var root = builder.Build();
                var keyVaultEndpoint = root["KeyVault:vaultBaseUrl"];
                if (!string.IsNullOrEmpty(keyVaultEndpoint))
                {
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                      new KeyVaultClient.AuthenticationCallback(
                         azureServiceTokenProvider.KeyVaultTokenCallback));
                    builder.AddAzureKeyVault(
                     keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                }
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                });
    }
}
