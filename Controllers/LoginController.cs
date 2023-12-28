using SewaKendaraan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace SewaKendaraan.Controllers
{
    public class LoginController : Controller
    {
        SewaKendaraanEntities entity = new SewaKendaraanEntities();
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel credential)
        {
            bool userExist = entity.Users.Any
                (x => x.username == credential.username && x.passwords == credential.Password);
            User u = entity.Users.FirstOrDefault(x => x.username == credential.username && x.passwords == credential.Password);
            
            if (userExist)
            {
                ViewBag.IsAuthenticated = "true";
                Session["UserId"] = u.Id;
                string roles = u.Roles;
                Guid id = u.Id;
                FormsAuthentication.SetAuthCookie(u.username, false);   
                return RedirectToAction("Index", "Home", new {roles,id});
            }
            ModelState.AddModelError("", "Username or Password is Wrong");
            return View();

        }

        [HttpPost]
        public ActionResult SignUp(User userinfo)
        {
            userinfo.Id = Guid.NewGuid();
            entity.Users.Add(userinfo);
            entity.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            ViewBag.IsAuthenticated = "false";
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
    }
}