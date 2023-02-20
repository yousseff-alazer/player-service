using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class SportServiceManager
    {
        public static Sport AddOrEditSport(sportRecord SportRecord, Sport Sport = null)
        {
            if (Sport == null)
            {
                Sport = new Sport();
                Sport.CreatedBy = SportRecord.CreatedBy;
                Sport.CreatedAt = DateTime.Now;
                Sport.SportId = SportRecord.SportId;
            }
            else
            {
                Sport.UpdatedAt = DateTime.Now;
                Sport.ModifiedBy = SportRecord.ModifiedBy;
            }

            Sport.Name = SportRecord.name ?? Sport.Name;
            Sport.IconUrl = SportRecord.IconUrl ?? Sport.IconUrl;
            return Sport;
        }
        public static IQueryable<sportRecord> ApplyFilter(IQueryable<sportRecord> query, sportRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.name))
            {
                query = query.Where(p => p.name.ToLower().Contains(record.name.ToLower()));
            }

            if (record.id > 0)
            {
                query = query.Where(s => s.id == record.id);
            }

            if (record.IsDeleted.HasValue)
            {
                query = query.Where(p => p.IsDeleted.Value);
            }

            return query;
        }

    }
}
