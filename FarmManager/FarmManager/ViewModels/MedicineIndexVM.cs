using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmManager.ViewModels
{
    public class MedicineIndexVM
    {
        public int UserMedicineID { get; set; }
        public Nullable<int> MedicineID { get; set; }
        public string BatchNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> BottleSize { get; set; }

        public string MedicineName { get; set; }
        public Nullable<int> WithdrawalPeriod { get; set; }
    }
}