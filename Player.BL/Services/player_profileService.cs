
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Player.CommonDefinitions.Enums;
using Player.CommonDefinitions.Records;
using Player.DAL.mysqlplayerDB;
using Reservation.BL.Services;
using Reservation.BL.Services.Managers;
using Reservation.CommonDefinitions.MysqlRequests;
using Reservation.CommonDefinitions.Records;
using Reservation.CommonDefinitions.Responses;
using Reservation.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reservation.BL.Services
{
    public class Player_profileService : BaseService
    {

        #region Buddy_requestServices
        public static Player_profileResponse Listplayer_profile(Player_ProfileRequest request)
        {
            var res = new Player_profileResponse();
            //var blockedIDs = request._context.PlayerBlocks.Where(p => p.IsDeleted != 1 && (p.CreatedById == request.Player_profileRecord.Id)).Select(p => p.CreatedById).ToList();

            RunBaseMysql(request, res, (Player_ProfileRequest req) =>
            {
                try
                {
                    var query = request._context.PlayerInfos.Include(s=>s.Country)
                    .Where(t => t.IsDeleted != 1).Select(p => new player_profileRecord
                    {
                        Id = p.Id,
                        BirthDate = p.BirthDate,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        Height = p.Height,
                        ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "https://c4s-player.s3.amazonaws.com/blank.png" : p.ImageUrl,
                        LastName = p.LastName,
                        Location = p.Location,
                        MobileNumber = p.MobileNumber,
                        Weight = p.Weight,
                        CoverUrl = string.IsNullOrEmpty(p.CoverUrl) ? "https://c4s-player.s3.amazonaws.com/3c9ceb87-3114-49e7-b8ac-81e33c0066d6.png" : p.CoverUrl,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        Provider = p.Provider,
                        TotalPoints = p.TotalPoints,
                        CountryId = p.CountryId,
                        CountryName = p.Country.NameEn,
                        //IsBlocked= blockedIDs !=null && blockedIDs.Contains(p.Id)?true:false,
                        IsMyBuddy = (!string.IsNullOrEmpty(request.Player_profileRecord.Id) && !string.IsNullOrEmpty(request.Player_profileRecord.CreatedById)) ? (p.BuddyRequestCreatedBies.Any(p => p.IsDeleted != 1 && p.IsConnectionOnly == 0 && p.StatusId == 1 && (p.PlayerId == request.Player_profileRecord.CreatedById))) || (p.BuddyRequestPlayers.Any(p => p.IsDeleted != 1 && p.IsConnectionOnly == 0 && p.StatusId == 1 && (p.CreatedById == request.Player_profileRecord.CreatedById))) : false,
                        IsMyConnection = (!string.IsNullOrEmpty(request.Player_profileRecord.Id) && !string.IsNullOrEmpty(request.Player_profileRecord.CreatedById)) ? (p.BuddyRequestCreatedBies.Any(p => p.IsDeleted != 1 && p.IsConnectionOnly == 1 && p.StatusId == 1 && (p.PlayerId == request.Player_profileRecord.CreatedById))) || (p.BuddyRequestPlayers.Any(p => p.IsDeleted != 1 && p.IsConnectionOnly == 1 && p.StatusId == 1 && (p.CreatedById == request.Player_profileRecord.CreatedById))) : false,
                        Gender = p.Gender,
                        GenderText = p.Gender == 0 ? "Male" : p.Gender == 1 ? "Female" : "Not Detected",
                        CountryObject = p.Country
                    });

                    if (request.Player_profileRecord != null)
                        query = player_profileServiceManager.ApplyFilter(query, request.Player_profileRecord);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", true);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    var records = query.ToList();
                    if (records != null && records.Count() == 1)
                    {
                        if (records[0].IsBlocked)
                        {
                            records[0].FirstName = "Connect4Sports User";
                            records[0].ImageUrl = "https://media-exp1.licdn.com/dms/image/C4E0BAQHSN3RK-NVQMQ/company-logo_400_400/0/1635243375862?e=2147483647&v=beta&t=d3DdQqeQJ3cJw-EUXciv6PTjUVU48IShNFErjmP8pCk";
                            records[0].CoverUrl = "https://wallpapercrafter.com/th800/9019-smile-smiley-sad-neon-red-dark-4k.jpg";
                        }
                        res.Connections = request._context.BuddyRequests.Where(p => (p.PlayerId == records[0].Id || p.CreatedById == records[0].Id) && p.IsDeleted != 1 && p.StatusId == 1).Count();

                        var buddiesReqUPlayersList = request._context.BuddyRequests.Include(ss => ss.Sport).Where(s => s.SportId != null && s.IsConnectionOnly == 0 && s.IsDeleted != 1 && s.StatusId == 1 && (s.CreatedById == request.Player_profileRecord.CreatedById || request.Player_profileRecord.CreatedById == s.PlayerId)).ToList();
                        if(!string.IsNullOrEmpty(request.Player_profileRecord.CreatedById) && !string.IsNullOrEmpty(request.Player_profileRecord.Id))
                        {
                            foreach (var item in records)
                            {
                                try
                                {
                                    item.SportName = buddiesReqUPlayersList.Where(p => p.SportId > 0).FirstOrDefault(s => (s.CreatedById == item.Id || item.Id == s.PlayerId) && (s.CreatedById == request.Player_profileRecord.CreatedById || request.Player_profileRecord.CreatedById == s.PlayerId))?.Sport?.Name;
                                    item.SportIcon = buddiesReqUPlayersList.Where(p => p.SportId > 0).FirstOrDefault(s => (s.CreatedById == item.Id || item.Id == s.PlayerId) && (s.CreatedById == request.Player_profileRecord.CreatedById || request.Player_profileRecord.CreatedById == s.PlayerId))?.Sport?.IconUrl;
                                }
                                catch { }
                                
                            }
                        }
                        
                    }

                    res.player_profileRecords = records;
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }
        public static Player_profileResponse Deleteplayer_profile(Player_ProfileRequest request)
        {

            var res = new Player_profileResponse();
            RunBaseMysql(request, res, (Player_ProfileRequest req) =>
            {
                try
                {
                    var model = request.Player_profileRecord;
                    var profile = request._context.PlayerInfos.FirstOrDefault(c => c.Id == model.Id);
                    if (profile != null)
                    {
                        //update Agency IsDeleted
                        profile.IsDeleted = 1;
                        request._context.SaveChanges();

                        res.Message = "Deleted";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "InvalidData";
                        res.Success = false;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }
        public static Player_profileResponse Addplayer_profile(Player_ProfileRequest request)
        {

            var res = new Player_profileResponse();
            RunBaseMysql(request, res, (Player_ProfileRequest req) =>
            {
                try
                {
                    var player_Profile = player_profileServiceManager.AddOrEditplayer_profile(request.Player_profileRecord);
                    var existBefore = request._context.PlayerInfos.FirstOrDefault(p => p.Email == request.Player_profileRecord.Email);
                    if (existBefore == null)
                    {
                        request._context.PlayerInfos.Add(player_Profile);
                        request._context.SaveChanges();

                        res.Message = "User Added";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "User Exist";
                        res.Success = false;
                        res.StatusCode = HttpStatusCode.OK;
                    }

                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }
        public static Player_profileResponse Editplayer_profile(Player_ProfileRequest request)
        {

            var res = new Player_profileResponse();
            RunBaseMysql(request, res, (Player_ProfileRequest req) =>
            {
                try
                {
                    var model = request.Player_profileRecord;
                    var player_Profile = request._context.PlayerInfos.FirstOrDefault(p => p.Id == model.Id);
                    if (player_Profile != null)
                    {
                        player_Profile = player_profileServiceManager.AddOrEditplayer_profile(request.Player_profileRecord, player_Profile);
                        if (request.Player_profileRecord.ProfileFile != null)
                        {
                            var jsonString = UIHelper.UploadFileToS3(model.ProfileFile, model.ProfileFile.FileName);
                            player_Profile.ImageUrl = jsonString;
                        }
                        if (request.Player_profileRecord.CoverFile != null)
                        {
                            var jsonString = UIHelper.UploadFileToS3(model.CoverFile, model.CoverFile.FileName);
                            player_Profile.CoverUrl = jsonString;
                        }

                        request._context.SaveChanges();
                        res.Message = "Updated Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }
        public static async Task PlayerInfoRabbitMQAsync(Player_ProfileRequest request, IBus _bus, AppSettingsRecord appSettingsRecord)
        {
            var model = request.Player_profileRecord;
            var PlayerRMQRecord = new PlayerRMQRecord()
            {
                Id = model.Id,
                ImageUrl = model.ImageUrl,
                Name = model.FirstName + " " + model.LastName,
            };

            var task1 = new Task(async () =>
            {
                Uri PlayerGroupUri = new Uri(appSettingsRecord.RabbitMQ.RabbitMqRootUri + appSettingsRecord.RabbitMQ.PlayerGroup);
                var endPointPlayerGroupUri = await _bus.GetSendEndpoint(PlayerGroupUri);
                await endPointPlayerGroupUri.Send(PlayerRMQRecord);
            });
            task1.Start();

            var task2 = new Task(async () =>
            {
                Uri PlayerChallengeUri = new Uri(appSettingsRecord.RabbitMQ.RabbitMqRootUri + appSettingsRecord.RabbitMQ.PlayerChallenge);
                var endPointPlayerChallengeUri = await _bus.GetSendEndpoint(PlayerChallengeUri);
                await endPointPlayerChallengeUri.Send(PlayerRMQRecord);
            });
            task2.Start();

            var task3 = new Task(async () =>
            {
                Uri PlayerReviewUri = new Uri(appSettingsRecord.RabbitMQ.RabbitMqRootUri + appSettingsRecord.RabbitMQ.PlayerReview);
                var endPointPlayerReviewUri = await _bus.GetSendEndpoint(PlayerReviewUri);
                await endPointPlayerReviewUri.Send(PlayerRMQRecord);
            });
            task3.Start();

            var task4 = new Task(async () =>
            {
                Uri PlayerReservationUri = new Uri(appSettingsRecord.RabbitMQ.RabbitMqRootUri + appSettingsRecord.RabbitMQ.PlayerReservation);
                var endPointPlayerReservationUri = await _bus.GetSendEndpoint(PlayerReservationUri);
                await endPointPlayerReservationUri.Send(PlayerRMQRecord);
            });
            task4.Start();

            var task5 = new Task(async () =>
            {
                Uri PlayerOrderUri = new Uri(appSettingsRecord.RabbitMQ.RabbitMqRootUri + appSettingsRecord.RabbitMQ.PlayerOrder);
                var endPointPlayerOrderUri = await _bus.GetSendEndpoint(PlayerOrderUri);
                await endPointPlayerOrderUri.Send(PlayerRMQRecord);
            });
            task5.Start();
            try
            {
                Task.WaitAll(task1, task2, task3, task4, task5);
            }
            catch (Exception ex)
            {

            }


        }
        public static Player_profileResponse AddOrEditPlayerCountry(playerContext _context, string playerId , string countryId)
        {
            var res = new Player_profileResponse();

            var player_Profile = _context.PlayerInfos.FirstOrDefault(p => p.Id == playerId);
            var country = _context.Countries.FirstOrDefault(p => p.Id == countryId);
            if (player_Profile != null && country != null)
            {
                player_Profile.CountryId = countryId;
                _context.SaveChanges();
                res.Message = "Updated Successfully";
                res.Success = true;
                res.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                res.Message = "playerId or countryId is not correct";
                res.Success = false;
                res.StatusCode = HttpStatusCode.NotFound;
            }
            return res;

        }
        #endregion
    }
}