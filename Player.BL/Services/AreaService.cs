using Microsoft.EntityFrameworkCore;
using Player.BL.Services.Managers;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Reservation.BL.Services;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Player.BL.Services
{
    public class AreaService : BaseService
    {
        #region Area Service
        public static AreaResponse ListAreas(AreaRequest request)
        {
            var res = new AreaResponse();
            RunBaseMysql(request, res, (AreaRequest req) =>
            {
                try
                {
                    var query = request._context.Areas.Include(s => s.AreaLocalizes)
                                                       .Where(a => a.IsDeleted != 1)
                                                       .Select(p => new AreaRecord
                                                       {
                                                           Id = p.Id,
                                                           CountryId = p.CountryId,
                                                           Name = request.LanguageId > 0 ? p.AreaLocalizes.FirstOrDefault(s => s.LanguageId == request.LanguageId && s.IsDeleted != 1).Name ?? p.Name : p.Name,
                                                           CreatedOn = p.CreatedOn,
                                                           UpdatedOn = p.UpdatedOn,
                                                           CreatedBy = p.CreatedBy,
                                                           UpdatedBy = p.UpdatedBy
                                                       });

                    if (request.AreaRecord != null)
                        query = AreaServiceManager.ApplyFilter(query, request.AreaRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.AreaRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Area", "ListAreas", jsonRequest, ex);

                }
                return res;
            });
            return res;
        }
        public static AreaResponse DeleteArea(AreaRequest request)
        {

            var res = new AreaResponse();
            RunBaseMysql(request, res, (AreaRequest req) =>
            {
                try
                {
                    var model = request.AreaRecord;
                    var Area = request._context.Areas.FirstOrDefault(s => s.Id == model.Id);
                    if (Area != null)
                    {
                        //update Challenge IsDeleted
                        Area.IsDeleted = 1;
                        Area.UpdatedOn = DateTime.Now;
                        request._context.SaveChanges();

                        res.AreaRecords = new List<AreaRecord>
                        {
                           new AreaRecord
                           {
                               Id=Area.Id,
                               CountryId=Area.CountryId,
                               Name=Area.Name,
                               IsActive=Area.IsActive,
                               CreatedBy=Area.CreatedBy,
                               CreatedOn=Area.CreatedOn,
                               IsDeleted=Area.IsDeleted,
                               UpdatedBy=Area.UpdatedBy,
                               UpdatedOn= Area.UpdatedOn
                           }
                        };
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
                    LogHelper.LogException(request._context, "Area", "DeleteArea", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AreaResponse AddArea(AreaRequest request)
        {

            var res = new AreaResponse();
            RunBaseMysql(request, res, (AreaRequest req) =>
            {
                try
                {
                    var IsAreaExist = request._context.Areas.Any(s => s.Id == request.AreaRecord.Id || (s.Name.ToLower() == request.AreaRecord.Name.ToLower() && s.CountryId == request.AreaRecord.CountryId));
                    if (!IsAreaExist)
                    {
                        var Area = AreaServiceManager.AddOrEditArea(request.AreaRecord);

                        request._context.Areas.Add(Area);
                        request._context.SaveChanges();
                        res.AreaRecords = new List<AreaRecord>
                        {
                           new AreaRecord
                           {
                               Id=Area.Id,
                               CountryId=Area.CountryId,
                               Name=Area.Name,
                               IsActive=Area.IsActive,
                               CreatedBy=Area.CreatedBy,
                               CreatedOn=Area.CreatedOn,
                               IsDeleted=Area.IsDeleted,
                               UpdatedBy=Area.UpdatedBy,
                               UpdatedOn= Area.UpdatedOn,
                               IsQueueEdit= false
                           }
                        };
                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this Area is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Area", "AddArea", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AreaResponse EditArea(AreaRequest request)
        {

            var res = new AreaResponse();
            RunBaseMysql(request, res, (AreaRequest req) =>
            {
                try
                {
                    var model = request.AreaRecord;
                    var Area = request._context.Areas.FirstOrDefault(s => s.Id == model.Id);
                    if (Area != null)
                    {
                        //update whole Agency
                        Area = AreaServiceManager.AddOrEditArea(request.AreaRecord, Area);
                        request._context.SaveChanges();

                        res.AreaRecords = new List<AreaRecord>
                        {
                           new AreaRecord
                           {
                               Id=Area.Id,
                               CountryId=Area.CountryId,
                               Name=Area.Name,
                               IsActive=Area.IsActive,
                               CreatedBy=Area.CreatedBy,
                               CreatedOn=Area.CreatedOn,
                               IsDeleted=Area.IsDeleted,
                               UpdatedBy=Area.UpdatedBy,
                               UpdatedOn= Area.UpdatedOn,
                               IsQueueEdit=true
                           }
                        };
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
                    LogHelper.LogException(request._context, "Area", "EditArea", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
