using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VITV_Web.Models
{
    public class VNDExchangeRate
    {
        public int Id { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public double Buy { get; set; }
        public double Transfer { get; set; }
        public double Sell { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime UpdatedTime { get; set; }

        public virtual Currency Currency { get; set; }

    }
}