using Microsoft.EntityFrameworkCore;
using Player.BL.Services.Managers;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
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
    public class ZoneService : BaseService
    {
        #region Zone Service
        public static ZoneResponse ListZones(ZoneRequest request)
        {
            var res = new ZoneResponse();
            RunBaseMysql(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var query = request._context.Zones.Include(s => s.ZoneLocalizes)
                                                       .Where(a => a.IsDeleted != 1)
                                                       .Select(p => new ZoneRecord
                                                       {
                                                           Id = p.Id,
                                                           AreaId = p.AreaId,
                                                           Name = request.LanguageId > 0 ? p.ZoneLocalizes.FirstOrDefault(s => s.LanguageId == request.LanguageId && s.IsDeleted != 1).Name ?? p.Name : p.Name,
                                                           CreatedOn = p.CreatedOn,
                                                           UpdatedOn = p.UpdatedOn,
                                                           CreatedBy = p.CreatedBy,
                                                           UpdatedBy = p.UpdatedBy
                                                       });

                    if (request.ZoneRecord != null)
                        query = ZoneServiceManager.ApplyFilter(query, request.ZoneRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.ZoneRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Zone", "ListZones", jsonRequest, ex);

                }
                return res;
            });
            return res;
        }
        public static ZoneResponse DeleteZone(ZoneRequest request)
        {

            var res = new ZoneResponse();
            RunBaseMysql(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var model = request.ZoneRecord;
                    var Zone = request._context.Zones.FirstOrDefault(s => s.Id == model.Id);
                    if (Zone != null)
                    {
                        //update Challenge IsDeleted
                        Zone.IsDeleted = 1;
                        Zone.UpdatedOn = DateTime.Now;
                        request._context.SaveChanges();

                        res.ZoneRecords = new List<ZoneRecord>
                        {
                            new ZoneRecord
                            {
                                Id=Zone.Id,
                                AreaId=Zone.AreaId,
                                Name=Zone.Name,
                                IsActive=Zone.IsActive,
                                CreatedBy=Zone.CreatedBy,
                                CreatedOn=Zone.CreatedOn,
                                IsDeleted=Zone.IsDeleted,
                                UpdatedBy=Zone.UpdatedBy,
                                UpdatedOn=Zone.UpdatedOn,
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
                    LogHelper.LogException(request._context, "Zone", "DeleteZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static ZoneResponse AddZone(ZoneRequest request)
        {

            var res = new ZoneResponse();
            RunBaseMysql(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var IsZoneExist = request._context.Zones.Any(s => s.Id == request.ZoneRecord.Id || (s.Name.ToLower() == request.ZoneRecord.Name.ToLower() && s.AreaId == request.ZoneRecord.AreaId));
                    if (!IsZoneExist)
                    {
                        var Zone = ZoneServiceManager.AddOrEditZone(request.ZoneRecord);

                        request._context.Zones.Add(Zone);
                        request._context.SaveChanges();

                        res.ZoneRecords = new List<ZoneRecord>
                        {
                            new ZoneRecord
                            {
                                Id=Zone.Id,
                                AreaId=Zone.AreaId,
                                Name=Zone.Name,
                                IsActive=Zone.IsActive,
                                CreatedBy=Zone.CreatedBy,
                                CreatedOn=Zone.CreatedOn,
                                IsDeleted=Zone.IsDeleted,
                                UpdatedBy=Zone.UpdatedBy,
                                UpdatedOn=Zone.UpdatedOn,
                                IsQueueEdit=false
                            }
                        };
                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this Zone is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Zone", "AddZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static ZoneResponse EditZone(ZoneRequest request)
        {

            var res = new ZoneResponse();
            RunBaseMysql(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var model = request.ZoneRecord;
                    var Zone = request._context.Zones.FirstOrDefault(s => s.Id == model.Id);
                    if (Zone != null)
                    {
                        //update whole Agency
                        Zone = ZoneServiceManager.AddOrEditZone(request.ZoneRecord, Zone);
                        request._context.SaveChanges();

                        res.ZoneRecords = new List<ZoneRecord>
                        {
                            new ZoneRecord
                            {
                                Id=Zone.Id,
                                AreaId=Zone.AreaId,
                                Name=Zone.Name,
                                IsActive=Zone.IsActive,
                                CreatedBy=Zone.CreatedBy,
                                CreatedOn=Zone.CreatedOn,
                                IsDeleted=Zone.IsDeleted,
                                UpdatedBy=Zone.UpdatedBy,
                                UpdatedOn=Zone.UpdatedOn,
                                IsQueueEdit=true
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
                    LogHelper.LogException(request._context, "Zone", "EditZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
