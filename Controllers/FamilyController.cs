using CoreWebApiDemo1.IRepository;
using CoreWebApiDemo1.Models;
using CoreWebApiDemo1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreWebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HttpResponseExceptionFilter]
    public class FamilyController : ControllerBase
    {
        private IOptions<EnvironmentConfig> _appsettings;
        
        private readonly ILogger<FamilyController> _logger;
        private readonly IConfiguration _configuration;
        //private readonly DocumentClient _documentClient;
        private readonly DocumentDBCollection documentDBCollections;
        public FamilyController(ILogger<FamilyController> logger, IOptions<EnvironmentConfig> app, IConfiguration configuration)

        {
            _configuration = configuration;
            _appsettings = app;
            _logger = logger;
            documentDBCollections = new DocumentDBCollection(_appsettings, _configuration);
        }
        // GET: api/<FamilyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FamilyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]

        [HttpResponseExceptionFilter]
        public async Task<List<Family>> PostAsync(string value)
        {
            _logger.LogInformation("Recevied request with Family details");
            var content = "";
            using (var reader = new StreamReader(Request.Body))
                content = reader.ReadToEndAsync().Result;
            var response = JsonConvert.DeserializeObject<Family>(content);
            
            List<Family> familyList= new List<Family>();
            try
            {
                //CosmosDBCollection comosCollections = new CosmosDBCollection(_appsettings, _keyVault);
                //familyList = await comosCollections.GetItemsFromContainer(response);
                familyList = await documentDBCollections.CreateDocumentInDBCollection(response);
                
            }
            catch(Exception ex)
            {
                _logger.LogError("Error has occured while processing the request" + ex);
                throw ex;

            }



            return familyList;
        }

        // PUT api/<FamilyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FamilyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
