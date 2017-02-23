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
        //public int ProdColor { get; set; }
        //public int ProdSize { get; set; }
        //public int ProdUnits { get; set; }

    }
}
