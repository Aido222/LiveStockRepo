using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace FarmManager.ViewModels
{
    public class CowPurchVM
    {
        [Required]
        public string TagNo { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public string Breed { get; set; }

        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Location { get; set; }
        public string BoughtFrom { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Notes { get; set; }

        public IEnumerable<SelectListItem> BreedList { get; set; }



    }
}