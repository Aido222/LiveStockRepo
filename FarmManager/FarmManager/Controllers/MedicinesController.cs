using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmManager.Filters;
using FarmManager.Models;
using FarmManager.ViewModels;
using FarmManager.ViewModels.NewFolder1;
using WebMatrix.WebData;

namespace FarmManager.Controllers
{
    public class MedicinesController : Controller
    {
        private FarmManagementDBEntities db = new FarmManagementDBEntities();

        //
        // GET: /Medicines/
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult Index()
        {

            //query selects all medicines from the db belonging to the user and puts them into the MedicineIndexVM
            List<MedicineIndexVM> medList = (from umed in db.UserMedicines
                                             join defMed in db.DefaultMedicines on umed.MedicineID equals defMed.MedicineID
                                             where umed.UserID == WebSecurity.CurrentUserId
                                             select new MedicineIndexVM
                                             {
                                                 UserMedicineID = umed.UserMedicineID,
                                                 BatchNo = umed.BatchNo,
                                                 MedicineName = defMed.MedicineName,
                                                 BottleSize = umed.BottleSize,
                                                 WithdrawalPeriod = defMed.WithdrawalPeriod,
                                                 ExpiryDate = umed.ExpiryDate
                                             }).ToList();





            

            return View(medList);
        }

        //
        // GET: /Medicines/Details/5

        public ActionResult Details(int id = 0)
        {
            //selects medicine details from db where id = id passed from querystring
            //joins on the usermedicine and the default medicine to give all details related to the medicine
            MedicineDetailVM medDetails = (from umed in db.UserMedicines
                                           join dmed in db.DefaultMedicines on umed.MedicineID equals dmed.MedicineID
                                           join species in db.Species on dmed.TargetSpecies equals species.SpeciesID
                                           where umed.UserMedicineID == id && umed.UserID == WebSecurity.CurrentUserId
                                           select new MedicineDetailVM
                                           {
                                               UserMedicineID = umed.UserMedicineID,
                                               BatchNo = umed.BatchNo,
                                               ExpiryDate = umed.ExpiryDate,
                                               OpenDate = umed.OpenDate,
                                               BottleSize = umed.BottleSize,
                                               SuppliedBy = umed.SuppliedBy,
                                               DateOfPurchase = umed.DateOfPurchase,
                                               Notes = umed.Notes,

                                               MedicineName = dmed.MedicineName,
                                               MedicineBrand = dmed.MedicineBrand,
                                               WithdrawalPeriod = dmed.WithdrawalPeriod,
                                               BreachPeriod = dmed.BreachPeriod,
                                               TargetSpecies = dmed.TargetSpecies,
                                               MainUse = dmed.MainUse,
                                               DefNotes = dmed.Notes,

                                               AvgDose = dmed.AvgDosage,
                                               
                                               Species1 = species.Species1
                                           }).FirstOrDefault();
   
            //selects the every treatment the medicine was used for so the dossage can be added up
            var numberOfUses = (from c in db.Treatments
                                where c.UserMedicineID == id && c.UserID == (int)WebSecurity.CurrentUserId
                                select c).ToArray();

            //if it has never been used
            if (numberOfUses != null)
            {
                //amount used it set to 0
                int? dosage = 0;

                for (int i = 0; i < numberOfUses.Length; i++)
                {
                    //adds up ml's used
                    dosage += Convert.ToInt32(numberOfUses[i].DosageAmount);
                }

                // if amount used is > than bottle size. The bottle is empty
                if (dosage > medDetails.BottleSize)
                {
                    medDetails.RemainingML = 0;
                }
                else
                //else amount left = bottle size - amount used (dosage)
                {
                    medDetails.RemainingML = medDetails.BottleSize - dosage;
                }

                //number of avg doses left  = amount left / avgdose 
                medDetails.NumberOfUsesLeft = medDetails.RemainingML / medDetails.AvgDose;
            }
            else
            {
                medDetails.RemainingML = medDetails.BottleSize;
                medDetails.NumberOfUsesLeft = medDetails.RemainingML / medDetails.AvgDose;
            }



            //list of treatemts foe the medicine gets sent to view
            List<Treatment> treatments = (from treat in db.Treatments
                                       where treat.UserID == WebSecurity.CurrentUserId && treat.UserMedicineID == medDetails.UserMedicineID
                                       select treat).ToList();

            ViewBag.data = treatments;

            return View(medDetails);
        }

        //
        // GET: /Medicines/Create

        public ActionResult Create()
        {
            //create new CreateMedicineVM
            CreateMedicineVM createMed = new CreateMedicineVM();

            //Select list of default medicines the user can select from 
            List<SelectListItem> DefMedList = new List<SelectListItem>();
            var defmedList = (from dmed in db.DefaultMedicines
                              select dmed).ToArray();
            for (int i = 0; i < defmedList.Length; i++)
            {
                DefMedList.Add(new SelectListItem
                {
                    Text = defmedList[i].MedicineBrand + ", " + defmedList[i].MedicineName,
                    Value = Convert.ToString(defmedList[i].MedicineID),
                });
            }

            //puts med list in vm
            createMed.MedList = DefMedList;


            return View(createMed);
        }

        //
        // POST: /Medicines/Create

        [HttpPost]
        //create post method
        public ActionResult Create(CreateMedicineVM usermedicineVM)
        {

            //Create new UserMedicine Model
            UserMedicine userMedicine = new UserMedicine();

            //populates fields in model with data passed back from create view in viewmodel
            userMedicine.MedicineID = usermedicineVM.MedicineID;
            userMedicine.UserID = WebSecurity.CurrentUserId;
            userMedicine.BatchNo = usermedicineVM.BatchNo;
            userMedicine.ExpiryDate = usermedicineVM.ExpiryDate;
            userMedicine.OpenDate = usermedicineVM.OpenDate;
            userMedicine.BottleSize = usermedicineVM.BottleSize;
            userMedicine.SuppliedBy = usermedicineVM.SuppliedBy;
            userMedicine.DateOfPurchase = usermedicineVM.DateOfPurchase;
            userMedicine.Notes = usermedicineVM.Notes;

            db.UserMedicines.Add(userMedicine);
            db.SaveChanges();







            return RedirectToAction("Index");
        }

        //
        // GET: /Medicines/Edit/5

        public ActionResult Edit(int id = 0)
        {
            //passes def med list foe edit view
            List<SelectListItem> DefMedList = new List<SelectListItem>();
            var defmedList = (from dmed in db.DefaultMedicines
                              select dmed).ToArray();
            for (int i = 0; i < defmedList.Length; i++)
            {
                DefMedList.Add(new SelectListItem
                {
                    Text = defmedList[i].MedicineBrand + ", " + defmedList[i].MedicineName,
                    Value = Convert.ToString(defmedList[i].MedicineID),
                });
            }

            
            ViewBag.MedList = DefMedList;





            //selects medicine to be edited
            UserMedicine usermedicine = db.UserMedicines.Find(id);
            if (usermedicine == null)
            {
                return HttpNotFound();
            }



            //send back name of medicine used initially
            var nameMed = (from umed in db.UserMedicines
                           join defmed in db.DefaultMedicines on umed.MedicineID equals defmed.MedicineID
                           where umed.UserMedicineID == id
                           select defmed).FirstOrDefault();
            //puts name in viewbag
            ViewBag.MedName = nameMed.MedicineName;


            return View(usermedicine);
        }

        //
        // POST: /Medicines/Edit/5

        [HttpPost]
        public ActionResult Edit(UserMedicine usermedicine)
        {
            //retrieves medicine to be edited
            var query =
            from umed in db.UserMedicines
            where umed.UserMedicineID == usermedicine.UserMedicineID
            select umed;

            //updates record from data passed from view in model
            foreach (UserMedicine umed in query)
            {
                umed.BatchNo = usermedicine.BatchNo;
                umed.ExpiryDate = usermedicine.ExpiryDate;
                umed.BottleSize = usermedicine.BottleSize;
                umed.SuppliedBy = usermedicine.SuppliedBy;
                umed.DateOfPurchase = usermedicine.DateOfPurchase;
                umed.Notes = usermedicine.Notes;
                // Insert any additional changes to column values.
            }

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return HttpNotFound();

            }

            

            return RedirectToAction("Details/" + usermedicine.UserMedicineID);
        }

        //

        public ActionResult Delete(int id = 0)
        {
            UserMedicine usermedicine = db.UserMedicines.Find(id);
            if (usermedicine == null)
            {
                return HttpNotFound();
            }
            return View(usermedicine);
        }

        //
        // POST: /Medicines/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMedicine usermedicine = db.UserMedicines.Find(id);
            db.UserMedicines.Remove(usermedicine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}