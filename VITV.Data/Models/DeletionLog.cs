using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class DeletionLog
    {
        [Key]
        public DateTime DeletionTime { get; set; }
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public string CategoryName { get; set; }
        public DateTime DisplayTime { get; set; }//giờ phát sóng trên truyền hình
        public string CreationUser { get; set; }
        public string DeletionUser { get; set; }
        public string Note { get; set; }
    }
}
