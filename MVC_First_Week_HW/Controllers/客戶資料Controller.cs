using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVC_First_Week_HW.Models;
using X.PagedList;

namespace MVC_First_Week_HW.Controllers
{
    [Authorize(Roles = "admin")]
    public class 客戶資料Controller : Controller
    {
        客戶資料Repository repo;
        private int pageSize = 5;

        public 客戶資料Controller()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
        }

        // GET: 客戶資料
        [計算時間Attribute]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Validation.DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult Index(客戶資料ViewModel 客戶資料)
        {
            var client = repo.All();
            if (!string.IsNullOrEmpty(客戶資料.搜尋客戶名稱))
            {
                client = repo.FindName(客戶資料.搜尋客戶名稱, client);
            }
            if (!string.IsNullOrEmpty(客戶資料.篩選分類))
            {
                client = repo.GetCategory(客戶資料.篩選分類, client);
            }
            if (!string.IsNullOrEmpty(客戶資料.sort_col))
            {
                bool sort = 客戶資料.isSort;
                if (sort == false)
                {
                    client = client.OrderByField(客戶資料.sort_col, true);
                }
                else
                {
                    client = client.OrderByField(客戶資料.sort_col, false);
                }
                if (ViewBag.isSort != sort)
                    ViewBag.isSort = sort;
            }
            else
            {
                client = client.OrderBy(c => c.Id);
                ViewBag.isSort = true;
            }
            
            var orderClient = client.ToPagedList(客戶資料.page == 0 ? 1 : 客戶資料.page, pageSize);
            ViewBag.currentPage = 客戶資料.page == 0 ? 1 : 客戶資料.page;
            ViewBag.搜尋客戶名稱 = 客戶資料.搜尋客戶名稱;
            ViewBag.篩選分類 = 客戶資料.篩選分類;
            ViewBag.CategoryList = GetCategorySelect();
            ViewBag.sort_col = 客戶資料.sort_col;
            return View(orderClient);
        }

        [計算時間Attribute]
        public FileResult ExportExcel(客戶資料ViewModel 客戶資料)
        {
            List<string> show_col = new List<string> { "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email", "客戶分類" };
            return Excel.exportExcel(repo.GetFilterItem(客戶資料.搜尋客戶名稱, 客戶資料.篩選分類, 客戶資料.sort_col, 客戶資料.isSort),
                "客戶資料", show_col, new List<string>());
        }

        //public ActionResult Search(string keyword)
        //{
        //    var client = repo.FindName(keyword);
        //    ViewBag.客戶分類 = GetCategorySelect();
        //    return View("Index", client);
        //}

        //public ActionResult Filter(客戶資料 客戶資料)
        //{
        //    var client = repo.GetCategory(客戶資料.客戶分類);
        //    ViewBag.客戶分類 = GetCategorySelect();
        //    return View("Index", client);
        //}

        public IEnumerable<SelectListItem> GetCategorySelect()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var category in repo.FindCategory())
            {
                items.Add(new SelectListItem()
                {
                    Text = category,
                    Value = category
                });
            }
            //IEnumerable<SelectListItem> items = repo.FindCategory()
            //.Select(x => new SelectListItem
            //{
            //    Value = x.客戶分類,
            //    Text = x.客戶分類
            //});
            return items;
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        public ActionResult Details_客戶聯絡人清單(int id)
        {
            ViewData.Model = repo.Find(id).客戶聯絡人.ToList();
            return PartialView("客戶聯絡人清單");
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號,密碼,Role")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.密碼 = LoginViewModel.Encrypt(客戶資料.密碼);
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號,密碼,Role")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //var db = repo.UnitOfWork.Context;
                //db.Entry(客戶資料).State = EntityState.Modified;
                //db.SaveChanges();

                客戶資料 client = repo.Find(客戶資料.Id);
                client.客戶名稱 = 客戶資料.客戶名稱;
                client.統一編號 = 客戶資料.統一編號;
                client.電話 = 客戶資料.電話;
                client.傳真 = 客戶資料.傳真;
                client.地址 = 客戶資料.地址;
                client.Email = 客戶資料.Email;
                client.客戶分類 = 客戶資料.客戶分類;
                client.帳號 = 客戶資料.帳號;
                client.密碼 = string.IsNullOrEmpty(客戶資料.密碼) ? client.密碼 : LoginViewModel.Encrypt(客戶資料.密碼);
                client.Role = 客戶資料.Role;
                repo.UnitOfWork.Commit();


                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            repo.Delete(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
