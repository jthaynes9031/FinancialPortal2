using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinacialPortal2.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string IsValid { get; set; }
        public DateTime Created { get; set; }
        public int TTL { get; set; }
        public string RecipientEmail { get; set; }
        public string Code { get; set; }
        public virtual Household Household { get; set; }
    }
}