using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using Reservation.DAL.DB;
using System.Linq;
using Reservation.Helpers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Reservation.CommonDefinitions.Records
{
    public class S3Record
    {
        [JsonPropertyName("status")]
        public bool Status;
        [JsonPropertyName("msg")]
        public string Msg;
        [JsonPropertyName("data")]
        public Data Data;
    }
    public class Data
    {
        public string file;
        public string serviceName;
        public string serviceId;
        public string objectId;
        public DateTime updated_at;
        public DateTime created_at;
        public int id;
    }

    public class ImageRequest
    {
        public IFormFile ImageFile { get; set; }
        public string Bucket { get; set; }


    }
}
