using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Microsoft.EntityFrameworkCore;
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
    public class AreaLocalizeService : BaseService
    {
        #region AreaLocalize Service
        public static AreaLocalizeResponse ListAreaLocalizes(AreaLocalizeRequest request)
        {
            var res = new AreaLocalizeResponse();
            RunBaseMysql(request, res, (AreaLocalizeRequest req) =>
            {
                try
                {
                    var query = request._context.AreaLocalizes.Where(a => a.IsDeleted != 1).Select(p => new AreaLocalizeRecord
                    {
                        Id = p.Id,
                        LanguageId = p.LanguageId,
                        AreaId = p.AreaId,
                        Name = p.Name,
                        CreatedOn = p.CreatedOn,
                        UpdatedOn = p.UpdatedOn,
                        CreatedBy = p.CreatedBy,
                        UpdatedBy = p.UpdatedBy,
                        IsActive= p.IsActive
                    });

                    if (request.AreaLocalizeRecord != null)
                        query = AreaLocalizeServiceManager.ApplyFilter(query, request.AreaLocalizeRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.AreaLocalizeRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "AreaLocalize", "ListAreaLocalizes", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AreaLocalizeResponse DeleteAreaLocalize(AreaLocalizeRequest request)
        {

            var res = new AreaLocalizeResponse();
            RunBaseMysql(request, res, (AreaLocalizeRequest req) =>
            {
                try
                {
                    var model = request.AreaLocalizeRecord;
                    var AreaLocalize = request._context.AreaLocalizes.FirstOrDefault(s => s.AreaId == model.AreaId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (AreaLocalize != null)
                    {
                        //update Challenge IsDeleted
                        AreaLocalize.IsDeleted = 1;
                        AreaLocalize.UpdatedOn = DateTime.Now;
                        request._context.SaveChanges();

                        res.AreaLocalizeRecords = new List<AreaLocalizeRecord>
                        {
                           new AreaLocalizeRecord
                           {
                               Id=AreaLocalize.Id,
                               AreaId=AreaLocalize.AreaId,
                               LanguageId=AreaLocalize.LanguageId,
                               Name=AreaLocalize.Name,
                               IsActive=AreaLocalize.IsActive,
                               CreatedBy=AreaLocalize.CreatedBy,
                               CreatedOn=AreaLocalize.CreatedOn,
                               IsDeleted=AreaLocalize.IsDeleted,
                               UpdatedBy=AreaLocalize.UpdatedBy,
                               UpdatedOn= AreaLocalize.UpdatedOn,
                               IsQueueEdit=false
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
                    LogHelper.LogException(request._context, "AreaLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AreaLocalizeResponse AddAreaLocalize(AreaLocalizeRequest request)
        {

            var res = new AreaLocalizeResponse();
            RunBaseMysql(request, res, (AreaLocalizeRequest req) =>
            {
                try
                {
                    var model = request.AreaLocalizeRecord;
                    var IsAreaLocalizeExist = request._context.AreaLocalizes.Any(s => s.AreaId == model.AreaId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (!IsAreaLocalizeExist)
                    {
                        var AreaLocalize = AreaLocalizeServiceManager.AddOrEditAreaLocalize(request.AreaLocalizeRecord);
                        request._context.AreaLocalizes.Add(AreaLocalize);
                        request._context.SaveChanges();

                        res.AreaLocalizeRecords = new List<AreaLocalizeRecord>
                        {
                           new AreaLocalizeRecord
                           {
                               Id=AreaLocalize.Id,
                               AreaId=AreaLocalize.AreaId,
                               LanguageId=AreaLocalize.LanguageId,
                               Name=AreaLocalize.Name,
                               IsActive=AreaLocalize.IsActive,
                               CreatedBy=AreaLocalize.CreatedBy,
                               CreatedOn=AreaLocalize.CreatedOn,
                               IsDeleted=AreaLocalize.IsDeleted,
                               UpdatedBy=AreaLocalize.UpdatedBy,
                               UpdatedOn= AreaLocalize.UpdatedOn,
                               IsQueueEdit=false
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
                    LogHelper.LogException(request._context, "AreaLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AreaLocalizeResponse EditAreaLocalize(AreaLocalizeRequest request)
        {

            var res = new AreaLocalizeResponse();
            RunBaseMysql(request, res, (AreaLocalizeRequest req) =>
            {
                try
                {
                    var model = request.AreaLocalizeRecord;
                    var AreaLocalize = request._context.AreaLocalizes.FirstOrDefault(s => s.AreaId == model.AreaId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (AreaLocalize != null)
                    {
                        //request.AreaLocalizeRecord.SportId = GetSportId(request._context, request.AreaLocalizeRecord.ExternalSportId.Value);
                        AreaLocalize = AreaLocalizeServiceManager.AddOrEditAreaLocalize(request.AreaLocalizeRecord, AreaLocalize);
                        request._context.SaveChanges();

                        res.AreaLocalizeRecords = new List<AreaLocalizeRecord>
                        {
                           new AreaLocalizeRecord
                           {
                               Id=AreaLocalize.Id,
                               AreaId=AreaLocalize.AreaId,
                               LanguageId=AreaLocalize.LanguageId,
                               Name=AreaLocalize.Name,
                               IsActive=AreaLocalize.IsActive,
                               CreatedBy=AreaLocalize.CreatedBy,
                               CreatedOn=AreaLocalize.CreatedOn,
                               IsDeleted=AreaLocalize.IsDeleted,
                               UpdatedBy=AreaLocalize.UpdatedBy,
                               UpdatedOn= AreaLocalize.UpdatedOn,
                               IsQueueEdit=true
                           }
                        };
                        res.Message = "UpdatedSuccessfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.AreaLocalizeRecords = new List<AreaLocalizeRecord>();
                        res.Message = "Not Exist";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "AreaLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
