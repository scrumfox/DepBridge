using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace depBridger.Controllers
{
    public class SecurityDemoController : Controller
    {
        // GET: SecurityDemoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SecurityDemoController/Details/5
        public ActionResult Details(int id)
        {
            string query = "select * from user where userid=" + id;
            return View(query);
        }

        // GET: SecurityDemoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SecurityDemoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SecurityDemoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SecurityDemoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SecurityDemoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SecurityDemoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
