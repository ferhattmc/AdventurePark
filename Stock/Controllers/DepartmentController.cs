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
    /*
    [EnableCorsAttribute("*", "*", "*")]
    public class DepartmentController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetDepartments()
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Department.ToList();
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


        [HttpGet]
        public HttpResponseMessage GetDepartmentNameById(int id)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {

                    var entity = entities.Department.FirstOrDefault(d => d.DepartmentId==id);
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
        
    }*/
}
