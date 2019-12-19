using FinacialPortal2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinacialPortal2.Helpers;
using Microsoft.AspNet.Identity;

namespace FinacialPortal2.Controllers
{
   
    public class HomeController : Controller
    { 
        private RoleHelper roleHelper = new RoleHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var users = db.Users;
            var user = users.Find(User.Identity.GetUserId());
            var newVm = new Dashboard();
            newVm.myHouseholds = db.Households.Where(h => h.Id == user.HouseholdId).ToList() ;
            return View(newVm);
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
    }
}