using CoreWebApiDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.IRepository
{
    public interface ICosmosDBCollectionRepository
    {
        void AddItemsToContainerAsync(Family familyResponse);
        void GetItemsFromContainer(Family familyResponse);
    }
}
