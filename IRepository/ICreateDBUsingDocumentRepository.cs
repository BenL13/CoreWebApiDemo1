using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.IRepository
{
    interface ICreateDBUsingDocumentRepository
    {
        Task CreateDatabaseAsync();
        Task CreateCollectionsAsync();
    }
}
