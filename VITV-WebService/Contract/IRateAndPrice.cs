using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VITV.DataService.Entity;

namespace VITV.DataService.Contract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRateAndPrice" in both code and config file together.
    [ServiceContract]
    public interface IRateAndPrice
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "GetUSDExchangeRate?name={name}")]
        [return: MessageParameter(Name = "GetUSDExchangeRate")]
        IEnumerable<ExchangeRateEntity> GetUSDExchangeRate(string name);

        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    UriTemplate = "GetStockBy?name={name}")]
        //[return: MessageParameter(Name = "GetStockBy")]
        //IEnumerable<StockChartEntity> GetStockBy(string name);
    }
}
