using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.Records;
using Reservation.DAL.DB;
using System;
using System.Linq;


namespace Reservation.BL.Services.Managers
{
    public class Common_UserDeviceServiceManager
    {
        public static CommonUserDevice AddOrEditCommon_UserDevice(Common_UserDeviceRecord common_UserDeviceRecord, CommonUserDevice common_UserDevice = null)
        {
            if (common_UserDevice == null)
            {
                common_UserDevice = new CommonUserDevice();
            }


            //common_UserDevice.CommonUserId = common_UserDeviceRecord.UserID;
            common_UserDevice.DeviceName = common_UserDeviceRecord.DeviceType;
            common_UserDevice.DeviceImei = common_UserDeviceRecord.DeviceIMEI;
            common_UserDevice.DeviceType = common_UserDeviceRecord.DeviceType;
            common_UserDevice.DeviceOsversion = common_UserDeviceRecord.DeviceOSVersion;
            common_UserDevice.DeviceToken = common_UserDeviceRecord.DeviceToken;
            common_UserDevice.DeviceEmail = common_UserDeviceRecord.DeviceEmail;
            //common_UserDevice.EnableNotification = common_UserDeviceRecord.EnableNotification;
            common_UserDevice.AuthToken = common_UserDeviceRecord.AuthToken;
            common_UserDevice.AuthIp = common_UserDeviceRecord.AuthIP;
            common_UserDevice.AuthCreationDate = common_UserDeviceRecord.AuthCreationDate;
            common_UserDevice.AuthExpirationDate = common_UserDeviceRecord.AuthExpirationDate;
            //common_UserDevice.IsLoggedIn = common_UserDeviceRecord.IsLoggedIn;
            common_UserDevice.DeviceMobileNumber = common_UserDeviceRecord.DeviceMobileNumber;
            common_UserDevice.LastActiveDate = DateTime.Now;
            common_UserDevice.Lang = common_UserDeviceRecord.Lang;
            common_UserDevice.CreatedBy = common_UserDeviceRecord.CreatedBy;
            common_UserDevice.ModifiedBy = common_UserDeviceRecord.ModifiedBy;
            common_UserDevice.LastUpdateDate = DateTime.Now;
            common_UserDevice.CreationDate = common_UserDeviceRecord.CreationDate;
            //common_UserDevice.IsDeleted = common_UserDeviceRecord.IsDeleted;
            return common_UserDevice;
        }

        public static IQueryable<Common_UserDeviceRecord> ApplyFilter(IQueryable<Common_UserDeviceRecord> query, Common_UserDeviceRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.DeviceName))
            {
                query = query.Where(p => p.DeviceName != null && p.DeviceName.Contains(record.DeviceName));
            }
            if (!string.IsNullOrWhiteSpace(record.UserID))
            {
                query = query.Where(p => p.UserID == record.UserID);
            }
            if (!string.IsNullOrWhiteSpace(record.DeviceIMEI))
            {
                query = query.Where(p =>p.DeviceIMEI==record.DeviceIMEI);
            }
            if (!string.IsNullOrWhiteSpace(record.DeviceType))
            {
                query = query.Where(p => p.DeviceType != null && p.DeviceType.Contains(record.DeviceType));
            }
            if (!string.IsNullOrWhiteSpace(record.DeviceOSVersion))
            {
                query = query.Where(p => p.DeviceOSVersion != null && p.DeviceOSVersion.Contains(record.DeviceOSVersion));
            }
            if (!string.IsNullOrWhiteSpace(record.DeviceToken))
            {
                query = query.Where(p => p.DeviceToken != null && p.DeviceToken.Contains(record.DeviceToken));
            }
            if (!string.IsNullOrWhiteSpace(record.DeviceEmail))
            {
                query = query.Where(p => p.DeviceEmail != null && p.DeviceEmail.Contains(record.DeviceEmail));
            }
            if (!string.IsNullOrWhiteSpace(record.AuthToken))
            {
                query = query.Where(p => p.AuthToken != null && p.AuthToken.Contains(record.AuthToken));
            }
            if (!string.IsNullOrWhiteSpace(record.AuthIP))
            {
                query = query.Where(p => p.AuthIP != null && p.AuthIP.Contains(record.AuthIP));
            }
            if (record.AuthCreationDate.HasValue)
            {
                query = query.Where(p => p.AuthCreationDate == record.AuthCreationDate);
            }
            if (record.AuthExpirationDate.HasValue)
            {
                query = query.Where(p => p.AuthExpirationDate == record.AuthExpirationDate);
            }
            if (!string.IsNullOrWhiteSpace(record.DeviceMobileNumber))
            {
                query = query.Where(p => p.DeviceMobileNumber != null && p.DeviceMobileNumber.Contains(record.DeviceMobileNumber));
            }
            if (record.LastActiveDate.HasValue)
            {
                query = query.Where(p => p.LastActiveDate == record.LastActiveDate);
            }
            if (!string.IsNullOrWhiteSpace(record.Lang))
            {
                query = query.Where(p => p.Lang != null && p.Lang.Contains(record.Lang));
            }
            if (record.LastUpdateDate.HasValue)
            {
                query = query.Where(p => p.LastUpdateDate == record.LastUpdateDate);
            }
            return query;
        }
    }
}