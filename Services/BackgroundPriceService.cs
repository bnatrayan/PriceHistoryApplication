using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace PriceHistoryApplication
{
    public class BackgroundPriceService : BackgroundService
    {
        #region " Variables "

        private readonly IBackgroundWorker backgroundworker;

        #endregion

        #region " Constructor "
        public BackgroundPriceService(IBackgroundWorker worker)
        {
            this.backgroundworker = worker;
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// Invokes the Backgroundservice task  
        /// </summary>
        /// <param name="stoppingToken"></param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await backgroundworker.DoWork(stoppingToken);    
        }

        #endregion
    }
}
