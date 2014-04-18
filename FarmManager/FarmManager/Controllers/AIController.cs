using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmManager.Models;
using FarmManager.ViewModels;
using WebMatrix.WebData;

namespace FarmManager.Controllers
{
    public class AIController : Controller
    {

        //creates connection to farm management db
        private FarmManagementDBEntities db = new FarmManagementDBEntities();

        //
        // GET: /AI/

        public ActionResult Index()
        {
            //displays list of ai's in index view
            //return View(db.AIs.Where())ToList());

            List<AI> aiList = (from ai in db.AIs
                               where ai.UserID == WebSecurity.CurrentUserId
                               select ai).ToList();

            return View(aiList);
        }

        //
        // GET: /AI/Details/5

        public ActionResult Details(int id = 0)
        {
            //returns the details of a specific ai
            AI ai = db.AIs.Find(id);
            if (ai == null)
            {
                return HttpNotFound();
            }
            return View(ai);
        }

        //
        // GET: /AI/Create

        //actionresult below creates a new ai in the ai table for the tagno of the animal passed in
        public ActionResult Create(string id)
        {

            AICreate ai = new AICreate();

            
            ai.TagNo = id;

            //takes all breeds from breeds table and sends to view
            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var breedsList = (from b in db.Breeds where b.SpeciesID == 2 select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].id.ToString(),
                });
            }
            //puts breedlist into vm
            ai.BreedList = BreedList;

            //sends vm to view
            return View(ai);
        }

        //
        // POST: /AI/Create

        [HttpPost]
        //create ai post event
        public ActionResult Create(AI ai)
        {
            if (ModelState.IsValid)
            {
                //creates ai and populates it from the passed model
                AI newAI = new AI();

                newAI.UserID = (int)WebSecurity.CurrentUserId;
                newAI.TagNo = ai.TagNo;
                newAI.Date = ai.Date;
                newAI.Breed = ai.Breed;
                newAI.BullID = ai.BullID;
                newAI.AIOperator = ai.AIOperator;
                newAI.ExpectedDueDate = ai.Date.Value.AddDays(273);

                db.AIs.Add(newAI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ai);
        }

        //
        // GET: /AI/Edit/5
        //select ai from db based on passed id and then sends to edit view
        public ActionResult Edit(int id = 0)
        {
            var aiQuery = (from a in db.AIs
                               where a.AIID == id && a.UserID == WebSecurity.CurrentUserId
                               select a).FirstOrDefault();

            AICreate ai = new AICreate();

            ai.AIOperator = aiQuery.AIOperator;
            ai.Breed = aiQuery.Breed;
            ai.BullID = aiQuery.BullID;
            ai.Date = aiQuery.Date;
            ai.TagNo = aiQuery.TagNo;
            ai.AIID = id;


            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var breedsList = (from b in db.Breeds where b.SpeciesID == 2 select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].id.ToString(),
                });
            }

            ai.BreedList = BreedList;

            return View(ai);
        }

        //
        // POST: /AI/Edit/5

        [HttpPost]
        //edit AI
        public ActionResult Edit(AICreate ai)
        {

            //selects row to be updated
            var queryAI = from a in db.AIs
                               where a.AIID == ai.AIID
                               select a;

            //updates the fields of the selected row with data from the model
            foreach (AI a in queryAI)
            {
                a.AIOperator = ai.AIOperator;
                a.Breed = ai.Breed;
                a.BullID = ai.BullID;
                a.Date = ai.Date;
                a.ExpectedDueDate = ai.Date.Value.AddDays(273);
            }


            try
            {
                db.SaveChanges();
            }
            catch
            {
                return HttpNotFound();



            }


            return RedirectToAction("Index");
        }


        //Deletes ai
        public ActionResult Delete(int id = 0)
        {
            AI ai = db.AIs.Find(id);
            if (ai == null)
            {
                return HttpNotFound();
            }
            return View(ai);
        }

        //
        // POST: /AI/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AI ai = db.AIs.Find(id);
            db.AIs.Remove(ai);
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