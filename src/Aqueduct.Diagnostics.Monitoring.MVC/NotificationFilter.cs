using System.Web.Mvc;
using Aqueduct.Diagnostics.Monitoring.Sensors;

namespace Aqueduct.Diagnostics.Monitoring.MVC
{
    public class NotificationFilter : FilterAttribute, IActionFilter, IResultFilter
    {
        private System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();

        CountSensor sensor = new CountSensor("Request");
        TimingSensor timingSensor = new TimingSensor("RequestExecutionTime");

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string dataPointName = filterContext.Controller.GetType().Name + "/" + filterContext.ActionDescriptor.ActionName;
            SensorBase.SetThreadwiseFeatureName(dataPointName);

            sensor.Increment();
            _watch.Start();

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            _watch.Stop();
            timingSensor.Add(_watch.ElapsedMilliseconds);
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }
    }
}
