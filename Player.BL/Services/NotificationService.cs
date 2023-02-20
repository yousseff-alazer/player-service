
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.BL.Services.Managers;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using Reservation.Helpers;
using Shared.CommonDefinitions.Records;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Reservation.BL.Services
{
    public class NotificationService : BaseService
    {


        #region NotificationServices
        public static NotificationResponse ListNotification(NotificationRequest request)
        {
            var res = new NotificationResponse();
            RunBaseMysql(request, res, (NotificationRequest req) =>
            {
                try
                {
                    var query = request._context.Notifications.Where(p => p.IsSent == 1 && p.RecipientId == request.NotificationRecord.RecipientId).Join(request._context.PlayerInfos,
                 notify => notify.SenderId,
                 user => user.Id,
                 (notify, user) => new { notify, user }).Select(p => new NotificationRecord
                 {

                     Id = p.notify.NotificationId,
                     Body = p.notify.Body,
                     IsDeleted = p.notify.IsDeleted == 1 ? true : false,
                     Title = p.user.FirstName + " " + p.user.LastName,
                     CreatedByName = p.user.FirstName + " " + p.user.LastName,
                     CreatedByImage = p.user.ImageUrl,
                     RecipientId = p.notify.RecipientId,
                     ProviderAction = p.notify.NotificationTypeId ?? 0,
                     CreationDate = p.notify.CreationDate != null ? p.notify.CreationDate.Value : DateTime.MinValue,
                     IsSeen = p.notify.IsSeen,
                     AppearDate = p.notify.AppearDate
                 }) ;

                    //if (request.NotificationRecord != null)
                    // query = NotificationServiceManager.ApplyFilter(query, request.NotificationRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", true);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    var records = query.ToList();
                    var types = request._context.NotificationTypes.ToList();
                    foreach (var item in records)
                    {
                        var currentType = types.FirstOrDefault(p => p.Id == item.ProviderAction);
                        item.Body = currentType.Message;
                    }
                    res.NotificationRecords = records;
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Player_Profile", "ListNotification", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }


        public static NotificationResponse AddNotification(NotificationRequest request)
        {

            var res = new NotificationResponse();
            RunBaseMysql(request, res, (NotificationRequest req) =>
            {
                try
                {
                    if (request.NotificationRecord.ToUserIds != null && request.NotificationRecord.ToUserIds.Count() > 0)
                    {
                        //Continue
                    }
                    else
                        request.NotificationRecord.ToUserIds = request._context.PlayerInfos.Where(p => p.IsDeleted != 1 && string.IsNullOrEmpty(p.Provider)).Select(p => p.Id).ToList();


                    //notify
                    try
                    {
                        foreach (var item in request.NotificationRecord.ToUserIds)
                        {
                            request._context.Notifications.Add(new Notification()
                            {
                                AppearDate = DateTime.Now,
                                Body = request.NotificationRecord.ProviderType,
                                Title = request.NotificationRecord.ProviderType,
                                CreationDate = DateTime.Now,
                                IsDeleted = 0,
                                IsSeen = 0,
                                IsSent = 0,
                                ObjectId = request.NotificationRecord.ObjectId,
                                RecipientId = item,
                                SenderId = request.NotificationRecord.CreatedBy,
                                NotificationTypeId = request.NotificationRecord.ProviderAction
                            });
                        }

                        request._context.SaveChanges();
                    }
                    catch( Exception ex) {
                        var jsonRequest = JsonSerializer.Serialize(request);
                        LogHelper.LogException(request._context, "Player_Profile", "NotificationConsumer", jsonRequest, ex);
                    }

                    res.Message = "Added";
                    res.Success = false;
                    res.StatusCode = HttpStatusCode.OK;

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Player_Profile", "NotificationConsumer", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}