/// <summary>
/// PictureIOManager.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Picture IO manager. Handles reading and writing the Images files and the Image Information file
	/// </summary>
	public class PictureIOManager : IPictureIOManager
	{
		/// <summary>
		/// The filename resolver.
		/// </summary>
		private IFilenameResolver filenameResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PictureIOManager"/> class.
		/// </summary>
		/// <param name='filenameResolver'>Filename resolver.</param>
		public PictureIOManager(IFilenameResolver filenameResolver)
		{
			this.filenameResolver = filenameResolver;
		}
		
		/// <summary>
		/// Saves all image data to disk
		/// </summary>
		/// <param name='imageStateData'>Image state data.</param>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		public void SaveData(ImageStateData imageStateData, RenderTarget2D[] undoRedoRenderTargets)
		{
			this.WriteImageStateData(imageStateData);
			
			int end = imageStateData.FirstSavePoint == 0 ? imageStateData.LastSavePoint : imageStateData.MaxUndoRedoCount - 1;
			
			for (int count = 0; count <= end; count++)
			{
				var renderTarget = undoRedoRenderTargets[count];
				
				renderTarget.SaveAsPng(
					this.filenameResolver.ImageSavePointFilename(count),
					renderTarget.Width,
					renderTarget.Height);
			}
			
			
			// TODO - write out real image and also thumbnail images
		}
		
		/// <summary>
		/// Loads existing image data from disk.
		/// </summary>
		/// <param name='device'>Graphics device required for rendering</param>
		/// <param name='spriteBatch'>Sprite Batch for rendering the images into the rendertargets</param>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		/// <param name='backgroundColor'>background color for all rendering</param>
		/// <returns>ImageStateData</returns>
		public ImageStateData LoadData(
			GraphicsDevice device, 
			SpriteBatch spriteBatch, 
			RenderTarget2D[] undoRedoRenderTargets,
			Color backgroundColor)
		{
			var imageStateData = ReadImageStateData();
			
			int end = imageStateData.FirstSavePoint == 0 ? imageStateData.LastSavePoint : imageStateData.MaxUndoRedoCount - 1;
			
			// First write out all the undo/redo render target images
			for (int count = 0; count <= end; count++)
			{
				using(var imageTexture= Texture2D.FromFile(
						device, 
			    		this.filenameResolver.ImageSavePointFilename(count)))
				{
					device.SetRenderTarget(undoRedoRenderTargets[count]);
					spriteBatch.Begin();
					device.Clear(backgroundColor);
					spriteBatch.Draw(imageTexture, Vector2.Zero, backgroundColor);
					spriteBatch.End();
				}
			}

			return imageStateData;
		}		
		
		/// <summary>
		/// Reads the image state data.
		/// </summary>
		/// <returns>
		/// The image state data.
		/// </returns>
		private ImageStateData ReadImageStateData()
		{
			var informationFile = this.filenameResolver.ImageInfoFilename();
			var dataList = new List<int>();
			
			using (var stream = File.OpenRead(informationFile))
			{
				for (short count = 0; count < 4; count++)
				{
					dataList.Add(
						stream.ReadByte() |
						(stream.ReadByte()) << 8 |
						(stream.ReadByte()) << 16 |
						(stream.ReadByte()) << 24);
				}
			}
			
			return new ImageStateData(dataList[0], dataList[1], dataList[2], dataList[3]);				
		}
		
		/// <summary>
		/// Writes the image state data.
		/// </summary>
		/// <param name='imageStateData'>
		/// Image state data.
		/// </param>
		private void WriteImageStateData(ImageStateData imageStateData)
		{
			// Next write out the information file
			var dataArray = new int[] {
				imageStateData.FirstSavePoint,
				imageStateData.LastSavePoint,
				imageStateData.CurrentSavePoint,
				imageStateData.MaxUndoRedoCount
			};
			
			var informationFile = this.filenameResolver.ImageInfoFilename();
			using (var stream = File.Open(informationFile, FileMode.Create, FileAccess.Write))
			{
				foreach (int val in dataArray)
				{
					stream.WriteByte((byte)val);
					stream.WriteByte((byte)(val >> 8));
					stream.WriteByte((byte)(val >> 16));
					stream.WriteByte((byte)(val >> 24));
				}
			}
		}
	}
}

