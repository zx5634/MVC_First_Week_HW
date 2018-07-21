using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MVC_First_Week_HW.Models
{
    public class Excel
    {
        public static FileStreamResult exportExcel(IQueryable items, string name, List<string> show_col, List<string> relationCol = null)
        {
            string fileName = name + ".xlsx";
            string contentType = "application/vnd.ms-excel";
            var stream = new MemoryStream();
            XLWorkbook workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(name);
            int index = 1;
            int index1 = 0;
            List<string> col_Name = new List<string>();
            foreach (PropertyInfo pi in items.ElementType.GetProperties())
            {
                if (show_col.Contains(pi.Name))
                {
                    sheet.Cell(1, index).Value = pi.Name;
                    col_Name.Add(pi.Name);
                    index++;
                }
            }
            if(relationCol.Count > 0)
            {
                foreach(string str in relationCol)
                {
                    string[] param = str.Split('.');
                    if(param.Length == 2)
                    {
                        sheet.Cell(1, index).Value = param[1];
                        col_Name.Add(param[1]);
                        if (index1 == 0)
                            index1 = index -1;
                        index++;
                    }
                }
            }
            int index2 = 2;
            foreach (var item in items)
            {
                for (int i = 0; i < col_Name.Count; i++)
                {
                    if(i < index1)
                        sheet.Cell(index2, i + 1).Value = item.GetType().GetProperty(col_Name[i]).GetValue(item, null);
                    else
                    {
                        foreach (string str in relationCol)
                        {
                            string[] param = str.Split('.');
                            if (param.Length == 2 && param[1] == col_Name[i])
                            {
                                //var parent = item.GetType().GetProperty(param[0]).GetValue(item, null);
                                //var child = parent.GetType().GetProperty(param[1]).GetValue(parent, null);
                                sheet.Cell(index2, i + 1).Value = GetPropValue(str, item, item.GetType());
                                break;
                            }
                        }
                    }
                }
                index2++;
            }
            workbook.SaveAs(stream);
            stream.Position = 0;
            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = fileName
            };
        }

        public static Object GetPropValue(String name, object obj, Type type)
        {
            var parts = name.Split('.').ToList();
            var currentPart = parts[0];
            PropertyInfo info = type.GetProperty(currentPart);
            if (info == null) { return null; }
            if (name.IndexOf(".") > -1)
            {
                parts.Remove(currentPart);
                return GetPropValue(String.Join(".", parts), info.GetValue(obj, null), info.PropertyType);
            }
            else
            {
                return info.GetValue(obj, null).ToString();
            }
        }

        //public static FileStreamResult export客戶聯絡人Excel(IQueryable<客戶聯絡人> items, string name, List<string> show_col)
        //{
        //    string fileName = name + ".xlsx";
        //    string contentType = "application/vnd.ms-excel";
        //    List<string> relationCol = new List<string> { "客戶名稱" };
        //    var stream = new MemoryStream();
        //    XLWorkbook workbook = new XLWorkbook();
        //    var sheet = workbook.Worksheets.Add(name);
        //    int index = 1;
        //    List<string> col_Name = new List<string>();
        //    foreach (PropertyInfo pi in items.ElementType.GetProperties())
        //    {
        //        if (show_col.Contains(pi.Name))
        //        {
        //            sheet.Cell(1, index).Value = pi.Name;
        //            col_Name.Add(pi.Name);
        //            index++;
        //        }
        //    }
        //    foreach (string str in relationCol)
        //    {
        //        sheet.Cell(1, index).Value = str;
        //        col_Name.Add(str);
        //        index++;
        //    }

        //    int index2 = 2;
        //    foreach (var item in items)
        //    {
        //        for (int i = 0; i < col_Name.Count; i++)
        //        {
        //            if (!relationCol.Contains(col_Name[i]))
        //                sheet.Cell(index2, i + 1).Value = item.GetType().GetProperty(col_Name[i]).GetValue(item, null);
        //            else
        //            {
        //                if (col_Name[i] == "客戶名稱")
        //                    sheet.Cell(index2, i + 1).Value = item.客戶資料.客戶名稱;
        //            }
        //        }
        //        index2++;
        //    }
        //    workbook.SaveAs(stream);
        //    stream.Position = 0;
        //    return new FileStreamResult(stream, contentType)
        //    {
        //        FileDownloadName = fileName
        //    };
        //}

        //public static FileStreamResult export客戶銀行資訊Excel(IQueryable<客戶銀行資訊> items, string name, List<string> show_col)
        //{
        //    string fileName = name + ".xlsx";
        //    string contentType = "application/vnd.ms-excel";
        //    List<string> relationCol = new List<string> { "客戶名稱" };
        //    var stream = new MemoryStream();
        //    XLWorkbook workbook = new XLWorkbook();
        //    var sheet = workbook.Worksheets.Add(name);
        //    int index = 1;
        //    List<string> col_Name = new List<string>();
        //    foreach (PropertyInfo pi in items.ElementType.GetProperties())
        //    {
        //        if (show_col.Contains(pi.Name))
        //        {
        //            sheet.Cell(1, index).Value = pi.Name;
        //            col_Name.Add(pi.Name);
        //            index++;
        //        }
        //    }
        //    foreach (string str in relationCol)
        //    {
        //        sheet.Cell(1, index).Value = str;
        //        col_Name.Add(str);
        //        index++;
        //    }

        //    int index2 = 2;
        //    foreach (var item in items)
        //    {
        //        for (int i = 0; i < col_Name.Count; i++)
        //        {
        //            if (!relationCol.Contains(col_Name[i]))
        //                sheet.Cell(index2, i + 1).Value = item.GetType().GetProperty(col_Name[i]).GetValue(item, null);
        //            else
        //            {
        //                if (col_Name[i] == "客戶名稱")
        //                    sheet.Cell(index2, i + 1).Value = item.客戶資料.客戶名稱;
        //            }
        //        }
        //        index2++;
        //    }
        //    workbook.SaveAs(stream);
        //    stream.Position = 0;
        //    return new FileStreamResult(stream, contentType)
        //    {
        //        FileDownloadName = fileName
        //    };
        //}
    }
}