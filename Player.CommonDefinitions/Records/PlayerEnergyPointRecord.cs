using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CommonDefinitions.Records
{
    public class NotificationRecord
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public long? ModifiedBy { get; set; }
        public string? CreatedBy { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? Priority { get; set; }
        public string Sound { get; set; }
        public string ClickAction { get; set; }
        public string Icon { get; set; }
        public List<string> ToUserIds { get; set; }
        public string ResponseObj { get; set; }
        //public IFormFile IconFile { get; set; }
        public int? ObjectId { get; set; }
        public int ProviderAction { get; set; }
        public string ProviderType { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByImage { get; set; }
        public string RecipientId { get; set; }
        public ulong? IsSeen { get; set; }
        public DateTime? AppearDate { get; set; }
    }
}
