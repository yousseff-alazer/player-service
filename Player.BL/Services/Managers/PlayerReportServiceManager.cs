using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class PlayerReportServiceManager
    {
        public static PlayerReport AddOrEditPlayerReport(PlayerReportRecord PlayerReportRecord, PlayerReport PlayerReport = null)
        {
            if (PlayerReport == null)
            {
                PlayerReport = new PlayerReport();
                PlayerReport.CreatedAt = DateTime.Now;
                PlayerReport.IsDeleted = 0;

            }

            PlayerReport.Comment = PlayerReportRecord.Comment ?? PlayerReport.Comment;
            PlayerReport.UserId = PlayerReportRecord.UserId ?? PlayerReport.UserId;
            PlayerReport.PlayerId = PlayerReportRecord.PlayerId ?? PlayerReport.PlayerId;
            PlayerReport.UpdatedAt = DateTime.Now;

            return PlayerReport;
        }

        public static IQueryable<PlayerReportRecord> ApplyFilter(IQueryable<PlayerReportRecord> query, PlayerReportRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.Comment))
            {
                query = query.Where(p => p.Comment != null && p.Comment.Contains(record.Comment));
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            if (!string.IsNullOrWhiteSpace(record.PlayerId))
            {
                query = query.Where(s => s.PlayerId == record.PlayerId);
            }

            if (!string.IsNullOrWhiteSpace(record.UserId))
            {
                query = query.Where(p => p.UserId == record.UserId);
            }

            if (record.IsDeleted.HasValue)
            {
                query = query.Where(p => p.IsDeleted == record.IsDeleted);
            }
            return query;
        }

    }
}
