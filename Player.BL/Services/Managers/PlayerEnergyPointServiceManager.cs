using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services.Managers
{
    public class PlayerEnergyPointServiceManager
    {
        public static PlayerEnergyPoint AddOrEditPlayerEnergyPoint(PlayerEnergyPointRecord PlayerEnergyPointRecord, PlayerEnergyPoint PlayerEnergyPoint = null)
        {
            if (PlayerEnergyPoint == null)
            {
                PlayerEnergyPoint = new PlayerEnergyPoint();
                if (PlayerEnergyPointRecord != null)
                    PlayerEnergyPoint.PlayerId = PlayerEnergyPointRecord.PlayerId;
            }
            else
                PlayerEnergyPoint.UpdatedAt = DateTime.Now;

            PlayerEnergyPoint.ProviderId = PlayerEnergyPointRecord.ProviderId ?? PlayerEnergyPoint.ProviderId;
            PlayerEnergyPoint.ProviderType = PlayerEnergyPointRecord.ProviderType ?? PlayerEnergyPoint.ProviderType;
            PlayerEnergyPoint.ProviderName = PlayerEnergyPointRecord.ProviderName ?? PlayerEnergyPoint.ProviderName;
            PlayerEnergyPoint.ProviderImageUrl = PlayerEnergyPointRecord.ProviderImageUrl ?? PlayerEnergyPoint.ProviderImageUrl;
            PlayerEnergyPoint.Points = PlayerEnergyPointRecord.Points ?? PlayerEnergyPoint.Points;

            return PlayerEnergyPoint;
        }

        public static IQueryable<PlayerEnergyPointRecord> ApplyFilter(IQueryable<PlayerEnergyPointRecord> query, PlayerEnergyPointRecord record)
        {

            if (!string.IsNullOrWhiteSpace(record.ProviderType))
            {
                query = query.Where(p => p.ProviderType.Contains(record.ProviderType));
            }

            if (!string.IsNullOrWhiteSpace(record.PlayerId))
            {
                query = query.Where(p => p.PlayerId == record.PlayerId);
            }


            if (!string.IsNullOrWhiteSpace(record.ProviderId))
            {
                query = query.Where(p => p.ProviderId == record.ProviderId);
            }

            if (!string.IsNullOrWhiteSpace(record.ProviderType))
            {
                query = query.Where(p => p.ProviderType == record.ProviderType);
            }

            if (record.Id > 0)
            {
                query = query.Where(s => s.Id == record.Id);
            }

            return query;
        }
    }
}
