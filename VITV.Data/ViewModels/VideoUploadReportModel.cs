using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VITV.Data.ViewModels
{
    public class VideoUploadReportModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public int VideoCount { get; set; }
    }
}
