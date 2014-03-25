using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmManager.ViewModels
{
    public class MedicineDetailVM
    {
        public int UserMedicineID { get; set; }
        
        
        public string BatchNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> OpenDate { get; set; }
        public Nullable<int> BottleSize { get; set; }
        public string SuppliedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DateOfPurchase { get; set; }
        public string Notes { get; set; }

        public string MedicineName { get; set; }
        public string MedicineBrand { get; set; }
        public Nullable<int> WithdrawalPeriod { get; set; }
        public Nullable<int> BreachPeriod { get; set; }
        public Nullable<int> TargetSpecies { get; set; }
        public string MainUse { get; set; }
        public string DefNotes { get; set; }

        public string Species1 { get; set; }


        public int? RemainingML { get; set; }
        public int? AvgDose { get; set; }
        public int? NumberOfUsesLeft { get; set; }
    }
}