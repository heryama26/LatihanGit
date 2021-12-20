using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Transaksi
    {
        public string NO { get; set; }
        public string AccountId { get; set; }
        public string TransactionDate { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string DebitCreditStatus { get; set; }
    }
}