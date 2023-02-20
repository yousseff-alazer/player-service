using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class Zone
    {
        public Zone()
        {
            AddressZones = new HashSet<AddressZone>();
            PlayerZones = new HashSet<PlayerZone>();
            ZoneLocalizes = new HashSet<ZoneLocalize>();
        }

        public int Id { get; set; }
        public int AreaId { get; set; }
        public string Name { get; set; }
        public ulong IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ulong IsDeleted { get; set; }

        public virtual Area Area { get; set; }
        public virtual ICollection<AddressZone> AddressZones { get; set; }
        public virtual ICollection<PlayerZone> PlayerZones { get; set; }
        public virtual ICollection<ZoneLocalize> ZoneLocalizes { get; set; }
    }
}
