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

namespace Reservation.Portal.Controllers
{
    [Route("api/sports")]
    public class Player_SportController : Controller
    {
        private readonly playerContext _context;
        private readonly IConfiguration configuration;

        public Player_SportController(playerContext context,
        IConfiguration _configuration)
        {
            _context = context;
            configuration = _configuration;

        }

        [HttpPost]
        [Route("ListSports")]
        public ActionResult List([FromBody] Player_SportRequest request)
        {
            var player_SportResponse = new Player_SportResponse();

            try
            {
                if (request.Player_SportRecord != null)
                {
                    if (!string.IsNullOrEmpty(request.Player_SportRecord.PlayerId)) {
                        var sports = _context.Sports.Select(p => new Player_SportRecord()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            IconUrl = p.IconUrl,
                            IsSelected = p.PlayerSports.Any(s => s.PlayerId == request.Player_SportRecord.PlayerId && s.SportId == p.Id)

                        }).ToList();
                        player_SportResponse.Success = true;
                        player_SportResponse.Player_SportRecords = sports;
                    }
                    else
                    {
                        var sports = _context.Sports.Select(p => new Player_SportRecord()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            IconUrl = p.IconUrl

                        }).ToList();
                        player_SportResponse.Success = true;
                        player_SportResponse.Player_SportRecords = sports;
                    }
                }
                else
                {
                    player_SportResponse.Success = false;
                    player_SportResponse.Message = "PlayerID Empty";


                }
                return Ok(new
                {
                    player_SportResponse
                });

            }
            catch (Exception ex)
            {
                player_SportResponse.Success = false;
                player_SportResponse.Message = ex.Message;
                return Ok(new { player_SportResponse });
            }


        }

        [HttpPost]
        [Route("PlayerSelectedSports")]
        public ActionResult PlayerSelectedSports([FromBody] Player_SportRequest request)
        {
            var player_SportResponse = new Player_SportResponse();

            if (request.Player_SportRecord != null && !string.IsNullOrEmpty(request.Player_SportRecord.PlayerId))
            {

                try
                {
                    var sports = _context.PlayerSports.Where(p => p.PlayerId == request.Player_SportRecord.PlayerId).Select(p => new Player_SportRecord()
                    {
                        Id = p.Id,
                        Name = p.Sport.Name,
                        SportId = p.SportId,
                        PlayerId = p.PlayerId,
                        IconUrl = p.Sport.IconUrl

                    }).ToList();
                    player_SportResponse.Success = true;
                    player_SportResponse.Player_SportRecords = sports;
                    return Ok(new
                    {
                        player_SportResponse
                    });

                }
                catch (Exception ex)
                {
                    player_SportResponse.Success = false;
                    player_SportResponse.Message = ex.Message;
                    return Ok(new { player_SportResponse });
                }
            }
            else
            {
                player_SportResponse.Success = false;
                player_SportResponse.Message = "Empty Request Body PlayerID";
                return Ok(new { player_SportResponse });
            }

        }

        [Route("PlayerSportsAdd_Update")]
        [HttpPost]
        public ActionResult Add([FromBody] Player_SportRequest request)
        {
            var player_SportResponse = new Player_SportResponse();

            if (request.Player_SportRecord != null && request.Player_SportRecord.SelectedSports.Any() && !string.IsNullOrEmpty(request.Player_SportRecord.PlayerId))
            {

                //Remove old sports
                var oldSports = _context.PlayerSports.Where(p => p.PlayerId == request.Player_SportRecord.PlayerId);
                _context.PlayerSports.RemoveRange(oldSports);
                _context.SaveChanges();
                foreach (var sportid in request.Player_SportRecord.SelectedSports)
                {
                    var playerSport = new PlayerSport();
                    playerSport.PlayerId = request.Player_SportRecord.PlayerId;
                    playerSport.SportId = sportid;
                    playerSport.CreatedAt = DateTime.Now;
                    _context.PlayerSports.Add(playerSport);
                }

                _context.SaveChanges();
                player_SportResponse.Success = true;
                player_SportResponse.Message = "Added";
                return Ok(new { player_SportResponse });
            }
            else
            {
                player_SportResponse.Success = false;
                player_SportResponse.Message = "Empty Request Body Sport list";
                return Ok(new { player_SportResponse });
            }



        }


    }
}