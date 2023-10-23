using EcommerceApplication.Models;
using EcommerceApplication.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApplication.Controllers
{
	public class AdminController : Controller
	{
		private RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult CreateRole()
		{
			return View();

		}
		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleDto model)
		{
			if (ModelState.IsValid)
			{
				IdentityRole identityRole = new IdentityRole()
				{
					Name = model.RoleName
				};
				IdentityResult result = await _roleManager.CreateAsync(identityRole);
				if (result.Succeeded)
				{
					RedirectToAction("ListRole", "Admin");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult ListRole()
		{
			var roles = _roleManager.Roles;
			return View(roles);
		}
		[HttpGet]
		public async Task<IActionResult> EditRole(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id ={Id} cannot be found";
				return View("Error");
			}
			var model = new EditRoleDto
			{
				Id = role.Id,
				RoleName = role.Name

			};

			//      _userManager..Where(x=>x.Id == Id).

			foreach (var user in await _userManager.GetUsersInRoleAsync(role.Name))
			{
				//var boolval = await _userManager.IsInRoleAsync(user, role.Name);
				//if(boolval)
				model.Users.Add(user.UserName);
			}
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> EditRole(EditRoleDto model)
		{     
			var role = await _roleManager.FindByIdAsync(model.Id);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id ={model.Id} cannot be found";
				return View("Error");
			}
			else
			{
				role.Name = model.RoleName;
				var result = await _roleManager.UpdateAsync(role);
				if (result.Succeeded)
				{
					return RedirectToAction("ListRole");
				}
			}
           
            return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> EditUsersInRole(string roleId)
		{
			ViewBag.roleId = roleId;
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("Error");

			}           
            var model = new List<UserRoleDto>();
			//get all the data either or admin, user or without role
			var list = _userManager.Users.ToList();
            //get all the user which role is user in database using the GetUsersInRoleAsync method
            var roleList = await _userManager.GetUsersInRoleAsync(role.Name);

            foreach (var user in list)
			{
				var UserRoleDto = new UserRoleDto
				{
					UserId = user.Id,
					UserName = user.UserName,				
				};
                //these inner loop is for the matching the role which are in the table getting using GetUsersInRoleAsync method
                //and make Isselected = true
                foreach (var roleuser in roleList)
				{
                    if (user.Id == roleuser.Id)
                    {
                        UserRoleDto.Isselected = true;
                    }
     //               else if (user.Id == roleuser.Id)
					//{
     //                   UserRoleDto.Isselected = false;
     //               }
                }               
				model.Add(UserRoleDto);
            }
			return View(model);
		}
		[HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleDto> model, string roleId)
		{

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("Error");

            }
			for (int i = 0; i < model.Count; i++)
			{
				var user = await _userManager.FindByIdAsync(model[i].UserId);
				IdentityResult result = null;
                if (model[i].Isselected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                   result = await _userManager.AddToRoleAsync(user, role.Name);
                }else if (!model[i].Isselected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
				}
				else
				{
					continue;
				}
				if(result.Succeeded) 
				{
					if (i < (model.Count - 1))
						continue;
					else
						return RedirectToAction("EditRole", new { Id = roleId });
				}
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

    }
}

