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
    public class PlayerEnergyPointService : BaseService
    {
        #region PlayerEnergyPointsServices
        public static PlayerEnergyPointResponse ListPlayerEnergyPoints(PlayerEnergyPointRequest request)
        {
            var res = new PlayerEnergyPointResponse();
            RunBaseMysql(request, res, (PlayerEnergyPointRequest req) =>
            {
                try
                {
                    var query = request._context.PlayerEnergyPoints.Select(p => new PlayerEnergyPointRecord
                    {

                        Id = p.Id,
                        PlayerId = p.PlayerId,
                        Points = p.Points,
                        UpdatedAt = p.UpdatedAt,
                        ProviderType = p.ProviderType,
                        ProviderId = p.ProviderId,
                        CreatedAt = p.CreatedAt,
                        ProviderImageUrl = p.ProviderImageUrl,
                        ProviderName = p.ProviderName,
                    });

                    if (request.PlayerEnergyPointRecord != null)
                        query = PlayerEnergyPointServiceManager.ApplyFilter(query, request.PlayerEnergyPointRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.PlayerEnergyPointRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerEnergyPoint", "ListPlayerEnergyPoints", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        
        public static PlayerEnergyPointResponse DeletePlayerEnergyPoint(PlayerEnergyPointRequest request)
        {

            var res = new PlayerEnergyPointResponse();
            var PlayerEnergyPoint = new PlayerEnergyPoint();
            RunBaseMysql(request, res, (PlayerEnergyPointRequest req) =>
            {
                try
                {
                    var model = request.PlayerEnergyPointRecord;
                    if (model.Id > 0)
                        PlayerEnergyPoint = request._context.PlayerEnergyPoints.Find(model.Id);
                    else
                        PlayerEnergyPoint = request._context.PlayerEnergyPoints.FirstOrDefault(s => s.ProviderId == model.ProviderId
                                                                                                 && s.ProviderType == model.ProviderType
                                                                                                 && s.PlayerId == model.PlayerId);
        
                    if (PlayerEnergyPoint != null)
                    {
                        PlayerEnergyPoint.UpdatedAt = DateTime.Now;
                        request._context.Remove(PlayerEnergyPoint);
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
                    LogHelper.LogException(request._context, "PlayerEnergyPoint", "DeletePlayerEnergyPoint", jsonRequest, ex);
                }
                res.PlayerEnergyPointRecords = new List<PlayerEnergyPointRecord>
                {
                    new PlayerEnergyPointRecord
                    {
                        Id = PlayerEnergyPoint.Id,
                        PlayerId = PlayerEnergyPoint.PlayerId,
                        Points = PlayerEnergyPoint.Points,
                        UpdatedAt = PlayerEnergyPoint.UpdatedAt,
                        ProviderType = PlayerEnergyPoint.ProviderType,
                        ProviderId = PlayerEnergyPoint.ProviderId,
                        CreatedAt = PlayerEnergyPoint.CreatedAt,
                    }
            };
                return res;
            });
            return res;
        }
        public static PlayerEnergyPointResponse AddPlayerEnergyPoint(PlayerEnergyPointRequest request)
        {

            var res = new PlayerEnergyPointResponse();
            RunBaseMysql(request, res, (PlayerEnergyPointRequest req) =>
            {
                try
                {
                    var model = request.PlayerEnergyPointRecord;
                    var isExist = request._context.PlayerEnergyPoints.Any(s => s.PlayerId == model.PlayerId
                                                                       && s.ProviderId == model.ProviderId
                                                                       && s.ProviderType == model.ProviderType);
                    if (!isExist)
                    {
                        var PlayerEnergyPoint = PlayerEnergyPointServiceManager.AddOrEditPlayerEnergyPoint(request.PlayerEnergyPointRecord);
                        request._context.PlayerEnergyPoints.Add(PlayerEnergyPoint);
                        request._context.SaveChanges();
                        
                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "You tooke this points before !";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerEnergyPoint", "AddPlayerEnergyPoint", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static PlayerEnergyPointResponse EditPlayerEnergyPoint(PlayerEnergyPointRequest request)
        {

            var res = new PlayerEnergyPointResponse();
            RunBaseMysql(request, res, (PlayerEnergyPointRequest req) =>
            {
                try
                {
                    var model = request.PlayerEnergyPointRecord;
                    var PlayerEnergyPoint = request._context.PlayerEnergyPoints.Find(model.Id);
                                                                                       
                                                                                          
                    if (PlayerEnergyPoint != null)
                    {
                        PlayerEnergyPoint = PlayerEnergyPointServiceManager.AddOrEditPlayerEnergyPoint(request.PlayerEnergyPointRecord, PlayerEnergyPoint);
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
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerEnergyPoint", "EditPlayerEnergyPoint", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
