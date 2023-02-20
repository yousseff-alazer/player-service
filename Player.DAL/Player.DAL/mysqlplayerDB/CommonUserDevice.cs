using System;
using System.Collections.Generic;

#nullable disable

namespace Player.DAL.mysqlplayerDB
{
    public partial class CommonUserDevice
    {
        public long Id { get; set; }
        public string CommonUserId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceImei { get; set; }
        public string DeviceType { get; set; }
        public string DeviceOsversion { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceEmail { get; set; }
        public ulong EnableNotification { get; set; }
        public string AuthToken { get; set; }
        public string AuthIp { get; set; }
        public DateTime? AuthCreationDate { get; set; }
        public DateTime? AuthExpirationDate { get; set; }
        public ulong IsLoggedIn { get; set; }
        public string DeviceMobileNumber { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public string Lang { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
        public ulong IsDeleted { get; set; }
        public ulong? IsGoogleSupport { get; set; }

        public virtual PlayerInfo CommonUser { get; set; }
    }
}
