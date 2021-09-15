using CoreWebApiDemo1.Models;
using CoreWebApiDemo1.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [TypeFilter(typeof(HttpResponseExceptionFilterAttribute))]
        [HttpGet]
        public IActionResult Filter()
        {
            throw new ErrorResponse("Testing custom exception filter.");
        }
    }
}
