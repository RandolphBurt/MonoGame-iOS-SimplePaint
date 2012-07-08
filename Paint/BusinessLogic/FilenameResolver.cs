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
		/// </summary>
		private const string FileExtensionPNGImage = "PNG";
		
		/// <summary>
		/// Unique identifier for this picture
		/// </summary>
		private Guid pictureId;
		
		/// <summary>
		/// Path to the 'home' folder for the app.
		/// </summary>
		private string basePath;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.FilenameResolver"/> class.
		/// </summary>
		/// <param name='pictureId'>Picture identifier.</param>
		/// <param name='applicationBasePath'>Application base path.</param>
		public FilenameResolver(Guid pictureId, string applicationBasePath)
		{
			this.pictureId = pictureId;
			this.basePath =  applicationBasePath;
			this.DataFolder = Path.Combine(this.basePath, this.pictureId.ToString());
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
		/// <returns>filename</returns>
		/// <param name='savepoint'>save point id for this image - i.e location in the undo/redo chain</param>
		public string CanvasRecorderFilename(int savepoint)
		{
			return Path.Combine(
				this.DataFolder, 
				string.Format("{0}.{1}", savepoint, FileExtensionCanvasRecorder));
		}
		
		/// <summary>
		/// Determines the filename to use for the Image Information File
		/// </summary>
		/// <returns>filename</returns>
		public string ImageInfoFilename()
		{
			return Path.Combine(this.DataFolder, InfoFile);
		}
	}
}

