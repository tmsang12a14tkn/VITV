using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class VideoTranscriptView
    {
        public Video Video { get; set; }
        public List<VideoTranscriptModel> VideoTranscriptModels { get; set; }
    }
}
