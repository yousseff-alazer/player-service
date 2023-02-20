using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class AddressZone
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string PlayerId { get; set; }
        public string Address { get; set; }
        public ulong IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ulong IsDeleted { get; set; }
        public ulong IsHomeAddress { get; set; }
        public string Street { get; set; }
        public int? BuildingNumber { get; set; }
        public string Floor { get; set; }

        public virtual PlayerInfo Player { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
