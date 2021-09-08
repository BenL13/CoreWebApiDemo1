using CoreWebApiDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.IRepository
{
    public interface ICosmosDBCollectionRepository
    {
        Task<Family> AddItemsToContainerAsync(Family familyResponse);
        Task<List<Family>> GetItemsFromContainer(Family familyResponse);
        Task<List<Family>> QueryItemsAsync(Family response);
    }
}
