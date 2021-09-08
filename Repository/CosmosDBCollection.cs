using CoreWebApiDemo1.BO;
using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public async Task<Family> AddItemsToContainerAsync(Family familyResponse)
        {
            ItemResponse<Family> itemResponse;
            try
            {
                 itemResponse = await this._container.CreateItemAsync<Family>(familyResponse, new PartitionKey(familyResponse.Job));

            }
            catch (CosmosException ex)
            {
                throw ex;
            }

            return itemResponse.Resource;
        }

        public async Task<List<Family>> GetItemsFromContainer(Family family)
        {

           var families = await QueryItemsAsync(family);
            if (families.ToList().Count == 0)
            {
                Family fam = await AddItemsToContainerAsync(family);
                Thread.Sleep(3000);
                families.Add(fam);
            }

            return families;
             

        }
        public  async Task<List<Family>> QueryItemsAsync(Family response)
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.Email = '"+response.Email+"'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Family> queryResultSetIterator = this._container.GetItemQueryIterator<Family>(queryDefinition);

            List<Family> families = new List<Family>();
            ValidateFamilyRecord validateFamilyRecord = new ValidateFamilyRecord();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Family> currentResultSet = await queryResultSetIterator.ReadNextAsync(); 
                foreach (Family family in currentResultSet)
                {
                    
                    bool status= validateFamilyRecord.CheckIfRecordByEmail(family, response.Id);
                    if (status)
                    {
                        if(family.BaseLocation==null)
                        {
                            family.BaseLocation = response.BaseLocation;
                        }
                        families.Add(family);
                    }
                }
            }
            return families;


        }
    }
}
