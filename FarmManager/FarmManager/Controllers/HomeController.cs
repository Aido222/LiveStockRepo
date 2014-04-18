using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmManager.Filters;
using FarmManager.Models;
using FarmManager.ViewModels;
using WebMatrix.WebData;
namespace FarmManager.Controllers
{
        [InitializeSimpleMembership]

    public class HomeController : Controller
    {
        private FarmManagementDBEntities db = new FarmManagementDBEntities();

        public ActionResult Index()
        
        {

            //WebSecurity.Logout();
            //return RedirectToAction("Index", "Home");


            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";


            if (WebSecurity.CurrentUserId != -1)
            {






                var q = (from p in db.Births
                         
                         where p.UserId == WebSecurity.CurrentUserId
                         group p  by new{p.MotherTagNo}
                             into g
                             select new { g.Key.MotherTagNo , Count = g.Count() }).ToArray();



                var q2 = (from items in q
                          orderby items.Count descending
                          select items).Take(5).ToArray();

                //if (q2 != null)
                //{
                //    string tag = q[0].Count.ToString();
                //}
                
                ViewBag.MotherData = q2;





                
                HomeIndexVM indexModel = new HomeIndexVM();


                indexModel.NoOwnedCows = (from tlCows in db.Animals
                                 where tlCows.UserId == WebSecurity.CurrentUserId && tlCows.OwnershipStatus == 1
                                 select tlCows).Count().ToString();

                indexModel.TotalTreatments = (from totalTreats in db.Treatments
                                              where totalTreats.UserID == WebSecurity.CurrentUserId
                                              select totalTreats).Count().ToString();


                indexModel.TotalPrescribedTreats = (from totalTreatsPre in db.Treatments
                                                    where totalTreatsPre.UserID == WebSecurity.CurrentUserId && totalTreatsPre.PrescribingVet != ""
                                                    select totalTreatsPre).Count().ToString();

                indexModel.OfWhichBorn = (from bornCows in db.Animals
                                          where bornCows.UserId == WebSecurity.CurrentUserId && bornCows.BornOnFarm == true && bornCows.OwnershipStatus == 1
                                          select bornCows).Count().ToString();

                indexModel.OfWhichPurchased = (from purchCows in db.Animals
                                               where purchCows.UserId == WebSecurity.CurrentUserId && purchCows.BornOnFarm == false && purchCows.OwnershipStatus == 1
                                               select purchCows).Count().ToString();


                
                var sumPurchases = (from sumPurch in db.Purchases
                                           where sumPurch.UserId == WebSecurity.CurrentUserId && sumPurch.DateBought.Value.Year == DateTime.Now.Year
                                           select sumPurch.Price).Sum();
                indexModel.SumPurchased = String.Format("{0:0.00}", sumPurchases);

                indexModel.MostValuablePurchased = (from purch in db.Purchases
                                                    where purch.UserId == WebSecurity.CurrentUserId && purch.DateBought.Value.Year == DateTime.Now.Year
                                                    orderby purch.Price descending
                                                    select purch.TagNo).FirstOrDefault();
                indexModel.MostValuablePurchased = indexModel.MostValuablePurchased ?? "-";

                indexModel.LatestPurchased = (from purch in db.Purchases
                                              where purch.UserId == WebSecurity.CurrentUserId
                                              orderby purch.DateBought descending
                                              select purch.TagNo).FirstOrDefault();
                indexModel.LatestPurchased = indexModel.LatestPurchased ?? "-";

                indexModel.NoOfPurchases = (from purch in db.Purchases
                                            where purch.UserId == WebSecurity.CurrentUserId && purch.DateBought.Value.Year == DateTime.Now.Year
                                            select purch).Count().ToString();


                var sumSales = (from sale in db.Sales
                                where sale.UserId == WebSecurity.CurrentUserId && sale.DateSold.Value.Year == DateTime.Now.Year
                                select sale.Price).Sum();
                indexModel.SumSales = String.Format("{0:0.00}", sumSales);

                indexModel.HighestSales = (from sale in db.Sales
                                           where sale.UserId == WebSecurity.CurrentUserId && sale.DateSold.Value.Year == DateTime.Now.Year
                                           orderby sale.Price descending
                                           select sale.TagNo).FirstOrDefault();
                indexModel.HighestSales = indexModel.HighestSales ?? "-";


                indexModel.LatestSales = (from sale in db.Sales
                                          where sale.UserId == WebSecurity.CurrentUserId
                                          orderby sale.DateSold descending
                                          select sale.TagNo).FirstOrDefault();
                indexModel.LatestSales = indexModel.LatestSales ?? "-";


                indexModel.NoOfSales = (from sale in db.Sales
                                        where sale.UserId == WebSecurity.CurrentUserId && sale.DateSold.Value.Year == DateTime.Now.Year
                                        select sale).Count().ToString();



                List<SelectListItem> RecentTreatments = new List<SelectListItem>();
                var treatList = (from treat in db.Treatments
                                 where treat.UserID == WebSecurity.CurrentUserId
                                 orderby treat.TreatmentDate descending
                                 select treat).Take(5).ToArray();
                for (int i = 0; i < treatList.Length; i++)
                {
                    RecentTreatments.Add(new SelectListItem
                    {
                        Text = treatList[i].TagNo + " (" + treatList[i].TreatmentDate.ToString().Substring(0,10) + ")",
                        Value = treatList[i].TreatmentId.ToString(),
                    });
                }

                indexModel.RecentTreatList = RecentTreatments;


                indexModel.TotalViableMedicines = (from totalViable in db.UserMedicines
                                                   where totalViable.UserID == WebSecurity.CurrentUserId && totalViable.ExpiryDate > DateTime.Now
                                                   select totalViable).Count().ToString();



                //most common breed
                var mostcommonBreed = db.Animals
                                    .GroupBy(q3 => q3.AnimalBreed)
                                    .OrderByDescending(gp => gp.Count())
                                    .Take(5)
                                    .Select(g => g.Key).ToList();

                int breedID = Convert.ToInt32(mostcommonBreed[0]);

                var breedName = (from breed in db.Breeds
                                 where breed.id == breedID
                                 select breed.Breed1).FirstOrDefault();

                indexModel.MostCommonBreed = breedName;


                indexModel.TotalDeaths = (from deaths in db.Deaths
                                          where deaths.UserId == WebSecurity.CurrentUserId && deaths.DOD.Value.Year == DateTime.Now.Year
                                          select deaths).Count().ToString();



                //id of most common cause
                var mostcommonDeathID = db.Deaths
                                    .GroupBy(q3 => q3.Cause)
                                    .OrderByDescending(gp => gp.Count())
                                    .Take(5)
                                    .Select(g => g.Key).ToList();



                int deathCauseID = Convert.ToInt32(mostcommonDeathID[0]);

                var deathName = (from death in db.DeathCauses
                                 where death.Id == deathCauseID
                                 select death.DeathCauses).FirstOrDefault();


                indexModel.MostCommonDeaths = deathName;
                indexModel.MostCommonDeaths = indexModel.MostCommonDeaths ?? "-";


                indexModel.LatestDeath = (from d in db.Deaths
                                          where d.UserId == WebSecurity.CurrentUserId
                                          select d.TagNo).FirstOrDefault();
                indexModel.LatestDeath = indexModel.LatestDeath ?? "-";

                indexModel.BirthThisYear = (from b in db.Births
                                            join a in db.Animals on b.TagNo equals a.TagNo
                                            where b.UserId == WebSecurity.CurrentUserId && a.DOB.Value.Year == DateTime.Now.Year
                                            select b).Count().ToString();

                
                List<SelectListItem> RecentMeds = new List<SelectListItem>();
                var medList = (from m in db.UserMedicines
                               join defMed in db.DefaultMedicines on m.MedicineID equals defMed.MedicineID
                               orderby m.DateOfPurchase descending
                               where m.UserID == (int)WebSecurity.CurrentUserId
                               select new { m, defMed }).Take(5).ToArray();
                for (int i = 0; i < medList.Length; i++)
                {
                    RecentMeds.Add(new SelectListItem
                    {
                        Text = medList[i].defMed.MedicineName + " (" + medList[i].m.BatchNo + ")",
                        Value = medList[i].m.UserMedicineID.ToString()
                    });
                }

                indexModel.RecentMedPurchases = RecentMeds;

                return View(indexModel);

            }
            else
            {
                string[] myArray = new string[] { "Empty" };

                return View();
            }

            

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        
        
        }



        public JsonResult MotherNumbers()
        {



            if (WebSecurity.CurrentUserId != -1)
            {



                //string[] myArray = new string[12];

                //for (int i = 0; i < purchaseData.Length; i++)
                //{
                //    myArray[i] = purchaseData[i].Sum.ToString();
                //}


                var q = (from p in db.Births

                         where p.UserId == WebSecurity.CurrentUserId
                         group p by new { p.MotherTagNo }
                             into g
                             select new { g.Key.MotherTagNo, Count = g.Count() }).ToArray();

                //                                    group p by new {p.DateBought.Value.Month} into g


                var q2 = (from items in q
                          orderby items.Count descending
                          select items).Take(5).ToArray();

   

                //string[,] bothArray = new string[5, 2];

                //bothArray[0, 0] = q2[0].Count.ToString();
                //bothArray[1, 0] = q2[1].Count.ToString();
                //bothArray[2, 0] = q2[2].Count.ToString();
                //bothArray[3, 0] = q2[3].Count.ToString();
                //bothArray[4, 0] = q2[4].Count.ToString();



                //bothArray[0, 1] = q2[0].MotherTagNo;
                //bothArray[1, 1] = q2[1].MotherTagNo;
                //bothArray[2, 1] = q2[2].MotherTagNo;
                //bothArray[3, 1] = q2[3].MotherTagNo;
                //bothArray[4, 1] = q2[4].MotherTagNo;

                return Json(q2);
                //return Json(q);

            }
            else
            {
                string[] myArray = new string[] { "Empty" };

                return Json(myArray);

            }
        }


        public JsonResult PurchData()
        {
            if (WebSecurity.CurrentUserId != -1)
            {



                //string[] myArray = new string[12];

                //for (int i = 0; i < purchaseData.Length; i++)
                //{
                //    myArray[i] = purchaseData[i].Sum.ToString();
                //}

                var months = Enumerable.Range(1,12);

                                var purchaseData  = (from p in db.Purchases 
                                    where p.UserId == WebSecurity.CurrentUserId
                                    group p by new {p.DateBought.Value.Month} into g
                                    select new {g.Key.Month, Sum = g.Sum(p => p.Price)}).ToArray();
                                string[] myArray = (from month in months
                                                    from p in purchaseData.Where(x => month == x.Month).DefaultIfEmpty()
                                                    select p == null ? "0" : p.Sum.ToString()).ToArray();
                    //string[] myArray = new string[] { purchaseData };

                                //foreach (string sum in myArray)
                                //{
                                //    if (sum.EndsWith()
                                //    {

                                //    }
                                //}

                    return Json(myArray);

            }
            else
            {
                string[] myArray = new string[] { "Empty" };

                return Json(myArray);

            }

        }




        public JsonResult SaleData()
        {
            if (WebSecurity.CurrentUserId != -1)
            {



                //string[] myArray = new string[12];

                //for (int i = 0; i < purchaseData.Length; i++)
                //{
                //    myArray[i] = purchaseData[i].Sum.ToString();
                //}

                var months = Enumerable.Range(1, 12);

                var saleData = (from p in db.Sales
                                    where p.UserId == WebSecurity.CurrentUserId
                                    group p by new { p.DateSold.Value.Month } into g
                                    select new { g.Key.Month, Sum = g.Sum(p => p.Price) }).ToArray();

                string[] myArray = (from month in months
                                    from p in saleData.Where(x => month == x.Month).DefaultIfEmpty()
                                    select p == null ? "0" : p.Sum.ToString()).ToArray();
                //string[] myArray = new string[] { purchaseData };

                //foreach (string sum in myArray)
                //{
                //    if (sum.EndsWith()
                //    {

                //    }
                //}

                return Json(myArray);

            }
            else
            {
                string[] myArray = new string[] { "Empty" };

                return Json(myArray);

            }

        
        
        }







        public JsonResult DeathData()
        {
            if (WebSecurity.CurrentUserId != -1)
            {



                //string[] myArray = new string[12];

                //for (int i = 0; i < purchaseData.Length; i++)
                //{
                //    myArray[i] = purchaseData[i].Sum.ToString();
                //}

                var causes = Enumerable.Range(1, 6);

                var q = (from p in db.Deaths
                        group p by p.Cause
                            into g
                            select new { DeathID = g.Key, Count = g.Count() }).ToArray();

                string[] myArray = (from cause in causes
                                    from p in q.Where(x => cause == x.DeathID).DefaultIfEmpty()
                                    select p == null ? "0" : p.Count.ToString()).ToArray();

                return Json(myArray);

            }
            else
            {
                string[] myArray = new string[] { "Empty" };

                return Json(myArray);

            }

        }

    }
}
