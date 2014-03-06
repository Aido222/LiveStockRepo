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


            /*List<Animal> Animal1 = (from animals in db.Animals
                                        where animals.Species == 2 & animals.OwnershipStatus == 1
                                        & animals.UserId == WebSecurity.CurrentUserId
                                        select animals).ToList();*/

            List <CowIndexVM> myCowIndexVM = (from animals in db.Animals
                                    join aBreed in db.Breeds on animals.AnimalBreed equals aBreed.id
                                    where animals.Species == 2 && animals.OwnershipStatus == 1
                                    & animals.UserId == WebSecurity.CurrentUserId
                                    orderby animals.DateAdded descending
                                    select new CowIndexVM
                                    {
                                        TagNo = animals.TagNo,
                                        AnimalBreed = aBreed.Breed1,
                                        DateAdded = animals.DateAdded
                                    }).ToList();
                                    
                                    
                                   // animals).ToList();




            return View("Index", myCowIndexVM);
        }

        //
        // GET: /Cows/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {


            var isBorn = (from c in db.Animals
                          where c.TagNo == id && c.UserId == (int)WebSecurity.CurrentUserId
                          select c).FirstOrDefault();


            
              //  string isBornOnFarm = Convert.ToString(isBorn.BornOnFarm);
            


            
           /* if (isBornOnFarm == "true")
            {

                CowDetailVM animal = (from animals in db.Animals
                                      join breed in db.Breeds on animals.AnimalBreed equals breed.id
                                      join birth in db.Births on animals.TagNo equals birth.TagNo
                                      where animals.TagNo == id && animals.UserId == WebSecurity.CurrentUserId
                                      orderby animals.DateAdded descending

                                      select new CowDetailVM
                                      {
                                          TagNo = animals.TagNo,
                                          Sex = animals.Sex,
                                          AnimalBreed = breed.Breed1,
                                          DOB = animals.DOB,
                                          OwnershipStatus = animals.OwnershipStatus,
                                          BornOnFarm = animals.BornOnFarm,


                                          MotherTagNo = birth.MotherTagNo,
                                          SireTagNo = birth.SireTagNo,
                                          Difficult = birth.Difficult

                                      }).FirstOrDefault();

                if (animal == null)
                {
                    return HttpNotFound();
                }
                return View("Details", animal);


            }
            else
            {

                CowDetailVM animal = (from animals in db.Animals
                                      join breed in db.Breeds on animals.AnimalBreed equals breed.id
                                      join purchase in db.Purchases on animals.TagNo equals purchase.TagNo
                                      where animals.TagNo == id && animals.UserId == WebSecurity.CurrentUserId
                                      orderby animals.DateAdded descending

                                      select new CowDetailVM
                                      {
                                          TagNo = animals.TagNo,
                                          Sex = animals.Sex,
                                          AnimalBreed = breed.Breed1,
                                          DOB = animals.DOB,
                                          OwnershipStatus = animals.OwnershipStatus,
                                          BornOnFarm = animals.BornOnFarm,


                                          DateBought = purchase.DateBought,
                                          BoughtFrom = purchase.BoughtFrom,
                                          Price = purchase.Price,
                                          Location = purchase.Location,

                                      }).FirstOrDefault();

                if (animal == null)
                {
                    return HttpNotFound();
                }
                return View("Details", animal);
            }*/



            CowDetailVM animal = (from animals in db.Animals
                                  join breed in db.Breeds on animals.AnimalBreed equals breed.id
                                  join birth in db.Births on animals.TagNo equals birth.TagNo into j0
                                  from birth in j0.DefaultIfEmpty()
                                  join purchase in db.Purchases on animals.TagNo equals purchase.TagNo into j1
                                  from purchase in j1.DefaultIfEmpty()
                                  where animals.TagNo == id && animals.UserId == WebSecurity.CurrentUserId
                                  orderby animals.DateAdded descending

                                  select new CowDetailVM
                                  {
                                      TagNo = animals.TagNo,
                                      Sex = animals.Sex,
                                      AnimalBreed = breed.Breed1,
                                      DOB = animals.DOB,
                                      OwnershipStatus = animals.OwnershipStatus,
                                      BornOnFarm = animals.BornOnFarm,

                                      DateBought = purchase.DateBought,
                                      BoughtFrom = purchase.BoughtFrom,
                                      Price = purchase.Price,
                                      Location = purchase.Location,

                                      MotherTagNo = birth.MotherTagNo,
                                      SireTagNo = birth.SireTagNo,
                                      Difficult = birth.Difficult

                                  }).FirstOrDefault();




            List<SelectListItem> cowCalves = new List<SelectListItem>();
            var calfList = (from b in db.Births
                            where b.MotherTagNo == id && b.UserId == WebSecurity.CurrentUserId 
                            select b).ToArray();
            for (int i = 0; i < calfList.Length; i++)
            {
                cowCalves.Add(new SelectListItem
                {
                    Text = calfList[i].TagNo,
                    Value = calfList[i].TagNo,
                });
            }

         

            animal.calvesList = cowCalves;


            List<SelectListItem> Notes = new List<SelectListItem>();
            var noteList = (from b in db.AnimalNotes
                            where b.TagNo == id
                            select b).ToArray();
            for (int i = 0; i < noteList.Length; i++)
            {
                Notes.Add(new SelectListItem
                {
                    Text = noteList[i].Description,
                    Value = Convert.ToString(noteList[i].NoteId)
                });
            }

            animal.notesList = Notes;

            

            if (animal == null)
            {
                return HttpNotFound();
            }
            return View("Details", animal);
          


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
          



            List<SelectListItem> BreedList = new List<SelectListItem>();
            BreedList.Add(new SelectListItem { Text = "Please select", Value = "Please select" });
            var breedsList = (from b in db.Breeds where b.SpeciesID == 2 select b).ToArray();
            for (int i = 0; i < breedsList.Length; i++)
            {
                BreedList.Add(new SelectListItem
                {
                    Text = breedsList[i].Breed1,
                    Value = breedsList[i].id.ToString(),
                });
            }

            cowPurch.BreedList = BreedList;


            return View(cowPurch);
        }


        // POST: /Cows/Create

        [HttpPost]
        [Authorize]
        public ActionResult CreatePurchase(CowPurchVM cowPurc, FormCollection fCollection)
        {
            

        
           






            //if (ModelState.IsValid)
           // {
                Animal animal = new Animal();

                animal.TagNo = cowPurc.TagNo;
                animal.AnimalBreed = Convert.ToInt32(cowPurc.Breed);

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
                //int age = Convert.ToInt32(cowPurc.Age);
                //var todaysDate = DateTime.Now;
                //var birthDate = todaysDate.AddYears(-age);
                //animal.DOB = birthDate;
                animal.DOB = cowPurc.BirthDate;

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
            //}

            //return View(cowPurc);
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
            //foreach (var item in motherList)
            //{
              //  MotherList.Add(new SelectListItem { Text = item.TagNo.ToString(), Value = item.TagNo.ToString() });
            //}
            for (int j = 0; j < motherList.Length; j++)
            {
                MotherList.Add(new SelectListItem
                {
                    //Assign Values to text
                    Text = motherList[j].TagNo,
                    Value = motherList[j].TagNo.ToString(),
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
                          where b.UserID == (int)WebSecurity.CurrentUserId && b.Date > upperDate && b.Date < lowerDate
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





        public JsonResult CheckTagNo(FormCollection form)
        {
            string name = Convert.ToString(form["TagNumber"]).ToUpper();

            //if (name.Equals("Sumit"))
              //  return Json(false);

            var dbCows = (from c in db.Animals
                          where c.TagNo == name && c.UserId == (int)WebSecurity.CurrentUserId
                          select c).Count();

            

            if (dbCows != 0)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
            

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
            animal.DOB = Convert.ToDateTime(cowBith.BirthDate);

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
            animal.AnimalBreed = Convert.ToInt32(cowBith.Breed);



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
            DeathList.Add(new SelectListItem { Text = "Please select", Value = "Please select" });
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




        public ActionResult AddCowNote(string id)
        {
            AnimalNote newNote =  new AnimalNote();

            newNote.TagNo = id;

            return View(newNote);
        }


        [HttpPost]
        public ActionResult AddCowNote(AnimalNote animalNote)
        {

            animalNote.Date = DateTime.Now;
            db.AnimalNotes.Add(animalNote);
            db.SaveChanges();


            return RedirectToAction("Index");
        }






       

    }
}