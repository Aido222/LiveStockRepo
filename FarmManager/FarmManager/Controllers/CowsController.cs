using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmManager.Models;
using WebMatrix.WebData;
using FarmManager.Filters;
using FarmManager.ViewModels;



namespace FarmManager.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class CowsController : Controller
    {
        private FarmManagementDBEntities db = new FarmManagementDBEntities();
        private UsersContext Udb = new UsersContext();




        [Authorize]
        public ActionResult Index()
        {
            //return View(db.Animals.);

            List<Animal> Animal1 = (from animals in db.Animals
                              where animals.Species == "Cow" && animals.OwnershipStatus == "Owned" &&
                              animals.UserId == (int)WebSecurity.CurrentUserId
                              select animals).ToList();

            return View(Animal1);
        }

        //
        // GET: /Cows/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        //
        // GET: /Cows/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        
        // POST: /Cows/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Animal animal, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                




                animal.Species = "Cow";
                animal.OwnershipStatus = "Owned";
                animal.DateAdded = DateTime.Now;
                animal.UserId = (int)WebSecurity.CurrentUserId;
                animal.Sex = collection["Sex"];

                db.Animals.Add(animal);
                db.SaveChanges();


                //Creating instance of animal in purchase tbl
                Purchase Purch1 = new Purchase();

                //Purch1.UserId = (int)Session["USERID"];
                Purch1.TagNo = animal.TagNo;
                Purch1.BoughtFrom = collection["PurchasedFrom"];
                Purch1.Location = collection["PurchaseLocation"];
                Purch1.DateBought = DateTime.Now;
                Purch1.Notes = collection["Notes"];
                Purch1.Price = decimal.Parse(collection["Price"]);

                db.Purchases.Add(Purch1);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(animal);
        }

        //
        // GET: /Cows/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        //
        // POST: /Cows/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(animal);
        }

        //
        // GET: /Cows/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        //
        // POST: /Cows/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal animal = db.Animals.Find(id);
            db.Animals.Remove(animal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //MY Methods
        [Authorize]
        public ActionResult CreatePurchase()
        {
            /*var breeds = (from t in db.Breeds
                          select new SelectListItem()
                          {
                              Text = t.Breed1,
                              Value = t.Breed1
                          }).ToList<SelectListItem>();

            ViewData["breeds"] = breeds;*/



            List<SelectListItem> BreedList= new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var breedsList = (from b in db.Breeds select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].Breed1.ToString(),
                    Selected = (breedsList[i].id == 1)
                });
            }

            ViewData["Breeds"] = BreedList;

            return View();
        }


        // POST: /Cows/Create

        [HttpPost]
        [Authorize]
        public ActionResult CreatePurchase(CowPurchVM cowPurc, FormCollection fCollection)
        {


            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var breedsList = (from b in db.Breeds select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].Breed1.ToString(),
                    Selected = (breedsList[i].id == 1)
                });
            }

            ViewData["Breeds"] = BreedList;


            if (ModelState.IsValid)
            {
                Animal animal = new Animal();

                animal.TagNo = cowPurc.TagNo;
                animal.Breed = cowPurc.Breed;
                animal.Age = cowPurc.Age;
                animal.Sex = cowPurc.Sex;
                animal.Species = "Cow";
                animal.OwnershipStatus = "Owned";
                animal.DateAdded = DateTime.Now;
                animal.UserId = (int)WebSecurity.CurrentUserId;
                animal.Origin = "Purchased";

                db.Animals.Add(animal);
                db.SaveChanges();


                //Creating instance of animal in purchase tbl
                Purchase Purch1 = new Purchase();

                Purch1.UserId = (int)WebSecurity.CurrentUserId;
                Purch1.TagNo = animal.TagNo;
                Purch1.BoughtFrom = cowPurc.BoughtFrom;
                Purch1.Location = cowPurc.Location;
                Purch1.DateBought = DateTime.Now;
                Purch1.Notes = cowPurc.Notes;
                Purch1.Price = cowPurc.Price;

                db.Purchases.Add(Purch1);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cowPurc);
        }

        
        [Authorize]
        public ActionResult CreateCowBirth()
        {

            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var breedsList = (from b in db.Breeds select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].Breed1.ToString(),
                    Selected = (breedsList[i].id == 1)
                });
            }

            ViewData["Breeds"] = BreedList;



            List<SelectListItem> MotherList = new List<SelectListItem>();
            MotherList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var motherList = (from b in db.Animals
                                  where b.Sex=="Female" && b.UserId == (int)WebSecurity.CurrentUserId
                                  select b).ToArray();
            for (int i = 0; i < motherList.Length; i++)
            {
                MotherList.Add(new SelectListItem
                {
                    Text = motherList[i].TagNo,
                    Value = motherList[i].TagNo.ToString(),
                    Selected = (motherList[i].AnimalId == 1)
                });
            }


            ViewData["tags"] = MotherList;



          
 
            /*
            List<SelectListItem> BullList = new List<SelectListItem>();
            BullList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var bullList = (from b in db.Animals
                            where b.Sex == "Male" && b.UserId == (int)WebSecurity.CurrentUserId
                            select b).ToArray();
            for (int i = 0; i < bullList.Length; i++)
            {
                BullList.Add(new SelectListItem
                {
                    Text = bullList[i].TagNo,
                    Value = bullList[i].TagNo.ToString(),
                    Selected = (bullList[i].AnimalId == 1)
                });
            }

            ViewData["bullTags"] = BullList;
            */

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateCowBirth(CowBirthVM cowBith, FormCollection fCollection)
        {

            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var breedsList = (from b in db.Breeds select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].Breed1.ToString(),
                    Selected = (breedsList[i].id == 1)
                });
            }

            ViewData["Breeds"] = BreedList;



            List<SelectListItem> MotherList = new List<SelectListItem>();
            MotherList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var motherList = (from b in db.Animals
                              where b.Sex == "Female" && b.UserId == (int)WebSecurity.CurrentUserId
                              select b).ToArray();
            for (int i = 0; i < motherList.Length; i++)
            {
                MotherList.Add(new SelectListItem
                {
                    Text = motherList[i].TagNo,
                    Value = motherList[i].TagNo.ToString(),
                    Selected = (motherList[i].AnimalId == 1)
                });
            }


            ViewData["tags"] = MotherList;



            /*List<SelectListItem> BullList = new List<SelectListItem>();
            BullList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var bullList = (from b in db.Animals
                            where b.Sex == "Male" && b.UserId == (int)WebSecurity.CurrentUserId
                            select b).ToArray();
            for (int i = 0; i < bullList.Length; i++)
            {
                BullList.Add(new SelectListItem
                {
                    Text = bullList[i].TagNo,
                    Value = bullList[i].TagNo.ToString(),
                    Selected = (bullList[i].AnimalId == 1)
                });
            }

            ViewData["bullTags"] = BullList;*/



            Animal animal = new Animal();

            animal.UserId = (int)WebSecurity.CurrentUserId;
            animal.TagNo = cowBith.TagNo;
            animal.Species = "Cow";
            animal.Sex = cowBith.Sex;
            animal.OwnershipStatus = "Owned";
            animal.DateAdded = DateTime.Now;
            animal.Origin = "Born";
            animal.Breed = cowBith.Breed;



            Birth birth = new Birth();
            birth.UserId = (int)WebSecurity.CurrentUserId;
            birth.TagNo = animal.TagNo;
            birth.MotherTagNo = cowBith.MotherTagNo;
            birth.SireTagNo = cowBith.SireTagNo;
            birth.Notes = cowBith.Notes;
            birth.BirthDate = cowBith.BirthDate;





            db.Animals.Add(animal);
            db.SaveChanges();

            db.Births.Add(birth);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        

        [Authorize]
        public ActionResult CowDeath(string id)
        {
            List<SelectListItem> DeathList = new List<SelectListItem>();
            DeathList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var deathList = (from b in db.DeathCauses select b).ToArray();
            for (int i = 0; i < deathList.Length; i++)
            {
                DeathList.Add(new SelectListItem
                {
                    Text = deathList[i].DeathCauses,
                    Value = deathList[i].DeathCauses.ToString(),
                    
                });
            }

            ViewData["Cause"] = DeathList;
            ViewData["TagNo"] = id;

            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult CowDeath(Death death, FormCollection collection)
        {
            var deathCauses = (from t in db.DeathCauses
                               select new SelectListItem()
                               {
                                   Text = t.DeathCauses,
                                   Value = t.DeathCauses
                               }).ToList<SelectListItem>();

            ViewData["Cause"] = deathCauses;



            death.UserId = (int)WebSecurity.CurrentUserId;
            death.Cause = collection["Cause"];
            db.Deaths.Add(death);
            db.SaveChanges();


            var animalStatus =
    (from c in db.Animals
     where c.TagNo == death.TagNo && c.UserId == (int)WebSecurity.CurrentUserId
     select c).First();

            animalStatus.OwnershipStatus = "Deceased";

            db.SaveChanges();

            return RedirectToAction("Index");
        }





       

    }
}