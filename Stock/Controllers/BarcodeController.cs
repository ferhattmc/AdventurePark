
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
    /*
    [EnableCorsAttribute("*", "*", "*")]
    public class BarcodeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Add([FromBody] Product product)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Product.FirstOrDefault(p => p.BarcodeNumber == product.BarcodeNumber);
                    var entity2 = entities.BarcodeDwh.FirstOrDefault(b => b.BarcodeNumber == product.BarcodeNumber);
               
                    if (entity == null)
                    {
                        var person = entities.Customers.FirstOrDefault(p => p.Id == product.ScannedBy);
                        var department = entities.Department.FirstOrDefault(d => d.DepartmentId == product.DepartmentId);
                        Log log1 = new Log()
                        {
                            CustomerId = person.Id,
                            Name = person.Name,
                            Surname = person.Surname,
                            ProductName = product.Name,
                            Department = department.DepartmentName,
                            Process = "İlk Kayıt",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Amount = 0
                        };
                        entities.Log.Add(log1);
                        entities.Product.Add(product);
                        BarcodeDwh barcodeDwh = new BarcodeDwh()
                        {
                            BarcodeNumber = product.BarcodeNumber,
                            Name = product.Name,
                            ScannedBy = product.ScannedBy,
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Amount = product.Amount,
                            DepartmentId = product.DepartmentId,
                            Description = product.Description
                        };
                        Log log2 = new Log()
                        {
                            CustomerId = person.Id,
                            Name = person.Name,
                            Surname = person.Surname,
                            ProductName = product.Name,
                            Department = department.DepartmentName,
                            Process ="Giriş",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Amount = product.Amount
                        };
                        entities.Log.Add(log2);
                        entities.BarcodeDwh.Add(barcodeDwh);
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Ürün Kayıt Edildi."));
                        message.Headers.Location = new Uri(Request.RequestUri + product.Id.ToString());
                        return message;
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Error("Ürün Kayıtlıdır."));
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
        public HttpResponseMessage Insert([FromBody] BarcodeDwh barcode)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entityProduct = entities.Product.FirstOrDefault(p => p.BarcodeNumber == barcode.BarcodeNumber);
                    var entityBarcode = entities.BarcodeDwh.FirstOrDefault(b => b.BarcodeNumber == barcode.BarcodeNumber);
                    if (entityProduct == null)
                    {
                        var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Ürünü ilk defa girerken önce stok kaydından ekleyip sonrasında girişini yapınız!"));
                        message.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                        return message;
                    }
                    else 
                    {
                        entityBarcode.Amount += barcode.Amount;
                        entities.SaveChanges();
                        var person = entities.Customers.FirstOrDefault(p => p.Id == barcode.ScannedBy);
                        var department = entities.Department.FirstOrDefault(d => d.DepartmentId == barcode.DepartmentId);
                       
                        Log log1 = new Log()
                        {
                            CustomerId = person.Id,
                            Name = person.Name,
                            Surname = person.Surname,
                            ProductName = entityBarcode.Name,
                            Department = department.DepartmentName,
                            Process = "Giriş",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Amount = barcode.Amount
                        };
                        entities.Log.Add(log1);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Ürün başarıyla eklendi."));
                        message.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                        return message;
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
        public HttpResponseMessage Delete([FromBody] BarcodeDwh barcode)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.BarcodeDwh.FirstOrDefault(b => b.BarcodeNumber == barcode.BarcodeNumber);
                    if (entity == null)
                    {
                        var messagedelete = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Girilen ürün sistemde bulunamadı!"));
                        messagedelete.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                        return messagedelete;
                    }
                    else if(entity.Amount >= barcode.Amount)
                    {
                        entity.Amount -= barcode.Amount;
                        var person = entities.Customers.FirstOrDefault(p => p.Id == barcode.ScannedBy);
                        var department = entities.Department.FirstOrDefault(d => d.DepartmentId == barcode.DepartmentId);

                        Log log1 = new Log()
                        {
                            CustomerId = person.Id,
                            Name = person.Name,
                            Surname = person.Surname,
                            ProductName = entity.Name,
                            Department = department.DepartmentName,
                            Process = "Çıkış",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Amount = barcode.Amount
                        };
                        entities.Log.Add(log1);
                    }
                    else
                    {
                        var messagedelete = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Çıkarılmak istenen ürün yeteri kadar bulunmamakta tekrar kontrol edin!"));
                        messagedelete.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                        return messagedelete;
                    }


                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Barkodun çıkışı başarıyla yapıldı"));
                    message.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage Drop([FromBody] BarcodeDwh barcode)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.BarcodeDwh.FirstOrDefault(b => b.BarcodeNumber == barcode.BarcodeNumber);
                    var entityProduct = entities.Product.FirstOrDefault(p => p.BarcodeNumber == barcode.BarcodeNumber);
                    if (entity == null)
                    {
                        var messagedelete = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Girilen ürün sistemde bulunamadı!"));
                        messagedelete.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                        return messagedelete;
                    }
                    else
                    {
                        entities.BarcodeDwh.Remove(entity);
                        entities.Product.Remove(entityProduct);
                        var person = entities.Customers.FirstOrDefault(p => p.Id == barcode.ScannedBy);
                        var department = entities.Department.FirstOrDefault(d => d.DepartmentId == barcode.DepartmentId);
                        Log log1 = new Log()
                        {
                            CustomerId = person.Id,
                            Name = person.Name,
                            Surname = person.Surname,
                            ProductName = entity.Name,
                            Department = department.DepartmentName,
                            Process = "Silme",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Amount = 0
                        };
                        entities.Log.Add(log1);
                    }


                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Ürün Komple silindi"));
                    message.Headers.Location = new Uri(Request.RequestUri + barcode.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }
        }


        [HttpPost]
        public HttpResponseMessage GetProductByName([FromBody] Product product)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.Product.Where(c => c.Name.Contains(product.Name)&& c.DepartmentId == product.DepartmentId);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity.ToList());
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "İlgili bölümde ilgili ürün bulunamadı!");
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
        public HttpResponseMessage GetAllBarcode()
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.BarcodeDwh.ToList();
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Barcode not list");
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
        public HttpResponseMessage GetBarcodeByDepartment(int id)
        {
            try
            {
                using (StockManagamentEntities entities = new StockManagamentEntities())
                {
                    var entity = entities.BarcodeDwh.Where(c => c.DepartmentId == id);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity.ToList());
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "bulunmadı");
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
