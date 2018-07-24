using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_First_Week_HW.Models
{
    public class 計算時間Attribute: ActionFilterAttribute
    {
        public System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            sw.Start();
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            sw.Stop();
            System.Diagnostics.Debug.Print("Action Execute Time = " + sw.ElapsedMilliseconds.ToString());
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            sw.Start();
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            sw.Stop();
            System.Diagnostics.Debug.Print("Result Execute Time = " + sw.ElapsedMilliseconds.ToString());
            base.OnResultExecuted(filterContext);
        }
    }
}