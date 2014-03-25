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
            /*UserMedicine usermedicine = db.UserMedicines.Find(id);
            if (usermedicine == null)
            {
                return HttpNotFound();
            }*/
            var numberOfUses = (from c in db.Treatments
                                where c.UserMedicineID == id && c.UserID == (int)WebSecurity.CurrentUserId
                                select c).ToArray();


            if (numberOfUses != null)
            {

                int? dosage = 0;

                for (int i = 0; i < numberOfUses.Length; i++)
                {
                    dosage += Convert.ToInt32(numberOfUses[i].DosageAmount);
                }

                if (dosage > medDetails.BottleSize)
                {
                    medDetails.RemainingML = 0;
                }
                else
                {
                    medDetails.RemainingML = medDetails.BottleSize - dosage;
                }

                medDetails.NumberOfUsesLeft = medDetails.RemainingML / medDetails.AvgDose;
            }
            else
            {
                medDetails.RemainingML = medDetails.BottleSize;
                medDetails.NumberOfUsesLeft = medDetails.RemainingML / medDetails.AvgDose;
            }




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
            return View();
        }

        //
        // POST: /Medicines/Create

        [HttpPost]
        public ActionResult Create(UserMedicine usermedicine)
        {
            if (ModelState.IsValid)
            {
                db.UserMedicines.Add(usermedicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usermedicine);
        }

        //
        // GET: /Medicines/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserMedicine usermedicine = db.UserMedicines.Find(id);
            if (usermedicine == null)
            {
                return HttpNotFound();
            }
            return View(usermedicine);
        }

        //
        // POST: /Medicines/Edit/5

        [HttpPost]
        public ActionResult Edit(UserMedicine usermedicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usermedicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usermedicine);
        }

        //
        // GET: /Medicines/Delete/5

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