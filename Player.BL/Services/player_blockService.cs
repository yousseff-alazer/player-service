
using Reservation.BL.Services;
using Reservation.BL.Services.Managers;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using Reservation.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Reservation.BL.Services
{
    public class Player_blockService : BaseService
    {
        #region player_blockServices
        public static Player_blockResponse Listplayer_block(player_blockRequest request)
        {
            var res = new Player_blockResponse();
            RunBaseMysql(request, res, (player_blockRequest req) =>
            {
                try
                {
                 var query = request._context.PlayerBlocks.Join(request._context.PlayerInfos,
                 buddy => buddy.PlayerId,
                 user => user.Id,
                 (buddy, user) => new { buddy, user }).Where(p =>p.buddy.IsDeleted!=1&& p.user.Id != request.player_blockRecord.CreatedById).Select(p => new player_blockRecord
                 {

                     Id = p.buddy.Id,
                     CreatedAt = p.buddy.CreatedAt,
                     CreatedById = p.buddy.CreatedById,
                     PlayerId = p.buddy.PlayerId,
                     UpdatedAt = p.buddy.UpdatedAt,
                     FirstName = p.user.FirstName,
                     ImageUrl = p.user.ImageUrl,
                     LastName = p.user.LastName,
                     IsDeleted = p.buddy.IsDeleted

                 });

                    if (request.player_blockRecord != null)
                        query = player_blockServiceManager.ApplyFilter(query, request.player_blockRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", true);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.player_blockRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerBlock", "ListBlocked", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Player_blockResponse Deleteplayer_block(player_blockRequest request)
        {

            var res = new Player_blockResponse();
            RunBaseMysql(request, res, (player_blockRequest req) =>
            {
                try
                {
                    var model = request.player_blockRecord;
                    var player_block = request._context.PlayerBlocks.FirstOrDefault(c => c.Id == model.Id);
                    if (player_block != null)
                    {
                        //update Agency IsDeleted
                        if (player_block.IsDeleted == 1)
                        {
                            player_block.IsDeleted = 0;
                            res.Message = "Unblocked";
                        }
                        else
                        {
                            player_block.IsDeleted = 1;
                            res.Message = "Blocked";

                        }

                        request._context.SaveChanges();


                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "InvalidData";
                        res.Success = false;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerBlock", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Player_blockResponse Addplayer_block(player_blockRequest request)
        {
            var res = new Player_blockResponse();
            RunBaseMysql(request, res, (player_blockRequest req) =>
            {
                try
                {
                    var player_block = player_blockServiceManager.AddOrEditplayer_block(request.player_blockRecord);
                    var existBefore = request._context.PlayerBlocks.FirstOrDefault(p => p.PlayerId == request.player_blockRecord.PlayerId && p.CreatedById == request.player_blockRecord.CreatedById);
                    if (existBefore == null)
                    {
                        request._context.PlayerBlocks.Add(player_block);
                        res.Message = "Request sent";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        if (existBefore.IsDeleted == 1)
                        {
                            existBefore.IsDeleted = 0;
                            res.Message = "Blocked";
                        }
                        else
                        {
                            existBefore.IsDeleted = 1;
                            res.Message = "UnBlocked";

                        }
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    request._context.SaveChanges();
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "PlayerBlock", "AddOrEditBlock", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}