using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Annety;
using System.IO;

namespace Annety.Controllers
{
    public class ProductsController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Categories);
            return View(product.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryCode = new SelectList(db.Categories, "CategoryCode", "LongName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductKey,Barcode,Image,ImagePath,Desc,CategoryCode,Price,SearchWords")] Product product)
        {
            if (ModelState.IsValid)
            {
                var ext = Path.GetExtension(product.Image.FileName);
                string imageName = product.Barcode.ToString();
                string myimage = imageName + ext;
                product.ImagePath = Path.Combine(Server.MapPath("~/ProductImages"), myimage);
                product.Image.SaveAs(product.ImagePath);
                product.ChangeDate = DateTime.Now;
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryCode = new SelectList(db.Categories, "CategoryCode", "LongName", product.CategoryCode);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryCode = new SelectList(db.Categories, "CategoryCode", "Desc", product.CategoryCode);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductKey,Barcode,ImagePath,Desc,CategoryCode,Price,SearchWords")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryCode = new SelectList(db.Categories, "CategoryCode", "Desc", product.CategoryCode);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Item(int? id)
        {
            List<Colors> colorsl = new List<Colors>();
            Item item = new Annety.Item();
            int mone = 0;
            Product p = db.Product.SingleOrDefault(i=>i.ProductKey == id);

            //add to session
            if (Session["MyWatchList"] == null)
                Session["MyWatchList"] = new Stack<Product>();
            Stack<Product> f = (Stack<Product>)Session["MyWatchList"];
            foreach (var x in f)
            {
                if (p.ProductKey == x.ProductKey)
                    mone = 1;
            }
            if(mone==0)
            f.Push(p);
            Session["MyWatchList"] = f;
             
            IEnumerable<ProductSize> l = from s in db.ProductSize
                                  where s.Stocks.Any(pr => pr.ProductKey == p.ProductKey)
                                  select s;
            ViewBag.ProductSize = new SelectList(l, "CodeSize", "SizeDesc",item);
            item.DisProd = p;
            item.UMayLike = this.MayLike(p.Desc);
            //get product colors list
            colorsl = this.GetColorsList(item.DisProd);
            ViewBag.ProductColors = new SelectList(colorsl, "CodeColor", "ColorName",item);

            return View(item);
        }
        private List<Colors> GetColorsList( Product p)
        {
            List<Colors> colorsl = new List<Colors>();
            List<Colors> Finalcl = new List<Colors>();
            IQueryable<Stocks> stackl = db.Stocks.Where(s=>s.ProductKey == p.ProductKey);
            foreach (var pr in stackl)
            { 
                Colors c = db.Colors.SingleOrDefault(i => i.CodeColor == pr.ProdColorKey);
                colorsl.Add(c);
            }

            foreach (var item in colorsl)
            {
                if (Finalcl.Exists(x => x.CodeColor == item.CodeColor) == false)
                    Finalcl.Add(item);
            }
            return Finalcl;
        }
        private List<Product> MayLike(string Desc)
        {
            List<Product> pl = new List<Annety.Product>();
            List<Product> fpl = new List<Annety.Product>();

            var products = db.Product.ToList();
            //מורה לפונקציה באיזה סימנים עליה לחלק את המערך מילים שקיבלה 
            char[] delimiterChars = { ' ', ',', ':', '.' };
            String[] splited = Desc.Split(delimiterChars);
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
            }

            for (int i = 1; i <5 && i< pl.Count(); i++)
            {
               
                fpl.Add(pl[i]);
            }
            return fpl;
        }

        public ActionResult WatchList( )
        {
            //get from to session
            if (Session["MyWatchList"] != null)
            {
                List<Product> l = ((Stack<Product>)Session["MyWatchList"]).ToList().Take(10).ToList();

                return View(l);
            }
            return View( );
        }
      
        public ActionResult GoToPayment(int? Price )
        {
            return View("../Index", Price);
        }

        public void AddToCart(int Product, int Size, int Quantity, int Color)
        {
            int mone = 1;
            if (Session["MyCart"] == null)
                Session["MyCart"] = new Stack<ItemInCart>();
            Stack<ItemInCart> f = (Stack<ItemInCart>)Session["MyCart"];
            Product p = db.Product.First(pr => pr.ProductKey == Product);
            ItemInCart item = new ItemInCart();
            item.Product = p;
            ProductSize size = db.ProductSize.First(s => s.CodeSize == Size);
            item.SizeDesc = size.SizeDesc;
            Colors color = db.Colors.First(c => c.CodeColor == Color);
            item.ColorName = color.ColorName;
            item.Units = Quantity;
            List<ItemInCart> l = ((Stack<ItemInCart>)Session["MyCart"]).ToList();
           
           

            

            foreach (ItemInCart o in l)
            {
                if (o.Product.Barcode==item.Product.Barcode)
                {
                    mone = 0;
                    //To display a warning use
                    //this.TempData["Notification"] = "This item is already exist in your cart.";
                    //this.TempData["NotificationCSS"] = "notificationbox nb-warning";
                }
            }
            if (mone==1)
            {
                f.Push(item);
                //this.TempData["Notification"] = "This item has been added to your cart successfully.";
                //this.TempData["NotificationCSS"] = "notificationbox nb-success";


            }

            Session["MyCart"] = f;
            //return RedirectToAction("Index").SetStatusMessage("This item has been added successfuly to your cart" );

        }
    }
}
