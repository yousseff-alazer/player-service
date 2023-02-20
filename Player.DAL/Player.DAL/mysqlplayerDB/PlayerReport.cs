using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class PlayerReport
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string PlayerId { get; set; }
        public string Comment { get; set; }
        public ulong IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual PlayerInfo Player { get; set; }
    }
}
