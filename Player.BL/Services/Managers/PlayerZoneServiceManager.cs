using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class PlayerZoneServiceManager
    {
        public static PlayerZone AddOrEditPlayerZone(PlayerZoneRecord PlayerZoneRecord, PlayerZone PlayerZone = null)
        {
            if (PlayerZone == null)
            {
                PlayerZone = new PlayerZone();
                PlayerZone.CreatedBy = PlayerZoneRecord.CreatedBy;
                PlayerZone.CreatedOn = DateTime.Now;
                PlayerZone.IsActive = 1;
                PlayerZone.IsDeleted = 0;
            }
            else
            {
                PlayerZone.UpdatedOn = DateTime.Now;
                PlayerZone.UpdatedBy = PlayerZoneRecord.UpdatedBy;
            }

            PlayerZone.Tax = PlayerZoneRecord.Tax ?? PlayerZone.Tax;
            PlayerZone.ZoneId = PlayerZoneRecord.ZoneId ?? PlayerZone.ZoneId;
            PlayerZone.PlayerId = PlayerZoneRecord.PlayerId ?? PlayerZone.PlayerId;
            return PlayerZone;
        }
        public static IQueryable<PlayerZoneRecord> ApplyFilter(IQueryable<PlayerZoneRecord> query, PlayerZoneRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.PlayerId))
            {
                query = query.Where(p => p.PlayerId== record.PlayerId);
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
            return query;
        }

    }
}
