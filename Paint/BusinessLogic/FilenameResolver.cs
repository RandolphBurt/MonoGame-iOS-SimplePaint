/// <summary>
/// FilenameResolver.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;
	using System.IO;
	
	/// <summary>
	/// FilenameResolver
	/// </summary>
	public class FilenameResolver : IFilenameResolver
	{
		/// <summary>
		/// The filename for the InfoFile
		/// </summary>
		private const string InfoFile = "DATA.INF";

		/// <summary>
		/// The file extension to use for the canvas recorder.
		/// </summary>
		private const string FileExtensionCanvasRecorder = "REC";
				
		/// <summary>
		/// The file extension to use for the image files.
		/// PNG files have an alpha channel which we want to keep
		/// </summary>
		private const string FileExtensionPNGImage = "PNG";

		/// <summary>
		/// The file extension to use for the master image file.
		/// JPG files have no alpha channel which is ideal for displaying on the home screen where we don't
		/// want the background image to show through the picture
		/// </summary>
		private const string FileExtensionJPGImage = "JPG";

		/// <summary>
		/// The folder where we store the canvas recorder files whilst we are editing a picture
		/// along with the current copy of the ImageDataFile
		/// Intention is to only save them to the DataFolder when we save at the end when we save all the images.  
		/// Thus if the app crashes then we have not overridden all the canvas recorder files in the data folder
		/// hence no longer matching the image files
		/// </summary>
		private const string WorkingFolderName = "WORKING";
		
		/// <summary>
		/// Unique identifier for this picture
		/// </summary>
		private Guid pictureId;
		
		/// <summary>
		/// Path to where all the image data (save points and info file) should be saved
		/// </summary>
		private string imageDataPath;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.FilenameResolver"/> class.
		/// </summary>
		/// <param name='pictureId'>Picture identifier.</param>
		/// <param name='applicationBasePath'>Application base path.</param>
		public FilenameResolver(Guid pictureId, string imageDataPath, string masterImageFolder)
		{
			this.pictureId = pictureId;
			this.imageDataPath = imageDataPath;
			this.DataFolder = Path.Combine(this.imageDataPath, this.pictureId.ToString());
			this.WorkingFolder = Path.Combine(this.DataFolder, WorkingFolderName);
			this.MasterImageInfoFilename = Path.Combine(this.DataFolder, InfoFile);
			this.WorkingImageInfoFilename = Path.Combine(this.DataFolder, this.WorkingFolder, InfoFile); 
			this.MasterImageFilename = Path.Combine(masterImageFolder, String.Format("{0}.{1}", this.pictureId.ToString(), FileExtensionJPGImage));
		}
		
		/// <summary>
		/// Gets the data folder.
		/// </summary>
		public string DataFolder
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the Canvas Recorder Working folder.
		/// </summary>
		public string WorkingFolder
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the filename to use for the Image Information File
		/// </summary>
		public string MasterImageInfoFilename
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the filename to use for the Image Information File (stored in the working folder) i.e. Is updated as we are
		/// drawing the image 
		/// </summary>
		public string WorkingImageInfoFilename
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the master image filename.
		/// </summary>
		public string MasterImageFilename
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Determines the filename to use for a 'save point' image (one of the undo/redo render targets)
		/// </summary>
		/// <returns>filename</returns>
		/// <param name='savepoint'>save point id for this image - i.e location in the undo/redo chain</param>
		public string ImageSavePointFilename(int savepoint)
		{
			return Path.Combine(
				this.DataFolder, 
				string.Format("{0}.{1}", savepoint, FileExtensionPNGImage));
		}
		
		/// <summary>
		/// Determines the filename to use for a 'save point' playback file associated 
		/// with one of the undo/redo render targets
		/// </summary>
		/// <param name='savepoint'>save point id for this image - i.e location in the undo/redo chain</param>
		/// <returns>filename</returns>
		public string WorkingCanvasRecorderFilename(int savepoint)
		{
			return Path.Combine(
				this.WorkingFolder, 
				string.Format("{0}.{1}", savepoint, FileExtensionCanvasRecorder));
		}

		/// <summary>
		/// Determines the filename to use for a 'save point' playback file associated 
		/// with one of the undo/redo render targets
		/// </summary>
		/// <param name='savepoint'>save point id for this image - i.e location in the undo/redo chain</param>
		/// <returns>filename</returns>
		public string MasterCanvasRecorderFilename(int savepoint)
		{
			return Path.Combine(
				this.DataFolder, 
				string.Format("{0}.{1}", savepoint, FileExtensionCanvasRecorder));
		}
	}
}

