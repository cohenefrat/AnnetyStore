using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annety
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Item
    {
        public Product DisProd { get; set; }
        public List<Product> UMayLike { get; set; }
        //public int ProductKey { get; set; }
        //public string Barcode { get; set; }
        //public string ImagePath { get; set; }
        //public string Desc { get; set; }
        //public int CategoryCode { get; set; }
        //public decimal Price { get; set; }
        //public Nullable<int> SearchWords { get; set; }
        //public System.DateTime ChangeDate { get; set; }

        //[NotMapped]
        //public HttpPostedFileBase Image { get; set; }
        //public List<Product> MayLike { get; set; }
    }
}
