using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordInverterApi.Models
{
    public class Dtos
    {
        // What the client sends
        public record InvertRequest(string Sentence);

        // What the API returns
        public record InvertResponse(
            int Id,
            string OriginalSentence,
            string InvertedSentence,
            DateTime CreatedAt
        );
    }
}