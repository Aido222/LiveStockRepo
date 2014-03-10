using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmManager.ViewModels
{
    public class TreatmentDetailsVM
    {
        //treatment tbl
        public int TreatmentId { get; set; }
        public Nullable<int> UserMedicineID { get; set; }
        public string TagNo { get; set; }
        public Nullable<int> UserID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> TreatmentDate { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> DosageAmount { get; set; }
        public string PrescribingVet { get; set; }
        public string Administrator { get; set; }

        //Usermedicine tbl
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public Nullable<int> BottleSize { get; set; }
        public string SuppliedBy { get; set; }
        public Nullable<System.DateTime> DateOfPurchase { get; set; }


        //defaultmedicine tbl
        public string MedicineName { get; set; }
        public string MedicineBrand { get; set; }
        public Nullable<int> WithdrawalPeriod { get; set; }
        public Nullable<int> BreachPeriod { get; set; }
        public Nullable<int> TargetSpecies { get; set; }
        public string MainUse { get; set; }
        public string DefaultNotes { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> WithdrawalEnd { get; set; }

        public string Species { get; set; }


    }
}