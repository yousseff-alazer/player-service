using MongoDB.Driver;
using Newtonsoft.Json;
using Reservation.BL.Services.Managers;
using Reservation.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Requests;
using Reservation.CommonDefinitions.Responses;
using Reservation.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reservation.BL.Services
{
    public class player_agendaService : BaseService
    {
        #region player_agendaServices
        public static player_agendaResponse Listplayer_agenda(player_agendaRequest request)
        {
            var res = new player_agendaResponse();
            RunBase(request, res, (player_agendaRequest req) =>
            {
                try
                {
                    var query = request._context.PlayerAgenda.Find(c => true).ToList().Select(p => new player_agendaRecord
                    {
                        activity = p.Activity,
                        activityId = p.ActivityId,
                        providerId = p.ProviderId,
                        providerName = p.ProviderName,
                        providerType = p.ProviderType,
                        sportName = p.SportName,
                        createdAt = p.CreatedAt,
                        date = p.Date,
                        slotID = p.SlotId,
                        endTime = p.EndTime,
                        Id = p.Id,
                        playerId = p.PlayerId,
                        startTime = p.StartTime,
                        status = p.Status,
                        updatedAt = p.UpdatedAt,
                    }).AsQueryable();

                    if (request.player_agendaRecord != null)
                        query = player_agendaServiceManager.ApplyFilter(query, request.player_agendaRecord);

                    res.TotalCount = query.Count();

                    //query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    //if (request.PageSize > 0)
                    //    query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.player_agendaRecords = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._playerContext, "player_agenda", "playerAgendaList", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static player_agendaResponse Addplayer_agenda(player_agendaRequest request)
        {

            var res = new player_agendaResponse();
            RunBase(request, res, (player_agendaRequest req) =>
            {
                try
                {
                    //Check if Addded Before
                    var isAddedBefore = request._context.PlayerAgenda.Find(p => (p.ProviderId == request.player_agendaRecord.providerId && p.ProviderType == request.player_agendaRecord.providerType) && request.player_agendaRecord.slotID == p.SlotId).Any();
                    if (!isAddedBefore)
                    {
                        if (ValidateQuoteAvailable(req))
                        {
                            if (ValidateSlotAvailable(req))
                            {
                                var player_agenda = player_agendaServiceManager.AddOrEditplayer_agenda(0, request.player_agendaRecord);
                                player_agenda.Id = DateTime.Now.Ticks;

                                //Check if collection exist
                                var xxx = request._context.DB.GetCollection<object>(request.player_agendaRecord.playerId);
                                request._context.PlayerAgenda.InsertOne(player_agenda);

                                //Update Slots & Quote
                                RefreshQuoteAndSlots(request);
                                res.Message = "Added";
                                res.Success = true;
                                res.StatusCode = HttpStatusCode.OK;
                            }
                            else
                            {
                                res.Message = "Sorry, this slot not available for reservation";
                                res.Success = false;
                            }
                        }
                        else
                        {
                            res.Message = "Sorry, You have no available sesseions";
                            res.Success = false;
                        }
                    }
                    else
                    {
                        res.Message = "Sorry, Slot Added Before";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._playerContext, "player_agenda", "playerAgendaAdd", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static player_agendaResponse Editplayer_agenda(player_agendaRequest request)
        {

            var res = new player_agendaResponse();
            RunBase(request, res, (player_agendaRequest req) =>
            {
                try
                {
                    var model = request.player_agendaRecord;
                    var player_agenda = request._context.PlayerAgenda.Find(p => p.Id == model.Id).FirstOrDefault();
                    //var playerQuote = request._context.PlayerQuotes.FirstOrDefault(p => p.PlayerId == request.player_agendaRecord.playerId && request.player_agendaRecord.providerId == p.ProviderId && p.ProviderType == request.player_agendaRecord.providerType);

                    if (player_agenda != null)
                    {

                        request._context.PlayerAgenda.DeleteOneAsync(p => p.Id == player_agenda.Id);
                        RefreshQuoteAndSlots(request);

                        res.Message = "Your reservation is canceled, you can make a new one instead";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Sorry, You have no available sesseions";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
                    LogHelper.LogException(request._playerContext, "player_agenda", "playerAgendaUpdate", jsonRequest, ex);
                }
                return res;
            });
            return res;
        }
        public static bool ValidateSlotAvailable(player_agendaRequest request)
        {
            //var model = new slotRecord()
            //{
            //    id = request.player_agendaRecord.slotID ?? 0,
            //    isReserved = false
            //};
            //var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            //var response = UIHelper.AddRequestToServiceApi(request.ValidateSlotApiBaseUrl, json);

            var url = "";
            if (request.player_agendaRecord.providerType == "NUTRITIONIST")
                url = request.ValidateSlotApiBaseNUTRITIONISTUrl;
            else if (request.player_agendaRecord.providerType == "PHYSIOTHERAPIST")
                url = request.ValidateSlotApiBaseUrlPHYSIOTHERAPIST;
            else
                url = request.ValidateSlotApiBaseUrl;


            var model = new slotRecord()
            {
                id = request.player_agendaRecord.slotID ?? 0,
                isReserved = false
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            var response = UIHelper.AddRequestToServiceApi(url, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var isAvailable = result.ToString();
            var boolresult = false;
            bool.TryParse(isAvailable, out boolresult);
            return boolresult;
        }
        public static bool ValidateQuoteAvailable(player_agendaRequest request)
        {
            var modelquote = new player_agendaRecord()
            {
                playerId = request.player_agendaRecord.playerId,
                providerId = request.player_agendaRecord.providerId,
                providerType = request.player_agendaRecord.providerType,
            };
            var qutobody = JsonConvert.SerializeObject(modelquote, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            var res = UIHelper.AddRequestToServiceApi(request.ValidateQuoteApiBaseUrl, qutobody);
            var result = res.Content.ReadAsStringAsync().Result;
            var isAvailable = result.ToString();
            var boolresult = false;
            bool.TryParse(isAvailable, out boolresult);
            return boolresult;
        }
        public static async Task<bool> RefreshQuoteAndSlots(player_agendaRequest request)
        {
            var model = new slotRecord()
            {
                id = request.player_agendaRecord.slotID ?? 0,
                isReserved = request.player_agendaRecord.Id > 0 ? false : true
            };
            var slotbody = JsonConvert.SerializeObject(model, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            Uri uri = new Uri(request.RabbitMqUri);
            var endPoint = await request._bus.GetSendEndpoint(uri);
            await endPoint.Send(slotbody);
            //var response = UIHelper.AddRequestToServiceApi(request.UpdateSoltsApiBaseUrl, slotbody);

            //update quote
            var modelquote = new player_agendaRecord()
            {
                playerId = request.player_agendaRecord.playerId,
                providerId = request.player_agendaRecord.providerId,
                providerType = request.player_agendaRecord.providerType,
                Id = request.player_agendaRecord.Id

            };
            var qutobody = JsonConvert.SerializeObject(modelquote, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            UIHelper.AddRequestToServiceApi(request.UpdateQuoteApiBaseUrl, qutobody);

            return true;
            #endregion
        }
    }
}