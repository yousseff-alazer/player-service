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
    public class PlayerReportService : BaseService
    {
        #region Player Report Service
        public static PlayerReportResponse ListPlayerReports(PlayerReportRequest request)
        {
            var res = new PlayerReportResponse();
            RunBaseMysql(request, res, (PlayerReportRequest req) =>
            {
                try
                {
                    var query = request._context.PlayerReports
                                                .Include(s => s.Player)
                                                .Where(s => s.IsDeleted != 1)
                                                .Select(p => new PlayerReportRecord
                                                {
                                                    Id = p.Id,
                                                    PlayerId = p.PlayerId,
                                                    UserId = p.UserId,
                                                    Comment = p.Comment,
                                                    PlayerName = p.Player.FirstName,
                                                    CreatedAt = p.CreatedAt,
                                                    UpdatedAt = p.UpdatedAt,
                                                    IsDeleted = p.IsDeleted,
                                                });

                    if (request.PlayerReportRecord != null)
                        query = PlayerReportServiceManager.ApplyFilter(query, request.PlayerReportRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.PlayerReportRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerReport", "ListPlayerReports", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static PlayerReportResponse DeletePlayerReport(PlayerReportRequest request)
        {

            var res = new PlayerReportResponse();
            RunBaseMysql(request, res, (PlayerReportRequest req) =>
            {
                try
                {
                    var model = request.PlayerReportRecord;
                    var Report = request._context.PlayerReports.FirstOrDefault(c => c.UserId == model.UserId
                                                                                   && c.PlayerId == model.PlayerId);
                    if (Report != null)
                    {
                        if (Report.IsDeleted != 1)
                        {
                            Report.IsDeleted = 1;
                            request._context.SaveChanges();
                            res.Message = "Deleted Sucessfully";
                        }
                        else
                            res.Message = "This Report Is Already Deleted Before";

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
                    LogHelper.LogException(request._context, "PlayerReport", "DeletePlayerReport", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static PlayerReportResponse AddPlayerReport(PlayerReportRequest request)
        {

            var res = new PlayerReportResponse();
            RunBaseMysql(request, res, (PlayerReportRequest req) =>
            {
                try
                {
                    var PlayerReport = PlayerReportServiceManager.AddOrEditPlayerReport(request.PlayerReportRecord);
                    var model = request.PlayerReportRecord;

                    var IsPlayerExist = request._context.PlayerInfos.Any(s => s.Id == model.UserId
                                                                      && s.IsDeleted == 0);

                    var IsReporterExist = request._context.PlayerInfos.Any(s => s.Id == model.PlayerId
                                                                        && s.IsDeleted == 0);
                    if (IsPlayerExist && IsReporterExist)
                    {
                        request._context.PlayerReports.Add(PlayerReport);
                        request._context.SaveChanges();
                        res.Message = "Added Successfully";
                    }
                    else
                        res.Message = "Member Not Exist or Player was Removed";

                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;


                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerReport", "AddPlayerReport", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static PlayerReportResponse EditPlayerReport(PlayerReportRequest request)
        {

            var res = new PlayerReportResponse();
            RunBaseMysql(request, res, (PlayerReportRequest req) =>
            {
                try
                {
                    var model = request.PlayerReportRecord;
                    var PlayerReport = request._context.PlayerReports.Find(model.Id);
                    var IsPlayerExist = request._context.PlayerInfos.Any(s => s.Id == model.UserId
                                                                      && s.IsDeleted == 0);

                    var IsReporterExist = request._context.PlayerInfos.Any(s => s.Id == model.PlayerId
                                                                        && s.IsDeleted == 0);
                    if (PlayerReport != null && IsPlayerExist && IsReporterExist)
                    {
                        PlayerReport = PlayerReportServiceManager.AddOrEditPlayerReport(request.PlayerReportRecord, PlayerReport);
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
                    LogHelper.LogException(request._context, "PlayerReport", "EditPlayerReport", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion

    }
}
