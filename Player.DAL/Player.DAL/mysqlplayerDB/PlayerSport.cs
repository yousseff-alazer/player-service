using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class PlayerSport
    {
        public long Id { get; set; }
        public string PlayerId { get; set; }
        public long SportId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual PlayerInfo Player { get; set; }
        public virtual Sport Sport { get; set; }
    }
}
