//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Annety
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    public partial class WatchList
    {   [Key]
        public int WatchCode { get; set; }
        public int UserCode { get; set; }
        public int ProductKey { get; set; }
        public System.DateTime WatchDate { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Users Users { get; set; }
    }
}
