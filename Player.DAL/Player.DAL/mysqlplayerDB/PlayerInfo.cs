using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class PlayerInfo
    {
        public PlayerInfo()
        {
            AddressZones = new HashSet<AddressZone>();
            BuddyRequestCreatedBies = new HashSet<BuddyRequest>();
            BuddyRequestPlayers = new HashSet<BuddyRequest>();
            CommonUserDevices = new HashSet<CommonUserDevice>();
            PlayerBlockCreatedBies = new HashSet<PlayerBlock>();
            PlayerBlockPlayers = new HashSet<PlayerBlock>();
            PlayerEnergyPoints = new HashSet<PlayerEnergyPoint>();
            PlayerReports = new HashSet<PlayerReport>();
            PlayerSports = new HashSet<PlayerSport>();
            PlayerZones = new HashSet<PlayerZone>();
        }

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
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CoverUrl { get; set; }
        public string Provider { get; set; }
        public long? TotalPoints { get; set; }
        public int? Gender { get; set; }
        public ulong IsDeleted { get; set; }
        public string CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<AddressZone> AddressZones { get; set; }
        public virtual ICollection<BuddyRequest> BuddyRequestCreatedBies { get; set; }
        public virtual ICollection<BuddyRequest> BuddyRequestPlayers { get; set; }
        public virtual ICollection<CommonUserDevice> CommonUserDevices { get; set; }
        public virtual ICollection<PlayerBlock> PlayerBlockCreatedBies { get; set; }
        public virtual ICollection<PlayerBlock> PlayerBlockPlayers { get; set; }
        public virtual ICollection<PlayerEnergyPoint> PlayerEnergyPoints { get; set; }
        public virtual ICollection<PlayerReport> PlayerReports { get; set; }
        public virtual ICollection<PlayerSport> PlayerSports { get; set; }
        public virtual ICollection<PlayerZone> PlayerZones { get; set; }
    }
}
