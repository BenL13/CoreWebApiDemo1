using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.IRepository
{
    public interface ICreateDBInstance
    {
        Task CreateDatabaseAsync();
        Task CreateContainerAsync();
    }
}
