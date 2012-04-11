using System.Web.Mvc;
using Aqueduct.Diagnostics.Monitoring.Sensors;

namespace Aqueduct.Diagnostics.Monitoring.MVC
{
    public class NotificationFilter : FilterAttribute, IActionFilter, IResultFilter
    {
        private System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();



        private static string GetReadingName(ControllerBase controller, string actionName)
        {
            return controller.GetType().Name + "/" + actionName;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new CountSensor(GetReadingName(filterContext.Controller, filterContext.ActionDescriptor.ActionName)).Increment();
            
            _watch.Start();

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _watch.Stop();
            TimingSensor timingSensor = new TimingSensor(GetReadingName(filterContext.Controller, filterContext.ActionDescriptor.ActionName));
            timingSensor.Add(_watch.ElapsedMilliseconds);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }
    }
}
