using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public PaymentChoice PaymentType { get; set; }
        public int CardNo { get; set; }
        public int CardControlNumber { get; set; }
    }
}
