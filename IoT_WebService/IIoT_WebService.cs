using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using IoT_Lib;
using IoT_Lib.DBClass;

namespace IoT_WebService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IIoT_WebService
    {
        #region Task
        [OperationContract]
        [WebGet(UriTemplate = "GetAllTask", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult GetAllTask();
        #endregion

        #region Student

        [OperationContract]
        [WebGet(UriTemplate = "GetStudentInfo/{Account}/{Password}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult GetStudentInfo(string Account, string Password);

        [OperationContract]
        [WebGet(UriTemplate = "SetPassWord/{Account}/{OldPassword}/{NewPassWord}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult SetPassWord(string Account, string OldPassword, string NewPassWord);

        [OperationContract]
        [WebGet(UriTemplate = "SetRaspBerryID/{Account}/{Password}/{RaspBerryID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult SetRaspBerryID(string Account, string Password, string RaspBerryID);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllStudent", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult GetAllStudent();
        #endregion

        #region Test

        [OperationContract]
        [WebGet(UriTemplate = "Test", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult Test();

        #endregion

        #region DHT22
        [OperationContract]
        [WebGet(UriTemplate = "SetDHT22/{Account}/{Password}/{ClientIP}/{Temp}/{Hum}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult SetDHT22(string Account,string Password,string ClientIP,string Temp,string Hum);
        #endregion

        #region TaskLog
        [OperationContract]
        [WebGet(UriTemplate = "GetTaskLog/{Account}/{Password}/{TaskID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ExecuteResult GetTaskLog(string Account,string Password,string TaskID);
        #endregion

    }


    [DataContract]
    public class ExecuteResult
    {
        [DataMember]
        public int Code { get; set; } = 1;
        [DataMember]
        public string Message { get; set; } = "正常執行";
        [DataMember]
        public string Result;
    }
}
