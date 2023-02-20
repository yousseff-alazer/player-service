using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class ZoneServiceManager
    {
        public static Zone AddOrEditZone(ZoneRecord ZoneRecord, Zone Zone = null)
        {
            if (Zone == null)
            {
                Zone = new Zone();
                Zone.CreatedBy = ZoneRecord.CreatedBy;
                Zone.CreatedOn = DateTime.Now;
                Zone.IsActive = 1;
                Zone.IsDeleted = 0;
            }
            else
            {
                Zone.UpdatedOn = DateTime.Now;
                Zone.UpdatedBy = ZoneRecord.UpdatedBy;
            }

            Zone.Name = ZoneRecord.Name ?? Zone.Name;
            Zone.AreaId = ZoneRecord.AreaId ?? Zone.AreaId;
            return Zone;
        }
        public static IQueryable<ZoneRecord> ApplyFilter(IQueryable<ZoneRecord> query, ZoneRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(record.Name.ToLower()));
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (record.AreaId > 0)
            {
                query = query.Where(s => s.AreaId == record.AreaId);
            }

            if (record.IsDeleted.HasValue)
            {
                query = query.Where(p => p.IsDeleted == record.IsDeleted);
            }

            if (record.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == record.IsActive);
            }
            return query;
        }

    }
}
