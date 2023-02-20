using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using Reservation.DAL.DB;
using System.Linq;
using Reservation.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Reservation.CommonDefinitions.Records
{
    public class player_blockRecord
    {

        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PlayerId { get; set; }
        public sbyte IsDeleted { get; set; }
        public string CreatedById { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public string LastName { get; set; }
    }

}
