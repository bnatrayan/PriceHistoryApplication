using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PriceHistoryApplication.Model;

namespace PriceHistoryApplication
{

    /// <summary>
    /// BackgroundWorker class to invoke the Backgroundservice task
    /// Returns the price history data
    /// </summary>
    public interface IBackgroundWorker
    {
        Task DoWork(CancellationToken cancellationToken);       
        public List<PriceDataUnix> GetPriceHistoryData();
    }
}