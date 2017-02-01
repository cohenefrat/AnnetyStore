using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Annety.Controllers
{
    public class SearchResultController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();

        // GET: SearchResult
        public ActionResult Index()
        {
            return View("SearchResult");
        }

        public ActionResult FromMenu(int CategoryCode)
        {
            var products = db.Product.Where(p => p.CategoryCode == CategoryCode).ToList();
            return View(products.ToList());


        }


        public ActionResult BoyOrGirl(int? CategoryCode)
        {
            List<Product> pl = new List<Annety.Product>();
            bool b;
            if (CategoryCode == 1)
                b = true;
            else
                b = false;
            //עבור כל הקטגוריות שמצאתי שולפת את כל המוצרים ויוצרת רשימה
            var categories = db.Categories.Where(c => c.ParentCategory == b).ToList();
                foreach (var item in categories)
                {   
                    var products = db.Product.Where(p => p.CategoryCode == item.CategoryCode);
                    foreach (var product in products)
                    {
                        pl.Add(product);
                    }
                    
                }

            return View("FromMenu", pl.ToList());

        }
      //  add another parameter which symbol the source
        [HttpGet]
        public ActionResult Search_Box(String Search_Box)
        {
            
            var products = db.Product.ToList();
            string a = "Dress";
            char[] delimiterChars = { ' ', ',', ':', '.' };
            String[] splited = a.Split(delimiterChars);
            int[] array = new int[products.Count ];
            foreach (string s in  splited)
            {
                products.Where(u=>u.Desc.Contains(s)).ToList().ForEach(p=>array[products.IndexOf(p)]++);
                //foreach (var item in products   )
                //{
                //    if (item.Desc.Contains(s))
                //        array[products.IndexOf(item)]++;
                //}
            }
            if (Session["kk"]==null )
                Session["kk"] = new Stack <Product>();

            Stack <Product> f = (Stack <Product>)Session["kk"];
            //f.Push();
            
                var max = array.Max();
          //select the products according the array

            return View("FromMenu",products.ToList());


            }
        

    public ActionResult Product(int ProductKey)
    {
        var products = db.Product.Where(p => p.ProductKey == ProductKey);
        return View();


    }

     public ActionResult ProductDetails(int? ProductKey)
     {
         var product = db.Product.Where(p => p.ProductKey == ProductKey);
         return View("Item",product);

     }
    }
}
