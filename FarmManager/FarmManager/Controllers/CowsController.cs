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
using System.Data.Entity.Validation;



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


            List<Animal> Animal1 = (from animals in db.Animals
                                        where animals.Species == 2 & animals.OwnershipStatus == 1
                                        & animals.UserId == WebSecurity.CurrentUserId
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
                




                animal.Species = 2;
                animal.OwnershipStatus = 1;
                animal.DateAdded = DateTime.Now;
                animal.UserId = (int)WebSecurity.CurrentUserId;
                //animal.Sex = collection["Sex"];
                animal.Sex = animal.Sex;

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

            CowPurchVM cowPurch = new CowPurchVM();
            /*var breeds = (from t in db.Breeds
                          select new SelectListItem()
                          {
                              Text = t.Breed1,
                              Value = t.Breed1
                          }).ToList<SelectListItem>();

            ViewData["breeds"] = breeds;*/



            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var breedsList = (from b in db.Breeds where b.SpeciesID == 2 select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].id.ToString(),
                    Selected = (breedsList[i].id == 1)
                });
            }

            cowPurch.BreedsList = BreedList;


            return View(cowPurch);
        }


        // POST: /Cows/Create

        [HttpPost]
        [Authorize]
        public ActionResult CreatePurchase(CowPurchVM cowPurc, FormCollection fCollection)
        {
            

        
           /* List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var breedsList = (from b in db.Breeds where b.SpeciesID == 2 select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].id.ToString(),
                    Selected = (breedsList[i].id == 1)
                });
            }

            ViewData["Breeds"] = BreedList;*/






            if (ModelState.IsValid)
            {
                Animal animal = new Animal();

                animal.TagNo = cowPurc.TagNo;
                animal.Breed = Convert.ToInt32(cowPurc.Breed);

                if (Convert.ToInt32(cowPurc.Sex) == 0)
                {
                    animal.Sex = false;
                }
                else
                {
                    animal.Sex = true;
                }



                animal.Species = 2;
                animal.OwnershipStatus = 1;
                animal.DateAdded = DateTime.Now;
                animal.UserId = (int)WebSecurity.CurrentUserId;
                animal.BornOnFarm = false;

                //set animals dob
                int age = Convert.ToInt32(cowPurc.Age);
                var todaysDate = DateTime.Now;
                var birthDate = todaysDate.AddYears(-age);
                animal.DOB = birthDate;

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
            CowBirthVM cowBirth = new CowBirthVM();


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

            cowBirth.BreedList = BreedList;


            //Cow List
            List<SelectListItem> MotherList = new List<SelectListItem>();
            MotherList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var motherList = (from b in db.Animals
                                  where b.Sex== false && b.UserId == (int)WebSecurity.CurrentUserId
                                  select b).ToArray();
            for (int i = 0; i < motherList.Length; i++)
            {
                MotherList.Add(new SelectListItem
                {
                    Text = motherList[i].TagNo,
                    Value = motherList[i].TagNo.ToString(),
                   // Selected = (motherList[i].AnimalId == 1)
                });
            }


            cowBirth.MotherTagList = MotherList;


            
          
 
            //Bull List
            List<SelectListItem> BullList = new List<SelectListItem>();
            BullList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var bullList = (from b in db.Animals
                            where b.Sex == true && b.UserId == (int)WebSecurity.CurrentUserId
                            select b).ToArray();
            for (int i = 0; i < bullList.Length; i++)
            {
                BullList.Add(new SelectListItem
                {
                    Text = bullList[i].TagNo,
                    Value = bullList[i].TagNo.ToString(),
                    //Selected = (bullList[i].AnimalId == 1)
                });
            }

            cowBirth.SireTagList = BullList;



            //Sneds ai list to view
            DateTime upperDate = DateTime.Now.AddMonths(-10);
            DateTime lowerDate = DateTime.Now.AddMonths(-4);

            List<SelectListItem> AIList = new List<SelectListItem>();
            AIList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var aiList = (from b in db.AIs
                          where b.UserID == (int)WebSecurity.CurrentUserId && b.Date > upperDate && b.Date < lowerDate && b.Born == false
                            select b).ToArray();
            for (int i = 0; i < aiList.Length; i++)
            {
                AIList.Add(new SelectListItem
                {
                    Text = aiList[i].TagNo + " (" + aiList[i].Date + ")",
                    Value = aiList[i].AIID.ToString(),
                    //Selected = (aiList[i].TagNo.ToString == 1)

                    
                });
            }

            cowBirth.AIList = AIList;

            

            return View(cowBirth);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateCowBirth(CowBirthVM cowBith, FormCollection fCollection)
        {
            

            int selectedInseminationType = Convert.ToInt32(fCollection["InseminationType"]);
            

            Animal animal = new Animal();
            Birth birth = new Birth();


            if (selectedInseminationType == 0)
            {
                birth.MotherTagNo = cowBith.MotherTagNo;
                birth.SireTagNo = cowBith.SireTagNo;
            }
            else
            {
                birth.AIID = Convert.ToInt32(cowBith.AICow);

                //works
                var aiRecord = (from b in db.AIs
                                where b.AIID == birth.AIID
                                select b).ToArray();

                aiRecord[0].Born = true;
            }


            animal.UserId = (int)WebSecurity.CurrentUserId;
            animal.TagNo = cowBith.TagNo;
            animal.Species = 2;

            if (Convert.ToInt32(cowBith.Sex) == 0)
            {
                animal.Sex = false;
            }
            else
            {
                animal.Sex = true;
            }

            animal.OwnershipStatus = 1;
            animal.DateAdded = DateTime.Now;
            animal.BornOnFarm = true;
            animal.Breed = Convert.ToInt32(cowBith.Breed);



            birth.UserId = (int)WebSecurity.CurrentUserId;
            birth.TagNo = animal.TagNo;

            birth.Notes = cowBith.Notes;
            //Determine wether birth was difficult
            if (Convert.ToInt32(cowBith.Difficult) == 1)
            {
                birth.Difficult = true;
            }
            else
            {
                birth.Difficult = false;
            }


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
                    Value = deathList[i].Id.ToString()
                    
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
            List<SelectListItem> DeathList = new List<SelectListItem>();
            DeathList.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var deathList = (from b in db.DeathCauses select b).ToArray();
            for (int i = 0; i < deathList.Length; i++)
            {
                DeathList.Add(new SelectListItem
                {
                    Text = deathList[i].DeathCauses,
                    Value = deathList[i].Id.ToString()

                });
            }

            ViewData["Cause"] = DeathList;



            death.UserId = (int)WebSecurity.CurrentUserId;
            death.Cause = death.Cause;
            db.Deaths.Add(death);
            db.SaveChanges();


            var animalStatus =
            (from c in db.Animals
             where c.TagNo == death.TagNo && c.UserId == (int)WebSecurity.CurrentUserId
             select c).First();

            animalStatus.OwnershipStatus = 3;

            db.SaveChanges();

            return RedirectToAction("Index");
        }





       

    }
}