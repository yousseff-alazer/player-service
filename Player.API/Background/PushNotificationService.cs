
using Microsoft.EntityFrameworkCore;
using Player.DAL.mysqlplayerDB;
using Reservation.API;
using Reservation.BL.Services;
using Reservation.BL.Services.Managers;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Player.Reminders
{
    internal interface IPushNotificationService
    {
        Task DoWork(DateTime fromDate, DateTime toDate, CancellationToken stoppingToken);
    }

    internal class PushNotificationService : IPushNotificationService
    {
        public async Task DoWork(DateTime fromDate, DateTime toDate, CancellationToken stoppingToken)
        {
            try
            {
                var notifications = ContextEntities.Notifications.Where(p => p.IsSent != 1 && p.RecipientId != null && p.SenderId != null).ToList();
                var userIDs = notifications.Select(p => p.RecipientId).ToList();
                var reciveTokens = ContextEntities.CommonUserDevices.Where(p => p.DeviceToken != null && userIDs.Contains(p.CommonUserId)).ToList();
                if (reciveTokens != null && reciveTokens.Any())
                {
                    for (int i = 0; i < notifications.Count(); i++)
                    {
                        var notification = notifications[i];
                        notification.IsSent = 1;
                        //Call Push Service
                        var sender = ContextEntities.PlayerInfos.FirstOrDefault(p => p.Id == notifications[i].SenderId);
                        var types = ContextEntities.NotificationTypes.ToList();

                        var user = reciveTokens.FirstOrDefault(p => p.CommonUserId == notifications[i].RecipientId);
                        if (user != null)
                        {
                            var LanguageId = user.Lang == "en" ? 0 : 1;

                            
                            var currentType = types.FirstOrDefault(p => p.Id == notifications[i].NotificationTypeId);
                            if (user != null && sender != null)
                            {
                                notification.IsSeen = 1;
                                notification.AppearDate = DateTime.Now;
                                NotificationManager.SendNotification(user.DeviceToken, sender.FirstName + sender.LastName, currentType.Message, currentType.Name);

                            }

                        }

                    }
                    ContextEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

        }
        #region repositories
        playerContext _context;
        string connectionString = Startup.Configuration["DatabaseSettings:ConnectionString"];
        playerContext ContextEntities
        {
            get
            {
                if (_context == null)
                    _context = new playerContext(BaseService.GetDBContextConnectionOptions(connectionString));
                return _context;
            }
        }
        #endregion
    }
}
