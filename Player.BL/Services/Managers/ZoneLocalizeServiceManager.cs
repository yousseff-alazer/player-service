using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class ZoneLocalizeServiceManager
    {
        public static ZoneLocalize AddOrEditZoneLocalize(ZoneLocalizeRecord ZoneLocalizeRecord, ZoneLocalize ZoneLocalize = null)
        {
            if (ZoneLocalize == null)
            {
                ZoneLocalize = new ZoneLocalize();
                ZoneLocalize.CreatedBy = ZoneLocalizeRecord.CreatedBy;
                ZoneLocalize.CreatedOn = DateTime.Now;
                ZoneLocalize.IsActive = 1;
                ZoneLocalize.IsDeleted = 0;
            }
            else
            {
                ZoneLocalize.UpdatedOn = DateTime.Now;
                ZoneLocalize.UpdatedBy = ZoneLocalizeRecord.UpdatedBy;
            }

            ZoneLocalize.Name = ZoneLocalizeRecord.Name ?? ZoneLocalize.Name;
            ZoneLocalize.LanguageId = ZoneLocalizeRecord.LanguageId ?? ZoneLocalize.LanguageId;
            ZoneLocalize.ZoneId = ZoneLocalizeRecord.ZoneId ?? ZoneLocalize.ZoneId;
            return ZoneLocalize;
        }
        public static IQueryable<ZoneLocalizeRecord> ApplyFilter(IQueryable<ZoneLocalizeRecord> query, ZoneLocalizeRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(record.Name.ToLower()));
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (record.LanguageId > 0)
            {
                query = query.Where(s => s.LanguageId == record.LanguageId);
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
