using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class NotificationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
