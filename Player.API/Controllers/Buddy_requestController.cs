using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Reservation.BL.Services;
using Reservation.CommonDefinitions.Requests;
using Reservation.DAL.DB;
using System;
using Player.DAL.mysqlplayerDB;
using Reservation.CommonDefinitions.Responses;
using System.Linq;
using Reservation.CommonDefinitions.Records;
using Reservation.Helpers;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Reservation.CommonDefinitions.MysqlRequests;
using Microsoft.EntityFrameworkCore;
using Reservation.BL.Services.Managers;

namespace Reservation.Portal.Controllers
{
    [Route("api/Buddies")]
    public class Buddy_requestController : Controller
    {
        private readonly playerContext _context;
        private readonly IConfiguration configuration;

        public Buddy_requestController(playerContext context,
        IConfiguration _configuration)
        {
            _context = context;
            configuration = _configuration;

        }



        [HttpPost]
        [Route("ListBuddyRequest")]
        public ActionResult List([FromBody] buddy_requestRequest request)
        {
            request._context = _context;

            var buddy_requestResponse = Buddy_requestService.ListBuddy_request(request);
            return Ok(new
            {
                buddy_requestResponse
            });

        }


        [Route("SendBuddyRequest")]
        [HttpPost]
        public ActionResult SendRequest([FromBody] buddy_requestRequest request)
        {
            request._context = _context;
            if (request.Buddy_requestRecord != null)
                request.Buddy_requestRecord.StatusId = 0;

            var buddy_requestResponse = Buddy_requestService.AddBuddy_request(request);
            return Ok(new
            {
                buddy_requestResponse
            });
        }


        [Route("CancelBuddyRequest")]
        [HttpPost]
        public ActionResult CancelRequest([FromBody] buddy_requestRequest request)
        {
            request._context = _context;
            var buddy_requestResponse = Buddy_requestService.DeleteBuddy_request(request);
            return Ok(new
            {
                buddy_requestResponse
            });
        }


        [Route("ApproveBuddyRequest")]
        [HttpPost]
        public ActionResult ApproveRequest([FromBody] buddy_requestRequest request)
        {
            request._context = _context;
            request.Buddy_requestRecord.StatusId = 1;
            request.Buddy_requestRecord.IsConnectionOnly = 0;
            var buddy_requestResponse = Buddy_requestService.EditBuddy_request(request);
            return Ok(new
            {
                buddy_requestResponse
            });
        }


        [Route("RejectBuddyRequest")]
        [HttpPost]
        public ActionResult RejectRequest([FromBody] buddy_requestRequest request)
        {
            request._context = _context;
            request.Buddy_requestRecord.StatusId = 2;
            var buddy_requestResponse = Buddy_requestService.EditBuddy_request(request);
            return Ok(new
            {
                buddy_requestResponse
            });
        }


        #region Connections & Buddies
        [HttpPost]
        [Route("MyBuddies")]
        public ActionResult MyBuddies([FromBody] Player_ProfileRequest request)
        {
            var player_profileResponse = new Player_profileResponse();
            try
            {
                if (request.Player_profileRecord != null)
                {
                    var blockedIDs = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.CreatedById == request.Player_profileRecord.Id).Select(p => p.PlayerId).ToList();
                    //var blockedIDsMine = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.PlayerId == request.Player_profileRecord.Id).Select(p => p.CreatedById).ToList();

                    var userinfo = _context.PlayerInfos.Include(p => p.PlayerSports).FirstOrDefault(p => p.Id == request.Player_profileRecord.Id);
                    if (userinfo != null)
                    {
                        var buddiesReqUPlayersList = _context.BuddyRequests.Include(ss=>ss.Sport).Where(s =>s.SportId!=null && s.IsConnectionOnly == 0 && s.IsDeleted != 1 && s.StatusId == 1 && (s.CreatedById == userinfo.Id || userinfo.Id == s.PlayerId)).ToList();
                        var buddiesReqUPlayers = buddiesReqUPlayersList.Select(p => p.PlayerId).Where(f=>f!=userinfo.Id).ToList();
                        var buddiesReqUCreated = buddiesReqUPlayersList.Select(p => p.CreatedById).Where(f => f != userinfo.Id).ToList();
                        var player = _context.PlayerInfos.Include(i => i.BuddyRequestPlayers).Where(p => p.IsDeleted != 1 && p.PlayerSports != null && (buddiesReqUPlayers.Contains(p.Id) || buddiesReqUCreated.Contains(p.Id)) && ((blockedIDs.Any() && !blockedIDs.Contains(p.Id)) || !blockedIDs.Any())).Select(p => new player_profileRecord()
                        {
                            Id = p.Id,
                            BirthDate = p.BirthDate,
                            Email = p.Email,
                            FirstName = p.FirstName,
                            Height = p.Height,
                            ImageUrl = p.ImageUrl,
                            LastName = p.LastName,
                            Location = p.Location,
                            MobileNumber = p.MobileNumber,
                            Weight = p.Weight,
                            CoverUrl = p.CoverUrl,
                            CreatedAt = p.CreatedAt,
                            UpdatedAt = p.UpdatedAt,
                           
                        }).ToList();
                        foreach (var item in player)
                        {
                            item.SportName = buddiesReqUPlayersList.Where(p => p.SportId > 0).FirstOrDefault(s => (s.CreatedById == item.Id || item.Id == s.PlayerId) && (s.CreatedById == userinfo.Id || userinfo.Id == s.PlayerId)).Sport.Name;
                            item.SportIcon = buddiesReqUPlayersList.Where(p => p.SportId > 0).FirstOrDefault(s => (s.CreatedById == item.Id || item.Id == s.PlayerId) && (s.CreatedById == userinfo.Id || userinfo.Id == s.PlayerId)).Sport.IconUrl;
                        }
                        player_profileResponse.player_profileRecords = player;
                        return Ok(new
                        {
                            player_profileResponse
                        });
                    }
                    else
                    {
                        player_profileResponse.Success = false;
                        player_profileResponse.Message = "User Not Found";
                        return Ok(new { player_profileResponse });
                    }
                }
                else
                {
                    player_profileResponse.Success = false;
                    player_profileResponse.Message = "Empty Request Body";
                    return Ok(new { player_profileResponse });
                }
            }
            catch (Exception ex)
            {
                player_profileResponse.Success = false;
                player_profileResponse.Message = ex.Message;
                return Ok(new { player_profileResponse });
            }
        }


        [HttpPost]
        [Route("Buddies")]
        public ActionResult Buddies([FromBody] Player_ProfileRequest request)
        {
            var player_profileResponse = new Player_profileResponse();
            try
            {
                if (request.Player_profileRecord != null)
                {
                    //var excluderdUserIDs = _context.BuddyRequests.Where(v =>v.IsDeleted!=1 && v.StatusId == 1).Select(ss => ss.PlayerId).ToList();
                    var buddiesReqUPlayers = _context.BuddyRequests.Where(s => s.IsDeleted != 1 && (s.StatusId != 2 && s.IsConnectionOnly != 1) && (s.CreatedById == request.Player_profileRecord.Id || request.Player_profileRecord.Id == s.PlayerId)).Select(p => p.PlayerId).Where(s => s != request.Player_profileRecord.Id).ToList();
                    var buddiesReqUCreated = _context.BuddyRequests.Where(s => s.IsDeleted != 1 && (s.StatusId != 2 && s.IsConnectionOnly != 1) && (s.CreatedById == request.Player_profileRecord.Id || request.Player_profileRecord.Id == s.PlayerId)).Select(p => p.CreatedById).Where(s => s != request.Player_profileRecord.Id).ToList();
                    var userinfo = _context.PlayerInfos.Include(p => p.PlayerSports).FirstOrDefault(p => p.Id == request.Player_profileRecord.Id);
                    if (userinfo != null)
                    {
                        var blockedIDs = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.CreatedById == request.Player_profileRecord.Id).Select(p => p.PlayerId).ToList();
                        //var blockedIDsMine = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.PlayerId == request.Player_profileRecord.Id).Select(p => p.CreatedById).ToList();

                        var userSportsIds = userinfo.PlayerSports.Select(p => p.SportId).ToList();
                        if (userSportsIds != null && userSportsIds.Count() > 0)
                        {
                            var player = _context.PlayerInfos.Include(i => i.PlayerSports).Where(p => p.IsDeleted != 1 && p.Provider == "PLAYER" && p.Id != userinfo.Id && p.PlayerSports != null && !buddiesReqUPlayers.Contains(p.Id) && !buddiesReqUCreated.Contains(p.Id) && ((blockedIDs.Any() && !blockedIDs.Contains(p.Id)) || !blockedIDs.Any())).Select(p => new player_profileRecord()
                            {
                                Id = p.Id,
                                BirthDate = p.BirthDate,
                                Email = p.Email,
                                FirstName = p.FirstName,
                                Height = p.Height,
                                ImageUrl = p.ImageUrl,
                                LastName = p.LastName,
                                Location = p.Location,
                                MobileNumber = p.MobileNumber,
                                Weight = p.Weight,
                                CoverUrl = p.CoverUrl,
                                CreatedAt = p.CreatedAt,
                                UpdatedAt = p.UpdatedAt,
                                BuddyRequestId = p.BuddyRequestCreatedBies.Where(s => s.StatusId == 0 && s.PlayerId == p.Id).Any() ? p.BuddyRequestCreatedBies.Where(s => s.StatusId == 0 && s.PlayerId == p.Id).FirstOrDefault().Id : 0,
                                CommonSportRecords = p.PlayerSports.Where(sp => userSportsIds.Contains(sp.SportId)).Select(n => new Player_SportRecord
                                {
                                    Id = n.Id,
                                    Name = n.Sport.Name,
                                    SportId = n.SportId,
                                    PlayerId = n.PlayerId,
                                    IconUrl = n.Sport.IconUrl
                                })
                            }).ToList();
                            if (player != null)
                            {
                                player_profileResponse.Success = true;
                                player_profileResponse.player_profileRecords = player.Where(p => p.CommonSportRecords != null && p.CommonSportRecords.Any()).ToList();
                                if (request.OrderByColumn == "SportCount")
                                {
                                    player_profileResponse.player_profileRecords = player_profileResponse.player_profileRecords.OrderByDescending(p => p.CommonSportRecords.Count()).ToList();
                                }
                                if (request.Player_profileRecord != null && !string.IsNullOrEmpty(request.Player_profileRecord.FirstName))
                                {
                                    player_profileResponse.player_profileRecords = player_profileResponse.player_profileRecords.Where(p => p.FirstName.ToLower().Contains(request.Player_profileRecord.FirstName.ToLower()) || p.LastName.ToLower().Contains(request.Player_profileRecord.FirstName.ToLower())).ToList();

                                }
                            }
                        }

                    }

                    return Ok(new
                    {
                        player_profileResponse
                    });
                }
                else
                {
                    player_profileResponse.Success = false;
                    player_profileResponse.Message = "Empty Request Body";
                    return Ok(new { player_profileResponse });
                }
            }
            catch (Exception ex)
            {
                player_profileResponse.Success = false;
                player_profileResponse.Message = ex.Message;
                return Ok(new { player_profileResponse });
            }
        }



        [HttpPost]
        [Route("MyConnections")]
        public ActionResult MyConnections([FromBody] Player_ProfileRequest request)
        {
            var player_profileResponse = new Player_profileResponse();
            try
            {
                if (request.Player_profileRecord != null)
                {
                    var userinfo = _context.PlayerInfos.Include(p => p.PlayerSports).FirstOrDefault(p => p.Id == request.Player_profileRecord.Id);
                    if (userinfo != null)
                    {
                        var buddiesReqUPlayers = _context.BuddyRequests.Where(s => s.IsDeleted != 1 && s.StatusId == 1 && (s.CreatedById == userinfo.Id || userinfo.Id == s.PlayerId)).Select(p => p.PlayerId).Where(s => s != userinfo.Id).ToList();
                        var buddiesReqUCreated = _context.BuddyRequests.Where(s => s.IsDeleted != 1 && s.StatusId == 1 && (s.CreatedById == userinfo.Id || userinfo.Id == s.PlayerId)).Select(p => p.CreatedById).Where(s => s != userinfo.Id).ToList();
                        var blockedIDs = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.CreatedById == request.Player_profileRecord.Id).Select(p => p.PlayerId).ToList();
                        //var blockedIDsMine = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.PlayerId == request.Player_profileRecord.Id).Select(p => p.CreatedById).ToList();

                        var player = _context.PlayerInfos.Include(i => i.BuddyRequestPlayers).Where(p => p.IsDeleted != 1 && p.PlayerSports != null && (buddiesReqUPlayers.Contains(p.Id) || buddiesReqUCreated.Contains(p.Id)) && ((blockedIDs.Any() && !blockedIDs.Contains(p.Id)) || !blockedIDs.Any())).Select(p => new player_profileRecord()
                        {
                            Id = p.Id,
                            BirthDate = p.BirthDate,
                            Email = p.Email,
                            FirstName = p.FirstName,
                            Height = p.Height,
                            ImageUrl = p.ImageUrl,
                            LastName = p.LastName,
                            Location = p.Location,
                            MobileNumber = p.MobileNumber,
                            Weight = p.Weight,
                            CoverUrl = p.CoverUrl,
                            CreatedAt = p.CreatedAt,
                            UpdatedAt = p.UpdatedAt,
                        }).ToList();

                        player_profileResponse.player_profileRecords = player;
                        player_profileResponse.TotalCount = player_profileResponse.player_profileRecords.Count();
                        return Ok(new
                        {
                            player_profileResponse
                        });
                    }
                    else
                    {
                        player_profileResponse.Success = false;
                        player_profileResponse.Message = "User Not Found";
                        return Ok(new { player_profileResponse });
                    }
                }
                else
                {
                    player_profileResponse.Success = false;
                    player_profileResponse.Message = "Empty Request Body";
                    return Ok(new { player_profileResponse });
                }
            }
            catch (Exception ex)
            {
                player_profileResponse.Success = false;
                player_profileResponse.Message = ex.Message;
                return Ok(new { player_profileResponse });
            }
        }


        [HttpPost]
        [Route("Connections")]
        public ActionResult Connections([FromBody] Player_ProfileRequest request)
        {
            var player_profileResponse = new Player_profileResponse();
            try
            {
                if (request.Player_profileRecord != null)
                {
                    var buddiesReqUPlayers = _context.BuddyRequests.Where(s => s.IsDeleted != 1 && s.StatusId == 1 && (s.CreatedById == request.Player_profileRecord.Id || request.Player_profileRecord.Id == s.PlayerId)).Select(p => p.PlayerId).Where(s => s != request.Player_profileRecord.Id).ToList();
                    var buddiesReqUCreated = _context.BuddyRequests.Where(s => s.IsDeleted != 1 && s.StatusId == 1 && (s.CreatedById == request.Player_profileRecord.Id || request.Player_profileRecord.Id == s.PlayerId)).Select(p => p.CreatedById).Where(s => s != request.Player_profileRecord.Id).ToList();
                    var blockedIDs = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.CreatedById == request.Player_profileRecord.Id ).Select(p => p.PlayerId).ToList();
                    var blockedIDsMine = _context.PlayerBlocks.Where(p => p.IsDeleted != 1 && p.PlayerId == request.Player_profileRecord.Id).Select(p => p.CreatedById).ToList();
                    var player = _context.PlayerInfos.Include(i => i.BuddyRequestPlayers).Where(p => p.IsDeleted != 1 && p.Provider == "PLAYER" && p.Id!= request.Player_profileRecord.Id &&!buddiesReqUPlayers.Contains(p.Id) && !buddiesReqUCreated.Contains(p.Id) && ((blockedIDs.Any() && !blockedIDs.Contains(p.Id)) || !blockedIDs.Any()) && ((blockedIDsMine.Any() && !blockedIDsMine.Contains(p.Id)) || !blockedIDsMine.Any())).Select(p => new player_profileRecord()
                    {
                        Id = p.Id,
                        BirthDate = p.BirthDate,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        Height = p.Height,
                        ImageUrl = p.ImageUrl,
                        LastName = p.LastName,
                        Location = p.Location,
                        MobileNumber = p.MobileNumber,
                        Weight = p.Weight,
                        CoverUrl = p.CoverUrl,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        BuddyRequestId = p.BuddyRequestCreatedBies.Where(s => s.StatusId == 0 && s.PlayerId == p.Id).Any() ? p.BuddyRequestCreatedBies.Where(s => s.StatusId == 0 && s.PlayerId == p.Id).FirstOrDefault().Id : 0,
                    }).ToList();
                    if (player != null)
                    {
                        player_profileResponse.Success = true;
                        player_profileResponse.player_profileRecords = player.ToList();
                        if (request.Player_profileRecord != null && !string.IsNullOrEmpty(request.Player_profileRecord.FirstName))
                        {
                            player_profileResponse.player_profileRecords = player_profileResponse.player_profileRecords.Where(p => p.FirstName.ToLower().Contains(request.Player_profileRecord.FirstName.ToLower()) || p.LastName.ToLower().Contains(request.Player_profileRecord.FirstName.ToLower())).ToList();

                        }
                        if (request.PageSize >= 0)
                        {
                            
                            var skipedPages = request.PageSize * request.PageIndex;
                            player_profileResponse.player_profileRecords = player_profileResponse.player_profileRecords.Skip(skipedPages).Take(request.PageSize).ToList();
                        }
                
                    }
                    return Ok(new
                    {
                        player_profileResponse
                    });
                }
                else
                {
                    player_profileResponse.Success = false;
                    player_profileResponse.Message = "Empty Request Body";
                    return Ok(new { player_profileResponse });
                }
            }
            catch (Exception ex)
            {
                player_profileResponse.Success = false;
                player_profileResponse.Message = ex.Message;
                return Ok(new { player_profileResponse });
            }
        }

        [Route("AddConnection")]
        [HttpPost]
        public ActionResult AddConnection([FromBody] buddy_requestRequest request)
        {
            request._context = _context;
            if (request.Buddy_requestRecord != null)
            {
                request.Buddy_requestRecord.StatusId = 1;
                request.Buddy_requestRecord.IsConnectionOnly = 1;
            }


            var buddy_requestResponse = Buddy_requestService.AddBuddy_request(request);
            return Ok(new
            {
                buddy_requestResponse
            });
        }
        #endregion

        [HttpPost]
        [Route("SendTestNotification")]
        public ActionResult SendTestNotification(string msg)
        {
            var dic = _context.CommonUserDevices.Where(p => !string.IsNullOrEmpty(p.DeviceToken)).Select(c => c.DeviceToken).ToList();
            foreach (var item in dic)
            {
                NotificationManager.SendNotification(item, "Test Notification", msg, "");

            }
            return Ok(
                "User Count" + dic.Count()
            );
        }
    }
}