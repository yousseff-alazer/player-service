using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;

namespace Reservation.Helpers
{
    public class UIHelper
    {

        public const string s3Path = "https://s3.console.aws.amazon.com/s3/buckets/c4s-player";
        public static HttpResponseMessage CreateRequest(string baseurl, HttpMethod method, string relativeUrl,
            string jsonObj = null, string lang = "", string basicAuthUser = "", string basicAuthPassword = "")
        {
            using (var client = new HttpClient())
            {
                //client.Timeout.Add(new TimeSpan(0, 3, 0));
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(basicAuthUser) && !string.IsNullOrWhiteSpace(basicAuthPassword))
                {
                    var byteArray = new UTF8Encoding().GetBytes(basicAuthUser + ":" + basicAuthPassword);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                }
                //client.DefaultRequestHeaders.Add("Accept-Language", lang);

                HttpRequestMessage request = new HttpRequestMessage(method, relativeUrl);

                if (jsonObj != null)
                    request.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                var Res = new HttpResponseMessage();
                try
                {
                    Res = client.SendAsync(request).Result;

                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }

                return Res;
            }
        }

        public static string HtmlToPlainText(string htmlString)
        {
            var text = Regex.Replace(htmlString, @"<(.|\n)*?>", "");
            return text;
        }

        public static string GetDateUrl(byte[] data)
        {
            string imageBase64Data = Convert.ToBase64String(data);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            return imageDataURL;
        }

        public static HttpResponseMessage AddRequestToServiceApi(string url, string json)
        {
            var Res = new HttpResponseMessage();
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    Res = client.PostAsync(url, content).Result;
                    //if (Res.StatusCode != HttpStatusCode.OK)
                    //{
                    //    try
                    //    {
                    //        var result = Res.Content.ReadAsStringAsync().Result;
                    //        var isAvailable = result.ToString();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogHelper.LogException(ex.Message, ex.StackTrace);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return Res;
        }

        public static string Upload(string url, IFormFile file, string objectId)
        {
            var Res = new HttpResponseMessage();
            var result = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    if (file.Length > 0)
                    {
                        client.DefaultRequestHeaders.Clear();
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        //form.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            //string s = Convert.ToBase64String(fileBytes);
                            //// act on the Base64 data
                            ByteArrayContent bytes = new ByteArrayContent(fileBytes);
                            form.Add(bytes, "file", objectId);
                            form.Add(new StringContent("c4s-player"), "BucketName");
                        }
                        Res = client.PostAsync(s3Path, form).Result;
                        //if (Res.StatusCode != HttpStatusCode.OK)
                        //{
                        try
                        {
                            result = Res.Content.ReadAsStringAsync().Result;

                            LogHelper.LogException("S3 Result", result);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogException(ex.Message, ex.StackTrace);
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }



            return result;
        }

        public static string UploadFileToS3(IFormFile file, string objectId)
        {
            //https://[application.bucket].s3.amazonaws.com/[key]

            using (var client = new AmazonS3Client("AKIAZSHDAIZIJGRUA5IW", "kr9PHE1hXvBGHQzugwY/OsXSFhFseddKCxNWIrBp", RegionEndpoint.USEast1))
            {
              
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = objectId, // filename
                        BucketName = "c4s-player",
                        CannedACL = S3CannedACL.PublicRead,


                    };
                    var fileTransferUtility = new TransferUtility(client);
                    fileTransferUtility.Upload(uploadRequest);

                }
            }
            return "https://" + "c4s-player" + ".s3.amazonaws.com/" + objectId;
        }

    }
}