using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class VideoTabFilterView
    {
        public string Name { get; set; }
        public DateTime Begin { get; set; }
        public List<Video> Videos { get; set; }

    }
}