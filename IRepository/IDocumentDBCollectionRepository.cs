using CoreWebApiDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.IRepository
{
    public interface IDocumentDBCollectionRepository
    {
        Task<List<Family>> CreateDocumentInDBCollection(Family family);
    }
}
