using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemGlobalServicesTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace SystemGlobalServicesTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyServiceController : ControllerBase
    {
        private readonly ILogger<CurrencyServiceController> _logger;
        private readonly List<CurrencyRateWithId> _model;
        private readonly int _modelCount;

        public CurrencyServiceController(ILogger<CurrencyServiceController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            if (!memoryCache.TryGetValue("key_valute", out List<CurrencyRateWithId> model)) return;
            _model = model;
            _modelCount = _model?.Count ?? 0;
        }

        [HttpGet("currencies")]
        public async Task<List<CurrencyRate>> Currencies() =>
            await Currencies(1, _modelCount);

        [HttpGet("currencies/{page}&{pageSize}")]
        public async Task<List<CurrencyRate>> Currencies(int page, int pageSize) =>
            await GetCurrencies(page, pageSize);

        private Task<List<CurrencyRate>> GetCurrencies(int page, int pageSize)
        {
            return Task.Run(() => _model?.Cast<CurrencyRate>().Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        [HttpGet("currency")]
        public async Task<CurrencyRate> Currency(string id) =>
            await GetCurrency(id);

        private Task<CurrencyRate> GetCurrency(string id)
        {
            return Task.Run(() =>
                _model?.Where(cur => cur.ID == id)
                    .Select(curWithId => new CurrencyRate(curWithId.CharCode, curWithId.Value)).FirstOrDefault());
        }
    }
}