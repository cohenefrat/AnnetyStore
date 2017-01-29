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
            IQueryable <Annety.Product> products ;
            
            if (CategoryCode == 1)
            { bool b = true;
                var categories = db.Categories.Where(c => c.ParentCategory == b).ToList();
                foreach (var item in categories)
                {   
                    var prod = db.Product.Where(p => p.CategoryCode == item.CategoryCode);
                     //products.  Add((Product)prod);
                }
                
            }
            //db.Product.OrderByDescending(u => u.DateEntered).Take(30);
            //if (ViewBag.gender == "Boy")
            //    var products = db.Product.Where(p => p.SearchWords.Contains("Boy")).ToList();
            //else
            //    var products = db.Product.Where(p => p.SearchWords.Contains(ViewBag.gender)).ToList();
            //return View(products.ToList());
            return View();

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

            return View(products.ToList());


            }
        

    public ActionResult Product(int ProductKey)
    {
        var products = db.Product.Where(p => p.ProductKey == ProductKey);
        return View();


    }
}
}
