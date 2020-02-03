using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITV.Data.Models
{
    public class ArticleComment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UserId { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ArticleCommentReply> Replies { get; set; }
    }
    public class ArticleCommentReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UserId { get; set; }
        public string ArticleCommentId { get; set; }

        public virtual ArticleComment ArticleComment { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}
