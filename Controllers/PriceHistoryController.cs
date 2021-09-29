using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PriceHistoryApplication.Model;

namespace PriceHistoryApplication.Controllers
{
    [ApiController]    
    [Route("")]
    public class PriceHistoryController : ControllerBase
    {
        #region " Variables "
        
        private readonly IBackgroundWorker _worker;

        #endregion

        #region " Constructor "

        public PriceHistoryController(IBackgroundWorker worker)
        {           
            _worker = worker;
        }

        #endregion

        #region GET: GetPriceHistory
        [HttpGet]        
        public ActionResult<IEnumerable<PriceDataUnix>> GetPriceHistory()
        {
            List<PriceDataUnix> result = null;
            result = _worker.GetPriceHistoryData();
            var jsonResult = JsonConvert.SerializeObject(new
            {
                BTCETHPriceHistory = result
            });
            return Ok(jsonResult);
        }

        #endregion
    }
}
