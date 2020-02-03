using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.ViewModels
{
    public class CommentModel
    {
        public string Content { get; set; }
        public string Id { get; set; }
        public int ArticleId { get; set; }
    }

    public class ReplyModel
    {
        public string Content { get; set; }
        public string CommentId { get; set; }
    }
}
