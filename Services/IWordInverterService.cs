using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WordInverterApi.Models.Dtos;

namespace WordInverterApi.Services
{
    public interface IWordInverterService
    {
        Task<InvertResponse> InvertSentenceAsync(string sentence);
        Task<IEnumerable<InvertResponse>> GetAllAsync();
        Task<IEnumerable<InvertResponse>> FindByWordAsync(string word);
    }
}