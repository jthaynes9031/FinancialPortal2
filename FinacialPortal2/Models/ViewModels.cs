using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinacialPortal2.Models
{
    public class BankAccountWizard
    {
        public BankAccount BankAccount { get; set; }
        public Budget Budget { get; set; }
        public BudgetItem BudgetItem { get; set; }

        public BankAccountWizard()
        {
            BankAccount = new BankAccount();
            Budget = new Budget();
            BudgetItem = new BudgetItem();
        }
    }


}