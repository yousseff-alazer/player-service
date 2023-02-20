using MassTransit;
using Microsoft.Extensions.Options;
using Player.BL.Services;
using Player.CommonDefinitions.Enums;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.MysqlRequests;
using Shared.CommonDefinitions.Records;
using System;
using System.Linq;

using System.Threading.Tasks;

namespace Player.API.Consumers
{
    public class NotificationConsumer : IConsumer<NotificationRecord>
    {
        private readonly playerContext _context;
        private readonly IOptions<AppSettingsRecord> _appSettings;

        public NotificationConsumer(IOptions<AppSettingsRecord> appSettings)
        {
            _appSettings = appSettings;
            _context = new playerContext(BaseService.GetDBContextConnectionOptions(_appSettings.Value.DatabaseSettings.ConnectionString));
        }
        public async Task Consume(ConsumeContext<NotificationRecord> context)
        {
            var data = context.Message;
            if (data != null)
            {
                
                var request = new NotificationRequest();
                request._context = _context;
                request.NotificationRecord = data;
                NotificationService.AddNotification(request);
                
            };
        }
    }
}

