using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Data.Entity;

namespace FarmManager.ViewModels
{
    public partial class CowIndexVM
    {

        public string TagNo { get; set; }
        public string AnimalBreed { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }


    }


}