using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin;

namespace FloPotatoes.IOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			Insights.Initialize("171ab89133056e09d9bc53dc25cfe80a7236ae48", "1,0", "FloPotatoes");
			Insights.Identify(UIDevice.CurrentDevice.IdentifierForVendor.ToString(), new Dictionary<string, string>{ {"Device", UIDevice.CurrentDevice.Name}, {"Model", UIDevice.CurrentDevice.LocalizedModel }, {"iOS Version", UIDevice.CurrentDevice.SystemVersion }, });
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
