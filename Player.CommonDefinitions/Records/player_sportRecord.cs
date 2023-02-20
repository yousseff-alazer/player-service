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
    public class Player_SportRecord
    {

        public long Id { get; set; }
        public string PlayerId { get; set; }
        public long SportId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }


        public bool IsSelected { get; set; }

        public List<long> SelectedSports  { get; set; }


    }

}
