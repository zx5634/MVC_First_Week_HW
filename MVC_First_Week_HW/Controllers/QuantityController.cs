using MVC_First_Week_HW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_First_Week_HW.Controllers
{
    public class QuantityController : Controller
    {
        客戶資料Entities db = new 客戶資料Entities();
        // GET: Quantity
        [計算時間Attribute]
        public ActionResult Index()
        {
            var items = db.客戶資料.Select(x => new QuantityView
            {
                客戶名稱 = x.客戶名稱,
                聯絡人數量 = db.客戶聯絡人.Where(y => y.客戶Id == x.Id).Count(),
                銀行帳戶數量 = db.客戶銀行資訊.Where(y => y.客戶Id == x.Id).Count()
            });
            return View(items);
        }

        [計算時間Attribute]
        public FileResult ExportExcel()
        {
            var items = db.客戶資料.Select(x => new QuantityView
            {
                客戶名稱 = x.客戶名稱,
                聯絡人數量 = db.客戶聯絡人.Where(y => y.客戶Id == x.Id).Count(),
                銀行帳戶數量 = db.客戶銀行資訊.Where(y => y.客戶Id == x.Id).Count()
            });
            List<string> show_col = new List<string> { "客戶名稱", "聯絡人數量", "銀行帳戶數量" };
            return Excel.exportExcel(items, "Quantity", show_col);
        }
    }
}