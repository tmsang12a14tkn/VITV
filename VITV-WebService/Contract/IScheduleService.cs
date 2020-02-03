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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IScheduleService" in both code and config file together.
    [ServiceContract]
    public interface IScheduleService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "GetBroadcastScheduleByDate?date={date}")]
        List<ProgramScheduleDetailModel> GetBroadcastScheduleByDate(string date);
    }
}
