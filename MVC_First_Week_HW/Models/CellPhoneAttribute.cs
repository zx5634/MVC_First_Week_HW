using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC_First_Week_HW.Models
{
    public class CellPhoneAttribute: DataTypeAttribute
    {
        public CellPhoneAttribute() : base(DataType.Text)
        {
            ErrorMessage = "Cell Phone error, e.g. 0911-123456";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            string str = (string)value;
            if (str == "")
                return true;

            return isCellPhoneValid((string) value);
        }

        private bool isCellPhoneValid(string phone)
        {
            return Regex.IsMatch(phone, @"\d{4}-\d{6}", RegexOptions.IgnoreCase);
        }
    }
}