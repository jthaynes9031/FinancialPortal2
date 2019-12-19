using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinacialPortal2.Extensions;
using FinacialPortal2.Helpers;
using FinacialPortal2.Models;
using Microsoft.AspNet.Identity;
using static FinacialPortal2.Models.BankAccountWizard;

namespace FinacialPortal2.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

       
        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                household.OwnerUserId = User.Identity.GetUserId();
                household.Created = DateTime.Now;
                roleHelper.AddUserToRole(user.Id, "HeadOfHouse");
                user.Household = household;
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> LeaveAsync()
        {
            var userId = User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var user = db.Users.Find(userId);

            switch (myRole)
            {
                case "Head Of Household":

                    var inhabitants = db.Users.Where(u => u.HouseholdId == user.HouseholdId).Count();
                    if (inhabitants > 1)
                    {
                        TempData["Message"] = $"You are unable to leave the Household at this time as there are still <b>{inhabitants} members in this house";
                        return RedirectToAction("ExitDenied");
                    }
                    user.HouseholdId = null;
                    db.SaveChanges();

                    roleHelper.RemoveUserFromRole(userId, "Head Of Household");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);

                    return RedirectToAction("Dashboard", "Home");

                case "Member":
                default:
                    user.HouseholdId = null;
                    db.SaveChanges();

                    roleHelper.RemoveUserFromRole(userId, "Member");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);

                    return RedirectToAction("Dashboard", "Home");

            }
        }

        public ActionResult ExitDenied()
        {
            return View();
        }

        public ActionResult AppointSuccessor()
        {
            var userId = User.Identity.GetUserId();
            var myHouseholdId = db.Users.Find(userId).HouseholdId ?? 0;

            if (myHouseholdId == 0)
                return RedirectToAction("Dashboard");

            var members = db.Users.Where(u => u.HouseholdId == myHouseholdId && u.Id != userId);
            ViewBag.NewHoh = new SelectList(members, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointSuccessorAsync(string newHoh)
        {
            if (string.IsNullOrEmpty(newHoh))
                return RedirectToAction("Dashboard", "Home");
            var me = db.Users.Find(User.Identity.GetUserId());
            me.Household = null;
            db.SaveChanges();

            roleHelper.RemoveUserFromRole(me.Id, "Head Of Household");
            await ControllerContext.HttpContext.RefreshAuthentication(me);

            roleHelper.RemoveUserFromRole(newHoh, "Member");
            roleHelper.AddUserToRole(newHoh, "Head Of Household");

            return RedirectToAction("Index", "Home");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AppointSuccessorAsync(string newHoh)

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
