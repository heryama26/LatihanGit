using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Report
    {
        public string NO { get; set; }
        public string AccountId { get; set; }
        public string StarDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public string TransactionDate { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string Amount { get; set; }
    }
}