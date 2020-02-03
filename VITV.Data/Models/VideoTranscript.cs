using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV.Data.Models
{
    public class VideoTranscript
    {
        public VideoTranscript()
        {
            HourSeek = 0;
            MinuteSeek = 0;
            SecondSeek = 0;
            ConvertedToSeconds = 0;
            Reporters = new HashSet<Employee>();
        }

        public int Id { get; set; }

        public int HourSeek { get; set; }

        public int MinuteSeek { get; set; }

        public int SecondSeek { get; set; }

        public int ConvertedToSeconds { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int VideoId { get; set; }

        public string UserEditedId { get; set; }

        public virtual Video Video { get; set; }

        [ForeignKey("UserEditedId")]
        public virtual ApplicationUser UserEdited { get; set; }

        public virtual ICollection<Employee> Reporters { get; set; }
    }
}
