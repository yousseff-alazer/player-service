using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class AreaLocalizeServiceManager
    {
        public static AreaLocalize AddOrEditAreaLocalize(AreaLocalizeRecord AreaLocalizeRecord, AreaLocalize AreaLocalize = null)
        {
            if (AreaLocalize == null)
            {
                AreaLocalize = new AreaLocalize();
                AreaLocalize.CreatedBy = AreaLocalizeRecord.CreatedBy;
                AreaLocalize.CreatedOn = DateTime.Now;
                AreaLocalize.IsActive = 1;
                AreaLocalize.IsDeleted = 0;
            }
            else
            {
                AreaLocalize.UpdatedOn = DateTime.Now;
                AreaLocalize.UpdatedBy = AreaLocalizeRecord.UpdatedBy;
            }

            AreaLocalize.Name = AreaLocalizeRecord.Name ?? AreaLocalize.Name;
            AreaLocalize.LanguageId = AreaLocalizeRecord.LanguageId ?? AreaLocalize.LanguageId;
            AreaLocalize.AreaId = AreaLocalizeRecord.AreaId ?? AreaLocalize.AreaId;
            return AreaLocalize;
        }
        public static IQueryable<AreaLocalizeRecord> ApplyFilter(IQueryable<AreaLocalizeRecord> query, AreaLocalizeRecord record)
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
