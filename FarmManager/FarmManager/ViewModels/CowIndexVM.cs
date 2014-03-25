using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Data.Entity;

namespace FarmManager.ViewModels
{
    public partial class CowIndexVM
    {

        public string TagNo { get; set; }
        public string AnimalBreed { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> OwnershipStatus { get; set; }
        public Nullable<bool> BornOnFarm { get; set; }



        //sale
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DateSold { get; set; }
        public string LocationSold { get; set; }

        //death
        public Nullable<System.DateTime> DOD { get; set; }


        //birth
        public Nullable<System.DateTime> DOB { get; set; }

        //purch
        public Nullable<System.DateTime> DateBought { get; set; }


        public Nullable<System.DateTime> TreatmentDate { get; set; }

        public Nullable<int> WithdrawalPeriod { get; set; }

        public int? TreatmentId { get; set; }


        public IEnumerable<SelectListItem> WithDrawalList { get; set; }




    }


}