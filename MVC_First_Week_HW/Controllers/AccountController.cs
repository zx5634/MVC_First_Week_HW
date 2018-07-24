using MVC_First_Week_HW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_First_Week_HW.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        客戶資料Repository repo;

        public AccountController()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
        }
        
        public ActionResult Index()
        {
            try
            {
                string name = ((FormsIdentity)User.Identity).Ticket.Name;
                客戶資料 client = repo.FindAccount(name);
                if (client != null)
                {
                    return View(client);
                }
            }
            catch
            {

            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public ActionResult normalUserEdit(客戶資料normalVM 客戶資料)
        {
            客戶資料 client = repo.Find(客戶資料.Id);
            if (ModelState.IsValid)
            {
                client.電話 = 客戶資料.電話;
                client.傳真 = 客戶資料.傳真;
                client.地址 = 客戶資料.地址;
                client.Email = 客戶資料.Email;
                if (!string.IsNullOrEmpty(客戶資料.密碼))
                {
                    string pd = LoginViewModel.Encrypt(客戶資料.密碼);
                    client.密碼 = pd;
                }
                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }
            ViewData.Model = client;
            return View("Index");
        }
    }
}