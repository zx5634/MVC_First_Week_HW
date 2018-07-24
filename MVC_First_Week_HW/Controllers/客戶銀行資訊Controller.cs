using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_First_Week_HW.Models;
using X.PagedList;

namespace MVC_First_Week_HW.Controllers
{
    public class 客戶銀行資訊Controller : Controller
    {
        客戶銀行資訊Repository repo;
        private int pageSize = 5;

        public 客戶銀行資訊Controller()
        {
            repo = RepositoryHelper.Get客戶銀行資訊Repository();
        }

        // GET: 客戶銀行資訊
        [計算時間Attribute]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Validation.DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult Index(客戶銀行資訊ViewModel 客戶銀行資訊)
        {
            var client = repo.All().Include(x => x.客戶資料);
            if (!string.IsNullOrEmpty(客戶銀行資訊.搜尋銀行名稱))
            {
                client = repo.FindName(客戶銀行資訊.搜尋銀行名稱, client);
            }

            if (!string.IsNullOrEmpty(客戶銀行資訊.sort_col))
            {
                bool sort = 客戶銀行資訊.isSort;
                if (sort == false)
                {
                    if (客戶銀行資訊.sort_col == "客戶名稱")
                    {
                        client = (from o in client
                                  orderby o.客戶資料.客戶名稱
                                  select o);
                    }
                    else
                        client = client.OrderByField(客戶銀行資訊.sort_col, true);
                }
                else
                {
                    if (客戶銀行資訊.sort_col == "客戶名稱")
                    {
                        client = (from o in client
                                  orderby o.客戶資料.客戶名稱 descending
                                  select o);
                    }
                    else
                        client = client.OrderByField(客戶銀行資訊.sort_col, false);
                }
                if (ViewBag.isSort != sort)
                    ViewBag.isSort = sort;
            }
            else
            {
                client = client.OrderBy(c => c.Id);
                ViewBag.isSort = true;
            }
            var orderClient = client.ToPagedList(客戶銀行資訊.page == 0 ? 1 : 客戶銀行資訊.page, pageSize);
            ViewBag.currentPage = 客戶銀行資訊.page == 0 ? 1 : 客戶銀行資訊.page;
            ViewBag.搜尋銀行名稱 = 客戶銀行資訊.搜尋銀行名稱;
            return View(orderClient);
        }

        public ActionResult Search(string keyword)
        {
            ViewBag.keyword = keyword;
            var client = repo.FindName(keyword);
            return View("Index", client);
        }

        [計算時間Attribute]
        public FileResult ExportExcel(客戶銀行資訊ViewModel 客戶銀行資訊)
        {
            List<string> show_col = new List<string> { "銀行名稱", "銀行代碼", "分行代碼", "帳戶名稱", "帳戶號碼", "客戶名稱" };
            List<string> relationCol = new List<string> { "客戶資料.客戶名稱" };
            return Excel.exportExcel(repo.GetFilterItem(客戶銀行資訊.搜尋銀行名稱, 客戶銀行資訊.sort_col, 客戶銀行資訊.isSort), "客戶銀行資訊", show_col, relationCol);
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶銀行資訊);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id);
            repo.Delete(客戶銀行資訊);
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
