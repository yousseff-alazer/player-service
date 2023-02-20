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
    public class PlayerZoneService : BaseService
    {
        #region PlayerZone Service
        public static PlayerZoneResponse ListPlayerZones(PlayerZoneRequest request)
        {
            var res = new PlayerZoneResponse();
            RunBaseMysql(request, res, (PlayerZoneRequest req) =>
            {
                try
                {
                    var query = request._context.PlayerZones.Include(s => s.Player).Include(s=>s.Zone)
                                                       .Where(a => a.IsDeleted != 1)
                                                       .Select(p => new PlayerZoneRecord
                                                       {
                                                           Id = p.Id,
                                                           ZoneId = p.ZoneId,
                                                           ZoneName = p.Zone.Name,
                                                           AreaName = p.Zone.Area.Name,
                                                           PlayerId = p.PlayerId,
                                                           Tax =  p.Tax,
                                                           CreatedOn = p.CreatedOn,
                                                           UpdatedOn = p.UpdatedOn,
                                                           CreatedBy = p.CreatedBy,
                                                           UpdatedBy = p.UpdatedBy
                                                       });

                    if (request.PlayerZoneRecord != null)
                        query = PlayerZoneServiceManager.ApplyFilter(query, request.PlayerZoneRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.PlayerZoneRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerZone", "ListPlayerZones", jsonRequest, ex);

                }
                return res;
            });
            return res;
        }
        public static PlayerZoneResponse DeletePlayerZone(PlayerZoneRequest request)
        {

            var res = new PlayerZoneResponse();
            RunBaseMysql(request, res, (PlayerZoneRequest req) =>
            {
                try
                {
                    var model = request.PlayerZoneRecord;
                    var PlayerZone = request._context.PlayerZones.FirstOrDefault(s => s.Id == model.Id);
                    if (PlayerZone != null)
                    {
                        //update Challenge IsDeleted
                        PlayerZone.IsDeleted = 1;
                        PlayerZone.UpdatedOn = DateTime.Now;
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
                    LogHelper.LogException(request._context, "PlayerZone", "DeletePlayerZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static PlayerZoneResponse AddPlayerZone(PlayerZoneRequest request)
        {

            var res = new PlayerZoneResponse();
            RunBaseMysql(request, res, (PlayerZoneRequest req) =>
            {
                try
                {
                    var IsPlayerZoneExist = request._context.PlayerZones.Any(s => s.Id == request.PlayerZoneRecord.Id);
                    if (!IsPlayerZoneExist)
                    {
                        var PlayerZone = PlayerZoneServiceManager.AddOrEditPlayerZone(request.PlayerZoneRecord);

                        request._context.PlayerZones.Add(PlayerZone);
                        request._context.SaveChanges();

                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this PlayerZone is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerZone", "AddPlayerZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static PlayerZoneResponse EditPlayerZone(PlayerZoneRequest request)
        {

            var res = new PlayerZoneResponse();
            RunBaseMysql(request, res, (PlayerZoneRequest req) =>
            {
                try
                {
                    var model = request.PlayerZoneRecord;
                    var PlayerZone = request._context.PlayerZones.FirstOrDefault(s => s.Id == model.Id);
                    if (PlayerZone != null)
                    {
                        //update whole Agency
                        PlayerZone = PlayerZoneServiceManager.AddOrEditPlayerZone(request.PlayerZoneRecord, PlayerZone);
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
                    LogHelper.LogException(request._context, "PlayerZone", "EditPlayerZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        public static PlayerZoneResponse ValidatePlayerZone(PlayerZoneRequest request)
        {

            var res = new PlayerZoneResponse();
            RunBaseMysql(request, res, (PlayerZoneRequest req) =>
            {
                try
                {
                    var model = request.PlayerZoneRecord;
                    var PlayerZone = request._context.PlayerZones.Any(s => s.PlayerId == model.PlayerId && s.ZoneId == model.ZoneId);
                    if (PlayerZone)
                    {
                        res.Message = "this is exist before";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Not Exist";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerZone", "DeletePlayerZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
