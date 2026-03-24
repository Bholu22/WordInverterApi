using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordInverterApi.Models
{
    public class InversionRecord
    {
        public int Id { get; set; }
        public string RequestSentence { get; set; } = string.Empty;
        public string ResponseSentence { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}