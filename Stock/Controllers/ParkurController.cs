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
    public class ParkurController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] PARKURS_USERS parkursUsers)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    if (parkursUsers.PRICE == null)
                    {
                        var entity = entities.PARKURS.FirstOrDefault(c => c.ID == parkursUsers.PARKUR_ID);
                        if (parkursUsers.KUR_ID == 1)
                            parkursUsers.PRICE = entity.DolarPrice;
                        else if (parkursUsers.KUR_ID == 2)
                            parkursUsers.PRICE = entity.EuroPrice;
                        else if (parkursUsers.KUR_ID == 3)
                            parkursUsers.PRICE = entity.PoundPrice;
                        else if (parkursUsers.KUR_ID == 4)
                            parkursUsers.PRICE = entity.RublePrice;
                        else if (parkursUsers.KUR_ID == 5)
                            parkursUsers.PRICE = entity.TurkishLiraPrice;

                    }
                    if (parkursUsers.UPDATE_DATE == null)
                    {
                        parkursUsers.UPDATE_DATE = DateTime.Now;
                    }

                    entities.PARKURS_USERS.Add(parkursUsers);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Ürün Kayıt Edildi."));
                    message.Headers.Location = new Uri(Request.RequestUri + parkursUsers.ID.ToString());
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
        public HttpResponseMessage Update([FromBody] PARKURS parkurs)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.PARKURS.FirstOrDefault(c => c.ID == parkurs.ID);
                    if (entity != null)
                    {
                        if (parkurs.DolarPrice != 0)
                            entity.DolarPrice = parkurs.DolarPrice;
                        else if (parkurs.EuroPrice != 0)
                            entity.EuroPrice = parkurs.EuroPrice;
                        else if (parkurs.PoundPrice != 0)
                            entity.PoundPrice = parkurs.PoundPrice;
                        else if (parkurs.TurkishLiraPrice != 0)
                            entity.TurkishLiraPrice = parkurs.TurkishLiraPrice;
                        else if (parkurs.RublePrice != 0)
                            entity.RublePrice = parkurs.RublePrice;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Success(entity));
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
        [HttpGet]
        public HttpResponseMessage GetParkurs()
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.PARKURS.ToList();
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parkurs not list");
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
        public HttpResponseMessage GetParkursPrice(int id)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.PARKURS.FirstOrDefault(c => c.ID == id);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parkurs not list");
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
