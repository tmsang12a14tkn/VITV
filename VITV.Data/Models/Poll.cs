using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace VITV.Data.Models
{
    public class PollQuestion
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Done { get; set; }
        public bool Published { get; set; } //Được xem kết quả hay ko
        public bool Displayed { get; set; } //Được hiển thị hay không
        public string Name { get; set; }
        [AllowHtml]
        public string Question { get; set; }
        public bool MultipleChoice { get; set; }
        public bool ViewTotal { get; set; }
        public string Image { get; set; }

        public virtual Video Video { get; set; }

        public virtual ICollection<PollAnswer> Answers { get; set; }
        public virtual ICollection<PollUserAnswer> UserAnswers { get; set; }
    }

    public class PollAnswer
    {

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [Key]
        [Column(Order=2)]
        public int AnswerId { get; set; }
        [AllowHtml]
        public string Answer { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }

        [NotMapped]
        public int Count { get; set; }

        public virtual PollQuestion Question { get; set; }

    }

    public class PollUserAnswer
    { 
        
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int AnswerId { get; set; }
        [Key]
        [Column(Order = 3)]
        public string IP { get; set; }
       

        public virtual PollQuestion Question { get; set; }
    }
}
