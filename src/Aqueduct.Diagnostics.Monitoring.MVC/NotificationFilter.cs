using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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
            SensorBase.SetDataPointName(dataPointName);

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
