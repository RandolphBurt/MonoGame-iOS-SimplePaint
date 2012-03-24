/// <summary>
/// AppDelegate.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;
	
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// TODO - launch initial page to select image
			this.EditImage();
			
			return true;
		}

		/// <summary>
		/// Edits a specific image.
		/// </summary>
		private void EditImage ()
		{
			// Simply instantiate the class derived from monogame:game and away we go...
			PaintApp paintApp  = new PaintApp();
			paintApp.Exiting += PaintAppExiting;
			paintApp.Run();
		}
		
		/// <summary>
		/// Called once the 'paint app' has exited.
		/// </summary>
		/// <param name='sender'>Sender</param>
		/// <param name='e'>Any relevant event args </param>
		private void PaintAppExiting (object sender, EventArgs e)
		{
			// TODO - Go back to main screen
			PaintApp paintApp = sender as PaintApp;
			if (paintApp != null)
			{
				paintApp.Exiting -= PaintAppExiting;
			}
			
			// TODO - temporary code until main screen developed
			this.EditImage();
		}
	}
}

