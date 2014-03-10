using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmManager.Models;
using FarmManager.ViewModels;
using WebMatrix.WebData;

namespace FarmManager.Controllers
{
    public class TreatmentController : Controller
    {

        private FarmManagementDBEntities db = new FarmManagementDBEntities();

        //
        // GET: /Treatment/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Treatment/Details/5

        public ActionResult Details(int id)
        {
            TreatmentDetailsVM treatment = (from treat in db.Treatments
                                            join userMed in db.UserMedicines on treat.UserMedicineID equals userMed.UserMedicineID
                                            join defMed in db.DefaultMedicines on userMed.MedicineID equals defMed.MedicineID
                                            join species in db.Species on defMed.TargetSpecies equals species.SpeciesID
                                            where treat.TreatmentId == id && treat.UserID == WebSecurity.CurrentUserId
                                            select new TreatmentDetailsVM
                                            {
                                                TreatmentId = treat.TreatmentId,
                                                TagNo = treat.TagNo,
                                                TreatmentDate = treat.TreatmentDate,
                                                Notes = treat.Notes,
                                                DosageAmount = treat.DosageAmount,
                                                PrescribingVet = treat.PrescribingVet,
                                                Administrator = treat.Administrator,

                                                BatchNo = userMed.BatchNo,
                                                ExpiryDate = userMed.ExpiryDate,
                                                OpenDate = userMed.OpenDate,
                                                BottleSize = userMed.BottleSize,
                                                SuppliedBy = userMed.SuppliedBy,
                                                DateOfPurchase = userMed.DateOfPurchase,

                                                MedicineName = defMed.MedicineName,
                                                MedicineBrand = defMed.MedicineBrand,
                                                WithdrawalPeriod = defMed.WithdrawalPeriod,
                                                BreachPeriod = defMed.BreachPeriod,
                                                Species = species.Species1,
                                                MainUse = defMed.MainUse,
                                                DefaultNotes = defMed.Notes,


                                                
                                
                                            }).FirstOrDefault();

            treatment.WithdrawalEnd = treatment.TreatmentDate.Value.AddDays(Convert.ToDouble(treatment.WithdrawalPeriod));

            
            
            
            if (treatment == null)
            {
                return HttpNotFound();
            }
             
            return View(treatment);
        }

        //
        // GET: /Treatment/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Treatment/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Treatment/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Treatment/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Treatment/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Treatment/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        
    }
}
