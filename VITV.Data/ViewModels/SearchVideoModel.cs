using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VITV.Data.ViewModels
{
    public class SearchVideoModel
    {
        private string Cat = "all";
        private string Rep = "";
        private string Title = "";
        private RangeType type = RangeType.All;
        private string p = "1";
        public int? evtId { get; set; }
        public DateTime? begin { get; set; }
        public DateTime? end { get; set; }
        [DefaultValue("all")]
        public string cat
        {
            get
            {
                return (this.Cat == default(string))
                   ? this.Cat = "all"
                   : this.Cat;
            }

            set { this.Cat = value; }
        }
        [DefaultValue("")]
        public string rep
        {
            get
            {
                return (this.Rep == default(string))
                   ? this.Rep = ""
                   : this.Rep;
            }

            set { this.Rep = value; }
        }
        [DefaultValue("")]
        public string title
        {
            get
            {
                return (this.Title == default(string))
                   ? this.Title = ""
                   : this.Title;
            }

            set { this.Title = value; }
        }
        [DefaultValue("1")]
        public string page
        {
            get
            {
                return (this.p == default(string))
                   ? this.p = "1"
                   : this.p;
            }

            set { this.p = value; }
        }
        [DefaultValue(RangeType.All)]
        public RangeType rangeType
        {
            get
            {
                return (this.type == default(RangeType))
                   ? this.type = RangeType.All
                   : this.type;
            }

            set { this.type = value; }
        }
    }
}