using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class AddressZoneServiceManager
    {
        public static AddressZone AddOrEditAddressZone(AddressZoneRecord AddressZoneRecord, AddressZone AddressZone = null)
        {
            if (AddressZone == null)
            {
                AddressZone = new AddressZone();
                AddressZone.CreatedBy = AddressZoneRecord.CreatedBy;
                AddressZone.CreatedOn = DateTime.Now;
                AddressZone.IsActive = 1;
                AddressZone.IsDeleted = 0;
            }
            else
            {
                AddressZone.UpdatedOn = DateTime.Now;
                AddressZone.UpdatedBy = AddressZoneRecord.UpdatedBy;
            }

            AddressZone.Address = AddressZoneRecord.Address ?? AddressZone.Address;
            AddressZone.BuildingNumber = AddressZoneRecord.BuildingNumber ?? AddressZone.BuildingNumber;
            AddressZone.Street = AddressZoneRecord.Street ?? AddressZone.Street;
            AddressZone.Floor = AddressZoneRecord.Floor ?? AddressZone.Floor;
            AddressZone.IsHomeAddress = AddressZoneRecord.IsHomeAddress ?? AddressZone.IsHomeAddress;
            AddressZone.PlayerId = AddressZoneRecord.PlayerId ?? AddressZone.PlayerId;
            AddressZone.ZoneId = AddressZoneRecord.ZoneId ?? AddressZone.ZoneId;
            return AddressZone;
        }
        public static IQueryable<AddressZoneRecord> ApplyFilter(IQueryable<AddressZoneRecord> query, AddressZoneRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Address))
            {
                query = query.Where(p => p.Address.ToLower().Contains(record.Address.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(record.PlayerId))
            {
                query = query.Where(p => p.PlayerId == record.PlayerId);
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (record.ZoneId > 0)
            {
                query = query.Where(s => s.ZoneId == record.ZoneId);
            }

            if (record.IsDeleted.HasValue)
            {
                query = query.Where(p => p.IsDeleted == record.IsDeleted);
            }

            if (record.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == record.IsActive);
            }

            if (record.IsHomeAddress.HasValue)
            {
                query = query.Where(p => p.IsHomeAddress == record.IsHomeAddress);
            }
            return query;
        }

    }
}
