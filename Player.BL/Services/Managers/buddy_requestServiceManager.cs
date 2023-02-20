using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Reservation.BL.Services.Managers
{
    public class buddy_requestServiceManager
    {
        public static BuddyRequest AddOrEditbuddy_request(buddy_requestRecord buddy_requestRecord, BuddyRequest buddyRequest = null)
        {
            if (buddyRequest == null)
            {
                buddyRequest = new BuddyRequest();
                buddyRequest.StatusId = buddy_requestRecord.StatusId;
                buddyRequest.CreatedById = buddy_requestRecord.CreatedById;
                buddyRequest.PlayerId = buddy_requestRecord.PlayerId;
                buddyRequest.SportId = buddy_requestRecord.SportId;
                buddyRequest.CreatedAt = DateTime.Now;

            }

            else
            {
                buddyRequest.StatusId = buddy_requestRecord.StatusId;
                buddyRequest.UpdatedAt = DateTime.Now;
            }
            if (buddy_requestRecord.IsConnectionOnly.HasValue)
                buddyRequest.IsConnectionOnly = buddy_requestRecord.IsConnectionOnly;


            return buddyRequest;
        }

        public static IQueryable<buddy_requestRecord> ApplyFilter(IQueryable<buddy_requestRecord> query, buddy_requestRecord record)
        {
            if (!string.IsNullOrWhiteSpace(record.CreatedById))
            {
                query = query.Where(p => p.CreatedById == record.CreatedById);
            }
            if (record.CreatedAt.HasValue)
            {
                query = query.Where(p => p.CreatedAt == record.CreatedAt);
            }
            if (!string.IsNullOrEmpty(record.PlayerId))
            {
                query = query.Where(p => p.PlayerId == record.PlayerId);
            }
            if (record.SportId > 0)
            {
                query = query.Where(p => p.SportId == record.SportId);
            }
            if (record.StatusId > 0)
            {
                query = query.Where(p => p.StatusId == record.StatusId);
            }
            return query;
        }
    }
}