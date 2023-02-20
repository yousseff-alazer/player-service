using Player.BL.Services.Managers;
using Player.CommonDefinitions.Records;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Reservation.CommonDefinitions.Responses;
using Reservation.CommonDefinitions.Requests;
using Player.DAL;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.Records;
using Reservation.BL.Services.Managers;

namespace Player.BL.Services
{
    public class Common_UserDeviceService : BaseService
    {

        #region Common_UserDeviceServices
        public static Common_UserDeviceResponse ListCommon_UserDevice(Common_UserDeviceRequest request)
        {
            var res = new Common_UserDeviceResponse();
            RunBaseMysql(request, res, (Common_UserDeviceRequest req) =>
            {
                try
                {
                    var query = request._context.CommonUserDevices.Include(c => c.CommonUser).Select(user => new Common_UserDeviceRecord
                    {
                       
                            ID = user.Id,
                            UserID = user.CommonUserId,
                            DeviceIMEI = user.DeviceImei,
                    });

                    if (request.Common_UserDeviceRecord != null)
                        query = Common_UserDeviceServiceManager.ApplyFilter(query, request.Common_UserDeviceRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.Common_UserDeviceRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Common_UserDevice", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Common_UserDeviceResponse DeleteCommon_UserDevice(Common_UserDeviceRequest request)
        {

            var res = new Common_UserDeviceResponse();
            RunBaseMysql(request, res, (Common_UserDeviceRequest req) =>
            {
                try
                {
                    var model = request.Common_UserDeviceRecord;
                    var Common_UserDevice = request._context.CommonUserDevices.Where(c => c.DeviceImei == model.DeviceIMEI);
                    if (Common_UserDevice != null)
                    {
                        request._context.CommonUserDevices.RemoveRange(Common_UserDevice);
                        //update Agency IsDeleted
                        request._context.SaveChanges();

                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "MessageKey.InvalidDate.ToString()";
                        res.Success = false;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Common_UserDevice", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static Common_UserDeviceResponse AddCommon_UserDevice(Common_UserDeviceRequest request)
        {

            var res = new Common_UserDeviceResponse();
            RunBaseMysql(request, res, (Common_UserDeviceRequest req) =>
            {
                try
                {
                    var Common_UserDevice = Common_UserDeviceServiceManager.AddOrEditCommon_UserDevice(request.Common_UserDeviceRecord);
                    request._context.CommonUserDevices.Add(Common_UserDevice);
                    request._context.SaveChanges();

                    res.Message = "AddedSuccessfully";
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;


                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._context, "Common_UserDevice", "", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        #endregion
    }
}