using IoT_Lib.DBClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

using IoT_Lib;
using IoT_Lib.DBClass;
using IoT_Lib.CustomClass;

using Newtonsoft.Json;

namespace IoT_WebService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 Service1.svc 或 Service1.svc.cs，然後開始偵錯。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class IoT_WebService : IIoT_WebService
    {
        #region Task
        public ExecuteResult GetAllTask()
        {
            DataBaseProcesser Db = new DataBaseProcesser();
            ExecuteResult result = new ExecuteResult();
            List<Task> All_Task = Db.GetAllTask();
            result.Result = JsonConvert.SerializeObject(All_Task);
            return result;
        }
        #endregion

        #region Student
        public ExecuteResult GetAllStudent()
        {
            DataBaseProcesser Db = new DataBaseProcesser();
            List<Student> Data = Db.GetAllStudent();
            ExecuteResult result = new ExecuteResult();
            result.Result = JsonConvert.SerializeObject(Data);
            return result;
        }

        public ExecuteResult GetStudentInfo(string Account, string Password)
        {
            ExecuteResult result = new ExecuteResult();
            DataBaseProcesser Db = new DataBaseProcesser();
            switch (Db.LogIn(Account,Password)) 
            {
                case 1:
                    //正常
                    break;
                case -1:
                    result.Code = -1;
                    result.Message = "無此帳號";
                    break;
                case -2:
                    result.Code = -2;
                    result.Message = "密碼錯誤";
                    break;
            }

            return result;
        }
        public ExecuteResult SetPassWord(string Account, string OldPassword, string NewPassWord)
        {
            ExecuteResult result = new ExecuteResult();
            DataBaseProcesser Db = new DataBaseProcesser();
            switch (Db.LogIn(Account, OldPassword))
            {
                case 1:
                    //正常
                    Db.SetPasswordByID(Account, NewPassWord);
                    break;
                case -1:
                    result.Code = -1;
                    result.Message = "無此帳號";
                    break;
                case -2:
                    result.Code = -2;
                    result.Message = "密碼錯誤";
                    break;
            }

            return result;
        }

        public ExecuteResult SetRaspBerryID(string Account, string Password, string RaspBerryID)
        {
            ExecuteResult result = new ExecuteResult();
            DataBaseProcesser Db = new DataBaseProcesser();
            switch (Db.LogIn(Account, Password))
            {
                case 1:
                    //正常
                    int set = Db.SetRaspberryIDByID(Account, RaspBerryID);
                    switch (set) 
                    {
                        case 1:
                            //更改成功
                            break;
                        case -1:
                            //已有設定
                            result.Code = -3;
                            result.Message = "不允許重複設定";
                            break;
                    }
                    break;
                case -1:
                    result.Code = -1;
                    result.Message = "無此帳號";
                    break;
                case -2:
                    result.Code = -2;
                    result.Message = "密碼錯誤";
                    break;
            }
            return result;
        }
        #endregion

        #region DHT22
        public ExecuteResult SetDHT22(string Account, string Password, string ClientIP, string Temp, string Hum)
        {
            Guid TaskID = Guid.Parse("65319464-E3FC-494E-8B63-654F844930CE");
            ExecuteResult result = new ExecuteResult();
            DataBaseProcesser Db = new DataBaseProcesser();
            switch (Db.LogIn(Account, Password))
            {
                case 1:
                    //正常
                    int x = new Random(DateTime.Now.GetHashCode()).Next(0, 3);
                    int IsVerification = 1;
                    string Data = JsonConvert.SerializeObject(new DHT22(Convert.ToDouble(Temp), Convert.ToDouble(Hum)));
                    //產生隨機失敗碼
                    if (x != 2)
                    {
                        result.Code = -4;
                        result.Message = "請再傳送一次";
                        IsVerification = -1;
                    }
                    //驗證資料正確性
                    if (Db.ConfirmDataIsRight(TaskID, Account, ClientIP) < 1)
                    {
                        result.Code = -5;
                        result.Message = "請再傳送一次";
                        IsVerification = -2;
                    }
                    Db.AddTaskLog(TaskID, Account, ClientIP, Data, IsVerification);
                    break;
                case -1:
                    result.Code = -1;
                    result.Message = "無此帳號";
                    break;
                case -2:
                    result.Code = -2;
                    result.Message = "密碼錯誤";
                    break;
            }
            return result;
        }
        #endregion


        #region Test
        public ExecuteResult Test()
        {
            string str = String.Empty;
            string IP = String.Empty;
            string Time = String.Empty;
            #region IP
            IP = GetClientIP();
            #endregion

            #region Time
            DateTime date = DateTime.Now;
            Time = date.ToString("yyyy-MM-dd HH:mm");
            #endregion

            #region Combine
            str = String.Format("Server OK,Request Time:{0},Request IP:{1}", Time, IP);
            #endregion

            ExecuteResult result = new ExecuteResult();
            result.Result = str;
            return result;
        }
        #endregion

        #region TaskLog
        public ExecuteResult GetTaskLog(string Account, string Password, string TaskID)
        {
            ExecuteResult result = new ExecuteResult();
            DataBaseProcesser Db = new DataBaseProcesser();
            switch (Db.LogIn(Account, Password))
            {
                case 1:
                    //正常
                    Guid TaskID_Guid = Guid.Parse(TaskID);
                    List<Task_Log> Logs = Db.GetTaskLogByAccount(TaskID_Guid, Account);
                    result.Result = JsonConvert.SerializeObject(Logs);
                    break;
                case -1:
                    result.Code = -1;
                    result.Message = "無此帳號";
                    break;
                case -2:
                    result.Code = -2;
                    result.Message = "密碼錯誤";
                    break;
            }
            return result;
        }
        #endregion


        private string GetClientIP() 
        {
            string IP = "";
            OperationContext operationContext = OperationContext.Current;
            MessageProperties messageProperties = operationContext.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = (RemoteEndpointMessageProperty)messageProperties[RemoteEndpointMessageProperty.Name];
            if (messageProperties.Keys.Contains(HttpRequestMessageProperty.Name))
            {
                HttpRequestMessageProperty endpointloadbalancer = (HttpRequestMessageProperty)messageProperties[HttpRequestMessageProperty.Name];
                if (endpointloadbalancer != null && endpointloadbalancer.Headers["X-Forwarded-For"] != null)
                {
                    IP = endpointloadbalancer.Headers["X-Forwarded-For"];
                }
            }
            if (String.IsNullOrEmpty(IP))
            {
                IP = endpoint.Address;
            }
            return IP;
        }

        

        
    }
}
