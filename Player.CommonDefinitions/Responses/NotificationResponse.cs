using Reservation.CommonDefinitions.Records;
using Shared.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class NotificationResponse : BaseResponse
    {

        public List<NotificationRecord> NotificationRecords { get; set; }
    }
}