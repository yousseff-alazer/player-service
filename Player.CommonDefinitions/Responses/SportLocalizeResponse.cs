using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Responses
{
    public class SportLocalizeResponse : BaseResponse
    {
        public List<SportLocalizeRecord> SportLocalizeRecords { get; set; }
    }
}
