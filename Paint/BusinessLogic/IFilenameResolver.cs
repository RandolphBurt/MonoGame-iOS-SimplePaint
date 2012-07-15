/// <summary>
/// FilenameResolver.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;

	/// <summary>
	/// IFilenameResolver. Determines the location of all files associated with an image.
	/// </summary>
	public interface IFilenameResolver
	{
		/// <summary>
		/// Gets the data folder.
		/// </summary>
		string DataFolder { get; }

		/// <summary>
		/// Gets the filename to use for the Image Information File
		/// </summary>
		string ImageInfoFilename { get; }
				
		/// <summary>
		/// Gets the master image filename.
		/// </summary>
	 	string MasterImageFilename { get; }
			
		/// <summary>
		/// Determines the filename to use for a 'save point' image (one of the undo/redo render targets)
		/// </summary>
		/// <returns>filename</returns>
		/// <param name='savepoint'>save point id for this image - i.e location in the undo/redo chain</param>
		string ImageSavePointFilename(int savepoint);

		/// <summary>
		/// Determines the filename to use for a 'save point' playback file associated 
		/// with one of the undo/redo render targets
		/// </summary>
		/// <returns>filename</returns>
		/// <param name='savepoint'>save point id for this image - i.e location in the undo/redo chain</param>
		string CanvasRecorderFilename(int savepoint);
	}
}

