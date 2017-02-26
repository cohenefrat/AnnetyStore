using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        public ActionResult Sale()
        {
           // List<Product> pl = new List<Product>();
            var pl = db.Product.Where(p => p.SearchWords > 0);
            return View("FromMenu", pl);

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

            return View("FromMenu", pl);

        }

        //  add another parameter which symbol the source
       // [HttpGet]
        public ActionResult Search_Box(String Search_Box)
        {
            List<Product> pl = new List<Annety.Product>();

            var products = db.Product.ToList();
            //מורה לפונקציה באיזה סימנים עליה לחלק את המערך מילים שקיבלה 
            char[] delimiterChars = { ' ', ',', ':', '.' };
            String[] splited = Search_Box.Split(delimiterChars);
            //מערך מונים - לדעת כמה מילים מופיעות בכל תיאור מוצר
            int[] array = new int[products.Count];
            foreach (string s in splited)
            {//עובר על כל המוצרים ןמחפש את אחת המילים ממילות חיפוש - כל מילה שנמצאת בתיאור המוצר - יעלה ערכו ב1
                db.Product.Where(u => u.Desc.Contains(s)).ToList().ForEach(p => array[products.IndexOf(p)]++);
            }
            //מציאת המילה בה נמצאה הכמות הגדולה ביותר ממילות החיפוש
            var max = array.Max();
            //במקרה שנמצאה לפחות מילה אחת ממילות החיפוש באחד מהמוצרים שבחנות
            if (max > 0)
            {
                //הצגת המוצרים בסדר מהגבוה לנמוך - קודם יוצר המוצר בו היו הכי הרבה מילים ממילות החיפוש - והולך פוחת
                for (int j = max; j > 0; j--)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i] == max)
                        {
                            var prod = products[i];
                            pl.Add(prod);
                        }
                    }
                    max--;
                }
                return View("FromMenu", pl);
            }

            return RedirectToAction("NewArrival");
        }

        public ActionResult Product(int ProductKey)
        {
            Product products = db.Product.Where(p => p.ProductKey == ProductKey).SingleOrDefault() ;
            
            return View();
        }

        //public ActionResult ProductDetails(int? ProductKey)
        //{
        //    Product product = db.Product.Where(p => p.ProductKey == ProductKey).SingleOrDefault();

        //    if (Session["MyWatchList"] == null)
        //        Session["MyWatchList"] = new Stack<Product>();

        //    Stack<Product> f = (Stack<Product>)Session["MyWatchList"];
        //    f.Push(product);
        //    Session["MyWatchList"] = f;

        //  return   RedirectToAction("../Products/Item" , product);
        //}

        public ActionResult WatchList()
        {
            if (Session["MyWatchList"] == null)
                Session["MyWatchList"] = new Stack<Product>();

            Stack<Product> f = (Stack<Product>)Session["MyWatchList"];

            return View("../SearchResult/FromMenu", f.ToList().Take(10));
            
        }

       public ActionResult DisplayProducts(List<Product> p)
        {
            return View("FromMenu", p.ToList());
        }



        public ActionResult NewArrival()
        {
            List<Product> products;
           //הצגת המוצרים שהוכנסו לאתר בחודש האחרון
                products = db.Product.Where(p => p.ChangeDate.Month == DateTime.Now.Month).ToList();
          
                
            return View("FromMenu",products);
        }
        // [HttpGet]
        //public ActionResult Payment(ItemInCart item)
        //{
        //    Session["ForPay"] = item;
        //   // return View("PaymentPage", item);
        //   return File("../index.html", "text/html");
        
        //}

        //[HttpPost]
        public ActionResult Payment()
        {
            ItemInCart item = (ItemInCart)Session["ItemForPayment"];
            ProductSize size;
            Colors color;
            Stocks st = new Stocks();

            //ItemInCart item = (ItemInCart)Session["ForPay"];
            //if payment success
            var stocks = db.Stocks.Where(s => s.ProductKey == item.Product.ProductKey);
            foreach (var i in stocks)
            {
                size = db.ProductSize.First(e => e.SizeDesc == item.SizeDesc);
                if (i.CodeSize == size.CodeSize)
                {
                    color = db.Colors.First(c => c.ColorName == item.ColorName);
                    if(i.ProdColorKey == color.CodeColor)
                     {
                        if (ModelState.IsValid)
                        {
                           
                            i.UnitsInStocks = i.UnitsInStocks - item.Units;
                            st = (Stocks)i;
                            db.Entry(st).State = EntityState.Modified;

                            
                        }
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

        public ActionResult BeforPayment(int? Product , int? Size , int? Quantity , int? Color)
        {
           Product p = db.Product.First(pr => pr.ProductKey == Product);
            ItemInCart item = new ItemInCart();
            item.Product = p;
            ProductSize size = db.ProductSize.First(s => s.CodeSize == Size);
            item.SizeDesc = size.SizeDesc;
            Colors color = db.Colors.First(c => c.CodeColor == Color);
            item.ColorName = color.ColorName;
            item.Units = (int)Quantity;
            item.Product.Price = item.Product.Price * item.Units;
            Session["ItemForPayment"] = item;
            return View(item);
        }


        public ActionResult DisplayMyCart()
        { List<ItemInCart> Items = new List<ItemInCart>();
            if(Session["MyCart"]!=null)
            {
                Stack<ItemInCart> MyCart = (Stack<ItemInCart>)Session["MyCart"];
                foreach (var item in MyCart)
                {
                    Items.Add((ItemInCart)item);
                }
              
            }
            return View("MyCart", Items);
        }

    }
}
