
using Player.DAL.mysqlplayerDB;
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
    public class Buddy_requestService : BaseService
    {

        #region Buddy_requestServices
        public static Buddy_requestResponse ListBuddy_request(buddy_requestRequest request)
        {
            var res = new Buddy_requestResponse();
            RunBaseMysql(request, res, (buddy_requestRequest req) =>
            {
                try
                {
                    var query = request._context.BuddyRequests.Join(request._context.PlayerInfos,
                 buddy => buddy.CreatedById,
                 user => user.Id,
                 (buddy, user) => new { buddy, user }).Select(p => new buddy_requestRecord
                 {

                     Id = p.buddy.Id,
                     CreatedAt = p.buddy.CreatedAt,
                     CreatedById = p.buddy.CreatedById,
                     PlayerId = p.buddy.PlayerId,
                     SportId = p.buddy.SportId,
                     SportName = p.buddy.Sport.Name,
                     SportIcon = p.buddy.Sport.IconUrl,
                     StatusId = p.buddy.StatusId,
                     UpdatedAt = p.buddy.UpdatedAt,
                     FirstName = p.user.FirstName,
                     ImageUrl = p.user.ImageUrl,
                     LastName = p.user.LastName,
                     SportIconUrl = p.buddy.Sport.IconUrl,
                     IsDeleted = p.buddy.IsDeleted,
                 }).Where(c => c.IsDeleted == 0 && c.StatusId != 1);

                    if (request.Buddy_requestRecord != null)
                        query = buddy_requestServiceManager.ApplyFilter(query, request.Buddy_requestRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", true);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.buddy_requestRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Buddies", "ListBuddyRequest", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Buddy_requestResponse DeleteBuddy_request(buddy_requestRequest request)
        {

            var res = new Buddy_requestResponse();
            RunBaseMysql(request, res, (buddy_requestRequest req) =>
            {
                try
                {
                    var model = request.Buddy_requestRecord;
                    var Buddy_request = request._context.BuddyRequests.FirstOrDefault(c => c.Id == model.Id);
                    if (Buddy_request != null)
                    {
                        //update Agency IsDeleted
                        Buddy_request.IsDeleted = 1;
                        request._context.SaveChanges();

                        res.Message = "Deleted";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;

                        //notify
                        try
                        {
                            request._context.Notifications.Add(new Notification()
                            {
                                AppearDate = DateTime.Now,
                                Body = "Cancel Buddy Request",
                                Title = "Cancel Buddy Request",
                                CreationDate = DateTime.Now,
                                IsDeleted = 0,
                                IsSeen = 0,
                                IsSent = 0,
                                ObjectId = 0,
                                RecipientId = Buddy_request.CreatedById,
                                SenderId = Buddy_request.PlayerId,
                                NotificationTypeId = 1
                            });
                            request._context.SaveChanges();
                        }
                        catch (Exception ex){
                            var jsonRequest = JsonSerializer.Serialize(request);
                            LogHelper.LogException(request._context, "Buddies", "CancelBuddyRequest", jsonRequest, ex);

                        }
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
                    LogHelper.LogException(request._context, "Buddies", "CancelBuddyRequest", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Buddy_requestResponse AddBuddy_request(buddy_requestRequest request)
        {

            var res = new Buddy_requestResponse();
            RunBaseMysql(request, res, (buddy_requestRequest req) =>
            {
                try
                {
                    if (request.Buddy_requestRecord.SportId.HasValue || request.Buddy_requestRecord.IsConnectionOnly == 1)
                    {
                        var Buddy_request = buddy_requestServiceManager.AddOrEditbuddy_request(request.Buddy_requestRecord);
                        var existBefore = request._context.BuddyRequests.FirstOrDefault(p => p.IsDeleted != 1 && p.StatusId < 2 && (p.PlayerId == request.Buddy_requestRecord.PlayerId && p.CreatedById == request.Buddy_requestRecord.CreatedById) || (p.PlayerId == request.Buddy_requestRecord.CreatedById && p.CreatedById == request.Buddy_requestRecord.PlayerId));
                        if (existBefore == null)
                        {
                            request._context.BuddyRequests.Add(Buddy_request);
                            request._context.SaveChanges();

                            res.Message = "Request sent";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            if (existBefore.IsDeleted == 1)
                            {
                                res.Message = "Request sent";
                                res.Success = true;
                                res.StatusCode = HttpStatusCode.OK;
                                existBefore.IsDeleted = 0;
                                existBefore.SportId = request.Buddy_requestRecord.SportId;
                                existBefore.StatusId = 0;
                                existBefore.CreatedById = request.Buddy_requestRecord.CreatedById;
                                existBefore.PlayerId = request.Buddy_requestRecord.PlayerId;
                                request._context.SaveChanges();

                            }
                            else
                            {
                                if (existBefore.StatusId == 1 && existBefore.IsConnectionOnly == 1)
                                {
                                    existBefore.IsConnectionOnly = 0;
                                    existBefore.SportId = request.Buddy_requestRecord.SportId;
                                    existBefore.StatusId = 0;
                                    existBefore.CreatedById = request.Buddy_requestRecord.CreatedById;
                                    existBefore.PlayerId = request.Buddy_requestRecord.PlayerId;
                                    request._context.SaveChanges();
                                    res.Message = "Request sent";
                                    res.Success = true;
                                    res.StatusCode = HttpStatusCode.OK;
                                }
                                else
                                {
                                    res.Message = "Sorry,Request sent before";
                                    res.Success = false;
                                    res.StatusCode = HttpStatusCode.OK;
                                }

                            }
                        }

                        //notify
                        try
                        {
                            request._context.Notifications.Add(new Notification()
                            {
                                AppearDate = DateTime.Now,
                                Body = "Send Buddy Request",
                                Title = "Send Buddy Request",
                                CreationDate = DateTime.Now,
                                IsDeleted = 0,
                                IsSeen = 0,
                                IsSent = 0,
                                ObjectId = 0,
                                RecipientId = request.Buddy_requestRecord.PlayerId,
                                SenderId = request.Buddy_requestRecord.CreatedById,
                                NotificationTypeId = 8
                            });
                            request._context.SaveChanges();
                        }
                        catch (Exception ex){
                            var jsonRequest = JsonSerializer.Serialize(request);
                            LogHelper.LogException(request._context, "Buddies", "AddConnection", jsonRequest, ex);
                        }
                    }
                    else
                    {
                        res.Message = "Sorry, No sport selected";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Buddies", "AddConnection", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Buddy_requestResponse EditBuddy_request(buddy_requestRequest request)
        {

            var res = new Buddy_requestResponse();
            RunBaseMysql(request, res, (buddy_requestRequest req) =>
            {
                try
                {
                    var model = request.Buddy_requestRecord;
                    var Buddy_request = request._context.BuddyRequests.Find(model.Id);
                    if (Buddy_request != null)
                    {
                        //update whole Agency
                        Buddy_request = buddy_requestServiceManager.AddOrEditbuddy_request(request.Buddy_requestRecord, Buddy_request);
                        request._context.SaveChanges();

                        res.Message = "UpdatedSuccessfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;

                        //notify
                        try
                        {
                            request._context.Notifications.Add(new Notification()
                            {
                                AppearDate = DateTime.Now,
                                Body = "Approved Buddy Request",
                                Title = "Approved Buddy Request",
                                CreationDate = DateTime.Now,
                                IsDeleted = 0,
                                IsSeen = 0,
                                IsSent = 0,
                                ObjectId = 0,
                                RecipientId = Buddy_request.CreatedById,
                                SenderId = Buddy_request.PlayerId,
                                NotificationTypeId = request.Buddy_requestRecord.StatusId==1?9:10
                            });
                            request._context.SaveChanges();
                        }
                        catch (Exception ex){
                            var jsonRequest = JsonSerializer.Serialize(request);
                            LogHelper.LogException(request._context, "Buddies", "RejectBuddyRequest", jsonRequest, ex);
                        }
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
                    LogHelper.LogException(request._context, "Buddies", "RejectBuddyRequest", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}