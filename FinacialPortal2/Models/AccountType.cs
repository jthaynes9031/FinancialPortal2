using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinacialPortal2.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; }

        public AccountType()
        {
            this.BankAccounts = new HashSet<BankAccount>();
        }
    }
}