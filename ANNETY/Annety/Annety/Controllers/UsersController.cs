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
    public class UsersController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,UserName,Email,Password,Address,Phone")] Users users)
        {
            if (ModelState.IsValid)
            {   //users.Password
                users.Password = AccountController.HashPass(users.Password);
                db.Users.Add(users);
                Session["UserDetails"] = users.UserName;
                Session["UserCode"] = users.Code;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            Session["User"] = users;
            return View(users);
        }

        

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,UserName,Email,Password,Address,Phone")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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

        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName_login, string Password_login)
        {
            Users u = new Users();
             
            //Session["EmailLogin"] = Email;
            //Session["PassLogin"] = password;
            Password_login = AccountController.HashPass(Password_login);
            //var em = Session["EmailLogin"].ToString();
            u = db.Users.FirstOrDefault (i => i.Email == UserName_login );
            if (u != null && u.UserName!=string.Empty )
                if (u.Password.Trim() == Password_login)
                {
                    Session["UserDetails"] = u.UserName;
                    Session["UserCode"] = u.Code;
                    Session["User"] = u;
                    return View("../Home/Index");
                }
                else
                    //add error message for user theat insert incorrect password or email

                    return View();
            else
                return View();
        }
        public JsonResult IsUserExist(string UserName)
        {
            return IsExist1(UserName) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.DenyGet);
        }

        public bool IsExist1(string UserName)
        {
            bool u = db.Users.Any(i => i.UserName == UserName);
            if (u)
                return true;
            else
            return false;

        }
    }
}
