﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinacialPortal2.Models;
using Microsoft.AspNet.Identity;

namespace FinacialPortal2.Controllers
{
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BankAccounts
        public ActionResult Index()
        {
            var bankAccounts = db.BankAccounts.Include(b => b.Household).Include(b => b.Owner);
            return View(bankAccounts.ToList());
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.AccountType = new SelectList(db.BankAccounts);
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseHoldId,OwnerId,Created,Name,AccountType,StartingBalance,CurrentBalance")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bankAccount.AccountType = bankAccount.AccountType;
                bankAccount.StartingBalance = bankAccount.StartingBalance;
                bankAccount.CurrentBalance = bankAccount.CurrentBalance;
                bankAccount.lowLevelBalance = ((float)bankAccount.CurrentBalance / 2);
                var user = db.Users.Find(User.Identity.GetUserId());
                bankAccount.OwnerId = User.Identity.GetUserId();
                bankAccount.Created = DateTime.Now;
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseHoldId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", bankAccount.OwnerId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseHoldId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", bankAccount.OwnerId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,OwnerId,Created,Name,AccountType,StartingBalance,CurrentBalance")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseHoldId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", bankAccount.OwnerId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
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
