using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class ArticleReview
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ReviewContent { get; set; }

        //public virtual Article Article { get; set; }
    }
}
