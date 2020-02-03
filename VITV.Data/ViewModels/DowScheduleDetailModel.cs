using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public class DowScheduleDetailModel //Model for displaying dayofweek's schedule
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public List<ProgramScheduleDetail> Details { get; set; }
    }
}