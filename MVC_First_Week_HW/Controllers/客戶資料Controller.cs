using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_First_Week_HW.Models;

namespace MVC_First_Week_HW.Controllers
{
    public class 客戶資料Controller : Controller
    {
        客戶資料Repository repo;
        public 客戶資料Controller()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
        }

        // GET: 客戶資料
        public ActionResult Index(客戶資料 客戶資料, string keyword, string sort_col, string current_sort)
        {
            var client = repo.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                client = repo.FindName(keyword, client);
            }
            if (!string.IsNullOrEmpty(客戶資料.客戶分類))
            {
                client = repo.GetCategory(客戶資料.客戶分類, client);
            }
            if (!string.IsNullOrEmpty(sort_col))
            {
                if(sort_col != current_sort)
                {
                    client = client.OrderByField(sort_col, true);
                    ViewBag.current_sort = sort_col;
                }
                else
                {
                    client = client.OrderByField(sort_col, false);
                    ViewBag.current_sort = "";
                }
            }
            else
                ViewBag.current_sort = "";
            ViewBag.keyword = keyword;
            ViewBag.篩選分類 = 客戶資料.客戶分類;
            ViewBag.客戶分類 = GetCategorySelect();
            ViewBag.sort_col = sort_col;
            ViewBag.current_sort = current_sort;
            return View(client);
        }

        public FileResult ExportExcel(string keyword, string 篩選分類, string sort_col, string current_sort)
        {
            List<string> show_col = new List<string> { "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email", "客戶分類" };
            return Excel.exportExcel(repo.GetFilterItem(keyword, 篩選分類, sort_col, current_sort),
                "客戶資料", show_col);
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
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
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
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
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
