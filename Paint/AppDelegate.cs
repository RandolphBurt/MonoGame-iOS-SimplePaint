/// <summary>
/// AppDelegate.cs
/// Randolph Burt - January 2012
/// </summary>
using MonoTouch.ObjCRuntime;


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
		/// Path to the library folder
		/// </summary>
		private const string FolderNameLibrary = "Library";
		
		/// <summary>
		/// Path to the ImageData folder
		/// </summary>
		private const string FolderNameImageData = "ImageData";
		
		/// <summary>
		/// Maximum number of changes we can undo
		/// </summary>
		private const int UndoRedoBufferSize = 10;

		/// <summary>
		/// Orientation Setter - allows us to set the device orientation
		/// </summary>
		private Selector orientationSetter;

		/// <summary>
		/// Main window.
		/// </summary>
		private UIWindow window;
		
		/// <summary>
		/// Home screen view controller.
		/// </summary>
		private HomeScreen viewController;
		
		/// <summary>
		/// The paint app.
		/// </summary>
		private PaintApp paintApp;
		
		/// <summary>
		/// The play back app.
		/// </summary>
		private CanvasPlaybackApp playBackApp;
		
		/// <summary>
		/// The path to the imageData folder
		/// </summary>
		private string imageDataPath = null;
		
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			this.imageDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", FolderNameLibrary, FolderNameImageData);
			
			// Temp code below for total clear down 
			/*
			if (Directory.Exists(imageDataPath))
			{
				Directory.Delete(imageDataPath, true);
			}
	
			var fileList=  Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).ToArray();
			if (fileList != null && fileList.Length > 0)
			{
				foreach (var file in fileList)
				{
					File.Delete(file);
				}
			} */
			// Temp code above for total clear down
			
			this.CreateDirectoryStructure();
			
			this.window = new UIWindow (UIScreen.MainScreen.Bounds);
			this.viewController = new HomeScreen();
			
			this.viewController.PaintSelected += (sender, e) => {
				this.EditImage(e.PictureId);
			};
			
			this.viewController.PlaybackSelected += (sender, e) => {
				this.PlaybackImage(e.PictureId);
			};
			
			this.viewController.NewImageLandscapeSelected += (sender, e) => {					                    
				this.NewImage(PictureOrientation.Landscape);
			};
			
			this.viewController.NewImagePortraitSelected += (sender, e) => {
				this.NewImage(PictureOrientation.Portrait);
			};

			this.window.RootViewController = viewController;
			this.window.MakeKeyAndVisible();
			
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
		
		/// <summary>
		/// Start painting a new image
		/// </summary>
		/// <param name='orientation'>
		/// The orientation of the new image
		/// </param>
		private void NewImage(PictureOrientation orientation)
		{
			// If the device is still mid turn then the reported width and height may be wrong - hence we are using 
			// Math.Max and Math.Min to ensure we get the right size.  [Actually I've delayed the turning until we are 
			// ready to display the app  (inside call to EditImage) so it wouldn't have turned yet anyway]
			ImageStateData imageStateData = null;
		
			int deviceWidth = (int)UIScreen.MainScreen.Bounds.Width;
			int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;
			
			if (orientation == PictureOrientation.Landscape)
			{
				imageStateData = new ImageStateData(Math.Max(deviceHeight, deviceWidth), Math.Min(deviceHeight, deviceWidth), 10);
			}
			else
			{
				imageStateData = new ImageStateData(Math.Min(deviceHeight, deviceWidth), Math.Max(deviceHeight, deviceWidth), 10);
			}

			this.EditImage(Guid.NewGuid(), imageStateData);			                 
		}
				
		/// <summary>
		/// Edits a specific image.
		/// </summary>
		/// <param name='pictureId'>
		/// Unique ID referencing the specific image we want to edit
		/// </param>
		/// <param name='imageStateData'>
		/// Image state data for this image - if null then it will be read from disk (so should not be null for new images)
		/// </param>
		private void EditImage(Guid pictureId, ImageStateData imageStateData = null)
		{
			var filenameResolver = this.CreateFilenameResolver(pictureId);
			var pictureIOManager = new PictureIOManager(filenameResolver);
			
			if (imageStateData == null)
			{
				imageStateData = pictureIOManager.LoadImageStateData();
			}
			
			if (imageStateData.IsNewImage())
			{
				// new image - so ensure directory structure is in place
				Directory.CreateDirectory(filenameResolver.DataFolder);
			}			
		
			this.SetOrientationForImage(imageStateData);

			// Simply instantiate the class derived from monogame:game and away we go...
			BusyMessageDisplay busyMessageDisplay = new BusyMessageDisplay("Saving", "Please wait...");

			this.paintApp  = new PaintApp(pictureIOManager, filenameResolver, imageStateData, busyMessageDisplay);
			this.paintApp.Exiting += PaintAppExiting;			
			
			this.paintApp.Run();
		}
		
		/// <summary>
		/// Playback an image.
		/// </summary>
		private void PlaybackImage(Guid pictureId)
		{	
			var filenameResolver = this.CreateFilenameResolver(pictureId);
			var pictureIOManager = new PictureIOManager(filenameResolver);
			ImageStateData imageStateData = pictureIOManager.LoadImageStateData();
			
			this.SetOrientationForImage(imageStateData);
			
			var canvasPlayback = new CanvasPlayback(filenameResolver.CanvasRecorderFilename(imageStateData.CurrentSavePoint));

			this.SetOrientationForImage(imageStateData);

			// Simply instantiate the class derived from monogame:game and away we go...
			this.playBackApp  = new CanvasPlaybackApp(canvasPlayback, imageStateData);
			this.playBackApp.Exiting += CanvasPlaybackAppExiting;
			this.playBackApp.Run();
		}
		
		/// <summary>
		/// Called once the 'playback app' has exited.
		/// </summary>
		/// <param name='sender'>Sender</param>
		/// <param name='e'>Any relevant event args </param>
		private void CanvasPlaybackAppExiting (object sender, EventArgs e)
		{
			if (this.playBackApp != null)
			{
				this.playBackApp.Exiting -= CanvasPlaybackAppExiting;
				this.playBackApp.Dispose();
				this.playBackApp = null;
			}
			
			this.window.MakeKeyAndVisible();
		}
		
		/// <summary>
		/// Called once the 'paint app' has exited.
		/// </summary>
		/// <param name='sender'>Sender</param>
		/// <param name='e'>Any relevant event args </param>
		private void PaintAppExiting (object sender, EventArgs e)
		{
			if (this.paintApp != null)
			{
				this.paintApp.Exiting -= PaintAppExiting;
				this.paintApp.Dispose();
				this.paintApp = null;
			}
			
			this.window.MakeKeyAndVisible();
		}
		
		/// <summary>
		/// Sets the orientation of the device.
		/// </summary>
		/// <param name='requiredOrientation'>
		/// The desired orientation.
		/// </param>
        private void SetOrientation(PictureOrientation requiredOrientation)
        {
            if (this.orientationSetter == null)
            {
                this.orientationSetter = new Selector ("setOrientation:");
            }
			
			bool changeResolution = false;
			UIInterfaceOrientation newOrientation = UIInterfaceOrientation.Portrait;
			
			if (requiredOrientation == PictureOrientation.Landscape)
			{
				if (UIDevice.CurrentDevice.Orientation != UIDeviceOrientation.LandscapeLeft &&
				    UIDevice.CurrentDevice.Orientation != UIDeviceOrientation.LandscapeRight)
				{
					newOrientation = UIInterfaceOrientation.LandscapeLeft;
					changeResolution = true;
				}
			}
			else
			{
				if (UIDevice.CurrentDevice.Orientation != UIDeviceOrientation.Portrait &&
				    UIDevice.CurrentDevice.Orientation != UIDeviceOrientation.PortraitUpsideDown)
				{
					newOrientation = UIInterfaceOrientation.Portrait;
					changeResolution = true;
				}
			}
			
			if (changeResolution)
			{
            	Messaging.void_objc_msgSend_int (UIDevice.CurrentDevice.Handle, this.orientationSetter.Handle, (int)newOrientation);
			}
        }
		
		/// <summary>
		/// Sets the orientation of the device based on the image dimensions.
		/// </summary>
		/// <param name='imageStateData'>
		/// Image state data.
		/// </param>
		private void SetOrientationForImage(ImageStateData imageStateData)
		{
			if (imageStateData.Height > imageStateData.Width)
			{
				this.SetOrientation(PictureOrientation.Portrait);
			}
			else
			{
				this.SetOrientation(PictureOrientation.Landscape);
			}
		}
		
		/// <summary>
		/// Creates the filename resolver.
		/// </summary>
		/// <returns>
		/// The filename resolver.
		/// </returns>
		/// <param name='pictureId'>
		/// Picture identifier.
		/// </param>
		private FilenameResolver CreateFilenameResolver(Guid pictureId)
		{
			return new FilenameResolver(pictureId, imageDataPath, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));	
		}
		
		/// <summary>
		/// Creates the directory structure required by this app
		/// </summary>
		private void CreateDirectoryStructure()
		{
			if (!Directory.Exists(imageDataPath))
			{
				Directory.CreateDirectory(imageDataPath);
			}
		}		
	}
}

