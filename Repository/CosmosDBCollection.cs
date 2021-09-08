using CoreWebApiDemo1.BO;
using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.Repository
{
    public class CosmosDBCollection: ICosmosDBCollectionRepository
    {
        private Container _container;
        private readonly IOptions<EnvironmentConfig> appSettings;
        private IGetKeyVaultSecret _KeyValue;
        
        public CosmosDBCollection(IOptions<EnvironmentConfig> app,IGetKeyVaultSecret keyVal)
        {
            appSettings = app;
            
            CosmosClient cosmosClient = new CosmosClient(keyVal.GetVaultValue());
            this._container = cosmosClient.GetContainer(appSettings.Value.CosmosDatabaseName, appSettings.Value.CosmosContainerName);
        }
        public async void AddItemsToContainerAsync(Family familyResponse)
        {
            try
            {
                ItemResponse<Family> itemResponse = await this._container.CreateItemAsync<Family>(familyResponse, new PartitionKey(familyResponse.Job));

            }
            catch (CosmosException ex)
            {
                throw ex;
            }
        }

        public async void GetItemsFromContainer(Family family)
        {
            ItemResponse<Family> response = await this._container.ReadItemAsync<Family>(family.Id, new PartitionKey(family.Job));
            ValidateFamilyRecord valRecord = new ValidateFamilyRecord();
            bool res = valRecord.CheckIfRecordByEmail(family, response.Resource.Email);


        }
    }
}
