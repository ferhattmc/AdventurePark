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
    public class CurrencyController : ApiController
    {
        [EnableCorsAttribute("*", "*", "*")]
        public class CompanyController : ApiController
        {
            [HttpGet]
            public HttpResponseMessage GetCurrency()
            {
                try
                {
                    using (ParkursAPIEntities entities = new ParkursAPIEntities())
                    {
                        var entity = entities.KURS.ToList();
                        if (entity != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, entity);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not list");
                        }
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
}
