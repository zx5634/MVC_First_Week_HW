namespace MVC_First_Week_HW.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶資料MetaData))]
    public partial class 客戶資料 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶資料Entities db = new 客戶資料Entities();
            if(!string.IsNullOrEmpty(this.帳號))
            {
                var data = db.客戶資料.Where(x => x.帳號 == this.帳號 && x.Id != this.Id).ToList();
                if (data.Count >= 1)
                {
                    yield return new ValidationResult("帳號重複", new string[] { "帳號" });
                }
            }

            //if (!string.IsNullOrEmpty(this.帳號) && string.IsNullOrEmpty(this.密碼))
            //{
            //    yield return new ValidationResult("密碼為空", new string[] { "密碼" });
            //}

            if (string.IsNullOrEmpty(this.帳號) && !string.IsNullOrEmpty(this.密碼))
            {
                yield return new ValidationResult("帳號為空", new string[] { "帳號" });
            }
        }
    }
    
    public partial class 客戶資料MetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶名稱 { get; set; }
        
        [StringLength(8, ErrorMessage="欄位長度不得大於 8 個字元")]
        [Required]
        public string 統一編號 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 電話 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 傳真 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string 地址 { get; set; }
        
        [EmailAddress]
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        public string Email { get; set; }
        
        public bool Disable { get; set; }
        
        [StringLength(200, ErrorMessage="欄位長度不得大於 200 個字元")]
        public string 客戶分類 { get; set; }
        
        public string 帳號 { get; set; }
        
        public string 密碼 { get; set; }
        
        public string Role { get; set; }

        public virtual ICollection<客戶銀行資訊> 客戶銀行資訊 { get; set; }
        public virtual ICollection<客戶聯絡人> 客戶聯絡人 { get; set; }
    }
}
