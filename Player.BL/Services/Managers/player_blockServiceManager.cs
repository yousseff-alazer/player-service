using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Reservation.BL.Services.Managers
{
    public class player_blockServiceManager
    {
        public static PlayerBlock AddOrEditplayer_block(player_blockRecord player_blockRecord, PlayerBlock playerBlock = null)
        {
            if (playerBlock == null)
                playerBlock = new PlayerBlock();

            playerBlock.CreatedById = player_blockRecord.CreatedById;
            playerBlock.PlayerId = player_blockRecord.PlayerId;
            playerBlock.CreatedAt = DateTime.Now;
            playerBlock.UpdatedAt = DateTime.Now;
            return playerBlock;
        }

        public static IQueryable<player_blockRecord> ApplyFilter(IQueryable<player_blockRecord> query, player_blockRecord record)
        {
            if (!string.IsNullOrWhiteSpace(record.CreatedById))
            {
                query = query.Where(p =>  p.CreatedById==record.CreatedById);
            }
            if (record.CreatedAt.HasValue)
            {
                query = query.Where(p => p.CreatedAt == record.CreatedAt);
            }
            if (!string.IsNullOrEmpty(record.PlayerId))
            {
                query = query.Where(p => p.PlayerId == record.PlayerId);
            }
            return query;
        }
    }
}