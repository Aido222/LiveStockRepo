using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FarmManager.ViewModels
{
    public class CowBirthVM
    {
        //[Required]
        public string TagNo { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public string Breed { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        [Required]
        public string MotherTagNo { get; set; }
        public string SireTagNo { get; set; }
        public string Notes { get; set; }
        public string Difficult { get; set; }
        public string AICow { get; set; }


        [Required]
        public IEnumerable<SelectListItem> SireTagList { get; set; }
        public IEnumerable<SelectListItem> MotherTagList { get; set; }
        public IEnumerable<SelectListItem> BreedList { get; set; }
        public IEnumerable<SelectListItem> AIList { get; set; }

    }
}