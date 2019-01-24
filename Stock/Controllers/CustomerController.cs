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
    public class CustomerController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login([FromBody] Customers customer)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Customers.FirstOrDefault(c => c.Username == customer.Username && c.Password == customer.Password);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Success(entity));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Kullanıcı Adı veya Şifre Yanlış"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage Register([FromBody] Customers customer)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    entities.Customers.Add(customer);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Kullanıcı Kayıt Edildi."));
                    message.Headers.Location = new Uri(Request.RequestUri + customer.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }

        }

        [HttpGet]
        public HttpResponseMessage GetCustomerById(int id)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Customers.FirstOrDefault(c => c.Id == id);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer with id=" + id.ToString() + "not created!");
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
        public HttpResponseMessage GetAllCustomer()
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Customers.ToList();
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
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
