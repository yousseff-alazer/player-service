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
    public class ZoneLocalizeService : BaseService
    {
        #region ZoneLocalize Service
        public static ZoneLocalizeResponse ListZoneLocalizes(ZoneLocalizeRequest request)
        {
            var res = new ZoneLocalizeResponse();
            RunBaseMysql(request, res, (ZoneLocalizeRequest req) =>
            {
                try
                {
                    var query = request._context.ZoneLocalizes.Where(a => a.IsDeleted != 1).Select(p => new ZoneLocalizeRecord
                    {
                        Id = p.Id,
                        LanguageId = p.LanguageId,
                        ZoneId = p.ZoneId,
                        Name = p.Name,
                        CreatedOn = p.CreatedOn,
                        UpdatedOn = p.UpdatedOn,
                        CreatedBy = p.CreatedBy,
                        UpdatedBy = p.UpdatedBy,
                        IsActive = p.IsActive
                    });

                    if (request.ZoneLocalizeRecord != null)
                        query = ZoneLocalizeServiceManager.ApplyFilter(query, request.ZoneLocalizeRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.ZoneLocalizeRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "ZoneLocalize", "ListZoneLocalizes", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static ZoneLocalizeResponse DeleteZoneLocalize(ZoneLocalizeRequest request)
        {

            var res = new ZoneLocalizeResponse();
            RunBaseMysql(request, res, (ZoneLocalizeRequest req) =>
            {
                try
                {
                    var model = request.ZoneLocalizeRecord;
                    var ZoneLocalize = request._context.ZoneLocalizes.FirstOrDefault(s => s.ZoneId == model.ZoneId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (ZoneLocalize != null)
                    {
                        //update Challenge IsDeleted
                        ZoneLocalize.IsDeleted = 1;
                        ZoneLocalize.UpdatedOn = DateTime.Now;
                        request._context.SaveChanges();

                        res.ZoneLocalizeRecords = new List<ZoneLocalizeRecord>
                        {
                            new ZoneLocalizeRecord
                            {
                                Id=ZoneLocalize.Id,
                                ZoneId=ZoneLocalize.ZoneId,
                                LanguageId=ZoneLocalize.LanguageId,
                                Name=ZoneLocalize.Name,
                                IsActive=ZoneLocalize.IsActive,
                                CreatedBy=ZoneLocalize.CreatedBy,
                                CreatedOn=ZoneLocalize.CreatedOn,
                                IsDeleted=ZoneLocalize.IsDeleted,
                                UpdatedBy=ZoneLocalize.UpdatedBy,
                                UpdatedOn=ZoneLocalize.UpdatedOn,
                                IsQueueEdit=false,
                            }
                        };
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
                    LogHelper.LogException(request._context, "ZoneLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static ZoneLocalizeResponse AddZoneLocalize(ZoneLocalizeRequest request)
        {

            var res = new ZoneLocalizeResponse();
            RunBaseMysql(request, res, (ZoneLocalizeRequest req) =>
            {
                try
                {
                    var model = request.ZoneLocalizeRecord;
                    var IsZoneLocalizeExist = request._context.ZoneLocalizes.Any(s => s.ZoneId == model.ZoneId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (!IsZoneLocalizeExist)
                    {
                        var ZoneLocalize = ZoneLocalizeServiceManager.AddOrEditZoneLocalize(request.ZoneLocalizeRecord);
                        request._context.ZoneLocalizes.Add(ZoneLocalize);
                        request._context.SaveChanges();

                        res.ZoneLocalizeRecords = new List<ZoneLocalizeRecord>
                        {
                            new ZoneLocalizeRecord
                            {
                                Id=ZoneLocalize.Id,
                                ZoneId=ZoneLocalize.ZoneId,
                                LanguageId=ZoneLocalize.LanguageId,
                                Name=ZoneLocalize.Name,
                                IsActive=ZoneLocalize.IsActive,
                                CreatedBy=ZoneLocalize.CreatedBy,
                                CreatedOn=ZoneLocalize.CreatedOn,
                                IsDeleted=ZoneLocalize.IsDeleted,
                                UpdatedBy=ZoneLocalize.UpdatedBy,
                                UpdatedOn=ZoneLocalize.UpdatedOn,
                                IsQueueEdit=false,
                            }
                        };
                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this Sport Localize is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "ZoneLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static ZoneLocalizeResponse EditZoneLocalize(ZoneLocalizeRequest request)
        {

            var res = new ZoneLocalizeResponse();
            RunBaseMysql(request, res, (ZoneLocalizeRequest req) =>
            {
                try
                {
                    var model = request.ZoneLocalizeRecord;
                    var ZoneLocalize = request._context.ZoneLocalizes.FirstOrDefault(s => s.ZoneId == model.ZoneId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (ZoneLocalize != null)
                    {
                        //request.ZoneLocalizeRecord.SportId = GetSportId(request._context, request.ZoneLocalizeRecord.ExternalSportId.Value);
                        ZoneLocalize = ZoneLocalizeServiceManager.AddOrEditZoneLocalize(request.ZoneLocalizeRecord, ZoneLocalize);
                        request._context.SaveChanges();

                        res.ZoneLocalizeRecords = new List<ZoneLocalizeRecord>
                        {
                            new ZoneLocalizeRecord
                            {
                                Id=ZoneLocalize.Id,
                                ZoneId=ZoneLocalize.ZoneId,
                                LanguageId=ZoneLocalize.LanguageId,
                                Name=ZoneLocalize.Name,
                                IsActive=ZoneLocalize.IsActive,
                                CreatedBy=ZoneLocalize.CreatedBy,
                                CreatedOn=ZoneLocalize.CreatedOn,
                                IsDeleted=ZoneLocalize.IsDeleted,
                                UpdatedBy=ZoneLocalize.UpdatedBy,
                                UpdatedOn=ZoneLocalize.UpdatedOn,
                                IsQueueEdit=true,
                            }
                        };
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
                    LogHelper.LogException(request._context, "ZoneLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
