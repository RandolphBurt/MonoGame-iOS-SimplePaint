/// <summary>
/// IPictureIOManager.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Interface for the picture IO manager.
	/// </summary>
	public interface IPictureIOManager
	{
		/// <summary>
		/// Saves all the undoRedoRenderTargets to disk and the imageStateData
		/// <param name='imageStateData'>Image state data.</param>
		/// <param name='masterImageRenderTarget' Master image render target/>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		void SaveData(ImageStateData imageStateData, RenderTarget2D masterImageRenderTarget, RenderTarget2D[] undoRedoRenderTargets);
		
		/// <summary>
		/// Loads existing image data from disk into the undoRedoRenderTargets
		/// and copies latest canvas recorder files into the 'working folder' 
		/// </summary>
		/// <param name='device'>Graphics device required for rendering</param>
		/// <param name='spriteBatch'>Sprite Batch for rendering the images into the rendertargets</param>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		/// <param name='backgroundColor'>background color for all rendering</param>
		/// <returns>ImageStateData</returns>
		void LoadData(
			GraphicsDevice device, 
			SpriteBatch spriteBatch, 
			RenderTarget2D[] undoRedoRenderTargets,
			Color backgroundColor);

		/// <summary>
		/// Loads the imageStateData from disk
		/// </summary>
		ImageStateData LoadImageStateData();
		
		/// <summary>
		/// Deletes the image and all associated data files
		/// </summary>
		void DeleteImage();
		
		/// <summary>
		/// Copies the image (and all associated data files) to the specified image name
		/// </summary>
		/// <param name='destinationImageFilenameResolver'>
		/// Details of where we need to copy the files.
		/// </param>
		void CopyImage(IFilenameResolver destinationImageFilenameResolver);

		/// <summary>
		/// Creates the directory structure for this image to be saved to disk
		/// </summary>
		void CreateDirectoryStructure();
	
		/// <summary>
		///  Saves the imageStateData to disk
		/// </summary>
		/// <param name='filename'>File to save the image data</param>
		/// <param name='imageStateData'>Image state data.</param>
		void SaveImageStateData(string filename, ImageStateData imageStateData);
	}
}

