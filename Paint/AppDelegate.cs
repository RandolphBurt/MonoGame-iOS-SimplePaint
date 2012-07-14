/// <summary>
/// AppDelegate.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;
	
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		/// <summary>
		/// Maximum number of changes we can undo
		/// </summary>
		private const int UndoRedoBufferSize = 10;

		// class-level declarations
		UIWindow window;
		HomeScreen viewController;
		PaintApp paintApp;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			this.window = new UIWindow (UIScreen.MainScreen.Bounds);
			this.viewController = new HomeScreen();
			this.viewController.PaintSelected += (sender, e) => {
				this.EditImage();
			};

			this.window.RootViewController = viewController;
			this.window.MakeKeyAndVisible ();
			
			/*			
			var basePath =  Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library");
			var dir = Path.Combine(basePath, "0fc348d2-83a2-4487-9536-98887c42aa8d");
			if (Directory.Exists(dir))
			{
				foreach (var f in Directory.EnumerateFiles(dir))
				{
//					File.Delete(f);
				}
			}
			                           
			// TODO - launch initial page to select image
			this.EditImage();
*/			
			return true;
		}
		
		public override void WillEnterForeground(UIApplication application)
		{
			// This is called when we re-enable the app from the background
		}
		
		public override void DidEnterBackground(UIApplication application)
		{
			// This is called when we enter the background. 
			// We are never notified if we are terminated.  Therefore we should take this opportunity to save the 
			// current image and info file etc in case we are then terminated
			// Then, if monogame is fixed for saving/loading images then we can just leave the app running in the
			// foreground and we'll be able to carry on.  
			// However if not fixed then we'll have to go back to monotouch front end and let them pick the picture
			// and carry on - problem is we lose our undo/redo buffer unless we also save all of those images?!?!
		}

		//PaintApp paintApp  = null;
		
		int count = 1;
		
		/// <summary>
		/// Edits a specific image.
		/// </summary>
		private void EditImage ()
		{
			// For new image we create new imageStateData(0,0,0,MaxUndoRedo, width, height) and pass in
			Guid pictureId = new Guid("{0fc348d2-83a2-4487-9536-98887c42aa8d}");
			ImageStateData imageStateData = null;
			
			/*
			if (count++%2 == 0)
			{
				pictureId = new Guid("{1fc348d2-83a2-4487-9536-98887c42aa8d}");
			}
			else
			{
				pictureId = new Guid("{0fc348d2-83a2-4487-9536-98887c42aa8d}");
			}*/

			var libraryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library");
			var filenameResolver = new FilenameResolver(pictureId, libraryPath);
			var pictureIOManager = new PictureIOManager(filenameResolver);
			
			imageStateData = pictureIOManager.LoadImageStateData();
			
			if (imageStateData.IsNewImage())
			{
				// new image - so ensure directory structure is in place
				Directory.CreateDirectory(filenameResolver.DataFolder);
			}			
		
			// Simply instantiate the class derived from monogame:game and away we go...
			this.paintApp  = new PaintApp(pictureIOManager, filenameResolver, imageStateData);
			this.paintApp.Exiting += PaintAppExiting;
			this.paintApp.Run();
		}
		
		/// <summary>
		/// Playback an image.
		/// </summary>
		private void PlayBackImage ()
		{
			// Simply instantiate the class derived from monogame:game and away we go...
			CanvasPlaybackApp playBackApp  = new CanvasPlaybackApp();
			playBackApp.Exiting += CanvasPlaybackAppExiting;
			playBackApp.Run();
		}
		
		/// <summary>
		/// Called once the 'playback app' has exited.
		/// </summary>
		/// <param name='sender'>Sender</param>
		/// <param name='e'>Any relevant event args </param>
		private void CanvasPlaybackAppExiting (object sender, EventArgs e)
		{
			// TODO - Go back to main screen
			CanvasPlaybackApp playBackApp = sender as CanvasPlaybackApp;
			if (playBackApp != null)
			{
				playBackApp.Exiting -= CanvasPlaybackAppExiting;
			}
			
			// TODO - temporary code until main screen developed
			this.PlayBackImage();
		}
		
		
		/// <summary>
		/// Called once the 'paint app' has exited.
		/// </summary>
		/// <param name='sender'>Sender</param>
		/// <param name='e'>Any relevant event args </param>
		private void PaintAppExiting (object sender, EventArgs e)
		{
			// TODO - Go back to main screen
			if (this.paintApp != null)
			{
				this.paintApp.Exiting -= PaintAppExiting;
				this.paintApp.Dispose();
				this.paintApp = null;
			}
			
			// TODO - temporary code until main screen developed
			this.EditImage();
		}
	}
}

