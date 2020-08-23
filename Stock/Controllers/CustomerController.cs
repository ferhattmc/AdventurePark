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
        public HttpResponseMessage Login([FromBody] USERS user)
        {
            try{
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.USERS.FirstOrDefault(c => c.USER_CODE == user.USER_CODE && c.PASSWORD == user.PASSWORD );
                    
                    if (entity != null)
                    {
                        //if(entity.UserRoleId != 0)
                            return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Success(entity));
                        //else
                         //   return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Kullanıcının Sisteme Girmesi için Bölüm Görevlisinin İşlemi Onaylaması gerekmektedir."));

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Email veya Şifre Yanlış"));
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
        public HttpResponseMessage AdminLogin([FromBody] USERS user)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.USERS.FirstOrDefault(c => c.USER_CODE == user.USER_CODE && c.PASSWORD == user.PASSWORD && c.IS_SUPER_USER ==user.IS_SUPER_USER );

                    if (entity != null)
                    {
                        //if(entity.UserRoleId != 0)
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Success(entity));
                        //else
                        //   return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Kullanıcının Sisteme Girmesi için Bölüm Görevlisinin İşlemi Onaylaması gerekmektedir."));

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Admin Şifresi Yanlış"));
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

        [HttpGet]
        public HttpResponseMessage GetCurrencyById(int id)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.KURS.FirstOrDefault(c => c.ID == id);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Currency with id=" + id.ToString() + "not found!");
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
        public HttpResponseMessage SetCurrency([FromBody] KURS currency)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.KURS.FirstOrDefault(c => c.ID == currency.ID);
                    if (entity != null)
                    {
                        entity.TL_KARSILIK = currency.TL_KARSILIK;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Sisteme kur girişi başarıyla tamamlandı");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, " !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }

        /*
        [HttpPost]
        public HttpResponseMessage Register([FromBody] Customers customer)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entityCustomer = entities.Customers.FirstOrDefault(c => c.Username == customer.Username);
                    if(entityCustomer == null)
                    {
                        entities.Customers.Add(customer);
                        entities.SaveChanges();
                        var entity = entities.Customers.FirstOrDefault(c => c.Username == customer.Username && c.UserRoleId == 0);
                        if(entity.CompanyId == 0)
                        {
                            Company c = new Company();
                            Department d = new Department();
                            d.DepartmentName = entity.Username;
                            entities.Department.Add(d);
                            entities.SaveChanges();
                            var entityDepartment = entities.Department.Max(department => department.DepartmentId);
                            entity.DepartmentId = entityDepartment;
                            CompanyController CompanyController = new CompanyController();
                            c.CompanyName = entity.Username;
                            entity.CompanyId = CompanyController.Add(c);
                            entity.UserRoleId = 2;
                            entities.SaveChanges();
                        }
                        var entityAdmin = entities.Customers.FirstOrDefault(c => c.UserRoleId == 1);
                        if (entity != null && entityAdmin != null && entity.UserRoleId == 0)
                        {
                            Approve app = new Approve()
                            {
                                Approver = entityAdmin.Id,
                                CustomerId = entity.Id,
                                Process = "Kullanıcı Kaydı",
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                Result = 0,
                            };
                            entities.Approve.Add(app);
                            entities.SaveChanges();
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Email ile zaten kayıt olunmuş.Eğer giriş yapamıyorsanız onayı bekleyiniz!."));
                    }
                  
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
        */
        [HttpGet]
        public HttpResponseMessage GetCustomerById(int id)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.USERS.FirstOrDefault(c => c.ID == id);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id=" + id.ToString() + "not found!");
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
        public HttpResponseMessage GetCustomerByName([FromBody] USERS user)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.USERS.Where(c => c.NAME.Contains(user.NAME));
                    if (entity != null)
                    {
                       return Request.CreateResponse(HttpStatusCode.OK, entity.ToList());
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "İlgili kişi bulunamadı!");
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
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.USERS.ToList();
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }
        /*
        [HttpGet]
        public HttpResponseMessage GetAllApproveByDepartment(int id)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entityCustomer = entities.USERS.Where(d => d.DepartmentId == id && d.UserRoleId == 0);
                    if (entityCustomer != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entityCustomer.ToList());
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Onay not list");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }
        */
        /*
        [HttpPost]
        public HttpResponseMessage Approve([FromBody] Customers customer)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Customers.FirstOrDefault(c => c.Id == customer.Id);
                    if (entity != null)
                    {
                        entity.UserRoleId = 2;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Başarılı");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, " !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }
        */
        /*
        [HttpPost]
        public HttpResponseMessage Reject([FromBody] Customers customer)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Customers.FirstOrDefault(c => c.Id == customer.Id);
                    if (entity != null)
                    {
                        entities.Customers.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Başarılı Silindi");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, " !");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }
        */

    }
}
