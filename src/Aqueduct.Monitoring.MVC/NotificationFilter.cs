using System.Web.Mvc;
using Aqueduct.Monitoring.Sensors;
using System;

namespace Aqueduct.Monitoring.MVC
{
    public class NotificationFilter : FilterAttribute, IActionFilter, IResultFilter
    {
        private System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();
        private readonly string _groupName;
        private readonly string _featureName;
        
        public NotificationFilter(string featureName, string groupName)
        {
            _featureName = featureName;
            _groupName = groupName;            
        }

        private static string GetReadingName(ControllerBase controller, string actionName)
        {
            return controller.GetType().Name + "/" + actionName;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SensorBase.SetThreadScopedFeatureName(_featureName, _groupName);
            _watch.Reset();
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
