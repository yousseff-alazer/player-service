using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using Reservation.DAL.DB;
using System.Linq;
using Reservation.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Player.DAL.mysqlplayerDB;

namespace Reservation.CommonDefinitions.Records
{
    public class player_profileRecord
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string Location { get; set; }
        public IFormFile ProfileFile { get; set; }
        public IFormFile CoverFile { get; set; }
        public string CoverUrl { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string CreatedById { get; set; }

        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Player_SportRecord> CommonSportRecords { get; set; }
        public long BuddyRequestId { get; set; }
        public bool IsMyBuddy { get; set; }
        public string Provider { get; set; }
        public long? TotalPoints { get; set; }
        public string SportName { get; set; }
        public string SportIcon { get; set; }
        public bool IsMyConnection { get; set; }
        public int? Gender { get; set; }
        public string GenderText { get; set; }
        public bool IsBlocked { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string PlayerId { get; set; }
        public Country CountryObject { get; set; }
    }

}
