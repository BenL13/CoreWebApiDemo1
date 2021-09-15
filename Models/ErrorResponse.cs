using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.Models
{
    public class ErrorResponse:Exception
    {
        public int Status { get; set; } = 400;

        public object Value { get; set; }

        public ErrorResponse(string value)
        {
            this.Value = value;
        }
    }
}
