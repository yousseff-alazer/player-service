using Player.BL.Services.Managers;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Player.BL.Services
{
    public class CountryLocalizeService : BaseService
    {
        #region CountryLocalize Service
        public static CountryLocalizeResponse ListCountryLocalizes(CountryLocalizeRequest request)
        {
            var res = new CountryLocalizeResponse();
            RunBaseMysql(request, res, (CountryLocalizeRequest req) =>
            {
                try
                {
                    var query = request._context.CountryLocalizes.Where(a => a.IsDeleted != 1).Select(p => new CountryLocalizeRecord
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Code = p.Code,
                        CountryId = p.CountryId,
                        LanguageId = p.LanguageId,
                        PhoneCode = p.PhoneCode,
                        CurrencyCode = p.CurrencyCode,
                        CurrencyName = p.CurrencyName,
                        Ibanprefix = p.Ibanprefix,
                        IsDeleted = p.IsDeleted,
                        CreationDate = p.CreationDate,
                        ModificationDate = p.ModificationDate,
                        CreatedBy = p.CreatedBy,
                        ModifiedBy = p.ModifiedBy,
                        //IsDeleted = p.IsDeleted.Value,

                    });

                    if (request.CountryLocalizeRecord != null)
                        query = CountryLocalizeServiceManager.ApplyFilter(query, request.CountryLocalizeRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.CountryLocalizeRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "CountryLocalize", "ListCountryLocalizes", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static CountryLocalizeResponse DeleteCountryLocalize(CountryLocalizeRequest request)
        {

            var res = new CountryLocalizeResponse();
            RunBaseMysql(request, res, (CountryLocalizeRequest req) =>
            {
                try
                {
                    var model = request.CountryLocalizeRecord;
                    var CountryLocalize = request._context.CountryLocalizes.FirstOrDefault(s => s.CountryId == model.CountryId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (CountryLocalize != null)
                    {
                        //update Challenge IsDeleted
                        CountryLocalize.IsDeleted = 1;
                        CountryLocalize.ModificationDate = DateTime.Now;
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
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "CountryLocalize", "DeleteCountryLocalize", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static CountryLocalizeResponse AddCountryLocalize(CountryLocalizeRequest request)
        {

            var res = new CountryLocalizeResponse();
            RunBaseMysql(request, res, (CountryLocalizeRequest req) =>
            {
                try
                {
                    var model = request.CountryLocalizeRecord;
                    var IsCountryLocalizeExist = request._context.CountryLocalizes.Any(s => s.CountryId == model.CountryId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (!IsCountryLocalizeExist)
                    {
                        
                        var CountryLocalize = CountryLocalizeServiceManager.AddOrEditCountryLocalize(request.CountryLocalizeRecord);
                        request._context.CountryLocalizes.Add(CountryLocalize);
                        request._context.SaveChanges();

                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this Country Localize is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "CountryLocalize", "AddCountryLocalize", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static CountryLocalizeResponse EditCountryLocalize(CountryLocalizeRequest request)
        {

            var res = new CountryLocalizeResponse();
            RunBaseMysql(request, res, (CountryLocalizeRequest req) =>
            {
                try
                {
                    var model = request.CountryLocalizeRecord;
                    var CountryLocalize = request._context.CountryLocalizes.FirstOrDefault(s => s.CountryId == model.CountryId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (CountryLocalize != null)
                    {
                        //request.CountryLocalizeRecord.CountryId = GetCountryId(request._context, request.CountryLocalizeRecord.ExternalCountryId.Value);
                        CountryLocalize = CountryLocalizeServiceManager.AddOrEditCountryLocalize(request.CountryLocalizeRecord, CountryLocalize);
                        request._context.SaveChanges();

                        res.Message = "UpdatedSuccessfully";
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
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "CountryLocalize", "EditCountryLocalize", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        public static CountryLocalizeResponse EditCountryLocalizes(CountryLocalizeRequest request)
        {
            var res = new CountryLocalizeResponse();
            RunBaseMysql(request, res, req =>
            {
                try
                {
                    foreach (var model in req.CountryLocalizeRecords)
                    {
                        var CountryLocalize = request._context.CountryLocalizes.Find(model.Id);
                        if (CountryLocalize != null)
                        {
                            //update whole CountryLocalize
                            CountryLocalize = CountryLocalizeServiceManager.AddOrEditCountryLocalize(model, CountryLocalize);
                                
                            request._context.SaveChanges();

                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Invalid CountryLocalize";
                            res.Success = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "CountryLocalize", "EditCountryLocalize", jsonRequest, ex);
                }

                return res;
            });
            return res;
        }

        public static CountryLocalizeResponse AddCountryLocalizes(CountryLocalizeRequest request)
        {
            var res = new CountryLocalizeResponse();
            RunBaseMysql(request, res, req =>
            {
                try
                {
                    foreach (var model in req.CountryLocalizeRecords)
                    {
                        var CountryLocalizeExist = request._context.CountryLocalizes.Any(m =>
                             m.IsDeleted != 1 && m.CountryId == model.CountryId && m.LanguageId == model.LanguageId);
                        if (!CountryLocalizeExist)
                        {
                            var CountryLocalize =
                                CountryLocalizeServiceManager.AddOrEditCountryLocalize(model);
                                    
                            request._context.CountryLocalizes.Add(CountryLocalize);
                            request._context.SaveChanges();
                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "CountryLocalize already exist";
                            res.Success = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message + ex.StackTrace;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "CountryLocalize", "AddCountryLocalize", jsonRequest, ex);
                }

                return res;
            });
            return res;
        }
        #endregion
    }
}
