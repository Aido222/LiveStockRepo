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
    //The authorize attribute tag allows only users who have been registered or logged in through simple membership
    [Authorize]
    [InitializeSimpleMembership]
    public class CowsController : Controller
    {
        //Makes connection to the farmmanagementdb in sql server, and also the simplemembership usercontext
        private FarmManagementDBEntities db = new FarmManagementDBEntities();
        private UsersContext Udb = new UsersContext();




        [Authorize]
        public ActionResult Index()
        {

            //Block of code below inistialises a List of type CowIndexVM, a viewmodel and fills it with a list of cows belonging to the user. 
            //It contains multiple outter joins on the animals, breed, birth, purchase, sale, death and purchase table so it can return every cow the user has ever owned
            //WebSecurity.CurrentUserId retrieved the current users user id and animal species = 2 makes sure the animals are cows
            List<CowIndexVM> myCowIndexVM = (from animals in db.Animals
                                             join breed in db.Breeds on animals.AnimalBreed equals breed.id
                                             join birth in db.Births on animals.TagNo equals birth.TagNo into j0
                                             from birth in j0.DefaultIfEmpty()
                                             join purchase in db.Purchases on animals.TagNo equals purchase.TagNo into j1
                                             from purchase in j1.DefaultIfEmpty()
                                             join sale in db.Sales on animals.TagNo equals sale.TagNo into j2
                                             from sale in j2.DefaultIfEmpty()
                                             join death in db.Deaths on animals.TagNo equals death.TagNo into j3
                                             from death in j3.DefaultIfEmpty()
                                             join purch in db.Purchases on animals.TagNo equals purch.TagNo into j4
                                             from purch in j4.DefaultIfEmpty()
                                             where animals.UserId == WebSecurity.CurrentUserId && animals.Species == 2
                      //Takes the rows from the query and inserts them into the view model fields
                                             select new CowIndexVM
                                             {
                                                 TagNo = animals.TagNo,
                                                 AnimalBreed = breed.Breed1,
                                                 DateAdded = animals.DateAdded,
                                                 OwnershipStatus = animals.OwnershipStatus,
                                                 DateSold = sale.DateSold,
                                                 LocationSold = sale.LocationSold,
                                                 BornOnFarm = animals.BornOnFarm,
                                                 DOD = death.DOD,
                                                 DOB = animals.DOB,
                                                 DateBought = purch.DateBought,



                                             }).ToList();



            //Code pulls back a list of treatments for the users cows and only selects the ones where the animal will still be in withdrawal from the treatment
            List<SelectListItem> WithDrawalList = new List<SelectListItem>();
            var withdrawlPeriod = (from animal2 in db.Animals
                                   join treat in db.Treatments on animal2.TagNo equals treat.TagNo
                                   join userMed in db.UserMedicines on treat.UserMedicineID equals userMed.UserMedicineID
                                   join defMed in db.DefaultMedicines on userMed.MedicineID equals defMed.MedicineID
                                   where DateTime.Now < System.Data.Objects.EntityFunctions.AddDays(treat.TreatmentDate, defMed.WithdrawalPeriod) && animal2.UserId == WebSecurity.CurrentUserId
                                   select new { treat, defMed }).ToArray();

            for (int i = 0; i < withdrawlPeriod.Length; i++)
            {
                WithDrawalList.Add(new SelectListItem
                {
                    Text = withdrawlPeriod[i].treat.TagNo,
                    Value = withdrawlPeriod[i].treat.TreatmentId.ToString()
                });
            }

            //Add the list of treatments to the ViewBag
            ViewBag.WithList = WithDrawalList;

            //Returns viewmodel to view
            return View("Index", myCowIndexVM);
        }


        [Authorize]
        //actionresult recieves the tagnumber of the selected animal and retrieves data baed on that
            //If id is invalid .net error handling displays an error page to the user
        public ActionResult Details(string id)
        {

 

            var isBorn = (from c in db.Animals
                          where c.TagNo == id && c.UserId == (int)WebSecurity.CurrentUserId
                          select c).FirstOrDefault();



            //Pulls all details for the specific cow. From animals, and either purchase tbl or birth depending on the animal
             CowDetailVM animal = (from animals in db.Animals
                                   join breed in db.Breeds on animals.AnimalBreed equals breed.id
                                   join birth in db.Births on animals.TagNo equals birth.TagNo into j0
                                   from birth in j0.DefaultIfEmpty()
                                   join purchase in db.Purchases on animals.TagNo equals purchase.TagNo into j1
                                   from purchase in j1.DefaultIfEmpty()
                                   where animals.TagNo == id
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
                                       AIID = birth.AIID,
                                       Difficult = birth.Difficult

                                   }).FirstOrDefault();

 

            //Retrieves a list of calfs for this cow if any
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

         
            //puts calf list in viewmodel
             animal.calvesList = cowCalves;


            //retrieves a list of note sfor the animal
             List<SelectListItem> Notes = new List<SelectListItem>();
             var noteList = (from b in db.AnimalNotes
                             where b.TagNo == id && b.UserID == (int)WebSecurity.CurrentUserId
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



            //
             List<SelectListItem> BullCalveList = new List<SelectListItem>();
             var bullcalveList = (from b in db.Births
                             where b.SireTagNo == id && b.UserId == (int)WebSecurity.CurrentUserId
                             select b).ToArray();
             for (int i = 0; i < bullcalveList.Length; i++)
             {
                 BullCalveList.Add(new SelectListItem
                 {
                     Text = bullcalveList[i].TagNo,
                     Value = Convert.ToString(bullcalveList[i].TagNo)
                 });
             }

             animal.bullCalveList = BullCalveList;

             //                                  join birth in db.Births on animals.TagNo equals birth.TagNo into j0



            //REtrieves the list of treatments for the animal
             List<SelectListItem> TreatList = new List<SelectListItem>();

             var treatList = (from treat in db.Treatments
                              join userMed in db.UserMedicines on treat.UserMedicineID equals userMed.UserMedicineID
                              join defMed in db.DefaultMedicines on userMed.MedicineID equals defMed.MedicineID into j0
                              from defMed in j0.DefaultIfEmpty()
                              where treat.TagNo == id && treat.UserID == (int)WebSecurity.CurrentUserId
                              select new { treat, userMed, defMed }).ToArray();
             for (int i = 0; i < treatList.Length; i++)
             {
                 TreatList.Add(new SelectListItem
                 {
                     Text = treatList[i].defMed.MedicineName + " (" + treatList[i].treat.TreatmentDate.ToString().Substring(0, 11) + ")",
                     Value = Convert.ToString(treatList[i].treat.TreatmentId)
                 });
             }

             animal.treatmentList = TreatList;




            //Retrieve only treatments where the animal is still in withdrawal
             List<SelectListItem> WithDrawalList = new List<SelectListItem>();

             var withdrawlPeriod = (from animal2 in db.Animals
                                    join treat in db.Treatments on animal2.TagNo equals treat.TagNo
                                    join userMed in db.UserMedicines on treat.UserMedicineID equals userMed.UserMedicineID
                                    join defMed in db.DefaultMedicines on userMed.MedicineID equals defMed.MedicineID
                                    where animal2.TagNo == id && DateTime.Now < System.Data.Objects.EntityFunctions.AddDays(treat.TreatmentDate, defMed.WithdrawalPeriod)
                                    select new { treat, defMed }).ToArray();

             for (int i = 0; i < withdrawlPeriod.Length; i++)
             {
                 WithDrawalList.Add(new SelectListItem
                 {
                     Text = withdrawlPeriod[i].defMed.MedicineName + " (" + withdrawlPeriod[i].treat.TreatmentDate.ToString().Substring(0, 10) +")",
                     Value = withdrawlPeriod[i].treat.TreatmentId.ToString()
                 });
             }


             animal.WithDrawalList = WithDrawalList;


            //returns view model to view with all select list included
            return View("Details", animal);
          


        }


        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        
        // POST: /Cows/Create
        [Authorize]
        //[HttpPost]
        //public ActionResult Create(Animal animal, FormCollection collection)
        //{
        //    if (ModelState.IsValid)
        //    {
                




        //        animal.Species = 2;
        //        animal.OwnershipStatus = 1;
        //        animal.DateAdded = DateTime.Now;
        //        animal.UserId = (int)WebSecurity.CurrentUserId;
        //        //animal.Sex = collection["Sex"];
        //        animal.Sex = animal.Sex;

        //        db.Animals.Add(animal);
        //        db.SaveChanges();


        //        //Creating instance of animal in purchase tbl
        //        Purchase Purch1 = new Purchase();

        //        //Purch1.UserId = (int)Session["USERID"];
        //        Purch1.TagNo = animal.TagNo;
        //        Purch1.BoughtFrom = collection["PurchasedFrom"];
        //        Purch1.Location = collection["PurchaseLocation"];
        //        Purch1.DateBought = DateTime.Now;
        //        Purch1.Notes = collection["Notes"];
        //        Purch1.Price = decimal.Parse(collection["Price"]);

        //        db.Purchases.Add(Purch1);
        //        db.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    return View(animal);
        //}

        //

        //Edit animal purchase actionresult
        [Authorize]
        public ActionResult Edit(string id)
        {

            //Returns row from animal table for this animal
            Animal myanimal = (from animals in db.Animals
                               where animals.TagNo == id && animals.UserId == WebSecurity.CurrentUserId
                               select animals).FirstOrDefault();

            //Returns row from purchase table for this animal
            Purchase myPurch = (from purch in db.Purchases
                                where purch.TagNo == id && purch.UserId == WebSecurity.CurrentUserId
                                select purch).FirstOrDefault();

            //declares new viewmodel
            CowPurchVM editCow = new CowPurchVM();

            //populates viewmodel with data from the two tables
            editCow.TagNo = myanimal.TagNo;
            editCow.BirthDate = myanimal.DOB;
            editCow.Breed = Convert.ToString(myanimal.AnimalBreed);

            if (myanimal.Sex == false)
            {
                editCow.Sex = "0";
            }
            else
            {
                editCow.Sex = "1";
            }

            editCow.BoughtFrom = myPurch.BoughtFrom;
            editCow.Location = myPurch.Location;
            editCow.Price = myPurch.Price;
            editCow.Notes = myPurch.Notes;
            


            //Send list of cow breeds to the view
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

            editCow.BreedList = BreedList;
 

            return View(editCow);
        }

        
        //purchase edit post actionresult
        [Authorize]
        [HttpPost]
        //recieves the viewmodel returned from view
        public ActionResult Edit(CowPurchVM animal)
        {
            //selects correct row from animals table so it can be updated
            var queryDetails = from a in db.Animals
                               where a.TagNo == animal.TagNo
                               select a;


            //updates the retrieved row
            foreach (Animal a in queryDetails)
            {
                a.AnimalBreed = Convert.ToInt32(animal.Breed);
                a.DOB = animal.BirthDate;
                a.BornOnFarm = false;

                if (Convert.ToInt32(animal.Sex) == 0)
                {
                    a.Sex = false;
                }
                else
                {
                    a.Sex = true;
                }

                a.DOB = animal.BirthDate;
            }


            try
            {
                db.SaveChanges();
            }
            catch
            {
                return HttpNotFound();

            
            
            }





            //does the same as before except for the purchase table
            var queryPurch = from p in db.Purchases
                               where p.TagNo == animal.TagNo
                               select p;



            foreach (Purchase p in queryPurch)
            {
                p.BoughtFrom = animal.BoughtFrom;
                p.Location = animal.Location;
                p.Notes = animal.Notes;
                p.Price = animal.Price;
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





        [HttpGet]
        public ActionResult EditBirth(string id)
        {


            ///As above retrieves the correct row from both animals and birth table
            Animal myanimal = (from animals in db.Animals
                               where animals.TagNo == id && animals.UserId == WebSecurity.CurrentUserId
                               select animals).FirstOrDefault();

            Birth myBirth = (from birth in db.Births
                                where birth.TagNo == id && birth.UserId == WebSecurity.CurrentUserId
                                select birth).FirstOrDefault();

            CowBirthVM editCow = new CowBirthVM();

            editCow.TagNo = myanimal.TagNo;
            editCow.BirthDate = myanimal.DOB;
            editCow.Breed = myanimal.AnimalBreed.ToString();
            editCow.MotherTagNo = myBirth.MotherTagNo;
            editCow.SireTagNo = myBirth.SireTagNo;


            if (myanimal.Sex == false)
            {
                editCow.Sex = "0";
            }
            else
            {
                editCow.Sex = "1";
            }



            //Puts all breed into viewmodel so they can be selected in view
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

            editCow.BreedList = BreedList;


            //Puts all alive currently owned female cows into a list
            List<SelectListItem> MotherList = new List<SelectListItem>();
            MotherList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var motherList = (from b in db.Animals
                              where b.Sex == false && b.UserId == (int)WebSecurity.CurrentUserId && b.OwnershipStatus == 1
                              select b).ToArray();

            for (int j = 0; j < motherList.Length; j++)
            {
                MotherList.Add(new SelectListItem
                {
                    //Assign Values to text
                    Text = motherList[j].TagNo,
                    Value = motherList[j].TagNo.ToString(),
                });
            }


            editCow.MotherTagList = MotherList;





            //puts all male alive, owned bulls into list
            List<SelectListItem> BullList = new List<SelectListItem>();
            BullList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var bullList = (from b in db.Animals
                            where b.Sex == true && b.UserId == (int)WebSecurity.CurrentUserId && b.OwnershipStatus == 1
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

            editCow.SireTagList = BullList;


            //Sneds ai list to view
            //sets the longest time an AI could be viable
            DateTime upperDate = DateTime.Now.AddMonths(-10);
            //Sets the shortest amount of time when a calf could be born from te ai
            DateTime lowerDate = DateTime.Now.AddMonths(-4);

            //select ai's that dates lie betrween the boundaries set
            List<SelectListItem> AIList = new List<SelectListItem>();
            AIList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var aiList = (from b in db.AIs
                          where b.UserID == (int)WebSecurity.CurrentUserId && b.Date > upperDate && b.Date < lowerDate where b.Born == false
                          select b).ToArray();
            for (int i = 0; i < aiList.Length; i++)
            {
                AIList.Add(new SelectListItem
                {
                    Text = aiList[i].TagNo + " (" + aiList[i].Date + ")",
                    Value = aiList[i].AIID.ToString(),



                });
            }

            editCow.AIList = AIList;


            return View(editCow);
        }



        [HttpPost]
        public ActionResult EditBirth(CowBirthVM editCow, FormCollection fCollection)
        {
            //Select srow in animals table for the animal being edited
            var queryDetails = from a in db.Animals
                               where a.TagNo == editCow.TagNo
                               select a;



            foreach (Animal a in queryDetails)
            {
                a.AnimalBreed = Convert.ToInt32(editCow.Breed);
                a.DOB = editCow.BirthDate;
                a.BornOnFarm = true;

                if (Convert.ToInt32(editCow.Sex) == 0)
                {
                    a.Sex = false;
                }
                else
                {
                    a.Sex = true;
                }

                a.DOB = editCow.BirthDate;
            }


            try
            {
                db.SaveChanges();
            }
            catch
            {
                return HttpNotFound();


            }




            var queryBirth= from b in db.Births
                               where b.TagNo == editCow.TagNo
                               select b;

            int selectedInseminationType = Convert.ToInt32(fCollection["InseminationType"]);


            foreach (Birth b in queryBirth)
            {

                if (selectedInseminationType == 0)
                {
                    b.MotherTagNo = editCow.MotherTagNo;
                    b.SireTagNo = editCow.SireTagNo;
                }
                else
                {
                    b.AIID = Convert.ToInt32(editCow.AICow);

                    //works
                    var aiRecord = from bAI in db.AIs
                                   where bAI.AIID == b.AIID
                                   select bAI;

               foreach (AI ai in aiRecord)
                {
                    ai.Born = true;

                }

                }

                if (Convert.ToInt32(editCow.Difficult) == 1)
                {
                    b.Difficult = true;
                }
                else
                {
                    b.Difficult = false;
                }




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
        //Creates new purchased cow
        public ActionResult CreatePurchase()
        {
            //creates new CowPurchVM
            CowPurchVM cowPurch = new CowPurchVM();
          


            //Puts all cow breed into a list
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

            //sends viewmodel to view
            return View(cowPurch);
        }


        // POST: /Cows/Create

        [HttpPost]
        [Authorize]
        public ActionResult CreatePurchase(CowPurchVM cowPurc, FormCollection fCollection)
        {
            

        
           




                //creates a new animal model
                Animal animal = new Animal();

                //populates animal model with data from view model from view 
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

            //again takes data from viewmodel
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

        
        [Authorize]
        public ActionResult CreateCowBirth()
        {

            //creates new cowbirthvm
            CowBirthVM cowBirth = new CowBirthVM();

            //puts breeds into list and send to view
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
                                  where b.Sex== false && b.UserId == (int)WebSecurity.CurrentUserId && b.OwnershipStatus == 1
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
                            where b.Sex == true && b.UserId == (int)WebSecurity.CurrentUserId && b.OwnershipStatus == 1
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
            //sets the longest time an AI could be viable
            DateTime upperDate = DateTime.Now.AddMonths(-10);
            //Sets the shortest amount of time when a calf could be born from te ai
            DateTime lowerDate = DateTime.Now.AddMonths(-4);

            List<SelectListItem> AIList = new List<SelectListItem>();
            AIList.Add(new SelectListItem { Text = "Please select", Value = "Please Select" });
            var aiList = (from b in db.AIs
                          where b.UserID == (int)WebSecurity.CurrentUserId && b.Date > upperDate && b.Date < lowerDate where b.Born == false
                            select b).ToArray();
            for (int i = 0; i < aiList.Length; i++)
            {
                AIList.Add(new SelectListItem
                {
                    Text = aiList[i].TagNo + " (" + aiList[i].Date + ")",
                    Value = aiList[i].AIID.ToString(),

                    
                });
            }

            cowBirth.AIList = AIList;

            

            return View(cowBirth);
        }




        //Jsonresult action method recieves an Ajax request from view and an animal tag no
        //it checks to see if this tagno already exists in the db and is currently owned by this farmer
        //if so the user cannot add the animal again
        public JsonResult CheckTagNo(FormCollection form)
        {
            string name = Convert.ToString(form["TagNumber"]).ToUpper();


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
            
            //Determines if the calf wa born through natural or artificial insemination
            int selectedInseminationType = Convert.ToInt32(fCollection["InseminationType"]);
            

            Animal animal = new Animal();
            Birth birth = new Birth();

            //if natural
            if (selectedInseminationType == 0)
            {
                birth.MotherTagNo = cowBith.MotherTagNo;
                birth.SireTagNo = cowBith.SireTagNo;
            }
                //if artificial
            else
            {
                birth.AIID = Convert.ToInt32(cowBith.AICow);

                //update ai record to show ai hass been born
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
            //Sends list to view of possible causes of death on farm
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


            //Records death and adds it to the death tbl
            death.UserId = (int)WebSecurity.CurrentUserId;
            death.Cause = death.Cause;
            db.Deaths.Add(death);
            db.SaveChanges();

            //update animal status in the animal table to reflect the animals death
            var animalStatus =
            (from c in db.Animals
             where c.TagNo == death.TagNo && c.UserId == (int)WebSecurity.CurrentUserId
             select c).First();
            //OWnershipstatus = 3 means the animal has died

            animalStatus.OwnershipStatus = 3;

            db.SaveChanges();
            //redirect to index
            return RedirectToAction("Index");
        }



        //new note for cow
        public ActionResult AddCowNote(string id)
        {
            AnimalNote newNote =  new AnimalNote();

            newNote.TagNo = id;

            return View(newNote);
        }


        [HttpPost]
        public ActionResult AddCowNote(AnimalNote animalNote)
        {
            //populates genral note data
            animalNote.Date = DateTime.Now;
            animalNote.UserID = (int)WebSecurity.CurrentUserId;

            //adds note and saves it
            db.AnimalNotes.Add(animalNote);
            db.SaveChanges();


            //redirects to details page for the animal
            return RedirectToAction("Details/" + animalNote.TagNo);
        }


        //Json actionresult retrieves note id from view and selects note from db where id equals retrieved note 
        public JsonResult RetrieveNote(FormCollection form)
        {
            int id = Convert.ToInt32((form["NoteID"]).ToUpper());


            var noteText = (from c in db.AnimalNotes
                          where c.NoteId == id && c.UserID == (int)WebSecurity.CurrentUserId
                          select c).FirstOrDefault();


            DateTime? myDate = noteText.Date;
            
            //sends data back in an array
            string[] myArray = new string[] { noteText.Note, noteText.Description, Convert.ToString(myDate), noteText.NoteId.ToString()};
                

            return Json(myArray);
        }



        [Authorize]
        //accepts note id and deletes the id from the db
        public ActionResult DeleteNote(int id = 0)
        {
            AnimalNote note = db.AnimalNotes.Find(id);
            db.AnimalNotes.Remove(note);
            db.SaveChanges();
            

            

            return RedirectToAction("Details/" + note.TagNo);
        }



        [Authorize]
        [HttpGet]
        //selects note where id = passe id and passes to view
        public ActionResult EditNote(int id = 0)
        {
            AnimalNote note = db.AnimalNotes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }


        [Authorize]
        [HttpPost]
        //edits note where note id = passed id
        public ActionResult EditNote(AnimalNote editnote)
        {

            var query =
            from note in db.AnimalNotes
            where note.NoteId == editnote.NoteId
            select note;

            foreach (AnimalNote note in query)
            {
                note.Note = editnote.Note;
                note.Description = editnote.Description;
            }

            try
            {
                db.SaveChanges();
            }
            catch 
            {
                return HttpNotFound();

            }



            return RedirectToAction("Details/" + editnote.TagNo);


        
        
        }



        public ActionResult Sale(string id)
        {

            Sale sale = new Sale();

            sale.TagNo = id;
            sale.UserId = WebSecurity.CurrentUserId;

            return View(sale);
        }


        [HttpPost]
        //adds new sale to the sale table
        public ActionResult Sale(Sale newSale)
        {

          

            db.Sales.Add(newSale);
            db.SaveChanges();



            var animalStatus =
            (from c in db.Animals
             where c.TagNo == newSale.TagNo && c.UserId == (int)WebSecurity.CurrentUserId
                select c).First();
            //updates the animal status n the animal table to reflect the sale
            //OWnershipstatus = 2 means the animal has been sold
            animalStatus.OwnershipStatus = 2;
            db.SaveChanges();


            return RedirectToAction("Details/" + newSale.TagNo);
        }



        //REtrieves sale and sends it back to view in a Json result through ajax
        public JsonResult RetrieveSale(FormCollection form)
        {
            string id = Convert.ToString((form["TagNo"]).ToUpper());


            var saleData = (from c in db.Sales
                            where c.TagNo == id && c.UserId == (int)WebSecurity.CurrentUserId
                            select c).FirstOrDefault();



            string[] myArray = new string[] { saleData.TagNo, saleData.LocationSold, saleData.DateSold.ToString(), saleData.SoldTo, saleData.Notes };
                        
            
            return Json(myArray);
        }



     


        //Actionresult is called by ajax. Retrieves info about the seath of this animal and sends it to the view via json
        public JsonResult RetrieveDeath(FormCollection form)
        {
            string id = Convert.ToString((form["TagNo"]).ToUpper());


            var deathData = (from c in db.Deaths
                             join death in db.DeathCauses on c.Cause equals death.Id
                             where c.TagNo == id && c.UserId == (int)WebSecurity.CurrentUserId
                             select new { c, death }).FirstOrDefault();



            string[] myArray = new string[] { deathData.c.TagNo, deathData.c.DOD.ToString(), deathData.c.Notes, deathData.death.DeathCauses };
                        
            
            return Json(myArray);
        }



        //Actionresult is called by ajax. Retrieves info about the purchase of this animal and sends it to the view via json

        public JsonResult RetrieveCowPurch(FormCollection form)
        {
            string id = Convert.ToString((form["TagNo"]).ToUpper());


            var purchData = (from p in db.Purchases
                             
                             where p.TagNo == id && p.UserId == (int)WebSecurity.CurrentUserId
                             select p).FirstOrDefault();



            string[] myArray = new string[] { purchData.TagNo, purchData.DateBought.ToString(), purchData.Notes, purchData.Location, purchData.Price.ToString(), purchData.BoughtFrom };


            return Json(myArray);
        }


        //Actionresult is called by ajax. Retrieves info for a specific treatment where treatment = id and sends back via json result

        public JsonResult RetrieveTreatments(FormCollection form)
        {
           // int id = Convert.ToString((form["ID"]).ToUpper());
            int id = Convert.ToInt32(form["ID"]);

            var treatments = (from t in db.Treatments
                              join um in db.UserMedicines on t.UserMedicineID equals um.UserMedicineID
                              join dm in db.DefaultMedicines on um.UserMedicineID equals dm.MedicineID
                              where t.TreatmentId == id && t.UserID == (int)WebSecurity.CurrentUserId
                              select new { t, dm, um }).FirstOrDefault();
           


            string[] myArray = new string[] { treatments.t.DosageAmount.ToString(), treatments.t.TreatmentDate.ToString().Substring(0,10), treatments.dm.MedicineName, treatments.um.BatchNo };

            return Json(myArray);
        }
       
        
    }
}