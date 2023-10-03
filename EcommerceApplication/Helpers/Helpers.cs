using EcommerceApplication.Context;
using EcommerceApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Helpers
{
    public static class Helpers
    {
        public static List<SelectListItem> GetCategories(ECommDbContext _context)
        {
            var lstcategory = new List<SelectListItem>();
            List<Category> categories = _context.categories.ToList();
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
    }
}
