using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class LogRecord
    {
        public long Id { get; set; }
        public string RequestUrl { get; set; }
        public string Request { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ulong IsDeleted { get; set; }
    }
}
