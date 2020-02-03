using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class UploadReportModel
    {
        public bool IsCheck { get; set; }
        public string empId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        //public DateTime LiveTime { get; set; } 
        public DateTime? UploadTime { get; set; } //Thời gian người dùng tải lên
        public DateTime PublishTime { get; set; } //Thời gian người dùng đăng
        public DateTime DisplayTime { get; set; } //Thời gian phát sóng truyền hình
    }
}