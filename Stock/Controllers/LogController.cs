using Stock.Models;
using Stock.Models.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Stock.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class LogController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllLog()
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                   return Request.CreateResponse(HttpStatusCode.OK, entities.Log.ToList());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }

    }
}
