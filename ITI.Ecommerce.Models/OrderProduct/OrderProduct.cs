using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Ecommerce.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductRate { get; set; }
        public int Quantity { get; set; }
        //Navigation property
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
