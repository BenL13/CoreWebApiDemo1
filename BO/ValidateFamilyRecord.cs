using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiDemo1.Models;

namespace CoreWebApiDemo1.BO
{
    public class ValidateFamilyRecord
    {
        public bool CheckIfRecordByEmail(Family family,string email)
        {
            bool status = false;
            if(family.Email==email)
            {
                status= true;
            }
            return status;
        }
    }
}
