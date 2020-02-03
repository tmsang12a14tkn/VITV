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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVideoService" in both code and config file together.
    [ServiceContract]
    public interface IVideoService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "getvideobycategoryid?id={id}&from={from}&no={no}")]
        [return: MessageParameter(Name = "GetVideoByCategoryId")]
        IEnumerable<VideoEntity> GetVideoByCategoryId(string id, string from, string no);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "countvideobycategoryid?id={id}")]
        [return: MessageParameter(Name = "Count")]
        int CountVideoByCategoryId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "getlistvideo?from={from}&no={no}")]
        [return: MessageParameter(Name = "GetListVideo")]
        IEnumerable<VideoEntity> GetListVideo(string from, string no);

    }
}
