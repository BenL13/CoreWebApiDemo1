using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.Repository
{
    public class DocumentDBCollection: IDocumentDBCollectionRepository
    {
        private DocumentClient _documentClient;
        private readonly IOptions<EnvironmentConfig> appSettings;
        private readonly IConfiguration _configuration;
        public DocumentDBCollection(IOptions<EnvironmentConfig> app, IConfiguration configuration)
        {
            appSettings = app;
            _configuration = configuration;
            _documentClient = new DocumentClient(new Uri(_configuration["DBEndpoint"]), _configuration["Key"]);

        }

        public async Task<List<Family>>  CreateDocumentInDBCollection(Family family)
        {
           
            var documentResponse = await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(appSettings.Value.CosmosDatabaseName, appSettings.Value.CosmosContainerName),
                family);
            List<Family> families = new List<Family>();
            families.Add(JsonConvert.DeserializeObject<Family>(documentResponse.Resource.ToString()));
            return families;
        }
        
    }
}
