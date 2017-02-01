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
            string a = Search_Box;
            //הגדרת הסימנים- בכל פעם שיופיע במילות החיפוש שהמשתמש הכניס אחד מהתווים הללו- המילה תתחיל משורה אחרת במערך
            //כלומר הוא יחלק את המערך למילים לפי הסימנים הללו
            char[] delimiterChars = { ' ', ',', ':', '.' };
            String[] splited = a.Split(delimiterChars);
            //יצירת מערך מונים שימנה כמה ממילות החיפוש הופיעו בתאור עבור כל מוצר
            int[] array = new int[products.Count() ];
            //כאן נעבור על המוצרים הנמצאים במבנה הנתונים שלנו
            //כאשר עבור כל מילה הנמצאת בתאור המוצר- נוסיף כביכול נקודת זכות למוצר
            //בסיום- המוצר בעל המס' הרב ביותר של נקודות זכות יוצג בהתחלה
            //וכך הולך ופוחת עד אלו שיש להם נקודת זכות אחת בלבד
            //יש לציין כי מוצרים- אשר להם 0 נקודות זכות לא יוצגו בדף התוצאות
            //ואם לא היו מוצרים התואמים את מילות החיפוש של המשתמש כלל
            //יוצר דף קולקציה חדשה למשתמש
            foreach (string s in  splited)
            {
                products.Where(u=>u.Desc.Contains(s)).ToList().ForEach(p=>array[products.IndexOf(p)]++);
                //foreach (var item in products)
                //{
                //    if (item.Desc.Contains(s))
                //        array[products.IndexOf(item)]++;
                //}
            }
            //
            //if (Session["user"]==null )
            //    Session["user"] = new Stack <Product>();

            //Stack <Product> f = (Stack <Product>)Session["kk"];
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
