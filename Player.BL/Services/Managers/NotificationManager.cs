using Newtonsoft.Json;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reservation.BL.Services.Managers
{
    public class NotificationManager
    {
        public static void SendNotification(string toTokensList, string title, string body, string img)
        {
            string serverKey = "AAAAhel2RNA:APA91bEa-SxCJtgIrAS2TZUiDTYac-IqZT_bKNdEaDGGrUusW6i-Uit5Iv0N3NKUDc4BWYySD0MNfiv4iObrwqhxxkRYM91JTiE895qd80mHkJnX-VTaOf8IAfnDZuG5CMhKJKhav5b1";
            var registration_ids = "{\"registration_ids\": [\"";
            var json = "";
            registration_ids += toTokensList + "\"]";

            //json = registration_ids + ",\"data\": {\"title\": \"" + (title!=null && title.Length > 30 ? title.Substring(0, 30) : title) + "\",\"body\": \"" + (body.Length > 100 ? body.Substring(0, 100) : body) + "\"},\"priority\":10}";
            json = registration_ids + ",\"notification\": {\"title\": \"" + (title.Length > 30 ? title.Substring(0, 30) : title) + "\",\"body\": \"" + (body.Length > 100 ? body.Substring(0, 100) : body) + "\"},\"priority\":10}";

            try
            {
                var webAddr = "https://fcm.googleapis.com/fcm/send";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //registration_ids, array of strings -  to, single recipient
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                //LogHelper.LogInfo(json);
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }
        }
    }

}
