using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarmManager.ViewModels
{
    public class AICreate
    {
        public int AIID { get; set; }

        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string TagNo { get; set; }
        public Nullable<int> Breed { get; set; }
        public string BullID { get; set; }
        public string AIOperator { get; set; }

        public IEnumerable<SelectListItem> BreedList { get; set; }

    }
}