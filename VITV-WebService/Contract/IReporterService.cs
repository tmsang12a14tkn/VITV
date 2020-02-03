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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReporterService" in both code and config file together.
    [ServiceContract]
    public interface IReporterService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "getreporterbyid?id={id}")]
        [return: MessageParameter(Name = "GetReporterById")]
        ReporterToListEntity GetReporterById(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "getlistreporter?name={name}")]
        [return: MessageParameter(Name = "GetListReporter")]
        IEnumerable<ReporterToListEntity> GetListReporter(string name);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "getlistreporterbygroup")]
        [return: MessageParameter(Name = "GetListReporterByGroup")]
        IEnumerable<ReporterGroupEntity> GetListReporterByGroup();
    }
}
