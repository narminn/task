using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task.Models;

namespace Task.Controllers
{
    public class employeesController : Controller
    {
        private TaskEntities db = new TaskEntities();

        // GET: employees
        public ActionResult Index()
        {
            return View(db.employee.ToList());
        }

        // GET: employees/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,surname,age,jobtitle,home_city,experience,photo")] employee employee,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                var randomName = random.Next(1, 1000000000);
                if (photo.ContentType != "image/png" && photo.ContentType != "image/jpg" && photo.ContentType != "image/jpeg")
                {
                    Session["Error"] = "You can upload png, jpg and jpeg files";
                    return RedirectToAction("create", "employees");
                }
                if (photo.ContentLength > 1048576)
                {
                    Session["Error"] = "Your file can be max 1 MB";
                    return RedirectToAction("create", "employees");
                }
                var nameMainImage = Path.GetFileName(photo.FileName);
                var srcMain = Path.Combine(Server.MapPath("~/Uploads/"), randomName + nameMainImage);
                photo.SaveAs(srcMain);
                employee.photo = "/Uploads/" + randomName + nameMainImage;
                db.employee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }
        
        
        // GET: employees/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            employee employee = db.employee.Find(id);
            db.employee.Remove(employee);
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
