using CoreWebApiDemo1.IRepository;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using CoreWebApiDemo1.Models;
using System.Collections.ObjectModel;

namespace CoreWebApiDemo1.Repository
{
    public class CreateDBUsingDocument : ICreateDBUsingDocumentRepository
    {
        public DocumentClient _documentClient;
        public Database database;
        public DocumentCollection collection;
        public EnvironmentConfig appsettings;

        public CreateDBUsingDocument(IOptions<EnvironmentConfig> app, DocumentClient documentClient)
        {
            _documentClient = documentClient;
            appsettings = app.Value;
        }
        public async Task CreateCollectionsAsync()
        {
            var collectionDefinition = new DocumentCollection { Id = appsettings.CosmosContainerName, PartitionKey = new PartitionKeyDefinition
            {
                Paths = new Collection<string> { "/Job" }
            }
            };
            collection = await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(appsettings.CosmosDatabaseName), collectionDefinition);

            
        }

        public async Task CreateDatabaseAsync()
        {
            var databaseDefinition = new Database { Id = appsettings.CosmosDatabaseName };
            database = await _documentClient.CreateDatabaseIfNotExistsAsync(databaseDefinition);
          
        }
    }
}
