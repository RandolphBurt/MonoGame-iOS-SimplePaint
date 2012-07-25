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
		/// Saves all the undoRedoRenderTargets to disk and the imageStateData
		/// </summary>
		/// <param name='imageStateData'>Image state data.</param>
		/// <param name='masterImageRenderTarget' Master image render target/>
		/// <param name='undoRedoRenderTargets'>Sequence of images representing the undo/redo chain</param>
		public void SaveData(ImageStateData imageStateData, RenderTarget2D masterImageRenderTarget, RenderTarget2D[] undoRedoRenderTargets)
		{
			this.SaveImageStateData(imageStateData);
			
			int end = imageStateData.FirstSavePoint == 0 ? imageStateData.LastSavePoint : imageStateData.MaxUndoRedoCount - 1;
			
			for (int count = 0; count <= end; count++)
			{
				var renderTarget = undoRedoRenderTargets[count];
				
				renderTarget.SaveAsPng(
					this.filenameResolver.ImageSavePointFilename(count),
					renderTarget.Width,
					renderTarget.Height);
			}
			
			masterImageRenderTarget.SaveAsPng(
				this.filenameResolver.MasterImageFilename,
				masterImageRenderTarget.Width,
				masterImageRenderTarget.Height);
		}
		
		/// <summary>
		///  Loads existing image data from disk into the undoRedoRenderTargets 
		/// </summary>
		/// <param name='device'>
		/// Graphics device required for rendering
		/// </param>
		/// <param name='spriteBatch'>
		/// Sprite Batch for rendering the images into the rendertargets
		/// </param>
		/// <param name='undoRedoRenderTargets'>
		/// Sequence of images representing the undo/redo chain
		/// </param>
		/// <param name='backgroundColor'>
		/// background color for all rendering
		/// </param>
		/// <returns>
		/// ImageStateData
		/// </returns>
		public void LoadUndoRedoRenderTargets(
			GraphicsDevice device, 
			SpriteBatch spriteBatch, 
			RenderTarget2D[] undoRedoRenderTargets,
			Color backgroundColor)
		{
			var imageStateData = this.LoadImageStateData();
			
			int end = imageStateData.FirstSavePoint == 0 ? imageStateData.LastSavePoint : imageStateData.MaxUndoRedoCount - 1;
			
			// Don't read more than we need to - hence above we are calculating whether we have used all the renderTarets
			for (int count = 0; count <= end; count++)
			{
				using (var imageTexture= Texture2D.FromFile(
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
		}		
		
		/// <summary>
		/// Deletes the image and all associated data files
		/// </summary>
		public void DeleteImage()
		{
			File.Delete(this.filenameResolver.MasterImageFilename);
			Directory.Delete(this.filenameResolver.DataFolder, true);
		}
		
		/// <summary>
		/// Copies the image (and all associated data files) to the specified image name
		/// </summary>
		/// <param name='destinationImageFilenameResolver'>
		/// Details of where we need to copy the files.
		/// </param>
		public void CopyImage(IFilenameResolver destinationImageFilenameResolver)
		{
			if (!Directory.Exists(destinationImageFilenameResolver.DataFolder))
			{
				Directory.CreateDirectory(destinationImageFilenameResolver.DataFolder);
			}
			
			File.Copy(this.filenameResolver.ImageInfoFilename, destinationImageFilenameResolver.ImageInfoFilename);
			File.Copy(this.filenameResolver.MasterImageFilename, destinationImageFilenameResolver.MasterImageFilename);
			
			var imageStateData = this.LoadImageStateData();
			int end = imageStateData.FirstSavePoint == 0 ? imageStateData.LastSavePoint : imageStateData.MaxUndoRedoCount - 1;
			
			for (int count = 0; count <= end; count++)
			{
				File.Copy(this.filenameResolver.ImageSavePointFilename(count), destinationImageFilenameResolver.ImageSavePointFilename(count));
				File.Copy(this.filenameResolver.CanvasRecorderFilename(count), destinationImageFilenameResolver.CanvasRecorderFilename(count));
			}			
		}
		
		/// <summary>
		///  Loads the imageStateData from disk 
		/// </summary>
		/// <returns>
		/// The image state data.
		/// </returns>
		public ImageStateData LoadImageStateData()
		{
			if (!File.Exists(this.filenameResolver.ImageInfoFilename))
			{
				throw new FileNotFoundException(String.Format("Image State Data file {0} does not exist", this.filenameResolver.ImageInfoFilename));				                             
			}
			
			var dataList = new List<int>();
			
			using (var stream = File.OpenRead(this.filenameResolver.ImageInfoFilename))
			{
				for (short count = 0; count < 6; count++)
				{
					dataList.Add(
						stream.ReadByte() |
						(stream.ReadByte()) << 8 |
						(stream.ReadByte()) << 16 |
						(stream.ReadByte()) << 24);
				}
			}
			
			return new ImageStateData(
				dataList[0], 
				dataList[1], 
				dataList[2], 
				dataList[3],
				dataList[4],
				dataList[5]);
		}
		
		/// <summary>
		///  Saves the imageStateData to disk
		/// </summary>
		/// <param name='imageStateData'>Image state data.</param>
		private void SaveImageStateData(ImageStateData imageStateData)
		{
			// Next write out the information file
			var dataArray = new int[] {
				imageStateData.Width,
				imageStateData.Height,
				imageStateData.MaxUndoRedoCount,
				imageStateData.FirstSavePoint,
				imageStateData.LastSavePoint,
				imageStateData.CurrentSavePoint				
			};
			
			using (var stream = File.Open(this.filenameResolver.ImageInfoFilename, FileMode.Create, FileAccess.Write))
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

