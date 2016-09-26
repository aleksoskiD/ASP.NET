using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using HotelWeb.Models;
using HotelWeb.DAL.Repositories;
using HotelWeb.DAL.Entities;
using System.Threading;

namespace HotelWeb.Controllers
{
    public class AccountController : Controller
    {
        private UserRepository userRepositoy;
        public AccountController()            
        {
            this.userRepositoy = new UserRepository(new DAL.Context.HotelContext());
        }



        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginModel();

            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = this.userRepositoy.ValidateUser(model.UserName, model.Password);
                if (user != null)
                {
                    SignInUser(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Невалидно корисничко име или лозинка.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.userRepositoy.Create(model.UserName,model.Password, model.Address, model.City, model.ContactNo);
                if (user != null)
                {
                    SignInUser(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Грешка. Обидете се повторно!");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

      

        #region Helpers

            // Used for XSRF protection when adding external logins
            private const string XsrfKey = "XsrfId";

            private IAuthenticationManager AuthenticationManager
            {
                get
                {
                    return HttpContext.GetOwinContext().Authentication;
                }
            }

            private void SignInUser(User user, bool isPersistent)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var identity = CreateClaimsIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
            }

            private ClaimsIdentity CreateClaimsIdentity(User user, string authenticationType)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.Id.ToString()));

                if (!string.IsNullOrEmpty(user.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Name, user.Name));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                }

                claims.Add(new Claim(ClaimTypes.Email, user.UserName));

                if (user.UserRoles != null && user.UserRoles.Count > 0)
                {
                    foreach (var r in user.UserRoles)
                    {
                        if (r.Role != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, r.Role.Name));
                        }
                    }
                }

                var claimsIdentity = new ClaimsIdentity(claims, authenticationType);
                Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);

                return claimsIdentity;
            }

            private void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            public enum ManageMessageId
            {
                ChangePasswordSuccess,
                SetPasswordSuccess,
                RemoveLoginSuccess,
                Error
            }

            private ActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            private class ChallengeResult : HttpUnauthorizedResult
            {
                public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
                {
                }

                public ChallengeResult(string provider, string redirectUri, string userId)
                {
                    LoginProvider = provider;
                    RedirectUri = redirectUri;
                    UserId = userId;
                }

                public string LoginProvider { get; set; }
                public string RedirectUri { get; set; }
                public string UserId { get; set; }

                public override void ExecuteResult(ControllerContext context)
                {
                    var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                    if (UserId != null)
                    {
                        properties.Dictionary[XsrfKey] = UserId;
                    }
                    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
                }
            }

        #endregion

    }
}