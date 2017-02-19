﻿using System;
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
            //במקרה שיוזר הקיש מילות חיפוש שלא קיימות בכלל במאגר מילות החיפוש של המוצרים - הוא יובל לדף קולקציה חדשה
            else {
                return RedirectToAction("NewArrival");
                 }
    
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

    }
}
