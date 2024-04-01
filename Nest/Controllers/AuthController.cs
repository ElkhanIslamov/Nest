using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nest.Models;
using Nest.ViewModels;

namespace Nest.Controllers;

public class AuthController : Controller
{
    public readonly UserManager<AppUser> _userManager;
    public readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager = null)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
            return View();

        var user = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Username/Email or password incorrect");
                return View();
            }
        }

        var singInResult = await _signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password, 
            loginViewModel.RememberMe, true);
        if (singInResult.Succeeded)
        {
            ModelState.AddModelError("", "Username/Email or password incorrect");
            return View();
        }

        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> Logout()
    {
        if (!User.Identity.IsAuthenticated)
            return BadRequest();

        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
    {
        if (!ModelState.IsValid)
        
            return View();

        var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Email not found");
            return View();
        }
        // https://localhost:7158/Auth/ResetPassword?email=&token=
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string link = Url.Action("ResetPassword", "Auth", new { email = user.Email, token=token  },
           HttpContext.Request.Scheme, HttpContext.Request.Host.Value);
        return Content(link);
        
        return RedirectToAction(nameof(Login));
        
    }
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
        if (user == null)
            return NotFound();

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(SubmitPasswordViewModel submitpasswordViewModel,
      string email, string token)
    {
        if (ModelState.IsValid)
            return View();
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();
        IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token,
            submitpasswordViewModel.Password);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        return RedirectToAction(nameof(Login));
    }


}
   
