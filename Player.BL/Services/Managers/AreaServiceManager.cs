using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class AreaServiceManager
    {
        public static Area AddOrEditArea(AreaRecord AreaRecord, Area Area = null)
        {
            if (Area == null)
            {
                Area = new Area();
                Area.CreatedBy = AreaRecord.CreatedBy;
                Area.CreatedOn = DateTime.Now;
                Area.IsActive = 1;
                Area.IsDeleted = 0;
            }
            else
            {
                Area.UpdatedOn = DateTime.Now;
                Area.UpdatedBy = AreaRecord.UpdatedBy;
            }

            Area.Name = AreaRecord.Name ?? Area.Name;
            Area.CountryId = AreaRecord.CountryId ?? Area.CountryId;
            return Area;
        }
        public static IQueryable<AreaRecord> ApplyFilter(IQueryable<AreaRecord> query, AreaRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(record.Name.ToLower()));
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (!string.IsNullOrWhiteSpace(record.CountryId))
            {
                query = query.Where(s => s.CountryId == record.CountryId);
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
