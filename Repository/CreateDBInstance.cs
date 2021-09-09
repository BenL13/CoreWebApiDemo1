using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using Microsoft.Azure.Cosmos;   
using Microsoft.Extensions.Options;
using Unity;

namespace CoreWebApiDemo1.Repository
{
    public class CreateDBInstance : ICreateDBInstance
    {

        public CosmosClient _cosmosClient;
        public Database database;
        public Container container;
        public EnvironmentConfig appsettings;
      
        public CreateDBInstance(IOptions<EnvironmentConfig> app,CosmosClient cosmos)
        {
            appsettings = app.Value;
            _cosmosClient = cosmos;
        }
        public async Task CreateContainerAsync()
        {
            container = await database.CreateContainerIfNotExistsAsync(appsettings.CosmosContainerName, "/Job");
            
            
        }

        public async Task CreateDatabaseAsync()
        {
            database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(appsettings.CosmosDatabaseName);
            Thread.Sleep(3000);
            
        }
    }
}
