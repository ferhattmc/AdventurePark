
using Stock.Models;
using Stock.Models.Orm;
using System;
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
        [HttpPost]
        public HttpResponseMessage DeleteProcess([FromBody] PARKURS_USERS parkursUsers)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.PARKURS_USERS.FirstOrDefault(b => b.ID == parkursUsers.ID);
                    if (entity != null)
                    {
                        entities.PARKURS_USERS.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, ResponseData.Message("Başarılı Silindi"));
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
        public HttpResponseMessage GetLogByDepartment(int id)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var entity = entities.PARKURS_USERS.ToList();
                    var currency = entities.KURS.ToList();
                    
                    if (id == 1)
                    {
                        var dateCriteria = DateTime.Now.Date.AddDays(-1);
                        var entityResult = entity.Where(i => i.UPDATE_DATE > dateCriteria);

                        return Request.CreateResponse(HttpStatusCode.OK, entityResult.ToList());
                    }
                    else if (id == 2)
                    {
                        var dateCriteria = DateTime.Now.Date.AddDays(-7);
                        return Request.CreateResponse(HttpStatusCode.OK, entity.Where(e => e.UPDATE_DATE > dateCriteria));
                    }
                    else if (id == 3)
                    {
                        var dateCriteria = DateTime.Now.Date.AddMonths(-1);
                        return Request.CreateResponse(HttpStatusCode.OK, entity.Where(e => e.UPDATE_DATE > dateCriteria));
                    }
                   
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Listelenecek işlem bulunamadı!.");
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
        public HttpResponseMessage GetAmountTl(GetIncome g)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    DateTime dateCriteria = g.Date;
                    int isMonth=0;
                    if (g.Type == "2")
                    {
                        isMonth = 1;
                        dateCriteria = dateCriteria.AddMonths(+1);
                    }
                  
                   
                    var entity = entities.PARKURS_USERS.ToList();
                    var currency = entities.KURS.ToList();

                    //if (id == 1)
                    //{
                    //    dateCriteria = DateTime.Now.Date.AddDays(-1);
                    //}
                    //else if (id == 2)
                    //{
                    //    dateCriteria = DateTime.Now.Date.AddDays(-7);
                    //}
                    //else if (id == 3)
                    //{
                    //    dateCriteria = DateTime.Now.Date.AddMonths(-1);
                    //}

                    //else
                    //{
                    //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Listelenecek işlem bulunamadı!.");
                    //}
                    Accounting a = new Accounting();
                    var entityResult = entity.Where(i => Convert.ToDateTime(i.UPDATE_DATE).Year == dateCriteria.Year && Convert.ToDateTime(i.UPDATE_DATE).Month == dateCriteria.Month && Convert.ToDateTime(i.UPDATE_DATE).Day == dateCriteria.Day);
                    if(isMonth == 1)
                    {
                        entityResult = entity.Where(i => Convert.ToDateTime(i.UPDATE_DATE).Year == dateCriteria.Year && Convert.ToDateTime(i.UPDATE_DATE).Month == dateCriteria.Month);
                    }

                    a.TotalCustomer = entityResult.Count();

                    var ziplineDolarLines = entityResult.Where(i => i.PARKUR_ID == 1 && i.KUR_ID == 1);
                    var ziplineSumDolar = ziplineDolarLines.Select(d => d.PRICE ?? 0).Sum();
                    a.ziplineSumDolar = ziplineSumDolar;

                    var duvarDolarLines = entityResult.Where(i => i.PARKUR_ID == 2 && i.KUR_ID == 1);
                    var duvarSumDolar = duvarDolarLines.Select(d => d.PRICE ?? 0).Sum();
                    a.duvarSumDolar = duvarSumDolar;

                    var parkurDolarLines = entityResult.Where(i => i.PARKUR_ID == 3 && i.KUR_ID == 1);
                    var parkurSumDolar = parkurDolarLines.Select(d => d.PRICE ?? 0).Sum();
                    a.parkurSumDolar = parkurSumDolar;

                    var sumdolar = ziplineSumDolar + duvarSumDolar + parkurSumDolar;
                    decimal ziplineSumDolarTl = Convert.ToDecimal(ziplineSumDolar * currency.FirstOrDefault(c => c.ID == 1).TL_KARSILIK);
                    decimal duvarSumDolarTl = Convert.ToDecimal(duvarSumDolar * currency.FirstOrDefault(c => c.ID == 1).TL_KARSILIK);
                    decimal parkurSumDolarTl = Convert.ToDecimal(parkurSumDolar * currency.FirstOrDefault(c => c.ID == 1).TL_KARSILIK);
                    a.DolarAmountTL = ziplineSumDolarTl + duvarSumDolarTl + parkurSumDolarTl;
                    a.DolarAmount = sumdolar;
                    


                    var ziplineEuroLines = entityResult.Where(i => i.PARKUR_ID == 1 && i.KUR_ID == 2);
                    var ziplinesumEuro = ziplineEuroLines.Select(d => d.PRICE ?? 0).Sum();
                    a.ziplineSumEuro = ziplinesumEuro;

                    var duvarEuroLines = entityResult.Where(i => i.PARKUR_ID == 2 && i.KUR_ID == 2);
                    var duvarsumEuro = duvarEuroLines.Select(d => d.PRICE ?? 0).Sum();
                    a.duvarSumEuro = duvarsumEuro;

                    var parkurEuroLines = entityResult.Where(i => i.PARKUR_ID == 3 && i.KUR_ID == 2);
                    var parkursumEuro = parkurEuroLines.Select(d => d.PRICE ?? 0).Sum();
                    a.parkurSumEuro = parkursumEuro;

                    var sumEuro = ziplinesumEuro + duvarsumEuro + parkursumEuro;
                    decimal ziplineSumEuroTl = Convert.ToDecimal(ziplinesumEuro * currency.FirstOrDefault(c => c.ID == 2).TL_KARSILIK);
                    decimal duvarSumEuroTl = Convert.ToDecimal(duvarsumEuro * currency.FirstOrDefault(c => c.ID == 2).TL_KARSILIK);
                    decimal parkurSumEuroTl = Convert.ToDecimal(parkursumEuro * currency.FirstOrDefault(c => c.ID == 2).TL_KARSILIK);
                    a.EuroAmountTL = ziplineSumEuroTl + duvarSumEuroTl + parkurSumEuroTl;
                    a.EuroAmount = sumEuro;


                    var ziplinePoundLines = entityResult.Where(i => i.PARKUR_ID == 1 && i.KUR_ID == 3);
                    var ziplinesumPaund = ziplinePoundLines.Select(d => d.PRICE ?? 0).Sum();
                    a.ziplineSumPaund = ziplinesumPaund;

                    var duvarPoundLines = entityResult.Where(i => i.PARKUR_ID == 2 && i.KUR_ID == 3);
                    var duvarsumPaund = duvarPoundLines.Select(d => d.PRICE ?? 0).Sum();
                    a.duvarSumPaund = duvarsumPaund;

                    var parkurPoundLines = entityResult.Where(i => i.PARKUR_ID == 3 && i.KUR_ID == 3);
                    var parkursumPaund = parkurPoundLines.Select(d => d.PRICE ?? 0).Sum();
                    a.parkurSumPaund = parkursumPaund;

                    var sumPound = ziplinesumPaund + duvarsumPaund + parkursumPaund;
                    decimal ziplineSumPoundTl = Convert.ToDecimal(ziplinesumPaund * currency.FirstOrDefault(c => c.ID == 3).TL_KARSILIK);
                    decimal duvarSumPoundTl = Convert.ToDecimal(duvarsumPaund * currency.FirstOrDefault(c => c.ID == 3).TL_KARSILIK);
                    decimal parkurSumPoundTl = Convert.ToDecimal(parkursumPaund * currency.FirstOrDefault(c => c.ID == 3).TL_KARSILIK);
                    a.PaundAmountTL = ziplineSumPoundTl + duvarSumPoundTl + parkurSumPoundTl;
                    a.PaundAmount = sumPound;


                    var ziplineTlLines = entityResult.Where(i => i.PARKUR_ID == 1 && i.KUR_ID == 5);
                    var ziplinesumTl = ziplineTlLines.Select(d => d.PRICE ?? 0).Sum();
                    a.ziplineSumTl = ziplinesumTl;

                    var duvarTlLines = entityResult.Where(i => i.PARKUR_ID == 2 && i.KUR_ID == 5);
                    var duvarsumTl = duvarTlLines.Select(d => d.PRICE ?? 0).Sum();
                    a.duvarSumTl = duvarsumTl;
                    
                    var parkurTlLines = entityResult.Where(i => i.PARKUR_ID == 3 && i.KUR_ID == 5);
                    var parkursumTl = parkurTlLines.Select(d => d.PRICE ?? 0).Sum();
                    a.parkurSumTl = parkursumTl;

                    a.TlAmountTL = ziplinesumTl + duvarsumTl + parkursumTl;
                    var ziplineSumTl = ziplinesumTl;
                    var duvarSumTl = duvarsumTl;
                    var parkurSumTl = parkursumTl;
                    a.TlAmount = a.TlAmountTL;
                    

                    var ziplineRubleLines = entityResult.Where(i => i.PARKUR_ID == 1 && i.KUR_ID == 4);
                    var ziplinesumRuble = ziplineRubleLines.Select(d => d.PRICE ?? 0).Sum();
                    a.ziplineSumRuble = ziplinesumRuble;

                    var duvarRubleLines = entityResult.Where(i => i.PARKUR_ID == 2 && i.KUR_ID == 4);
                    var duvarsumRuble = duvarRubleLines.Select(d => d.PRICE ?? 0).Sum();
                    a.duvarSumRuble = duvarsumRuble;

                    var parkurRubleLines = entityResult.Where(i => i.PARKUR_ID == 3 && i.KUR_ID == 4);
                    var parkursumRuble = parkurRubleLines.Select(d => d.PRICE ?? 0).Sum();
                    a.parkurSumRuble = parkursumRuble;

                    var sumRuble = ziplinesumRuble + duvarsumRuble + parkursumRuble;
                    decimal ziplineSumRubleTl = Convert.ToDecimal(ziplinesumRuble * currency.FirstOrDefault(c => c.ID == 4).TL_KARSILIK);
                    decimal duvarSumRubleTl = Convert.ToDecimal(duvarsumRuble * currency.FirstOrDefault(c => c.ID == 4).TL_KARSILIK);
                    decimal parkurSumRubleTl = Convert.ToDecimal(parkursumRuble * currency.FirstOrDefault(c => c.ID == 4).TL_KARSILIK);
                    a.RubleAmountTL = ziplineSumRubleTl + duvarSumRubleTl + parkurSumRubleTl;
                    a.RubleAmount = sumRuble;

                    a.ZiplineCustomerCount = ziplineDolarLines.Count() + ziplineEuroLines.Count() + ziplinePoundLines.Count() + ziplineTlLines.Count() + ziplineRubleLines.Count();
                    a.DuvarCustomerCount = duvarDolarLines.Count() + duvarEuroLines.Count() + duvarPoundLines.Count() + duvarTlLines.Count() + duvarRubleLines.Count();
                    a.ParkurCustomerCount = parkurDolarLines.Count() + parkurEuroLines.Count() + parkurPoundLines.Count() + parkurTlLines.Count() + parkurRubleLines.Count();
                    a.TotalCustomerCount = a.ZiplineCustomerCount + a.DuvarCustomerCount + a.ParkurCustomerCount;

                    a.ziplineFinalySumTl = ziplineSumDolarTl + ziplineSumEuroTl + ziplineSumTl + ziplineSumRubleTl;
                    a.duvarFinalySumTl = duvarSumDolarTl + duvarSumEuroTl + duvarSumTl + duvarSumRubleTl;
                    a.parkurFinalySumTl = parkurSumDolarTl + parkurSumEuroTl + parkurSumTl + parkurSumRubleTl;
                    a.FinalyTL = a.ziplineFinalySumTl + a.duvarFinalySumTl + a.parkurFinalySumTl;
                    return Request.CreateResponse(HttpStatusCode.OK, a);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw;
            }

        }

        [HttpGet]
        public HttpResponseMessage GetLogByCustomer(int id)
        {
            try
            {
                using (ParkursAPIEntities entities = new ParkursAPIEntities())
                {
                    var user = entities.USERS.FirstOrDefault(c => c.ID == id);
                    var entity = entities.PARKURS_USERS.Where(l => l.USER_ID == user.ID);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity.ToList().OrderByDescending(d => d.UPDATE_DATE));
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Listelenecek işlem bulunamadı!.");
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
    public partial class Accounting
    {
        public Nullable<decimal> DolarAmount { get; set; }
        public Nullable<decimal> DolarAmountTL { get; set; }
        
        public Nullable<decimal> EuroAmount { get; set; }
        public Nullable<decimal> EuroAmountTL { get; set; }
        
        public Nullable<decimal> PaundAmount { get; set; }
        public Nullable<decimal> PaundAmountTL { get; set; }
        
        public Nullable<decimal> TlAmount { get; set; }
        public Nullable<decimal> TlAmountTL { get; set; }
        
        public Nullable<decimal> RubleAmount { get; set; }
        public Nullable<decimal> RubleAmountTL { get; set; }

        public Nullable<decimal> ziplineSumDolar { get; set; }
        public Nullable<decimal> ziplineSumEuro { get; set; }
        public Nullable<decimal> ziplineSumPaund { get; set; }
        public Nullable<decimal> ziplineSumTl { get; set; }
        public Nullable<decimal> ziplineSumRuble { get; set; }
        public Nullable<decimal> ziplineFinalySumTl { get; set; }

        public Nullable<decimal> duvarSumDolar { get; set; }
        public Nullable<decimal> duvarSumEuro { get; set; }
        public Nullable<decimal> duvarSumPaund { get; set; }
        public Nullable<decimal> duvarSumTl { get; set; }
        public Nullable<decimal> duvarSumRuble { get; set; }
        public Nullable<decimal> duvarFinalySumTl { get; set; }

        public Nullable<decimal> parkurSumDolar { get; set; }
        public Nullable<decimal> parkurSumEuro { get; set; }
        public Nullable<decimal> parkurSumPaund { get; set; }
        public Nullable<decimal> parkurSumTl { get; set; }
        public Nullable<decimal> parkurSumRuble { get; set; }
        public Nullable<decimal> parkurFinalySumTl { get; set; }

        public Nullable<decimal> ParkurCustomerCount { get; set; }

        public Nullable<decimal> DuvarCustomerCount { get; set; }

        public Nullable<decimal> ZiplineCustomerCount { get; set; }

        public Nullable<decimal> TotalCustomerCount { get; set; }


        public Nullable<decimal> FinalyTL { get; set; }
        public Nullable<decimal> TotalCustomer { get; set; }

    }
    public partial class GetIncome
    {
    public DateTime Date { get; set; }
    public string Type { get; set; }
    
}
}
