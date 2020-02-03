using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VITV.Data.ViewModels
{
    public class VideoTranscriptModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReporterIds { get; set; }
        public string UserEditedId { get; set; }
        public int HourSeek { get; set; }
        public int MinuteSeek { get; set; }
        public int SecondSeek { get; set; }
        public int ConvertedToSeconds { get; set; }
        public int VideoId { get; set; }

        public VideoTranscriptModel()
        {
            HourSeek = 0;
            MinuteSeek = 0;
            SecondSeek = 0;
            ConvertedToSeconds = 0;
        }
    }
}