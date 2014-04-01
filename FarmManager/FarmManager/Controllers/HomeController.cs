using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarmManager.Filters;
using FarmManager.Models;
using WebMatrix.WebData;
namespace FarmManager.Controllers
{
        [InitializeSimpleMembership]

    public class HomeController : Controller
    {
        private FarmManagementDBEntities db = new FarmManagementDBEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";


            if (WebSecurity.CurrentUserId != -1)
            {



                //string[] myArray = new string[12];

                //for (int i = 0; i < purchaseData.Length; i++)
                //{
                //    myArray[i] = purchaseData[i].Sum.ToString();
                //}


                var q = (from p in db.Births
                         
                         where p.UserId == WebSecurity.CurrentUserId
                         group p  by new{p.MotherTagNo}
                             into g
                             select new { g.Key.MotherTagNo , Count = g.Count() }).ToArray();

                //                                    group p by new {p.DateBought.Value.Month} into g


                var q2 = (from items in q
                          orderby items.Count descending
                          select items).Take(5).ToArray();

                string tag = q[0].Count.ToString();

                ViewBag.MotherData = q2;

                return View();
                //return Json(q);

            }
            else
            {
                string[] myArray = new string[] { "Empty" };

                return Json(myArray);

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
