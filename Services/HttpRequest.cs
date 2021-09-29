using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PriceHistoryApplication.Model;
using Newtonsoft.Json;

namespace PriceHistoryApplication.Services
{
    
    /// <summary>
    /// HttpClientService class to retrieve API data    
    /// </summary>
    public interface IHttpClientHandler
    {
        Task<string> GetBitCoinPriceValue();
        Task<string> GetEthereumPriceValue();
    }

    public class HttpClientService : IHttpClientHandler
    {
        #region " Variables "

        private HttpClient _httpClient;

        #endregion

        #region " Constructor "
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// Make the HttpClient Get call to the Bitcoin API
        /// Retrieved data is converted to JSON format        
        /// </summary>        
        public async Task<string> GetBitCoinPriceValue()
        {            
            var APIURL = "https://api.blockchain.com/v3/exchange/tickers/BTC-USD";
            var response = await _httpClient.GetAsync(APIURL);
            var result = response.Content.ReadAsStringAsync().Result;
            BitCoin record = JsonConvert.DeserializeObject<BitCoin>(result);
            var priceValue = record.last_trade_price;                       
            return priceValue;
        }

        /// <summary>
        /// Make the HttpClient Get call to the Ethereum API
        /// Retrieved data is converted to JSON format        
        /// </summary>
        public async Task<string> GetEthereumPriceValue()
        {
            var APIURL = "https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD";
            var response = await _httpClient.GetAsync(APIURL);
            var result = response.Content.ReadAsStringAsync().Result;
            Ethereum record = JsonConvert.DeserializeObject<Ethereum>(result);
            var priceValue = record.USD;
            return priceValue;
        }

        #endregion
    }
}