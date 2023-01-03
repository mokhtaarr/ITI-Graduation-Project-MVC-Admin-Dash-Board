using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Ecommerce.Models
{
    

        public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
        {
            public void Configure(EntityTypeBuilder<OrderProduct> builder)
            {
                
                builder.HasKey(OP=>new { OP.OrderId, OP.ProductId });
                

            }
        }
    
}
