using EcommerceApplication.Context;
using EcommerceApplication.Models;
using EcommerceApplication.Models.ViewModel;
using EcommerceApplication.Service.Infrastructure;
using EcommerceApplication.Service.Interface;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Diagnostics;


namespace EcommerceApplication.Controllers
{
	public class HomeController : Controller
	{

		private readonly IHttpContextAccessor _httpContextAccessor;
		//private readonly ProductRepository _productRepository;
		private IGenericRepository<Category> _category;
		//private IGenericRepository<CategoryDto> _categorydto;
        private readonly IGenericRepository<Product> _product;
        //private readonly IGenericRepository<ProductDto> _productdto;
        private readonly ECommDbContext _context;
		private readonly IUnitOfWork _unitOfWork;
		public HomeController(ECommDbContext context, IUnitOfWork unitOfWork,
			IGenericRepository<Category> category, 
			IGenericRepository<Product> product, 
			IHttpContextAccessor httpContextAccessor 
			)
		{
			_category = category;
            _product = product;
            _unitOfWork = unitOfWork;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			//_productRepository = productRepository;
		}

		public IActionResult Index()
		{
			//geting all the categories who is not deleted
			var data = _unitOfWork.Category.GetAll().Where(x => x.DeletedAt == null);
			//prior these i have use the Viewbag.CategoryList and these viewbag.categorylist passed to the cshtml file for loop through and get the data
			//but now save that data to variable and create the list of categorydto class and 
			//using foreach loop get the data and save in the list categoryData object
			List<CategoryDto> categoryData = new List<CategoryDto>();
			foreach(var category in data)
			{
				categoryData.Add(new CategoryDto{
					CategoryId = category.CategoryId,
					CategoryName = category.CategoryName,
					colour = category.colour,
					description = category.description,
				});
			}
			return View(categoryData);
		}

		public IActionResult Privacy()
		{
			return View();
		}

        public IActionResult Product(int? id)
        {      
			//getting all the product who is not deleted and whose category id is passing through URL when we click on that product
			ViewBag.productList = _unitOfWork.Product.GetAll().Where(x => x.DeleteAt == null && x.CategoryId == id).ToList();
            return View();
        }

        public JsonResult AddToCart([FromBody] CartDto cartDto)
        {
			if (cartDto == null)
			{
				throw new ArgumentNullException(nameof(cartDto), "CartDto cannot be null.");
			}
			// if cartdto is null the throw exception
			// if not null then check cookies
			//if CartCookie is present then add cartDto in cookies list
			//else create cartCookies list and add cartDto and save it into cookies

			string cookieValue = _httpContextAccessor.HttpContext!.Request.Cookies["CartCookies"]!;
			List<CartDto> cartList;
			CookieOptions options = new CookieOptions()
			{
				HttpOnly = true,
				IsEssential = true,
				Secure = false,
				SameSite = SameSiteMode.Strict,
				Domain = "localhost", //using https://localhost:44340/ here doesn't work
				Expires = DateTime.UtcNow.AddDays(14)
			};
			//first get the data from the cookie using the JsonConvert.DeserializeObject which is the NewtonSoft.Json
			//Serialization :- is the process of converting .NET objects, such as strings, into a JSON format
			//deserialization:- is the process of converting JSON data into.NET objects.
			cartList = !string.IsNullOrEmpty(cookieValue) ? JsonConvert.DeserializeObject<List<CartDto>>(cookieValue)! : new List<CartDto>();
			// check if product is already inside cart
			//item.productId = which in cookie
			//cartDto.productId = 
			bool productExistsInCart = cartList.Any(item => item.ProductId == cartDto.ProductId);
			if (productExistsInCart)
			{
				return Json(new { message = "Product already exists in the cart." });

			}
			// if exist then throw error message else add into cookies
			cartList.Add(cartDto);
			_httpContextAccessor.HttpContext.Response.Cookies.Append("CartCookies", JsonConvert.SerializeObject(cartList), options);
			return Json(new { message ="Product Added Successfully" });
        }

		public IActionResult DeleteCart(int productId)
		{
			string cookieValue = _httpContextAccessor.HttpContext!.Request.Cookies["CartCookies"]!;
			if (productId == 0)
			{
				return Json(new { message = "Product Id Is Null" });
			}
			List<CartDto> cartList = JsonConvert.DeserializeObject<List<CartDto>>(cookieValue);
			cartList.RemoveAll(item => item.ProductId == productId);
			CookieOptions options = new CookieOptions
			{
				HttpOnly = true,
				IsEssential = true,
				Secure = false,
				SameSite = SameSiteMode.Strict,
				Domain = "localhost", //using https://localhost:44340/ here doesn't work
				Expires = DateTime.UtcNow.AddDays(14)
			};
			_httpContextAccessor.HttpContext.Response.Cookies.Append("CartCookies", JsonConvert.SerializeObject(cartList), options);
			//return Json(new { message = "Product removed from the cart." });
			return RedirectToAction("CartProducts");
		}

		[HttpGet]
		public IActionResult Checkout(int finalPrice)
        {
			ViewBag.FinalTotal = finalPrice;
			return View();
        }
        public IActionResult CartProducts()
		{
			//grtting the data from the cookies
			var cartJson = _httpContextAccessor.HttpContext.Request.Cookies["CartCookies"] ?? "";
			//deserialize that list json data which is save in the cookie 
			List<CartDto> cart = JsonConvert.DeserializeObject<List<CartDto>>(cartJson)!;

            return View(cart);
        }

		//public IActionResult OnGet(string SearchTerm)
		//{
		// var Product = _productRepository.Search(SearchTerm);
		//	return View(Product);
		//}

		// GET: UserController/Create
		//public ActionResult Create()
		//{
		//    return View();
		//}

		// POST: HomeController/Create
		//[]
		[HttpPost]
        public IActionResult Create([FromBody] Registration registration)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}