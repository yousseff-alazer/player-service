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
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Reservation.BL.Services.Managers;
using Shared.CommonDefinitions.Records;
using MassTransit;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Player.CommonDefinitions.Records;
using Player.BL.Services;

namespace Reservation.Portal.Controllers
{
    [Route("api/[controller]")]
    public class Player_ProfileController : Controller
    {
        private readonly playerContext _context;
        private readonly IConfiguration configuration;
        private readonly IOptions<AppSettingsRecord> _appSettings;
        private readonly IBus _bus;

        public Player_ProfileController(playerContext context,
        IConfiguration _configuration, IBus bus, IOptions<AppSettingsRecord> appSettings)
        {
            _context = context;
            configuration = _configuration;
            _bus = bus;
            _appSettings = appSettings;
        }

        //Get profile
        [HttpPost]
        [Route("playerinfoList")]
        public ActionResult playerinfoList([FromBody] Player_ProfileRequest request)
        {
            request._context = _context;
            var player_profileResponse = Player_profileService.Listplayer_profile(request);
            return Ok(new
            {
                player_profileResponse
            });
        }

        [Route("playerinfoAdd")]
        [HttpPost]
        public ActionResult Add([FromBody] Player_ProfileRequest request)
        {
            request._context = _context;
            var player_profileResponse = Player_profileService.Addplayer_profile(request);
            return Ok(new
            {
                player_profileResponse
            });
        }

        [HttpPost]
        [Route("playerinfoUpdate")]
        public async Task<ActionResult> Update([FromForm] Player_ProfileRequest request)
        {
            request._context = _context;
            var model = request.Player_profileRecord;
            var player_profileResponse = Player_profileService.Editplayer_profile(request);

            if (player_profileResponse.Success)
            {
                var playerDb = _context.PlayerInfos.FirstOrDefault(s => s.Id == request.Player_profileRecord.Id && s.IsDeleted != 1);
                if (playerDb != null)
                    if (model.FirstName != playerDb.FirstName || model.LastName != playerDb.LastName
                     || model.ImageUrl != playerDb.ImageUrl)
                    {
                        request.Player_profileRecord.FirstName = playerDb.FirstName;
                        request.Player_profileRecord.LastName = playerDb.LastName;
                        request.Player_profileRecord.ImageUrl = playerDb.ImageUrl;
                        await Player_profileService.PlayerInfoRabbitMQAsync(request, _bus, _appSettings.Value);
                    }
            }
            return Ok(new
            {
                player_profileResponse
            });
        }

        [HttpPost]
        [Route("playerinfoDelete")]
        public async Task<ActionResult> Delete([FromBody] Player_ProfileRequest request)
        {
            request._context = _context;
            var player_profileResponse = Player_profileService.Deleteplayer_profile(request);
            if (player_profileResponse.Success)
            {
                request.Player_profileRecord.FirstName = "C4S";
                request.Player_profileRecord.LastName = "User";
                request.Player_profileRecord.ImageUrl = "https://c4s-player.s3.amazonaws.com/profile_ic.png";
                await Player_profileService.PlayerInfoRabbitMQAsync(request, _bus, _appSettings.Value);
            }
            return Ok(new
            {
                player_profileResponse
            });
        }

        [HttpPost]
        [Route("AddOrEditPlayerCountry")]
        public ActionResult AddOrEditPlayerCountry([FromBody] player_profileRecord request)
        {
            var PlayerReportsResponse = new Player_profileResponse();
            if (request!=null)
            {
                 PlayerReportsResponse = Player_profileService.AddOrEditPlayerCountry(_context, request.PlayerId, request.CountryId);
                return Ok(new
                {
                    PlayerReportsResponse
                });
            }
            return Ok(new
            {
                PlayerReportsResponse
            });
        }

      
        #region Additional APIS
        [HttpPost]
        [Route("UploadImage")]
        public ActionResult UploadImage([FromForm] ImageRequest request)
        {
            if (request.ImageFile != null)
            {
                var jsonString = UIHelper.UploadFileToS3(request.ImageFile, request.ImageFile.FileName);
                return Ok(jsonString);
            }
            else
            {
                return Ok("Empty image");
            }
        }

        [HttpPost]
        [Route("playerinfoUpdateByEmail")]
        public ActionResult UpdateByEmail([FromForm] Player_ProfileRequest request)
        {
            var player = _context.PlayerInfos.FirstOrDefault(p => p.Email == request.Player_profileRecord.Email);
            if (player != null)
            {
                if (request.Player_profileRecord.ProfileFile != null)
                {

                    var jsonString = UIHelper.UploadFileToS3(request.Player_profileRecord.ProfileFile, request.Player_profileRecord.ProfileFile.FileName);
                    player.ImageUrl = jsonString;
                }
                if (request.Player_profileRecord.CoverFile != null)
                {
                    var jsonString = UIHelper.UploadFileToS3(request.Player_profileRecord.CoverFile, request.Player_profileRecord.CoverFile.FileName);
                    player.CoverUrl = jsonString;
                }
                player.BirthDate = request.Player_profileRecord.BirthDate;
                player.FirstName = request.Player_profileRecord.FirstName;
                player.Height = request.Player_profileRecord.Height;
                player.LastName = request.Player_profileRecord.LastName;
                player.Location = request.Player_profileRecord.Location;
                player.MobileNumber = request.Player_profileRecord.MobileNumber;
                player.Weight = request.Player_profileRecord.Weight;
                player.UpdatedAt = DateTime.Now;

            }

            //Check image

            _context.SaveChanges();
            var player_profileResponse = new Player_profileResponse();
            player_profileResponse.Success = true;
            var records = new List<player_profileRecord>();
            records.Add(new player_profileRecord()
            {
                Id = player.Id,
                BirthDate = player.BirthDate,
                Email = player.Email,
                FirstName = player.FirstName,
                Height = player.Height,
                ImageUrl = player.ImageUrl,
                LastName = player.LastName,
                Location = player.Location,
                MobileNumber = player.MobileNumber,
                Weight = player.Weight,
                CoverUrl = player.CoverUrl,
                CreatedAt = player.CreatedAt,
                UpdatedAt = player.UpdatedAt,
                Provider = player.Provider
            });
            player_profileResponse.player_profileRecords = records;
            player_profileResponse.Message = "Updated";
            return Ok(new { player_profileResponse });
        }

        [HttpPost]
        [Route("playerinfoUpdateImages")]
        public ActionResult UpdateImages([FromForm] player_profileRecord model)
        {
            var request = new Player_ProfileRequest();
            request._context = _context;
            request.Player_profileRecord = model;
            var player = _context.PlayerInfos.Find(request.Player_profileRecord.Id);
            if (player != null)
            {
                if (request.Player_profileRecord.ProfileFile != null)
                {

                    var jsonString = UIHelper.UploadFileToS3(request.Player_profileRecord.ProfileFile, request.Player_profileRecord.ProfileFile.FileName);
                    player.ImageUrl = jsonString;
                }
                if (request.Player_profileRecord.CoverFile != null)
                {
                    var jsonString = UIHelper.UploadFileToS3(request.Player_profileRecord.CoverFile, request.Player_profileRecord.CoverFile.FileName);
                    player.CoverUrl = jsonString;
                }

            }
            _context.SaveChanges();
            var record = new player_profileRecord()
            {
                Id = player.Id,
                ImageUrl = player.ImageUrl,
                CoverUrl = player.CoverUrl
            };
            return Ok(record);
        }

        [Route("RegisterDevice")]
        [HttpPost]
        public ActionResult RegisterDevice([FromBody] Common_UserDeviceRecord request)
        {

            if (request != null)
            {
                var userDeviceList = _context.CommonUserDevices.Where(p => p.CommonUserId == request.UserID).ToList();
                var userDevice = userDeviceList.FirstOrDefault(p => p.DeviceImei == request.DeviceIMEI);
                var isNewDevice = userDevice == null;
                if (isNewDevice)
                {
                    var deviceObj = Common_UserDeviceServiceManager.AddOrEditCommon_UserDevice(request);
                    deviceObj.IsLoggedIn = 1;
                    deviceObj.CommonUserId = request.UserID;
                    deviceObj.AuthExpirationDate = DateTime.Now.AddMonths(1);
                    _context.Add(deviceObj);
                    _context.SaveChanges();
                }
                else
                {
                    userDevice.DeviceToken = request.DeviceToken;
                    userDevice.LastUpdateDate = DateTime.Now;
                    _context.SaveChanges();
                }
            }
            return Ok();
        }
        #endregion

        #region Notifications
        [Route("ListNotification")]
        [HttpPost]
        public ActionResult ListNotification([FromBody] NotificationRequest request)
        {
            request._context = _context;
            var resp = NotificationService.ListNotification(request);

            return Ok(resp);
        }


        [Route("AddNotification")]
        [HttpPost]
        public ActionResult AddNotification([FromBody] NotificationRequest request)
        {
            request._context = _context;
            var resp = NotificationService.AddNotification(request);
            return Ok(resp);
        }
        #endregion

    }
}