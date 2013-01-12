/// <summary>
/// DefaultImageInstaller.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using System;
	using System.IO;

	/// <summary>
	/// Default image installer.
	/// </summary>
	public class DefaultImageInstaller : IDefaultImageInstaller
	{
		/// <summary>
		/// The device scale/resolution. 1 = normal.  2 = retina.
		/// </summary>
		private int deviceScale;

		/// <summary>
		/// The path to the imageData folder
		/// </summary>
		private string imageDataPath;
		
		/// <summary>
		/// Path to where all the master images are stored
		/// </summary>
		private string masterImagePath;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.DefaultImageInstaller"/> class.
		/// </summary>
		/// <param name='imageDataPath'>The path to the imageData folder</param>
		/// <param name='masterImagePath'>Path to where all the master images are stored</param>
		/// <param name='deviceScale'>The device scale/resolution. 1 = normal.  2 = retina.</param>
		public DefaultImageInstaller(string imageDataPath, string masterImagePath, int deviceScale)
		{
			this.deviceScale = deviceScale;
			this.imageDataPath = imageDataPath;
			this.masterImagePath = masterImagePath;
		}

		/// <summary>
		/// Installs the default images.
		/// </summary>
		public void InstallDefaultImages()
		{
			string imageFolder = this.deviceScale == 1 ? "StandardResolution" : "RetinaResolution";
			var searchFolder = Path.Combine("Content/DefaultImages", imageFolder);

			var folderList = Directory.EnumerateDirectories(searchFolder);
			foreach (var folder in folderList)
			{
				var pictureID = Path.GetFileName(folder);
				var imageDestination = Path.Combine(this.masterImagePath, String.Format("{0}.JPG", pictureID));
				var imageSource = Path.Combine(folder, String.Format("{0}.JPG", pictureID));

				if (!File.Exists(imageDestination) && File.Exists(imageSource))
				{
					// Copy the master jpg image
					File.Copy(imageSource, imageDestination);
				}

				var destinationImageDataFolder = Path.Combine(this.imageDataPath, pictureID);
				if (!File.Exists(destinationImageDataFolder))
				{
					// Ceate a folder for the data info file and undo/redo render buffer files...
					Directory.CreateDirectory(destinationImageDataFolder);
				}

				foreach (string fileToCopy in new string[] { "DATA.INF", "0.REC", "0.PNG", "1.REC", "1.PNG" })
				{
					// ... and then copy the data in.
					// Note the 0.xxx files contain blank data so the user can undo to a blank screen. Not idea however if we just had a single 
					// image/recorder file then the app thinks that it is a new image and shows it blank.  Might fix properly one day...
					var sourceDataFile = Path.Combine(folder, fileToCopy);
					var destinationDataFile = Path.Combine(destinationImageDataFolder, fileToCopy);

					if (!File.Exists(destinationDataFile) && File.Exists(sourceDataFile))
					{
						File.Copy(sourceDataFile, destinationDataFile);
					}
				}
			}
		}
	}
}

