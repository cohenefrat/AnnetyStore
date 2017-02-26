using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annety
{
    public partial class ItemInCart
    {   
        public int Code { get; set; }
        public Product Product { get; set; }
        public string SizeDesc { get; set; }

        public string ColorName { get; set; }
        public int Units { get; set; }
    }
}