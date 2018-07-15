using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NETIdentityWeb.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<IdentityUser> UserManager =>
            HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();
        //sign in manager to validate auth and use it for [authorize] tags
        public SignInManager<IdentityUser, string> SignInManager =>
            HttpContext.GetOwinContext().Get<SignInManager<IdentityUser, string>>();

        //login method
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// isPersistant bool - for issue cookie, else it uses session
        /// shouldLogout - to incredent logout count if auth fail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var _signInStatus = await SignInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                true, 
                true);

            switch (_signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "invalid credentials");
                    return View(model);
                      
            }

            return View();
        }



        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            //handling where user exist, go back to Home 
            var identityUser = await UserManager.FindByNameAsync(model.Username);
            if(identityUser != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var identityResult = await UserManager.CreateAsync
                (new IdentityUser(model.Username), model.Password);

            if(identityResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", identityResult.Errors.FirstOrDefault());

            return View(model);

        }
    }


}