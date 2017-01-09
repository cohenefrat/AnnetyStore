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
    public class UsersOrdersController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();

        // GET: UsersOrders
        public ActionResult Index()
        {
            var usersOrders = db.UsersOrders.Include(u => u.Users);
            return View(usersOrders.ToList());
        }

        // GET: UsersOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersOrders usersOrders = db.UsersOrders.Find(id);
            if (usersOrders == null)
            {
                return HttpNotFound();
            }
            return View(usersOrders);
        }

        // GET: UsersOrders/Create
        public ActionResult Create()
        {
            ViewBag.UserCode = new SelectList(db.Users, "Code", "UserName");
            return View();
        }

        // POST: UsersOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderCode,UserCode,OrderDate,Sent,Arrive")] UsersOrders usersOrders)
        {
            if (ModelState.IsValid)
            {
                db.UsersOrders.Add(usersOrders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserCode = new SelectList(db.Users, "Code", "UserName", usersOrders.UserCode);
            return View(usersOrders);
        }

        // GET: UsersOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersOrders usersOrders = db.UsersOrders.Find(id);
            if (usersOrders == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserCode = new SelectList(db.Users, "Code", "UserName", usersOrders.UserCode);
            return View(usersOrders);
        }

        // POST: UsersOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderCode,UserCode,OrderDate,Sent,Arrive")] UsersOrders usersOrders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usersOrders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserCode = new SelectList(db.Users, "Code", "UserName", usersOrders.UserCode);
            return View(usersOrders);
        }

        // GET: UsersOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersOrders usersOrders = db.UsersOrders.Find(id);
            if (usersOrders == null)
            {
                return HttpNotFound();
            }
            return View(usersOrders);
        }

        // POST: UsersOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsersOrders usersOrders = db.UsersOrders.Find(id);
            db.UsersOrders.Remove(usersOrders);
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
