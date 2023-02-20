using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public string RecipientId { get; set; }
        public int? NotificationTypeId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public ulong? IsSeen { get; set; }
        public DateTime? SeenDate { get; set; }
        public int? ObjectId { get; set; }
        public string SenderId { get; set; }
        public DateTime? CreationDate { get; set; }
        public ulong? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string TargetPath { get; set; }
        public DateTime? AppearDate { get; set; }
        public ulong? IsSent { get; set; }
        public int? NotificationEventId { get; set; }
    }
}
