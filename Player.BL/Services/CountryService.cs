using Player.BL.Services.Managers;
using Player.CommonDefinitions.Enums;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.Records;
using Reservation.Helpers;
using Review.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services
{
    public class CountryService : BaseService
    {
        private static readonly playerContext? _context;
        static CountryService()
        {
            _context = new playerContext();
        }

        #region CountrysServices
        public static CountryResponse ListCountries(CountryRequest request)
        {
            //var s = _context.Countrys.ToList();
            var res = new CountryResponse();
            RunBaseMysql(request, res, (CountryRequest req) =>
            {
                try
                {
                    var query = request._context.Countries.Where(s=>s.IsActive == 1||(request.CountryRecord!=null&&request.CountryRecord.IsAll)).Select(p => new CountryRecord
                    {

                        Id = p.Id,
                        Code = p.Code,
                        PhoneCode = p.PhoneCode,
                        IsActive = p.IsActive,
                        Ibanlength = p.Ibanlength,
                        Ibanprefix = p.Ibanprefix,
                        IconUrl = p.IconUrl,
                        IsArabic = p.IsArabic,
                        NameAr = p.NameAr,
                        NameEn = p.NameEn,
                        Tax = p.Tax,
                        ViewOrder = p.ViewOrder,
                        CreatedBy=p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        UpdatedBy=p.UpdatedBy,
                        UpdatedOn = p.UpdatedOn,
                        Vat= p.Vat,
                        CurrencyCode=p.CurrencyCode,
                        CurrencyName=p.CurrencyName,
                        IsPercentageTax = p.IsPercentageTax,
                   IsPercentageVat = p.IsPercentageVat
                    });

                    if (request.CountryRecord != null)
                        query = CountryServiceManager.ApplyFilter(query, request.CountryRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.CountryRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Country", "ListCountries", jsonRequest, ex);
                    //cloud Log
                    //request._seriLogger.Error(ex, ex.Message, "Country", "ListCountries");
                }
                return res;
            });
            return res;
        }
        public static CountryResponse DeleteCountry(CountryRequest request)
        {

            var res = new CountryResponse();
            RunBaseMysql(request, res, (CountryRequest req) =>
            {
                try
                {
                    var model = request.CountryRecord;
                    var Country = request._context.Countries.FirstOrDefault(s =>s.Id == model.Id);
                                                                                          
                    if (Country != null)
                    {
                        //update Country IsDeleted
                        Country.IsActive = 0;
                        Country.UpdatedOn = DateTime.Now;
                        request._context.SaveChanges();

                        res.Message = "Deleted Sucessfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Not Exist";
                        res.Success = false;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Country", "DeleteCountry", jsonRequest, ex);
                    //cloud Log
                    //request._seriLogger.Error(ex, ex.Message, "Country", "DeleteReview");
                }
                return res;
            });
            return res;
        }
        public static CountryResponse AddCountry(CountryRequest request)
        {

            var res = new CountryResponse();
            RunBaseMysql(request, res, (CountryRequest req) =>
            {
                try
                {
                    var Country = CountryServiceManager.AddOrEditCountry(request.CountryRecord);
                    request._context.Countries.Add(Country);
                    request._context.SaveChanges();
                    res.Message = "Added Successfully";
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;


                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Country", "AddCountry", jsonRequest, ex);
                    //cloud Log
                    //request._seriLogger.Error(ex, ex.Message, "Country", "AddReview");
                }
                return res;
            });
            return res;
        }
        public static CountryResponse EditCountry(CountryRequest request)
        {

            var res = new CountryResponse();
            RunBaseMysql(request, res, (CountryRequest req) =>
            {
                try
                {
                    var model = request.CountryRecord;
                    var Country = request._context.Countries.FirstOrDefault(s => s.Id == model.Id);
                    if (Country != null)
                    {


                        Country = CountryServiceManager.AddOrEditCountry(request.CountryRecord, Country);
                        request._context.SaveChanges();

                        res.Message = "Updated Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Country", "EditCountry", jsonRequest, ex);
                    //cloud Log
                    //request._seriLogger.Error(ex, ex.Message, "Country", "EditReview");
                }
                return res;
            });
            return res;
        }


        #endregion
    }
}
