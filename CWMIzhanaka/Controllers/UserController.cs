using CWMIzhanaka.Areas.Admin.Models;
using CWMIzhanaka.Models.Data;
using CWMIzhanaka.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CWMIzhanaka.Controllers
{
    public class UserController : Controller
    {

        // GET: /User
        public ActionResult Index()
        {
            return RedirectToAction("LoginPage");
        }
        // Login page 
        //GET: /User/LoginPage
        public ActionResult LoginPage()
        {
            string username = User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("AccProfile");
            }

            return View();

        }
        // used to get information about login
        // POST: /User/LoginPage
        [HttpPost]
        public ActionResult LoginPage(LoginAccountViewModel lavm)
        {

            if (!ModelState.IsValid)
            {
                return View(lavm);
            }



            bool isValidLogin = false;
            //Check if username and passwort is right to log in
            using (DataBase db = new DataBase())
            {
                if (db.Accounts.Any(x => x.Username.Equals(lavm.Username) && x.Password.Equals(lavm.Password)))
                {
                    isValidLogin = true;
                }
            }
            // tell user that login info was wrong
            if (!isValidLogin)
            {
                ModelState.AddModelError("", "Wrong username or password.");
                return View(lavm);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(lavm.Username, lavm.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(lavm.Username, lavm.RememberMe));
            }
        }
        //Used to create a new user
        //Get: /User/CreateUser
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View("CreateUser");
        }
        //Used to get info about newly created user
        // POST: /User/LoginPage
        [HttpPost]
        public ActionResult CreateUser(AccountViewModel avm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUser", avm);
            }
            //Passwords dont match
            if (!avm.Password.Equals(avm.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords are different");
                return View("CreateUser", avm);
            }

            using (DataBase db = new DataBase())
            {
                //Check is user exists
                if (db.Accounts.Any(a => a.Username.Equals(avm.Username)))
                {
                    ModelState.AddModelError("", "Username exists");
                    avm.Username = "";
                    return View("CreateUser", avm);
                }

                // add to database
                AccountsDataHandler adh = new AccountsDataHandler()
                {
                    Username = avm.Username,
                    Firstname = avm.Firstname,
                    Lastname = avm.Lastname,
                    EmailAddress = avm.EmailAddress,
                    Password = avm.Password
                };
                db.Accounts.Add(adh);
                db.SaveChanges();

                int id = adh.UserId;
                // add low priviledge user roles
                UserRolesDataHandler urdh = new UserRolesDataHandler()
                {
                    UserId = id,
                    RoleId = 2
                };
                db.UserRoles.Add(urdh);
                db.SaveChanges();
                //Used to tell if user was added
                TempData["MSG"] = "You have been registered, you can log in";

            }

            return Redirect("~/user/LoginPage");
        }
        //Log out from website
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginPage");
        }

        //View to show who is logged in if logged in
        public ActionResult AccNav()
        {

            string username = User.Identity.Name;


            AccNavViewModel anvm;

            using (DataBase db = new DataBase())
            {

                AccountsDataHandler adh = db.Accounts.FirstOrDefault(x => x.Username == username);

                anvm = new AccNavViewModel()
                {
                    Username = adh.Username
                };
            }

            return PartialView(anvm);
        }
        //Used to show user profile
        //GET: /User/AccProfile
        [HttpGet]
        public ActionResult AccProfile()
        {
            string username = User.Identity.Name;

            AccountProfileViewModel apvm;
            using (DataBase db = new DataBase())
            {
                AccountsDataHandler adh = db.Accounts.FirstOrDefault(x => x.Username == username);

                apvm = new AccountProfileViewModel(adh); 

            }
            return View(apvm); 
        }
        //Used to get edited user profile
        //POST: /User/AccProfile
        [HttpPost]
        public ActionResult AccProfile(AccountProfileViewModel apvm)
        {
            if (!ModelState.IsValid)
            {
                return View("AccProfile", apvm);
            }

            // if password was entered check if they match
            if (!string.IsNullOrWhiteSpace(apvm.Password))
            {
                if (!apvm.Password.Equals(apvm.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords are diferent.");
                    return View("AccProfile", apvm);
                }
            }
           
            using (DataBase db = new DataBase())
            {
               
                string username = User.Identity.Name;

              //Check if enetered username exists
                if (db.Accounts.Where(a => a.UserId != apvm.UserId).Any(a => a.Username == username))
                {
                    ModelState.AddModelError("", "Username " + apvm.Username + " is taken.");
                    apvm.Username = "";
                    return View("AccProfile", apvm);
                }

                //add user info to the database
                AccountsDataHandler adh = db.Accounts.Find(apvm.UserId);
                adh.Username = apvm.Username;
                adh.EmailAddress = apvm.EmailAddress;
                adh.Firstname = apvm.Firstname;
                adh.Lastname = apvm.Lastname;
                

                if (!string.IsNullOrWhiteSpace(apvm.Password))
                {
                    adh.Password = apvm.Password;
                }

              
                db.SaveChanges();
            }


            TempData["MSG"] = "You have edited your profile!";

            return RedirectToAction("AccProfile");
        }
    }
}