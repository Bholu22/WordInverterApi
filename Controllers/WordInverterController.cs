using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordInverterApi.Services;
using static WordInverterApi.Models.Dtos;

namespace WordInverterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WordInverterController : ControllerBase
    {
        private readonly IWordInverterService _service;
        private readonly ILogger<WordInverterController> _logger;

        public WordInverterController(IWordInverterService service, ILogger<WordInverterController> logger)
        {
            _service = service;
            _logger  = logger;
        }

        /// <summary>
        /// POST /api/wordinverter/invert
        /// Inverts each word in the sentence. "abc def" → "cba fed"
        /// Stores the request + response in SQL Server.
        /// </summary>
        [HttpPost("invert")]
        [ProducesResponseType(typeof(InvertResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Invert([FromBody] InvertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Sentence))
                return BadRequest(new { error = "Sentence must not be empty." });

            _logger.LogInformation("Inverting: {Sentence}", request.Sentence);

            var result = await _service.InvertSentenceAsync(request.Sentence);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/wordinverter/all
        /// Returns full list of all stored request/response pairs.
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<InvertResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var results = await _service.GetAllAsync();
            return Ok(results);
        }

        /// <summary>
        /// GET /api/wordinverter/find?word=abc
        /// Finds all pairs where the given word appears in the request OR response.
        /// </summary>
        [HttpGet("find")]
        [ProducesResponseType(typeof(IEnumerable<InvertResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByWord([FromQuery] string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return BadRequest(new { error = "Word parameter must not be empty." });

            _logger.LogInformation("Finding records for word: {Word}", word);

            var results = await _service.FindByWordAsync(word);
            return Ok(results);
        }
    }
}