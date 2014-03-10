using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FarmManager.ViewModels
{
    public class CowDetailVM
    {
        public string TagNo { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string AnimalBreed { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> OwnershipStatus { get; set; }
        public Nullable<bool> BornOnFarm { get; set; }

        //Purchase related data
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DateBought { get; set; }
        public string BoughtFrom { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Location { get; set; }

        //Birth related data
        public string MotherTagNo { get; set; }
        public string SireTagNo { get; set; }
        public Nullable<bool> Difficult { get; set; }



        public IEnumerable<SelectListItem> calvesList { get; set; }

        public IEnumerable<SelectListItem> notesList { get; set; }

        public IEnumerable<SelectListItem> bullCalveList { get; set; }

        public IEnumerable<SelectListItem> treatmentList { get; set; }



    }
}