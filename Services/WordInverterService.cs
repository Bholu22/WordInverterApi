using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordInverterApi.Data;
using WordInverterApi.Models;
using static WordInverterApi.Models.Dtos;

namespace WordInverterApi.Services
{
    public class WordInverterService : IWordInverterService
    {
        private readonly AppDbContext _db;

        public WordInverterService(AppDbContext db)
        {
            _db = db;
        }

        // Core logic: reverse each word individually
        // "abc def" → "cba fed"
        private static string InvertWords(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                return sentence;

            return string.Join(' ',
                sentence.Split(' ').Select(w => new string(w.Reverse().ToArray()))
            );
        }

        public async Task<InvertResponse> InvertSentenceAsync(string sentence)
        {
            var inverted = InvertWords(sentence);

            var record = new InversionRecord
            {
                RequestSentence  = sentence,
                ResponseSentence = inverted,
                CreatedAt        = DateTime.UtcNow
            };

            _db.InversionRecords.Add(record);
            await _db.SaveChangesAsync();

            return ToDto(record);
        }

        public async Task<IEnumerable<InvertResponse>> GetAllAsync()
        {
            var records = await _db.InversionRecords
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return records.Select(ToDto);
        }

        public async Task<IEnumerable<InvertResponse>> FindByWordAsync(string word)
        {
            // Pull all records, then filter in memory using word-boundary matching
            // (SQL Server doesn't support splitting strings easily in EF LINQ)
            var all = await _db.InversionRecords.ToListAsync();

            var lower = word.ToLowerInvariant();

            var matched = all.Where(r =>
                r.RequestSentence.ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Contains(lower)
                ||
                r.ResponseSentence.ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Contains(lower)
            );

            return matched
                .OrderByDescending(r => r.CreatedAt)
                .Select(ToDto);
        }

        // Helper to convert DB entity → DTO
        private static InvertResponse ToDto(InversionRecord r) =>
            new(r.Id, r.RequestSentence, r.ResponseSentence, r.CreatedAt);
    }
}