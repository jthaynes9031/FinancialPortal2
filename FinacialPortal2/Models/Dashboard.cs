using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinacialPortal2.Models
{
    public class Dashboard
    {
        public virtual ICollection<BankAccount> myBankAccounts { get; set; }
        public virtual ICollection<Household> myHouseholds { get; set; }
        public virtual ICollection<Budget> myBudgets { get; set; }
        public virtual ICollection<BudgetItem> myBudgetItems { get; set; }
        public virtual ICollection<Transaction> myTransactions { get; set; }

        public Dashboard()
        {
            myTransactions = new HashSet<Transaction>();
            myHouseholds = new HashSet<Household>();
            myBankAccounts = new HashSet<BankAccount>();
            myBudgets = new HashSet<Budget>();
            myBudgetItems = new HashSet<BudgetItem>();
        }
    }
}