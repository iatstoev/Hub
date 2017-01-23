using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Hub.Entity;
using Services;
using Hub.Models;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace Hub.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private UserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<UserService>();
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
          
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel details)
        {
            if (ModelState.IsValid)
            {
                HubUser user = await UserService.FindAsync(details.UserName, details.Password);
                if (user != null)
                {
                    ClaimsIdentity ident = await UserService.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                    return RedirectToAction("Index", "Section");
                }
            }
            return View(details);
        }
    }
}