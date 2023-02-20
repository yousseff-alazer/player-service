using Player.BL.Services.Managers;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Reservation.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Reservation.Helpers;
using Player.DAL.mysqlplayerDB;
using System.Text.Json;

namespace Player.BL.Services
{
    public class SportLocalizeService : BaseService
    {
        #region SportLocalize Service
        public static SportLocalizeResponse ListSportLocalizes(SportLocalizeRequest request)
        {
            var res = new SportLocalizeResponse();
            RunBaseMysql(request, res, (SportLocalizeRequest req) =>
            {
                try
                {
                    var query = request._context.SportLocalizes.Where(a => a.IsDeleted !=1).Select(p => new SportLocalizeRecord
                    {
                        Id = p.Id,
                        ExternalSportId = p.ExternalSportId ?? p.Id,
                        Name = p.Name,
                        CreationDate = p.CreationDate,
                        ModificationDate = p.ModificationDate,
                        CreatedBy = p.CreatedBy,
                        ModifiedBy = p.ModifiedBy,
                        //IsDeleted = p.IsDeleted.Value,

                    });

                    if (request.SportLocalizeRecord != null)
                        query = SportLocalizeServiceManager.ApplyFilter(query, request.SportLocalizeRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.SportLocalizeRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "SportLocalize", "ListSportLocalizes", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static SportLocalizeResponse DeleteSportLocalize(SportLocalizeRequest request)
        {

            var res = new SportLocalizeResponse();
            RunBaseMysql(request, res, (SportLocalizeRequest req) =>
            {
                try
                {
                    var model = request.SportLocalizeRecord;
                    var SportLocalize = request._context.SportLocalizes.FirstOrDefault(s => s.ExternalSportId == model.ExternalSportId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (SportLocalize != null)
                    {
                        //update Challenge IsDeleted
                        SportLocalize.IsDeleted = 1;
                        SportLocalize.ModificationDate = DateTime.Now;
                        request._context.SaveChanges();

                        res.Message = "Deleted Sucessfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Not Exist";
                        res.Success = false;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "SportLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static SportLocalizeResponse AddSportLocalize(SportLocalizeRequest request)
        {

            var res = new SportLocalizeResponse();
            RunBaseMysql(request, res, (SportLocalizeRequest req) =>
            {
                try
                {
                    var model = request.SportLocalizeRecord;
                    var IsSportLocalizeExist = request._context.SportLocalizes.Any(s => s.ExternalSportId == model.ExternalSportId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (!IsSportLocalizeExist)
                    {
                        request.SportLocalizeRecord.SportId = GetSportId(request._context, request.SportLocalizeRecord.ExternalSportId.Value);

                        var SportLocalize = SportLocalizeServiceManager.AddOrEditSportLocalize(request.SportLocalizeRecord);
                        request._context.SportLocalizes.Add(SportLocalize);
                        request._context.SaveChanges();

                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this Sport Localize is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "SportLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static SportLocalizeResponse EditSportLocalize(SportLocalizeRequest request)
        {

            var res = new SportLocalizeResponse();
            RunBaseMysql(request, res, (SportLocalizeRequest req) =>
            {
                try
                {
                    var model = request.SportLocalizeRecord;
                    var SportLocalize = request._context.SportLocalizes.FirstOrDefault(s => s.ExternalSportId == model.ExternalSportId && s.LanguageId == model.LanguageId && s.IsDeleted != 1);
                    if (SportLocalize != null)
                    {
                        //request.SportLocalizeRecord.SportId = GetSportId(request._context, request.SportLocalizeRecord.ExternalSportId.Value);
                        SportLocalize = SportLocalizeServiceManager.AddOrEditSportLocalize(request.SportLocalizeRecord, SportLocalize);
                        request._context.SaveChanges();

                        res.Message = "UpdatedSuccessfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Not Exist";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "SportLocalizeConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static long GetSportId(playerContext _context ,long ExternalSportId)
        {
            var sport = _context.Sports.FirstOrDefault(s=> s.SportId == ExternalSportId);
            return sport != null ? sport.Id : 0;
        }

        #endregion
    }
}
