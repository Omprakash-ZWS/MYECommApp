using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceApplication.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Identity;
using EcommerceApplication.Models.ViewModel;
using EcommerceApplication.Service.Interface;
using EcommerceApplication.Models.EmailModel;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Message = EcommerceApplication.Models.EmailModel.Message;
using Azure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> _roleManager ;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configurations;

        [TempData]
        public string StatusMessage { get; set; }
        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IConfiguration configuration, IEmailService emailService, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _configurations = configuration;
            _httpContextAccessor = httpContextAccessor;
           _roleManager = roleManager;
        }
        public List<SelectListItem> RoleList { get; set; }

        // GET: UserController
        public ActionResult Index()
        {

            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Register
        //Register User on Checkout Page. and save the data in cookie after click on next button.
        public ActionResult Register()
        {
            return View();
        }
        // GET: UserController/RegisterUser
        public ActionResult RegisterUser()
        {

            ViewBag.Name = _roleManager.Roles.Select(r => new SelectListItem
            {
                //ye dono diffrent hum tab use krte hai jab hume dikhana kuch aur hai, aur pass kuch aur krna rahata hai.
                //Text jo show hota
                Text = r.Name,
                // jo fill hota hai, ya passs hota hai view se controller me
                Value = r.Name
            }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                // Register User using UserManager and return JWT Token
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    PasswordHash = registerDto.Password,
                    FirstName = registerDto.FirstName,
                    MiddleName = registerDto.MiddleName,
                    LastName = registerDto.LastName,
                    Gender = registerDto.Gender,
                    Address = registerDto.address,
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);               
                if (result.Succeeded)
                {

                    //ApplicationUser users = new ApplicationUser
                    //{
                    //    UserName = registerDto.Email
                    //};
                    await _userManager.AddToRoleAsync(user, registerDto.UserRole);                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(nameof(ConfirmEmail), "User", new { code, Email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email! }, "Confirmation email link", callbackUrl!);
                    await _emailService.SendEmail(message);
                    return RedirectToAction("SendTokenMessage");
                }
                else
                {
                    return NotFound();
                }
            }
            else 
            {
              return RedirectToAction("RegisterUser"); 
            }
        }


        //after sending the token to mail these will show the action method view will show the please verify your email first before login.
        public ActionResult SendTokenMessage()
        {
            return View();
        }

        public ActionResult EmailSuccessMessage()
        {
            return View();
        }


        public async Task<IActionResult> ConfirmEmail(string Email, string code)
        {
            //
            if (Email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Email}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            //StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            //return Page();
            return RedirectToAction("EmailSuccessMessage");
        }

        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult>  Login(LoginDto loginDto, string? returnUrl)
        //{
        //    CookieOptions options = new CookieOptions()
        //    {
        //        HttpOnly = true,
        //        IsEssential = true,
        //        Secure = false,
        //        SameSite = SameSiteMode.Strict,
        //        Domain = "localhost", //using https://localhost:44340/ here doesn't work
        //        Expires = DateTime.UtcNow.AddDays(14)
        //    };
        //    var user = await _userManager.FindByEmailAsync(loginDto.Email);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    //Check Email is Confirm or not 
        //    bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
        //    if (emailStatus == false)
        //    {
        //        ModelState.AddModelError(nameof(loginDto.Email), "Email is unconfirmed, please confirm it first");
        //        return View();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        // Validate user credentials            
        //        // This doesn't count login failures towards account lockout
        //        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);
        //        if (!result.Succeeded)
        //        {                   
        //            return NotFound();
        //        }

        //        // Generate JWT token
        //        var token = GenerateJwtToken(user);
        //        _httpContextAccessor!.HttpContext.Response.Cookies.Append("JwtToken", JsonConvert.SerializeObject(token), options);

        //        //if the click unauthorize view then user first see the login page & the after successfull login
        //        //it will able to see that unauthorize view which prior click.
        //        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        //        {
        //            //instead of write condition we can write  return LocalRedirect(returnUrl);
        //            //it accept only local url and prevent from hackers to do attack.
        //            return Redirect(returnUrl);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        return NotFound(ModelState);
        //    }

        //}



        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return NotFound();
            }

            bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailStatus)
            {
                ModelState.AddModelError(nameof(loginDto.Email), "Email is unconfirmed, please confirm it first");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);
            if (ModelState.IsValid && result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                _httpContextAccessor?.HttpContext?.Response.Cookies.Append("JwtToken", JsonConvert.SerializeObject(token), new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Domain = "localhost", // Change this to your actual domain in a production environment
                    Expires = DateTime.UtcNow.AddDays(14)
                });
                //if (loginDto.RememberMe == false)
                //{
                //    _httpContextAccessor?.HttpContext?.Session.Remove(".AspNetCore.Identity.Application");
                //}
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return NotFound(ModelState);
        }
















        //for logout the User
        public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		// POST: UserController/Create
		[HttpPost]
        [ValidateAntiForgeryToken] //the AntiForgery token is a security feature used to prevent Cross-Site Request Forgery (CSRF) attacks.
        //The FormCollection class will automatically receive the posted form values in the controller action method in key/value pairs. 
        public ActionResult Register(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Registration registration)
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken] //the AntiForgery token is a security feature used to prevent Cross-Site Request Forgery (CSRF) attacks. 
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            string FullName = user.FirstName +" "+ user.LastName;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name ,$"{FullName}")              
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurations.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(1));

            var token = new JwtSecurityToken(
                issuer: _configurations.GetSection("Jwt:Issuer").Value,
                audience: _configurations.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken] //the AntiForgery token is a security feature used to prevent Cross-Site Request Forgery (CSRF) attacks. 
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
