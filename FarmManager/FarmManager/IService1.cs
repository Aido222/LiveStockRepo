using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FarmManager.Models;

namespace FarmManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<MartData> retrievePrices();

    }

    public class MartData
    {
        public int ID { get; set; }
        public string Breed { get; set; }
        public string Weight { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
}
