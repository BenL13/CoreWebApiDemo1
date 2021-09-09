using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.Models
{
    public class EnvironmentConfig
    {
        public string CosmosDatabaseName { get; set; }
        public string CosmosContainerName { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string resourceId { get; set; }
        public string tenantID { get; set; }
        public string ServiceBusQueueName { get; set; }
        public string vaultBaseUrl { get; set; }
        public string APPINSIGHTS_INSTRUMENTATIONKEY { get; set; }
        public string secretName { get; set; }
        public bool isDBConfigured { get; set; }
    }
}
