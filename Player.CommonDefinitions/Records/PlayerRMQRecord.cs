using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Records
{
    public class PlayerRMQRecord
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

}
