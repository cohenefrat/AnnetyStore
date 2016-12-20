using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annety.Controllers
{
    public class SearchResultController : Controller
    {
        // GET: SearchResult
        public ActionResult Index()
        {
            return View("SearchResult");
        }

        public ActionResult FromMenu(int CategoryCode)
        {
            return RedirectToAction("Index");
        }

        // GET: SearchResult/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchResult/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchResult/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchResult/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchResult/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchResult/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchResult/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
