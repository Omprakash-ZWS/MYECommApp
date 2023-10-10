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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using EcommerceApplication.Service.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using EcommerceApplication.Models.ViewModel;
using AutoMapper.Features;
using Humanizer;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApplication.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
		
        //private IHostingEnvironment Environment;
		private readonly IHostingEnvironment _environment;
        private IGenericRepository<Category> _category;      
        private readonly ECommDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        
        public CategoriesController(ECommDbContext context, IUnitOfWork unitOfWork,
            IGenericRepository<Category> category, IHostingEnvironment environment)
        {
            _category = category;            
            _unitOfWork = unitOfWork;
			_context = context;
            _environment = environment;
        }
        
        // GET: Categories
        public ViewResult Index()
		{
			
			List<CategoryDto> categories = _unitOfWork.Category.GetAll().Where(x => x.DeletedAt == null).Select(category  => 
			new CategoryDto{
				CategoryId = category.CategoryId,
				CategoryName = category.CategoryName,
				colour = category.colour,
				description = category.description

			}).ToList();


			return View(categories);
        }

        // GET: Categories/Create
        public IActionResult Create()
		{
			CategoryDto categoryDto = new CategoryDto();	
			ViewBag.CategoryId = GetCategories();
			return View(categoryDto);
		}

		// POST: Categories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]// the AntiForgery token is a security feature used to prevent Cross-Site Request Forgery(CSRF) attacks.

		public async Task<IActionResult> Create( CategoryDto categoryDto)
		{
			Category cat = new Category()
			{
				CategoryName = categoryDto.CategoryName,
				colour = categoryDto.colour,
				description	= categoryDto.description,
				CreatedAt = DateTime.Now
			};			
			if (ModelState.IsValid)	
			{			
				_unitOfWork.Category.Add(cat);	
                await Task.Run(() => _unitOfWork.Category.Save());
                return RedirectToAction("Index");
			}
			return View(categoryDto);
		}



		//
        private Category GetCategory(int id)
		{		
			return _unitOfWork.Category.GetById(id);
		}

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
		//Get All the Categories for showing in the dropdown
        private List<SelectListItem> GetCategories()
		{
			var lstcategory = new List<SelectListItem>();
			List<Category> categories = _context.categories.Where(x => x.DeletedAt == null).ToList();			
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
        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            Category category = GetCategory(id);
            ViewBag.CategoryId = GetCategories();
            return View(category);
        }

		// POST: Categories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
		[ValidateAntiForgeryToken] // the AntiForgery token is a security feature used to prevent Cross-Site Request Forgery(CSRF) attacks.
		public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CreatedAt,colour,description")] CategoryDto category)
		{
		    if (id != category.CategoryId)
		    {
		        return NotFound();
		    }

		    if (ModelState.IsValid)
		    {
		        try
		        {

                    Category cat = GetCategory(id);
					cat.description = category.description;
					cat.colour = category.colour;
					cat.CategoryName = category.CategoryName;
                    cat.UpdatedAt = DateTime.Now;
		            _unitOfWork.Category.Update(cat);
		           _unitOfWork.Save();
		        }
		        catch (DbUpdateConcurrencyException)
		        {					
					throw;
		        }
		        return RedirectToAction(nameof(Index));
		    }
		    return View(category);
		}

		// GET: Categories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.categories
				.FirstOrDefaultAsync(m => m.CategoryId == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken] //the AntiForgery token is a security feature used to prevent Cross-Site Request Forgery (CSRF) attacks. 
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			var category = _unitOfWork.Category.GetById(id);
			if (category == null)
			{
				return NotFound();
			}
			category.DeletedAt = DateTime.Now;					
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoryExists(int id)
		{
			return (_context.categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
		}

        [HttpGet, ActionName("GetById")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetById(int id)
        {
			_unitOfWork.Category.GetById(id);
			return View();
        }

		
    }
}
