using DTOs;
using ITI.Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Ecommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductImageService _productImageService;
        public ProductService(ApplicationDbContext context, IProductImageService productImageService)
        {
            _context = context;
            _productImageService = productImageService; 
        }

        public async Task add(ProductDto productDto)
        {
            
            Product product = new Product()
            {

                NameAR = productDto.NameAR,
                NameEN = productDto.NameEN,
                Description = productDto.Description,
                CategoryID = productDto.CategoryID,
                Quantity = productDto.Quantity,
                UnitPrice = productDto.UnitPrice,
                Discount = productDto.Discount,
                TotalPrice = productDto.TotalPrice,
                Brand = productDto.Brand,
            };
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            foreach (var img in productDto.ProductImageList)
            {
                img.ProductID = product.ID;
               await _productImageService.add(img);
            }
         
            _context.SaveChanges();
        }

        public void Delete(int product)
        {
            var Product = _context.Products.First(p => p.ID == product);
            Product.IsDeleted = true;
            _context.SaveChanges();

        }

        public async Task<IEnumerable<ProductDto>> FiletrProductByName(string name)
        {
            List<ProductDto> productDtoList = new List<ProductDto>();

            var products = await _context.Products.Where(p => p.IsDeleted == false && p.NameEN.Contains(name)).ToListAsync();
            foreach (var product in products)
            {
                ProductDto productDto = new ProductDto()
                {
                    ID = product.ID,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    TotalPrice = product.TotalPrice,
                    Brand = product.Brand,

                };
                var productImgList = await _context.ProductImages.Where(i => i.ProductID == product.ID).ToListAsync();
                var ProductImgDtoList = new List<ProductImageDto>();
                foreach (var img in productImgList)
                {
                    var imgDto = new ProductImageDto()
                    {
                        ID = img.ID,
                        Path = img.Path,
                        ProductID = img.ProductID,
                    };
                    ProductImgDtoList.Add(imgDto);
                }
                productDto.ProductImageList = ProductImgDtoList;
                productDtoList.Add(productDto);


            }

            return productDtoList;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            List<ProductDto> productDtoList = new List<ProductDto>();

            var products = await _context.Products.Where(p => p.IsDeleted == false).Distinct().ToListAsync();

            foreach (var product in products)
            {
                ProductDto productDto = new ProductDto()
                {
                    ID = product.ID,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    TotalPrice = product.TotalPrice,
                    Brand = product.Brand,

                };
                var productImgList = await _context.ProductImages.Where(i => i.ProductID == product.ID).ToListAsync();
                var ProductImgDtoList = new List<ProductImageDto>();
                foreach (var img in productImgList)
                {
                    var imgDto = new ProductImageDto()
                    {
                        ID = img.ID,
                        Path = img.Path,
                        ProductID = img.ProductID,
                    };
                    ProductImgDtoList.Add(imgDto);
                }
                productDto.ProductImageList = ProductImgDtoList;
                productDtoList.Add(productDto);


            }

            return productDtoList;
        }


        public async Task<IEnumerable<ProductDto>> GetAllDleted()
        {
            List<ProductDto> productDtoList = new List<ProductDto>();

            var products = await _context.Products.Where(p => p.IsDeleted == true).ToListAsync();

            foreach (var product in products)
            {
                ProductDto productDto = new ProductDto()
                {
                    ID = product.ID,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    TotalPrice = product.TotalPrice,
                    Brand = product.Brand,

                };
                var productImgList = await _context.ProductImages.Where(i => i.ProductID == product.ID).ToListAsync();
                var ProductImgDtoList = new List<ProductImageDto>();
                foreach (var img in productImgList)
                {
                    var imgDto = new ProductImageDto()
                    {
                        ID = img.ID,
                        Path = img.Path,
                        ProductID = img.ProductID,
                    };
                    ProductImgDtoList.Add(imgDto);
                }
                productDto.ProductImageList = ProductImgDtoList;
                productDtoList.Add(productDto);

            }

            return productDtoList;
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryId(int id)
        {
            List<ProductDto> productDtoList = new List<ProductDto>();

            var products = await _context.Products.Where(p => p.IsDeleted == false && p.CategoryID == id).Distinct().ToListAsync();

            foreach (var product in products)
            {
                ProductDto productDto = new ProductDto()
                {
                    ID = product.ID,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    TotalPrice = product.TotalPrice,
                    Brand = product.Brand,
                   



                };
                var productImgList = await _context.ProductImages.Where(i => i.ProductID == product.ID).ToListAsync();
                var ProductImgDtoList = new List<ProductImageDto>();
                foreach (var img in productImgList)
                {
                    var imgDto = new ProductImageDto()
                    {
                        ID = img.ID,
                        Path = img.Path,
                        ProductID = img.ProductID,
                    };
                    ProductImgDtoList.Add(imgDto);
                }
                productDto.ProductImageList = ProductImgDtoList;
                productDtoList.Add(productDto);

            }

            return productDtoList;
        }
        public async Task<ProductDto> GetById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id && p.IsDeleted == false);
            if (product == null)
            {
                return null;
            }
            else
            {

                ProductDto productDto = new ProductDto()
                {
                    ID = product.ID,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    TotalPrice = product.TotalPrice,
                    
                    Brand = product.Brand,

                };

                var productImgList = await _context.ProductImages.Where(i => i.ProductID == product.ID).ToListAsync();
                var ProductImgDtoList = new List<ProductImageDto>();
                foreach (var img in productImgList)
                {
                    var imgDto = new ProductImageDto()
                    {
                        ID = img.ID,
                        Path = img.Path,
                        ProductID = img.ProductID,
                    };
                    ProductImgDtoList.Add(imgDto);
                }
                productDto.ProductImageList = ProductImgDtoList;


                return productDto;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetByPrice(float price)
        {

            List<ProductDto> productDtoList = new List<ProductDto>();

            var products = await _context.Products.Where(p => p.IsDeleted == false && p.TotalPrice < price).ToListAsync();

            foreach (var product in products)
            {
                ProductDto productDto = new ProductDto()
                {
                    ID = product.ID,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Discount = product.Discount,
                    TotalPrice = product.TotalPrice,
                    Brand = product.Brand,

                };
                var productImgList = await _context.ProductImages.Where(i => i.ProductID == product.ID).ToListAsync();
                var ProductImgDtoList = new List<ProductImageDto>();
                foreach (var img in productImgList)
                {
                    var imgDto = new ProductImageDto()
                    {
                        ID = img.ID,
                        Path = img.Path,
                        ProductID = img.ProductID,
                    };
                    ProductImgDtoList.Add(imgDto);
                }
                productDto.ProductImageList = ProductImgDtoList;
                productDtoList.Add(productDto);


            }

            return productDtoList;
        }

        public void Restore(int pro)
        {
            var Product = _context.Products.First(p => p.ID == pro);
            Product.IsDeleted = false;
            _context.SaveChanges();
        }

        public void Update(ProductDto productDto)
        {
            var product = _context.Products.FirstOrDefault(p => p.ID == productDto.ID);

            if (product != null)
            {
                product.NameAR = productDto.NameAR;
                product.NameEN = productDto.NameEN;
                product.Description = productDto.Description;
                product.CategoryID = productDto.CategoryID;
                product.Quantity = productDto.Quantity;
                product.UnitPrice = productDto.UnitPrice;
                product.Discount = productDto.Discount;
                product.TotalPrice = productDto.UnitPrice - ((productDto.Discount / 100) * productDto.UnitPrice);
                product.IsDeleted = false;
                product.Brand = productDto.Brand;
               
            }

            var ProductImgDtoList = new List<ProductImage>();
            foreach (var img in productDto.ProductImageList)
            {
                var imgDto = new ProductImage()
                {
                    ID = img.ID,
                    Path = img.Path,
                    ProductID = img.ProductID,
                };
                ProductImgDtoList.Add(imgDto);
            }
            product.productImageList = ProductImgDtoList;
            _context.SaveChanges();

           

        }
    }
}
