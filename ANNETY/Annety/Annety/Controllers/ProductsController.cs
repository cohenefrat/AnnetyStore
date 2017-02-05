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
            int mone = 0;
            Product p = db.Product.SingleOrDefault(i=>i.ProductKey == id);

            //add to session
            if (Session["kk"] == null)
                Session["kk"] = new Stack<Product>();
            Stack<Product> f = (Stack<Product>)Session["kk"];
            foreach (var x in f)
            {
                if (p.ProductKey == x.ProductKey)
                    mone = 1;
            }
            if(mone==0)
            f.Push(p);
            Session["kk"] = f;
           
            return View(p);
        }

        public ActionResult WatchList( )
        {
            //get from to session
            if (Session["kk"] != null)
            {
                List<Product> l = ((Stack<Product>)Session["kk"]).ToList();
                return View(l.Take(10));
            }
            return View( );
        }

    }
}
