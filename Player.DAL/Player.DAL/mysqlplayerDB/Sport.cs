using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class Sport
    {
        public Sport()
        {
            BuddyRequests = new HashSet<BuddyRequest>();
            PlayerSports = new HashSet<PlayerSport>();
            SportLocalizes = new HashSet<SportLocalize>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ulong IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public long? SportId { get; set; }

        public virtual ICollection<BuddyRequest> BuddyRequests { get; set; }
        public virtual ICollection<PlayerSport> PlayerSports { get; set; }
        public virtual ICollection<SportLocalize> SportLocalizes { get; set; }
    }
}
