using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.IRepository
{
    public interface IGetKeyVaultSecret
    {
        string GetVaultValue();
        void DBInstance(string secret);
    }
}
