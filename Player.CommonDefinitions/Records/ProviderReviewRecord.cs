using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Review.CommonDefinitions.Records
{
    public class ProviderReviewRecord
    {
        public long Id { get; set; }
        public string? ProviderType { get; set; }
        public long ProviderId { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedByObj { get; set; }
        public string? Comment { get; set; }
        [Range(1, 5)]
        public int? Rate { get; set; }
        public double AvgRates { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
