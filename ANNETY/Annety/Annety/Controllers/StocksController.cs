using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Annety;

namespace Annety.Controllers
{
    public class StocksController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();

        // GET: Stocks
        public ActionResult Index()
        {
            var stocks = db.Stocks.Include(s => s.Colors).Include(s => s.Product).Include(s => s.ProductSize);
            return View(stocks.ToList());
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stocks stocks = db.Stocks.Find(id);
            if (stocks == null)
            {
                return HttpNotFound();
            }
            return View(stocks);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            ViewBag.ProdColorKey = new SelectList(db.Colors, "CodeColor", "ColorName");
            ViewBag.ProductKey = new SelectList(db.Product, "ProductKey", "Barcode");
            ViewBag.CodeSize = new SelectList(db.ProductSize, "CodeSize", "SizeDesc");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockCode,ProductKey,ProdColorKey,UnitsInStocks,CodeSize")] Stocks stocks)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stocks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProdColorKey = new SelectList(db.Colors, "CodeColor", "ColorName", stocks.ProdColorKey);
            ViewBag.ProductKey = new SelectList(db.Product, "ProductKey", "Barcode", stocks.ProductKey);
            ViewBag.CodeSize = new SelectList(db.ProductSize, "CodeSize", "SizeDesc", stocks.CodeSize);
            return View(stocks);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stocks stocks = db.Stocks.Find(id);
            if (stocks == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProdColorKey = new SelectList(db.Colors, "CodeColor", "ColorName", stocks.ProdColorKey);
            ViewBag.ProductKey = new SelectList(db.Product, "ProductKey", "Barcode", stocks.ProductKey);
            ViewBag.CodeSize = new SelectList(db.ProductSize, "CodeSize", "SizeDesc", stocks.CodeSize);
            return View(stocks);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockCode,ProductKey,ProdColorKey,UnitsInStocks,CodeSize")] Stocks stocks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stocks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProdColorKey = new SelectList(db.Colors, "CodeColor", "ColorName", stocks.ProdColorKey);
            ViewBag.ProductKey = new SelectList(db.Product, "ProductKey", "Barcode", stocks.ProductKey);
            ViewBag.CodeSize = new SelectList(db.ProductSize, "CodeSize", "SizeDesc", stocks.CodeSize);
            return View(stocks);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stocks stocks = db.Stocks.Find(id);
            if (stocks == null)
            {
                return HttpNotFound();
            }
            return View(stocks);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stocks stocks = db.Stocks.Find(id);
            db.Stocks.Remove(stocks);
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
    }
}
