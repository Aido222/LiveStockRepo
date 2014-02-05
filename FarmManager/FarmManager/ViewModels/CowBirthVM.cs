using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmManager.ViewModels
{
    public class CowBirthVM
    {
        public string TagNo { get; set; }
        public string Sex { get; set; }
        public string Breed { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string MotherTagNo { get; set; }
        public string SireTagNo { get; set; }
        public string Notes { get; set; }


    }
}