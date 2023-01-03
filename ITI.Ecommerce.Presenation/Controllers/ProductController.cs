using Microsoft.AspNetCore.Mvc;
using DTOs;
using ITI.Ecommerce.Services;
using ITI.Ecommerce.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace ITI.Ecommerce.Presenation.Controllersss
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        public IProductService _productService;
        public IProductImageService _productImageService;
        public IConfiguration _iconfiguration;
        public ICategoryServie _categoryServie;


        public ProductController(IProductService productService, IProductImageService productImageService, IConfiguration iconfiguration, ICategoryServie categoryServie)
        {

            _productService = productService;
            _productImageService = productImageService;
            _iconfiguration = iconfiguration;
            _categoryServie = categoryServie;   
        }

        public IActionResult Index()
        {


            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {


            var categories = await _categoryServie.GetAll();
            ViewBag.Cat = categories.Select(i => new SelectListItem(i.NameEN, i.ID.ToString()));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductModel dto)

        {
            ICollection<ProductImageDto> images = new List<ProductImageDto>();
            foreach (IFormFile file in dto.Images)
            {
                string NewName = Guid.NewGuid().ToString() + file.FileName;

                ProductImageDto prodImg = new ProductImageDto()
                {
                    Path = NewName,
                    ProductID = dto.ID,
                    
                };
                images.Add(prodImg);
                FileStream fs = new FileStream(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    "Content", "images", "Product", NewName)
                    , FileMode.OpenOrCreate, FileAccess.ReadWrite);
                file.CopyTo(fs);
                fs.Position = 0;
            }
            ProductDto dtos = new ProductDto()
            {
                NameAR = dto.NameAR,
                NameEN = dto.NameEN,
                Brand = dto.Brand,
                Description = dto.Description,
                CategoryID = dto.CategoryID,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
               
                Discount = dto.Discount,
                TotalPrice = dto.UnitPrice - ((dto.Discount/100)* dto.UnitPrice),
                

            };
            dtos.ProductImageList = images;


            await _productService.add(dtos);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 3)
        {
            var Proudicts = await _productService.GetAll();
            var Page = Proudicts.ToPagedList(pageIndex, pageSize);
            var categories = await _categoryServie.GetAll();

            ViewBag.Cat = categories;
            return View(Page);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int pro)
        {
            var categories = await _categoryServie.GetAll();
            ViewBag.Cat = categories.Select(i => new SelectListItem(i.NameEN, i.ID.ToString()));
            var Prod = await _productService.GetById(pro);
            List<IFormFile> li = new List<IFormFile>();

           


            return View(Prod);
        }
        [HttpPost]
        public IActionResult UpdateValue(ProductDto pro)
        {
            _productService.Update(pro);

            return RedirectToAction("GetAll", "Product");
        }
        public IActionResult Delete(int prod)
        {

            _productService.Delete(prod);
            return RedirectToAction("GetAll", "Product");

        }
        [HttpGet]
        public async Task<IActionResult> GetProductByID(int id)
        {

            var c = await _productImageService.GetByProductId(id);

            var Prod = await _productService.GetById(id);

            
            foreach (var x in c)
            {
                ViewBag.path = x.Path;
            }

        



            return View(Prod);

     

        }
        [HttpPost]
        public IActionResult GetProductByCats(int id)
        {


            return RedirectToAction("GetProductByCat", new { IId = 1 });
        }
        [HttpGet]
       
        [HttpGet]
        public async Task<IActionResult> AddProductImages(int img)
        {
            var Pro = await _productService.GetAll();
            ViewBag.Pro = Pro.Select(i => new SelectListItem(i.NameAR, i.ID.ToString()));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProductImages(ProductIMGDTOS img)
        {

            string NewName = Guid.NewGuid().ToString() + img.Path.FileName;

            ProductImageDto prodImg = new ProductImageDto()
            {
                Path = NewName,
                ProductID = img.ProductID,
               
            };
            await _productImageService.add(prodImg);

            FileStream fs = new FileStream(
              Path.Combine(Directory.GetCurrentDirectory(),
               "Content", "Images", "Product", NewName)
              , FileMode.OpenOrCreate, FileAccess.ReadWrite);
            img.Path.CopyTo(fs);
            fs.Position = 0;


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetProductImages(int img)
        {
            var ProImage = await _productImageService.GetByProductId(img);
            return View(ProImage);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProductImages(int im, int pro)
        {
            _productImageService.Delete(im);
            var ProImage = await _productImageService.GetByProductId(im);

            return RedirectToAction("GetProductImages", new { img = pro });

        }
        [HttpGet]


        [HttpPost]
        public async Task<IActionResult> Filter(string fil, float price, int Cat, int pageIndex = 1, int pageSize = 10)
        {

            var categories = await _categoryServie.GetAll();
            ViewBag.cat = categories;
            if (fil != null)
            {
                var pro = await _productService.FiletrProductByName(fil);
                var Page = pro.ToPagedList(pageIndex, pageSize);
                return View("GetAll", Page);
            }
            else if (price != 0)
            {
                var pro = await _productService.GetByPrice(price);
                var Page = pro.ToPagedList(pageIndex, pageSize);
                return View("GetAll", Page);
            }
            else if (Cat != 0)
            {
                var pro = await _productService.GetByCategoryId(Cat);
                var Page = pro.ToPagedList(pageIndex, pageSize);
                return View("GetAll", Page);

            }
            else if (Cat == 0)
            {
                var pro = await _productService.GetAll();
                var Page = pro.ToPagedList(pageIndex, pageSize);
                return View("GetAll", Page);

            }
            return View("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeleted(int pageIndex = 1, int pageSize = 3)
        {
            var Proudicts = await _productService.GetAllDleted();
            var Page = Proudicts.ToPagedList(pageIndex, pageSize);
             
            return View(Page);
        }

        [HttpGet]
        public async Task<IActionResult> Restore(int pro)
        {
            _productService.Restore(pro);


            return RedirectToAction("GetAllDeleted","Product");
        }
    }



}
