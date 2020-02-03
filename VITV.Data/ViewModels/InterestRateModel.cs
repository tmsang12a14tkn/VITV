using System.Collections.Generic;
using VITV.Data.Models;

namespace VITV.Data.ViewModels
{
    public enum TargetType : int
    {
        Personal = 1,
        Company = 2
    }

    public class InterestRateModel
    {
        public TargetType TargetValue { get; set; }
        public List<Bank> Banks { get; set; }
        public List<Term> Terms { get; set; }
        public List<InterestRate> InterestRates { get; set; }
    }
}