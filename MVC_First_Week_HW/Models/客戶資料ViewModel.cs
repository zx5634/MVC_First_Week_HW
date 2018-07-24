using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_First_Week_HW.Models
{
    public class 客戶資料ViewModel
    {
        public int Id { get; set; }
        public string 客戶名稱 { get; set; }
        public string 統一編號 { get; set; }
        public string 電話 { get; set; }
        public string 傳真 { get; set; }
        public string 地址 { get; set; }
        public string Email { get; set; }
        public bool Disable { get; set; }
        public string 客戶分類 { get; set; }
        public string 搜尋客戶名稱 { get; set; }
        public string 篩選分類 { get; set; }
        public string sort_col { get; set; }
        public bool isSort { get; set; }
        public int page { get; set; }
    }
}