using Reservation.CommonDefinitions.Records;
using Reservation.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Reservation.BL.Services.Managers
{
    public class player_agendaServiceManager
    {
        public static PlayerAgenda AddOrEditplayer_agenda(int createdBy, player_agendaRecord player_agendaRecord, PlayerAgenda player_agenda = null)
        {
            if (player_agenda == null)
                player_agenda = new PlayerAgenda();

            player_agenda.Activity = player_agendaRecord.activity;
            player_agenda.ActivityId = player_agendaRecord.activityId;
            player_agenda.ProviderId = player_agendaRecord.providerId;
            player_agenda.ProviderName = player_agendaRecord.providerName;
            player_agenda.SportName = player_agendaRecord.sportName;
            player_agenda.CreatedAt = DateTime.Now;
            player_agenda.Date = player_agendaRecord.date;
            player_agenda.EndTime = player_agendaRecord.endTime;
            player_agenda.PlayerId = player_agendaRecord.playerId;
            player_agenda.StartTime = player_agendaRecord.startTime;
            player_agenda.Status = "Active";
            player_agenda.UpdatedAt = DateTime.Now;

            return player_agenda;
        }

        public static IQueryable<player_agendaRecord> ApplyFilter(IQueryable<player_agendaRecord> query, player_agendaRecord record)
        {
            if (!string.IsNullOrWhiteSpace(record.activity))
            {
                query = query.Where(p => p.activity != null && p.activity.Contains(record.activity));
            }
            if (record.createdAt.HasValue)
            {
                query = query.Where(p => p.createdAt == record.createdAt);
            }
            if (!string.IsNullOrEmpty(record.playerId))
            {
                query = query.Where(p => p.playerId == record.playerId);
            }
            if (!string.IsNullOrEmpty(record.providerId))
            {
                query = query.Where(p => p.providerId == record.providerId);
            }
            if (!string.IsNullOrWhiteSpace(record.status))
            {
                query = query.Where(p => p.status != null && p.status.Contains(record.status));
            }
            if (record.updatedAt.HasValue)
            {
                query = query.Where(p => p.updatedAt == record.updatedAt);
            }
            return query;
        }
    }
}