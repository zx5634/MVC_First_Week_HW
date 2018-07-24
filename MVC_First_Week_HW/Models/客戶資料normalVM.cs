﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_First_Week_HW.Models
{
    public class 客戶資料normalVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 電話 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 傳真 { get; set; }

        [StringLength(100, ErrorMessage = "欄位長度不得大於 100 個字元")]
        public string 地址 { get; set; }

        [EmailAddress]
        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        public string Email { get; set; }
        
        public string 密碼 { get; set; }
    }
}