using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_First_Week_HW.Models
{
    public class 客戶銀行資訊ViewModel
    {
        public int Id { get; set; }
        public int 客戶Id { get; set; }
        public string 銀行名稱 { get; set; }
        public int 銀行代碼 { get; set; }
        public Nullable<int> 分行代碼 { get; set; }
        public string 帳戶名稱 { get; set; }
        public string 帳戶號碼 { get; set; }
        public string 搜尋銀行名稱 { get; set; }
        public string sort_col { get; set; }
        public bool isSort { get; set; }
        public int page { get; set; }
    }
}