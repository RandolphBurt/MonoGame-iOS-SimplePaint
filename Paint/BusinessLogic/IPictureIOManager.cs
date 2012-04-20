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
		/// Saves all image data to disk
		/// </summary>
		/// <param name='imageStateData'>Image state data.</param>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		void SaveData(ImageStateData imageStateData, RenderTarget2D[] undoRedoRenderTargets);
		
		/// <summary>
		/// Loads existing image data from disk.
		/// </summary>
		/// <param name='device'>Graphics device required for rendering</param>
		/// <param name='spriteBatch'>Sprite Batch for rendering the images into the rendertargets</param>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		/// <param name='backgroundColor'>background color for all rendering</param>
		/// <returns>ImageStateData</returns>
		ImageStateData LoadData(
			GraphicsDevice device, 
			SpriteBatch spriteBatch, 
			RenderTarget2D[] undoRedoRenderTargets,
			Color backgroundColor);
	}
}

