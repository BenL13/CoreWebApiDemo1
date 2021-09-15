using CoreWebApiDemo1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace CoreWebApiDemo1.Controllers
{
    public class HttpResponseExceptionFilterAttribute : Attribute, IFilterMetadata
    {
        public int Order { get; } = int.MaxValue - 10;
        
        public void OnException(HttpActionExecutedContext exceptionContext)
        {
            if (exceptionContext.Exception is ErrorResponse)
            {
                var res = "Error Occurred while processing request";

                //Define the Response Message
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(res),
                    ReasonPhrase = res
                };


                //Create the Error Response

                exceptionContext.Response= response;
            }
        }
       
    }
}