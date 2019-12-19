using FinacialPortal2.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinacialPortal2.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public string OwnerId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public virtual TransactionType AccountType { get; set; }
        public float StartingBalance { get; set; }
        public float lowLevelBalance { get; set; }
        public float CurrentBalance { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public BankAccount()
        {
            Transactions = new HashSet<Transaction>(); 
        }

    }
}