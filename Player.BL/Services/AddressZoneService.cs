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
    public class AddressZoneService : BaseService
    {
        #region AddressZone Service
        public static AddressZoneResponse ListAddressZones(AddressZoneRequest request)
        {
            var res = new AddressZoneResponse();
            RunBaseMysql(request, res, (AddressZoneRequest req) =>
            {
                try
                {
                    var query = request._context.AddressZones
                                                       .Where(a => a.IsDeleted != 1)
                                                       .Select(p => new AddressZoneRecord
                                                       {
                                                           Id = p.Id,
                                                           PlayerId = p.PlayerId,
                                                           ZoneId = p.ZoneId,
                                                           Address = p.Address,
                                                           //Address = request.LanguageId > 0 ? p.AddressZoneLocalizes.FirstOrDefault(s => s.LanguageId == request.LanguageId && s.IsDeleted != 1).Name ?? p.Name : p.Name,
                                                           CreatedOn = p.CreatedOn,
                                                           UpdatedOn = p.UpdatedOn,
                                                           CreatedBy = p.CreatedBy,
                                                           UpdatedBy = p.UpdatedBy,
                                                           IsActive= p.IsActive,
                                                           IsHomeAddress= p.IsHomeAddress,
                                                           BuildingNumber= p.BuildingNumber,
                                                           Floor = p.Floor,
                                                           Street= p.Street,
                                                           IsDeleted= p.IsDeleted,
                                                       });

                    if (request.AddressZoneRecord != null)
                        query = AddressZoneServiceManager.ApplyFilter(query, request.AddressZoneRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.AddressZoneRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "AddressZone", "ListAddressZones", jsonRequest, ex);

                }
                return res;
            });
            return res;
        }
        public static AddressZoneResponse DeleteAddressZone(AddressZoneRequest request)
        {

            var res = new AddressZoneResponse();
            RunBaseMysql(request, res, (AddressZoneRequest req) =>
            {
                try
                {
                    var model = request.AddressZoneRecord;
                    var AddressZone = request._context.AddressZones.FirstOrDefault(s => s.Id == model.Id);
                    if (AddressZone != null)
                    {
                        //update Challenge IsDeleted
                        AddressZone.IsDeleted = 1;
                        AddressZone.UpdatedOn = DateTime.Now;
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
                    LogHelper.LogException(request._context, "AddressZone", "DeleteAddressZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AddressZoneResponse AddAddressZone(AddressZoneRequest request)
        {

            var res = new AddressZoneResponse();
            RunBaseMysql(request, res, (AddressZoneRequest req) =>
            {
                try
                {
                    var IsAddressZoneExist = request._context.AddressZones.Any(s => s.Id == request.AddressZoneRecord.Id);
                    if (!IsAddressZoneExist)
                    {
                        var AddressZone = AddressZoneServiceManager.AddOrEditAddressZone(request.AddressZoneRecord);

                        request._context.AddressZones.Add(AddressZone);
                        request._context.SaveChanges();

                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "this AddressZone is already exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.Forbidden;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "AddressZone", "AddAddressZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static AddressZoneResponse EditAddressZone(AddressZoneRequest request)
        {

            var res = new AddressZoneResponse();
            RunBaseMysql(request, res, (AddressZoneRequest req) =>
            {
                try
                {
                    var model = request.AddressZoneRecord;
                    var AddressZone = request._context.AddressZones.FirstOrDefault(s => s.Id == model.Id);
                    if (AddressZone != null)
                    {
                        //update whole Agency
                        AddressZone = AddressZoneServiceManager.AddOrEditAddressZone(request.AddressZoneRecord, AddressZone);
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
                    LogHelper.LogException(request._context, "AddressZone", "EditAddressZone", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }

        #endregion
    }
}
