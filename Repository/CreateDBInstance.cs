using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using Microsoft.Azure.Cosmos;   
using Microsoft.Extensions.Options;

namespace CoreWebApiDemo1.Repository
{
    public class CreateDBInstance : ICreateDBInstance
    {

        public CosmosClient _cosmosClient;
        public Database database;
        public Container container;
        public EnvironmentConfig appsettings;

        public CreateDBInstance(EnvironmentConfig app,string secret)
        {
            appsettings = app;
            _cosmosClient = new CosmosClient(secret);
        }
        public async void CreateContainerAsync()
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(appsettings.CosmosContainerName, "/Job");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
            throw new NotImplementedException();
        }

        public async void CreateDatabaseAsync()
        {
            this.database = await this._cosmosClient.CreateDatabaseIfNotExistsAsync(appsettings.CosmosDatabaseName);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
            throw new NotImplementedException();
        }
    }
}
