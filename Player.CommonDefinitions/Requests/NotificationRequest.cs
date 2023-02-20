using Reservation.CommonDefinitions.Records;
using Shared.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.CommonDefinitions.MysqlRequests
{
    public class NotificationRequest : MysqlBaseRequest
    {
        public NotificationRecord NotificationRecord { get; set; }
     
    }
}