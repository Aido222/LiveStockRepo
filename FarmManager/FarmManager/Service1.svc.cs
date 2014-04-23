using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FarmManager.Models;

namespace FarmManager
{


    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private FarmManagementDBEntities db = new FarmManagementDBEntities();

        public List<MartData> retrievePrices()
        {



            var martQ = (from m in db.MartPrices
                         select new MartData { Breed = m.Breed, Price = m.Price, Weight = m.Weight, Date = m.Date }).Take(5).ToList();
            
            return (martQ.ToList());

            

        }
    }
}
