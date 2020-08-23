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
{/*
    [EnableCorsAttribute("*", "*", "*")]
    public class CompanyController : ApiController
    {
        [HttpPost]
        public int Add([FromBody] Company company)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    entities.Company.Add(company);
                    entities.SaveChanges();
                    var entity = entities.Company.Max(c => company.CompanyId);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                return -1;
                throw;
            }

        }
        [HttpGet]
        public HttpResponseMessage GetCompanies()
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Company.ToList();
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
