using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;

namespace ITI.Ecommerce.Models
{
    public static class RelationshipsMapping
    {
        public static void MapRelationships(this ModelBuilder builder)
        {
            //Relation between category and Product One To Many

            builder.Entity<Product>().HasOne(p => p.Category)
                .WithMany(c => c.ProductList).HasForeignKey(p => p.CategoryID);

            //Relation between Product and ProductImage One To Many

            builder.Entity<ProductImage>().HasOne(i => i.Product)
                .WithMany(p => p.productImageList).HasForeignKey(p => p.ProductID);


            //Relation between Customer and Order One To Many

            builder.Entity<Order>().HasOne(o=>o.customer)
                .WithMany(c=> c.orderList).HasForeignKey(o=>o.CustomerId);


            //Relation between Product and Order Many To Many

            builder.Entity<OrderProduct>().HasOne<Order>(OP=>OP.Order)
               .WithMany(O=>O.OrderProducts).HasForeignKey(OP=>OP.OrderId);


            builder.Entity<OrderProduct>().HasOne<Product>(OP => OP.Product)
               .WithMany(O => O.OrderProducts).HasForeignKey(OP => OP.ProductId);
        }
    }
}
