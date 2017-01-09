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
    public class OrdersDetailsController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();

        // GET: OrdersDetails
        public ActionResult Index()
        {
            var ordersDetails = db.OrdersDetails.Include(o => o.Stocks).Include(o => o.UsersOrders);
            return View(ordersDetails.ToList());
        }

        // GET: OrdersDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdersDetails ordersDetails = db.OrdersDetails.Find(id);
            if (ordersDetails == null)
            {
                return HttpNotFound();
            }
            return View(ordersDetails);
        }

        // GET: OrdersDetails/Create
        public ActionResult Create()
        {
            ViewBag.StockCode = new SelectList(db.Stocks, "StockCode", "StockCode");
            ViewBag.OrderCode = new SelectList(db.UsersOrders, "OrderCode", "OrderCode");
            return View();
        }

        // POST: OrdersDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,OrderCode,StockCode,Quantity")] OrdersDetails ordersDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrdersDetails.Add(ordersDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StockCode = new SelectList(db.Stocks, "StockCode", "StockCode", ordersDetails.StockCode);
            ViewBag.OrderCode = new SelectList(db.UsersOrders, "OrderCode", "OrderCode", ordersDetails.OrderCode);
            return View(ordersDetails);
        }

        // GET: OrdersDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdersDetails ordersDetails = db.OrdersDetails.Find(id);
            if (ordersDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockCode = new SelectList(db.Stocks, "StockCode", "StockCode", ordersDetails.StockCode);
            ViewBag.OrderCode = new SelectList(db.UsersOrders, "OrderCode", "OrderCode", ordersDetails.OrderCode);
            return View(ordersDetails);
        }

        // POST: OrdersDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,OrderCode,StockCode,Quantity")] OrdersDetails ordersDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordersDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StockCode = new SelectList(db.Stocks, "StockCode", "StockCode", ordersDetails.StockCode);
            ViewBag.OrderCode = new SelectList(db.UsersOrders, "OrderCode", "OrderCode", ordersDetails.OrderCode);
            return View(ordersDetails);
        }

        // GET: OrdersDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdersDetails ordersDetails = db.OrdersDetails.Find(id);
            if (ordersDetails == null)
            {
                return HttpNotFound();
            }
            return View(ordersDetails);
        }

        // POST: OrdersDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdersDetails ordersDetails = db.OrdersDetails.Find(id);
            db.OrdersDetails.Remove(ordersDetails);
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
