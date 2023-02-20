using Player.BL.Services.Managers;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Reservation.BL.Services;
using Reservation.Helpers;
using System.Text.Json;

namespace Player.BL.Services
{
    public class SportService : BaseService
    {
        #region Sport Service
        public static SportResponse ListSports(SportRequest request)
        {
            var res = new SportResponse();
            RunBaseMysql(request, res, (SportRequest req) =>
            {
                try
                {
                    var query = request._context.Sports.Include(s=> s.SportLocalizes)
                                                       .Where(a => a.IsDeleted != 1)
                                                       .Select(p => new sportRecord
                                                       {
                                                           id = p.Id,
                                                           SportId = p.SportId ?? p.Id,
                                                           name = request.LanguageId > 0 ? p.SportLocalizes.FirstOrDefault(s => s.LanguageId == request.LanguageId && s.IsDeleted != 1).Name ?? p.Name : p.Name,
                                                           IconUrl = p.IconUrl,
                                                           createdAt = p.CreatedAt,
                                                           updatedAt = p.UpdatedAt, 
                                                           CreatedBy = p.CreatedBy,
                                                           ModifiedBy = p.ModifiedBy,  
                                                           //IsDeleted = p.IsDeleted.Value,
                                                       
                                                       });

                    if (request.sportRecord != null)
                        query = SportServiceManager.ApplyFilter(query, request.sportRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.sportRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Sport", "ListSports", jsonRequest, ex);

                }
                return res;
            });
            return res;
        }
        public static SportResponse DeleteSport(SportRequest request)
        {

            var res = new SportResponse();
            RunBaseMysql(request, res, (SportRequest req) =>
            {
                try
                {
                    var model = request.sportRecord;
                    var Sport = request._context.Sports.FirstOrDefault(s=> s.SportId == model.SportId);
                    if (Sport != null)
                    {
                        //update Challenge IsDeleted
                        Sport.IsDeleted = 1;
                        Sport.UpdatedAt = DateTime.Now;
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
                    LogHelper.LogException(request._context, "SportConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static SportResponse AddSport(SportRequest request)
        {

            var res = new SportResponse();
            RunBaseMysql(request, res, (SportRequest req) =>
            {
                try
                {
                    var IsSportExist = request._context.Sports.Any(s => s.SportId == request.sportRecord.SportId);
                    if (!IsSportExist)
                    {
                        var Sport = SportServiceManager.AddOrEditSport(request.sportRecord);

                        request._context.Sports.Add(Sport);
                        request._context.SaveChanges();

                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this sport is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }
                    
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "SportConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static SportResponse EditSport(SportRequest request)
        {

            var res = new SportResponse();
            RunBaseMysql(request, res, (SportRequest req) =>
            {
                try
                {
                    var model = request.sportRecord;
                    var Sport = request._context.Sports.FirstOrDefault(s=> s.SportId == model.SportId);
                    if (Sport != null)
                    {
                        //update whole Agency
                        Sport = SportServiceManager.AddOrEditSport(request.sportRecord, Sport);
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
                    LogHelper.LogException(request._context, "SportConsumer", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion

    }
}
