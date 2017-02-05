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

        public ActionResult FromMenu(int? CategoryCode)
        {
            List<Product> products;
            if (CategoryCode.HasValue )
                products = db.Product.Where(p => p.CategoryCode == CategoryCode).ToList();
            else
                products = db.Product.ToList();
            return View(products );
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
            List<Product> pl = new List<Annety.Product>();

            var products = db.Product.ToList();
            //string a = "Dress";
            char[] delimiterChars = { ' ', ',', ':', '.' };
            String[] splited = Search_Box.Split(delimiterChars);
            int[] array = new int[products.Count];
            foreach (string s in splited)
            {
                db.Product.Where(u => u.Desc.Contains(s)).ToList().ForEach(p => array[products.IndexOf(p)]++);
            }
            var max = array.Max();
            if (max > 0)
                for (int j = max; j > 0; j--)
                { for (int i = 0; i < array.Length; i++)
                    {
                        if(array[i]==max)
                        { 
                        var prod = products[i];
                        pl.Add(prod);
                        }
                    }
                    max--;
                }
            return View("FromMenu", pl.ToList());

        }


        public ActionResult Product(int ProductKey)
        {
            Product products = db.Product.Where(p => p.ProductKey == ProductKey).SingleOrDefault() ;
            
            return View();
        }

        //public ActionResult ProductDetails(int? ProductKey)
        //{
        //    Product product = db.Product.Where(p => p.ProductKey == ProductKey).SingleOrDefault();

        //    if (Session["kk"] == null)
        //        Session["kk"] = new Stack<Product>();

        //    Stack<Product> f = (Stack<Product>)Session["kk"];
        //    f.Push(product);
        //    Session["kk"] = f;
             
        //  return   RedirectToAction("../Products/Item" , product);
        //}

        public ActionResult WatchList()
        {
            if (Session["kk"] == null)
                Session["kk"] = new Stack<Product>();

            Stack<Product> f = (Stack<Product>)Session["kk"];

            return View("../SearchResult/FromMenu", f.ToList());
            
        }
    }
}
