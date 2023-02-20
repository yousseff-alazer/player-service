using Player.CommonDefinitions.Enums;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services
{
    public class PlayerInfoService : BaseService
    {
        public static bool UpdatePlayerPoint(playerContext _context, string playerId, int points, bool isQuiet)
        {
            var player = _context.PlayerInfos.FirstOrDefault(s => s.Id == playerId);
            if (player != null)
            {
                if (player.TotalPoints==null)
                {
                    player.TotalPoints = 0;
                }
                if (isQuiet)
                    player.TotalPoints -= points;
                else
                    player.TotalPoints += points;

                var notify = new Notification
                {
                    AppearDate = DateTime.Now,
                    Body = nameof(ProviderType.PLAYER),
                    Title = nameof(ProviderType.PLAYER),
                    CreationDate = DateTime.Now,
                    IsDeleted = 0,
                    IsSeen = 0,
                    IsSent = 0,
                    RecipientId = playerId,
                    SenderId = playerId,
                    NotificationTypeId = 12
                };
                _context.Notifications.Add(notify);
                //_context2.SaveChanges();
                if (_context.SaveChanges() > 0)
                    return true;
            }
            return false;
        }
    }
}
