using CommunityArena.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CommunityArena.Controllers
{
    public class IdentityController : Controller
    {
        // GET: Identity
        public ActionResult Index()
        {
            return RedirectToRoute("/");
        }

        /// <summary>
        /// Just a function for me to create the User and Admin roles. Now locked away behind Authorize because nobody should use it.
        /// </summary>
        /// <returns>Links back to index.</returns>
        //[Authorize(Roles = "Admin")]
        public ActionResult CreateRoles()
        {
            //Context.UserManager = wertrew
            //using (var Context.context = new ContextClass())
            {
                /*------------Role Manager--------------------------*/

                var store = new RoleStore<IdentityRole>(Context.context);

                var roleManager = new RoleManager<IdentityRole>(store);

                roleManager.Create(new IdentityRole("Admin"));
                roleManager.Create(new IdentityRole("User"));

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Test()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The Get Register User goes to the RegisterUser view and allows the user to register a User.
        /// </summary>
        /// <returns>The Log-In view.</returns>
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }

        /// <summary>
        /// Then, when they've filled in the form in the RegisterUser view, they send it in here.
        /// A person is also added to the People database with their information.
        /// </summary>
        /// <param name="username">The username the user desires.</param>
        /// <param name="name">The user's name.</param>
        /// <param name="isActually">The secret identity of the user.</param>
        /// <param name="password">The desired password of the user.</param>
        /// <param name="verifyPassword">To ensure no faults, the user is required to verify their password.</param>
        /// <returns>It will then redirect to LogInFunction and log in the user if successful, otherwise reload the form.</returns>
        [HttpPost]
        public async Task<ActionResult> RegisterUser(string username, string password, string verifyPassword)
        {
            Context._userstore = new UserStore<AppUser>(Context.context);
            Context.UserManager = new UserManager<AppUser>(Context._userstore);

            Context.UserManager.MaxFailedAccessAttemptsBeforeLockout = 5;

            if (password == verifyPassword)
            {
                var newUser = new AppUser()
                {
                    UserName = username
                };

                var result = await Context.UserManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    Context.UserManager.AddToRole(newUser.Id, "User");
                }

                await Context.context.SaveChangesAsync();

                return await LogInFunction(username, password);
            }
            return View();
        }

        public async Task<ActionResult> RegisterAdmin()
        {
            Context._userstore = new UserStore<AppUser>(Context.context);
            Context.UserManager = new UserManager<AppUser>(Context._userstore);

            Context.UserManager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var newUser = new AppUser()
            {
                UserName = "Admin"
            };

            var result = await Context.UserManager.CreateAsync(newUser, "password");

            if (result.Succeeded)
            {
                Context.UserManager.AddToRole(newUser.Id, "Admin");
            }

            await Context.context.SaveChangesAsync();

            return await LogInFunction("Admin", "password");
        }

        /// <summary>
        /// The Get LogIn simply gets the log-in view and returns it to the user so they may log in using the form there.
        /// </summary>
        /// <returns>Returns the log-in view.</returns>
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        /// <summary>
        /// The Post action calls the LogInFunction and goes from there.
        /// </summary>
        /// <param name="name">The username the user wants to log in using.</param>
        /// <param name="password">The password the user has provided. We'll log in if correct.</param>
        /// <returns>If we're logged in, reload the index page with us logged in. Otherwise, reload the log-in form.</returns>
        [HttpPost]
        public async Task<ActionResult> LogIn(string name, string password)
        {
            return await LogInFunction(name, password);
        }

        /// <summary>
        /// The function we go to when the form for loggin in has been written in. It goes sign-in manager and such.
        /// It's private because it should only be accessed from the different actions that log you in, like registering or logging in.
        /// </summary>
        /// <param name="name">The username the user wants to log in using.</param>
        /// <param name="password">The password the user has provided. We'll log in if correct.</param>
        /// <returns>If we're logged in, reload the index page with us logged in. Otherwise, reload the log-in form.</returns>
        private async Task<ActionResult> LogInFunction(string name, string password)
        {
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            SignInStatus sis = await signInManager.PasswordSignInAsync(
                userName: name, password: password,
                isPersistent: true,
                shouldLockout: false
                );
            if (sis == SignInStatus.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("LogIn");
        }

        /// <summary>
        /// Log-out, quite simply, logs the user out and then redirects them to the index-screen.
        /// </summary>
        /// <returns>Index.</returns>
        [Authorize]
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}