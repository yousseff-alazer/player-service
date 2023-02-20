using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Reservation.BL.Services.Managers
{
    public class player_profileServiceManager
    {
        public static PlayerInfo AddOrEditplayer_profile(player_profileRecord player_profileRecord, PlayerInfo playerInfo = null)
        {
            if (playerInfo == null)
            {
                playerInfo = new PlayerInfo();
                playerInfo.Id = player_profileRecord.Id;
                playerInfo.Email = player_profileRecord.Email;
                playerInfo.Provider = player_profileRecord.Provider;
                playerInfo.CreatedAt = DateTime.Now;
                playerInfo.IsDeleted = 0;

            }


            if (!string.IsNullOrEmpty(player_profileRecord.FirstName))
                playerInfo.FirstName = player_profileRecord.FirstName;

            if (!string.IsNullOrEmpty(player_profileRecord.MobileNumber))
                playerInfo.MobileNumber = player_profileRecord.MobileNumber;

            if (!string.IsNullOrEmpty(player_profileRecord.LastName))
                playerInfo.LastName = player_profileRecord.LastName;

            playerInfo.UpdatedAt = DateTime.Now;

            if (player_profileRecord.Height.HasValue)
                playerInfo.Height = player_profileRecord.Height;

            if (player_profileRecord.Weight.HasValue)
                playerInfo.Weight = player_profileRecord.Weight;

            playerInfo.Gender = player_profileRecord.Gender ?? playerInfo.Gender;
            playerInfo.TotalPoints = player_profileRecord.TotalPoints ?? playerInfo.TotalPoints;

            if (!string.IsNullOrEmpty(player_profileRecord.Location))
                playerInfo.Location = player_profileRecord.Location;

            if (player_profileRecord.BirthDate.HasValue)
                playerInfo.BirthDate = player_profileRecord.BirthDate;



            return playerInfo;
        }

        public static IQueryable<player_profileRecord> ApplyFilter(IQueryable<player_profileRecord> query, player_profileRecord record)
        {
            //if (!string.IsNullOrWhiteSpace(record.CreatedById))
            //{
            //    query = query.Where(p => p.CreatedById == record.CreatedById);
            //}
            if (!string.IsNullOrWhiteSpace(record.Provider))
            {
                query = query.Where(p => p.Provider != null && p.Provider.ToLower() == record.Provider.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(record.Id))
            {
                query = query.Where(p => p.Id == record.Id);
            }
            if (!string.IsNullOrWhiteSpace(record.Email))
            {
                query = query.Where(p => p.Email == record.Email);
            }
            if (!string.IsNullOrWhiteSpace(record.Location))
            {
                query = query.Where(p => record.Location.Contains(p.Location));
            }
            if (!string.IsNullOrEmpty(record.FirstName))
            {
                query = query.Where(p => record.FirstName.Contains(p.FirstName) || record.FirstName.Contains(p.LastName));
            }

            return query;
        }
    }
}