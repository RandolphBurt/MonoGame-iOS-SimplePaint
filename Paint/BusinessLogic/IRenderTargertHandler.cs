/// <summary>
/// IRenderTargertHandler.cs
/// Randolph Burt - June 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for handling the saving and loading of the picture/image for the purposes of undo/redo
	/// </summary>
	public interface IRenderTargertHandler
	{
		/// <summary>
		/// Save the canvas/display to the specified save-point (undo/redo render target).
		/// </summary>
		/// <param name='savePoint'>specific save point we wish to use to store the canvas</param>
		void StoreSavePoint(int savePoint);
		
		/// <summary>
		/// Restore the image held in the specified save point to the canvas/display
		/// </summary>
		/// <param name='savePoint'>specific save point we wish to restore</param>
		void RestoreSavePoint(int savePoint);
	}
}

