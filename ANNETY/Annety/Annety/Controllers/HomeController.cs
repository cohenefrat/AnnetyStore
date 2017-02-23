using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annety.Controllers
{
    public class HomeController : Controller
    {
        private AnnetyEntities db = new AnnetyEntities();
        
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult EditDetails()
        {
            Users user = new Users();
            int usercode;
            if (Session["UserCode"] != null)
            {
                usercode = (int)Session["UserCode"];
                user = db.Users.FirstOrDefault(u => u.Code == usercode);
                user.Password = null;
            }
            return View("UserDetails", user);
        }

        [HttpPost]
        public ActionResult EditDetails(Users user)
        {
            Users u = new Users();
            u = user;
            u.Password = AccountController.HashPass(user.Password);
            if (ModelState.IsValid)
            {   
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return View("UserDetails", u);
            }

            return View("UserDetails");
        }

    }
}