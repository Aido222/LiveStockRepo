using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarmManager.ViewModels
{
    public class HomeIndexVM
    {
        public string NoOwnedCows { get; set; }
        public string OfWhichPurchased { get; set; }
        public string OfWhichBorn { get; set; }


        public string MostCommonBreed { get; set; }

        
        public string TotalTreatments { get; set; }
        public string TotalPrescribedTreats { get; set; }
        public IEnumerable<SelectListItem> RecentTreatList { get; set; }

        public string TotalViableMedicines { get; set; }
        public IEnumerable<SelectListItem> RecentMedPurchases { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public string SumPurchased { get; set; }
        public string MostValuablePurchased { get; set; }
        public string LatestPurchased { get; set; }
        public string NoOfPurchases { get; set; }


        public string SumSales { get; set; }
        public string HighestSales { get; set; }
        public string LatestSales { get; set; }
        public string NoOfSales { get; set; }


        public string TotalDeaths { get; set; }
        public string MostCommonDeaths { get; set; }
        public string LatestDeath { get; set; }

        public string BirthThisYear { get; set; }

    }
}