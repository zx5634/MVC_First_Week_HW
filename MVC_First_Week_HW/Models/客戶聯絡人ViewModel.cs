using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_First_Week_HW.Models
{
    public class 客戶聯絡人ViewModel
    {
        public int Id { get; set; }
        public int 客戶Id { get; set; }
        public string 職稱 { get; set; }
        public string 姓名 { get; set; }
        public string Email { get; set; }
        public string 手機 { get; set; }
        public string 電話 { get; set; }
        public string 搜尋姓名 { get; set; }
        public string 篩選職稱 { get; set; }
        public string sort_col { get; set; }
        public bool isSort { get; set; }
        public int page { get; set; }
    }
}