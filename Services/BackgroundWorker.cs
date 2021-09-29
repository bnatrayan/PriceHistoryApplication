using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PriceHistoryApplication.Model;
using PriceHistoryApplication.Services;
using System.Linq;

namespace PriceHistoryApplication
{
    public class BackgroundWorker : IBackgroundWorker
    {
        #region " Variables "

        private readonly ILogger<BackgroundWorker> logger;
        private readonly IHttpClientHandler _httpClientService;
        public List<PriceData> priceCollection = new List<PriceData>();

        #endregion

        #region " Constructor "

        public BackgroundWorker(ILogger<BackgroundWorker> logger, IHttpClientHandler httpClientService)
        {
            this.logger = logger;
            _httpClientService = httpClientService;
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// Responsible for making API call for every 5 mins 
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation($"Service is running at {DateTimeOffset.Now}");
                await FetchAPIData();                
                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }

        /// <summary>
        /// Make HttpClient call and retrive the data from API
        /// Assign the API data to the in memory collection
        /// </summary>
        /// <param name="prices"></param>
        public async Task FetchAPIData()
        {
            var bitcoinPrice = await _httpClientService.GetBitCoinPriceValue();
            var ethereumPrice = await _httpClientService.GetEthereumPriceValue();
            var priceRatio = Convert.ToDouble(bitcoinPrice) / Convert.ToDouble(ethereumPrice);
            PriceData priceObj = new PriceData();
            priceObj.time = DateTime.Now;
            priceObj.price = priceRatio;
            priceCollection.Add(priceObj);
        }

        /// <summary>
        /// Loop through the in memory collection and fetch the last 60 mins data
        /// Convert DateTime to Unix DateTime        
        /// This method will be called from GET API endpoint
        /// Returns a collection with the last 60 mins data
        /// </summary>        
        public List<PriceDataUnix> GetPriceHistoryData()
        {           
            var pricedataunix = new List<PriceDataUnix>();
            var pricedata = new List<PriceData>();
            foreach (var item in priceCollection)
            {
                var priceObj = new PriceDataUnix();
                var pricedataObj = new PriceData();
                if (item.time > DateTime.Now.AddMinutes(-60))
                {                    
                    var unixdatetime = ConvertToUnixTimeStamp(item.time);
                    //Populating data with unix timestamp for the last 60 mins
                    priceObj.time = unixdatetime;
                    priceObj.price = item.price;
                    pricedataunix.Add(priceObj);
                    //Populating data with DateTime for the last 60 mins in a temporary collection
                    pricedataObj.time = item.time;
                    pricedataObj.price = item.price;
                    pricedata.Add(pricedataObj);
                }
            }
            //Keep only the data for the last 60 mins in the in-memory collection
            priceCollection = pricedata.ToList();

            return pricedataunix;
        }

        /// <summary>
        /// Method to convert DateTime to Unix Timestamp
        /// </summary>
        public long ConvertToUnixTimeStamp(DateTime dateTime)
        {
            var datetime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                                        dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Local);
            var datetimeoffset = new DateTimeOffset(datetime);
            var unixdatetime = datetimeoffset.ToUnixTimeSeconds();

            return unixdatetime;
        }

        #endregion
    }
}
