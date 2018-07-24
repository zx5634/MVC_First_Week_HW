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
    [Authorize(Roles = "admin")]
    public class 客戶聯絡人Controller : Controller
    {
        客戶聯絡人Repository repo;
        private int pageSize = 5;

        public 客戶聯絡人Controller()
        {
            repo = RepositoryHelper.Get客戶聯絡人Repository();
        }

        // GET: 客戶聯絡人
        [計算時間Attribute]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Validation.DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult Index(客戶聯絡人ViewModel 客戶聯絡人)
        {
            var client = repo.All().Include(x=>x.客戶資料);

            if (!string.IsNullOrEmpty(客戶聯絡人.搜尋姓名))
            {
                client = repo.FindName(客戶聯絡人.搜尋姓名, client);
            }
            if (!string.IsNullOrEmpty(客戶聯絡人.篩選職稱))
            {
                client = repo.GetPosition(客戶聯絡人.篩選職稱, client);
            }
            
            if (!string.IsNullOrEmpty(客戶聯絡人.sort_col))
            {
                bool sort = 客戶聯絡人.isSort;
                if (sort == false)
                {
                    if (客戶聯絡人.sort_col == "客戶名稱")
                    {
                        client = (from o in client
                                 orderby o.客戶資料.客戶名稱
                                 select o);
                    }
                    else
                        client = client.OrderByField(客戶聯絡人.sort_col, true);
                }
                else
                {
                    if (客戶聯絡人.sort_col == "客戶名稱")
                    {
                        client = (from o in client
                                 orderby o.客戶資料.客戶名稱 descending
                                 select o);
                    }
                    else
                        client = client.OrderByField(客戶聯絡人.sort_col, false);
                }
                if (ViewBag.isSort != sort)
                    ViewBag.isSort = sort;
            }
            else
            {
                client = client.OrderBy(c => c.Id);
                ViewBag.isSort = true;
            }
            var orderClient = client.ToPagedList(客戶聯絡人.page == 0 ? 1 : 客戶聯絡人.page, pageSize);
            ViewBag.currentPage = 客戶聯絡人.page == 0 ? "1" : 客戶聯絡人.page.ToString();
            ViewBag.搜尋姓名 = 客戶聯絡人.搜尋姓名;
            ViewBag.篩選職稱 = 客戶聯絡人.篩選職稱;
            ViewBag.職稱 = GetPositionSelect();
            return View(orderClient);
        }

        [計算時間Attribute]
        public FileResult ExportExcel(客戶聯絡人ViewModel 客戶聯絡人)
        {
            List<string> show_col = new List<string> { "職稱", "姓名", "Email", "手機", "電話", "客戶名稱", "客戶名稱" };
            List<string> relationCol = new List<string> { "客戶資料.客戶名稱" };
            return Excel.exportExcel(repo.GetFilterItem(客戶聯絡人.搜尋姓名, 客戶聯絡人.篩選職稱, 客戶聯絡人.sort_col, 客戶聯絡人.isSort),
                "客戶聯絡人", show_col, relationCol);
        }

        [HttpPost]
        [計算時間Attribute]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Validation.DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(客戶聯絡人BatchVM[] data, 客戶聯絡人ViewModel 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                foreach (var vm in data)
                {
                    var client = repo.Find(vm.Id);
                    client.職稱 = vm.職稱;
                    client.手機 = vm.手機;
                    client.電話 = vm.電話;
                }
                repo.UnitOfWork.Commit();
                //return RedirectToAction("Index");
            }
            ViewBag.isSort = 客戶聯絡人.isSort;
            ViewBag.currentPage = 客戶聯絡人.page == 0 ? "1" : 客戶聯絡人.page.ToString();
            ViewBag.搜尋姓名 = 客戶聯絡人.搜尋姓名;
            ViewBag.篩選職稱 = 客戶聯絡人.篩選職稱;
            ViewBag.職稱 = GetPositionSelect();
            ViewData.Model = repo.GetFilterItem(客戶聯絡人.搜尋姓名, 客戶聯絡人.篩選職稱, 客戶聯絡人.sort_col, 客戶聯絡人.isSort).ToPagedList(客戶聯絡人.page == 0 ? 1 : 客戶聯絡人.page, pageSize);
            return View("Index");
        }

        //public ActionResult Search(string keyword)
        //{
        //    var client = repo.FindName(keyword);
        //    return View("Index", client);
        //}

        //public ActionResult Filter(客戶聯絡人 客戶聯絡人)
        //{
        //    var client = repo.GetPosition(客戶聯絡人.職稱);
        //    return View("Index", client);
        //}

        public IEnumerable<SelectListItem> GetPositionSelect()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var category in repo.FindPosition())
            {
                items.Add(new SelectListItem()
                {
                    Text = category,
                    Value = category
                });
            }
            return items;
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo.客戶資料(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            repo.Delete(客戶聯絡人);
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
