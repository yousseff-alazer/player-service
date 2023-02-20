using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Player.CommonDefinitions.Requests
{
    public class Common_UserDeviceRequest : MysqlBaseRequest
    {
        public Common_UserDeviceRecord Common_UserDeviceRecord { get; set; }

    
}
}