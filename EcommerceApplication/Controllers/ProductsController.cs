using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApplication.Context;
using EcommerceApplication.Models;
using EcommerceApplication.Service.Interface;
using Microsoft.CodeAnalysis.Elfie.Serialization;


namespace EcommerceApplication.Controllers
{
    public class ProductsController : Controller
    {
        public IWebHostEnvironment _enviorment;
        private IGenericRepository<Product> _product;
        private IGenericRepository<Category> _category;
        private readonly ECommDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(ECommDbContext context, IUnitOfWork unitOfWork,
            IGenericRepository<Product> product, IGenericRepository<Category> category,  IWebHostEnvironment environment)
        {
            _product = product;
            _unitOfWork = unitOfWork;
            _context = context;
            _category = category;
            _enviorment = environment;
        }

        // GET: Products
        public ViewResult Index()
        {
            var product = _unitOfWork.Product.GetAll().Where(x => x.DeleteAt == null);
            return View(product);
        }

        private Category GetCategory(int id)
        {
            return _unitOfWork.Category.GetById(id);
        }
        private Product GetProduct(int Id)
        {
            return _unitOfWork.Product.GetById(Id);
        }


        private List<SelectListItem> GetCategories()
        {
            var lstcategory = new List<SelectListItem>();            
            List<Category> categories = _unitOfWork.Category.GetAll().Where(x => x.DeletedAt == null).ToList();
            lstcategory = categories.Select(ct => new SelectListItem()
            {
                Value = ct.CategoryId.ToString(),
                Text = ct.CategoryName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "-----Select------"
            };
            lstcategory.Insert(0, defItem);
            return lstcategory;

        }
        //for the single product name while edit the list
        private List<SelectListItem> GetProducts(int productId = 1)
        {
            List<SelectListItem> lstproducts = _context.products
                 .Where(c => c.ProductId == productId)
                 .OrderBy(n => n.ProductName)
                 .Select(n =>
                 new SelectListItem
                 {
                     Value = n.ProductId.ToString(),
                     Text = n.ProductName
                 }).ToList();
            return lstproducts;
        }
        //get the list of product
        private List<SelectListItem> GetProductList()
        {
            var lstproducts = new List<SelectListItem>();            
            List<Product> products = _unitOfWork.Product.GetAll().Where(x => x.DeleteAt == null).ToList();
            lstproducts = products.Select(ct => new SelectListItem()
            {
                Value = ct.ProductId.ToString(),
                Text = ct.ProductName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "-----Select------"
            };
            lstproducts.Insert(0, defItem);
            return lstproducts;
        }


        public IActionResult Create()
        {
            Product Product = new Product();
            ViewBag.CategoryId = GetCategories();
            ViewBag.ProductId = GetProductList();
            return View(Product);
        }


       
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> imgfile)
        {
            //if (ModelState.IsValid)
            //{
           
            if (imgfile != null)
            {                
                string wwwPath = _enviorment.WebRootPath;
                string path = Path.Combine(_enviorment.WebRootPath, "product/images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //want to upload multiple file thats why we are use List<string>
                List<string> uploadedFiles = new List<string>();

                foreach (IFormFile postedFile in imgfile)
                {
                    //thse is our image filename
                    string fileName = Path.GetFileName(postedFile.FileName);
					product.Images += fileName + ",";

					//FileStream operates on a byte level, allowing you to read and write bytes or blocks of bytes from and to a file.

					using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }
                product.Images = product.Images.Substring(0, product.Images.Length - 1);
            }
            product.CreatedAt = DateTime.Now;
             _unitOfWork.Product.Add(product);
           
            await Task.Run(() => _unitOfWork.Save());
            return RedirectToAction(nameof(Index));
           }

       
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Products/Edit/5           
        private List<SelectListItem> Getcategorybyid(int CategoryId)
        {

            List<SelectListItem> listCategory = _context.categories
               .Where(c => c.CategoryId == CategoryId)
               .OrderBy(c => c.CategoryName)
               .Select(n =>
           new SelectListItem
           {
               Value = n.CategoryId.ToString(),
               Text = n.CategoryName
           }).ToList();

            return listCategory;
        }
        public  IActionResult Edit(int id)
        {
            
            Product Product = GetProduct(id);
            ViewBag.ProductId = GetProductList();
            ViewBag.CategoryId = GetCategories();            
            return View(Product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {



            if (id != product.ProductId)
            {
                return NotFound();
            }
            try
            {
                product.UpdatedAt = DateTime.Now;
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = _unitOfWork.Category.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
           product.DeletedAt = DateTime.Now;
            //_unitOfWork.Category.Delete(category);			
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        
    }
}
