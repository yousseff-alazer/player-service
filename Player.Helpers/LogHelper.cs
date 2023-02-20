using log4net;
using Player.DAL.mysqlplayerDB;
using System;
using System.Reflection;
namespace Reservation.Helpers
{
    public class LogHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogException(playerContext _context, string controller,string action, string request, Exception ex)
        {
            var log = new Log
            {
                Request = request,
                RequestUrl = controller + "/" + action,
                ExceptionMessage = ex.Message,
                ExceptionStackTrace = ex.StackTrace,
            };
            _context.Logs.Add(log);
            _context.SaveChanges(); 
        }

        public static void LogException(string msg, string stackTrace)
        {
            //log.Error($"Exception Msg: {msg}");
            log.Error($"Exception StackTrace: {stackTrace}");
        }

        public static void LogInfo(string msg)
        {
            log.Info(msg);
        }
    }
}
