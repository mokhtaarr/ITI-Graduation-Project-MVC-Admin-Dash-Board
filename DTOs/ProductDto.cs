using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Ecommerce.Models;

namespace DTOs
{
    public class ProductDto
    {
        public int ID { get; set; }
       
        public string NameAR { get; set; }
       
        public string NameEN { get; set; }
       
        public string Brand { get; set; }
      
        public string Description { get; set; }
        
        public int CategoryID { get; set; }

        
        public int Quantity { get; set; }
        
        public float UnitPrice { get; set; }
      
        public float Discount { get; set; }
     
        public float TotalPrice { get; set; }
    
        
        public ICollection<ProductImageDto> ProductImageList { get; set; }
       
    }
}

