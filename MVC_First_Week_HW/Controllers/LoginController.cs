using MVC_First_Week_HW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_First_Week_HW.Controllers
{
    public class LoginController : Controller
    {
        客戶資料Repository repo;

        public LoginController()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
        }

        // GET: Login
        public ActionResult Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                if(Request.Cookies["mvc_coures_remember"] != null)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                    (1, Request.Cookies["mvc_coures_remember"].Value, DateTime.Now, DateTime.Now.AddMinutes(60), true, "");
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult Login(string Account, string PD, bool remember)
        {
            客戶資料 client = repo.FindAccount(Account, PD);
            if(client != null)
            {
                string userData = client.Role;
                if (string.IsNullOrEmpty(userData))
                    userData = "normal";
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                (1, Account, DateTime.Now, DateTime.Now.AddMinutes(60), false, userData);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if(remember)
                {
                    HttpCookie cookie = new HttpCookie("mvc_coures_remember");
                    cookie.Value = LoginViewModel.Encrypt(Account);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                }

                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["mvc_coures_remember"] != null)
            {
                HttpCookie cookie = new HttpCookie("mvc_coures_remember");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            FormsAuthentication.SignOut();
            return View("Index");
        }
    }
}