using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiDemo1.Models;

namespace CoreWebApiDemo1.BO
{
    public class ValidateFamilyRecord
    {
        public bool CheckIfRecordByEmail(Family family,string ID)
        {
            bool status = false;
            if(family.Id==ID)
            {
                status= true;
            }
            return status;
        }
    }
}
