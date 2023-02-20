using Reservation.CommonDefinitions.Records;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Responses
{
    public class Common_UserDeviceResponse : BaseResponse
    {

        public List<Common_UserDeviceRecord> Common_UserDeviceRecords { get; set; }
    }
}