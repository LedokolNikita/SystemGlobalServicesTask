using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemGlobalServicesTask.Controllers;
using SystemGlobalServicesTask.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SystemGlobalServicesTask
{
    public class DataCache : BackgroundService
    {
        private readonly string _url;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CurrencyServiceController> _logger;

        public DataCache(IMemoryCache memoryCache, IConfiguration config, ILogger<CurrencyServiceController> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _url = config.GetValue<string>("Url");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var restoredData = JsonHelper.DownloadSerializedJsonData<ExchangeRate>(_url);
                    _memoryCache.Set("key_currency", restoredData);
                    var items = restoredData.Valute.Select(dict => dict.Value)
                        .Select(val => new CurrencyRateWithId(val.CharCode, val.Value, val.ID)).ToList();
                    _memoryCache.Set("key_valute", items);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.InnerException?.Message);
                }
                
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}