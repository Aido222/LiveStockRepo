using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarmManager.ViewModels.NewFolder1
{
    public class CreateMedicineVM
    {
        public int UserMedicineID { get; set; }
        public Nullable<int> MedicineID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public Nullable<int> BottleSize { get; set; }
        public string SuppliedBy { get; set; }
        public Nullable<System.DateTime> DateOfPurchase { get; set; }
        public string Notes { get; set; }

        public IEnumerable<SelectListItem> MedList { get; set; }

    }
}