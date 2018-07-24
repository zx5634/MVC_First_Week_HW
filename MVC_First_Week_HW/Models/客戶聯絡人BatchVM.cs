using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_First_Week_HW.Models
{
    public class 客戶聯絡人BatchVM
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        [CellPhone]
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 手機 { get; set; }
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    }
}