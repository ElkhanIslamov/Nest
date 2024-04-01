using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nest.Models;
using Nest.ViewModels;

namespace Nest.Controllers;

public class UserController : Controller
{
    public readonly UserManager<AppUser> _usermanager;

    public UserController(UserManager<AppUser> usermanager)
    {
        _usermanager = usermanager;
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        AppUser appUser = new AppUser()
        {
            
            Fullname = registerViewModel.Fullname,
            UserName =  registerViewModel.Username,
            Email = registerViewModel.Email,
            IsActive = true
        };
       IdentityResult identityresult = await _usermanager.CreateAsync(appUser,registerViewModel.Password);
        if (!identityresult.Succeeded) 
        {
            foreach (var error in  identityresult.Errors) 
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        return RedirectToAction("Index", "Home");
    }
}
