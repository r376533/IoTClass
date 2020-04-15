using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IoT_Lib.DBClass;

namespace IoT_Lib
{
    public class DataBaseProcesser
    {
        private IoT_ClassEntities db;
        public DataBaseProcesser()
        {
            db = new IoT_Lib.DBClass.IoT_ClassEntities();

        }

        public List<Student> GetAllStudent(string ID = "") 
        {
            List<Student> Students = db.Student.Where(m => m.IsEnabel == true).ToList();
            Students = Students.OrderBy(m => m.GroupID).ThenBy(m => m.ID).ToList();
            return Students;
        }

        public Student GetStudent(string ID) 
        {
            Student std = db.Student.Where(m => m.ID == ID).FirstOrDefault();
            return std;
        }


        public int LogIn(string ID, string PassWord) 
        {
            int result = 0;
            Student std = GetStudent(ID);
            if (std != null)
            {
                if (std.PassWord != PassWord)
                {
                    result = -2;
                }
                else
                {
                    result = 1;
                }
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public void SetPasswordByID(string ID, string NewPassword) 
        {
            var s = db.Student.Find(ID);
            s.PassWord = NewPassword;
            db.SaveChanges();
        }

        public int SetRaspberryIDByID(string ID, string RaspberryID) 
        {
            int Result = 0;
            var s = db.Student.Find(ID);
            if (String.IsNullOrEmpty(s.RaspBerryID))
            {
                s.RaspBerryID = RaspberryID;
                db.SaveChanges();
                Result = 1;

            }
            else 
            {
                Result = -1;
            }
            return Result;
        }


        /// <summary>
        /// 再新增TaskLog的時候，檢查該資料是否合理
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="UpdaterID"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public int ConfirmDataIsRight(Guid TaskID,string UpdaterID, string IP) 
        {
            int Result = 0;
            List<Task_Log> Datas = db.Task_Log.Where(m => m.TaskID == TaskID && m.UpdateIP == IP).ToList();
            if (Datas.Count < 1)
            {
                //目前資料庫中沒有資料
                Result = 1;
            }
            else if (Datas.Count >= 1) 
            {
                //上傳時已有相同IP，若檢查上傳人不同人，則不允許上傳
                int NotOKCount = Datas.Where(m => m.UpdateStudent != UpdaterID).Count();
                Result = NotOKCount > 0 ? -1 : 1;
            }
            return Result;
        }



        public void AddTaskLog(Guid TaskID,string UpDateID, string IP, string Data, int IsVerification) 
        {
            Guid LogID = Guid.NewGuid();
            DateTime ProcTime = DateTime.Now;
            Task_Log newLog = new Task_Log();
            newLog.ID = LogID;
            newLog.TaskID = TaskID;
            newLog.UpdateTime = ProcTime;
            newLog.UpdateStudent = UpDateID;
            newLog.UpdateIP = IP;
            newLog.Data = Data;
            newLog.IsVerification = IsVerification;
            db.Task_Log.Add(newLog);
            db.SaveChanges();
        }


        public List<Task> GetAllTask() 
        {
            List<Task> Result;
            Result = db.Task.ToList();
            return Result;
        }

        public List<Task_Log> GetTaskLogByAccount(Guid TaskID, string Account) 
        {
            List<Task_Log> Result = new List<Task_Log>();
            Result = db.Task_Log.Where(m => m.TaskID == TaskID && m.UpdateStudent == Account && m.IsVerification > 0).ToList();
            return Result;
        }
    }
}
