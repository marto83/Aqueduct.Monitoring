Monitor
----------

Easy application monitoring. 

Description
------
------

The aqueduct monitor is a tool for gathering statistics inside your application. It has been designed to allow developers to capture statistics from inside you code to help us better 
undestand how specific features of the application are behaving e.g. (Number of database calls, Number of logins, Avegare controller load times anything else you can think of).

How it works
------
------

Sensors collect data in push their readings to a ReadingPublisher. All the readings have a Feature Name and group assigned to them among each other. For example you can collect the number 
of database calls and also the number of actions per minute. Both readings will be called Number but one will be in the Database Feature and the other one in the Application feature. 
If no feature name and group are assigned the default is `Application` and the default group is `Global`. The group is currently only used for the Reading publishers to distignuish 
where they need to process a setting. 

Every minute (`default`) the publisher combines all collected readings (e.g. averages reading data) and notifies the subscribers. 
Go to Subscribers section for more info about subscribers. 


Installation:
------------
------------

Get the Aqueduct.Monitoring nuget package

    Install-Package Aqueduct.Monitoring

Setup
-----
-----
	
Generic setup for any web application. 

    protected void Application_Start() 
	{
		ReadingPublisher.Start((int)TimeSpan.FromMinutes(1).TotalMilliseconds);
	}

	public static void Shutdown ()
    {
        ReadingPublisher.Stop();
    }

To capture errors add the following
     
	protected void Application_Error(object sender, EventArgs args)
    {
        HandleError(Server.GetLastError());
    }
		
	public static void HandleError(Exception lastError)
    {
        Exception error = lastError.GetBaseException();
        if (error is HttpException && ((HttpException)error).ErrorCode == 404) return;
            
        var sensor = new ExceptionSensor();
        sensor.AddError(error);
    }

MVC Application setup

	//Add to Application Start
	MVCNotificationProcessor.Initialise(GlobalFilters.Filters);	

	protected void Application_End()
    {
        MVCNotificationProcessor.Shutdown();
    }

    protected void Application_Error(object sender, EventArgs args)
    {
        MVCNotificationProcessor.HandleError(Server.GetLastError());
    }

The mvc implementation automatically registers a filter that tracks average time controller actions take. 

Collecting stats
---------------
---------------

To collect custom stats you need to declare a sensor. For example you can count how many times you call an external service.
   var countSensor = new CountSesnor("Search webservice"); // give the name of the reading in the constructor
   countSensor.FeatureName = "3rd party integrations"; // we can assign a specific feature name for the reading

   //external service call
   countSensor.Increment(); 

Other sensors are:
 - AmountSensor
 - ExceptionSensor
 - TimingSensor


 Subscribers
 ---------------
 ---------------

 *LoggingSubscriber* - a simple subscriber that outputs directly to the log file. It can be used to debug and identify if you are monitoring the right sections in your code

 *ServerDensitySubscriber*
 
 To install it add the Aqueduct.Monitoring.ServerDensity nuget package.

 more info to follow


  
	


