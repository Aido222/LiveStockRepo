using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarmManager.ViewModels
{
    public class NewTreatmentVM
    {

        public int TreatmentId { get; set; }

        public Nullable<int> UserMedicineID { get; set; }
        public string TagNo { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> TreatmentDate { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> DosageAmount { get; set; }
        public string PrescribingVet { get; set; }
        public string Administrator { get; set; }

        public IEnumerable<SelectListItem> UserMedList { get; set; }

    }
}