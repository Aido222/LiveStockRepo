//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FarmManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Death
    {
        public int DeathId { get; set; }
        public string TagNo { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DOD { get; set; }
        public Nullable<int> Cause { get; set; }
        public string Notes { get; set; }
    }
}
