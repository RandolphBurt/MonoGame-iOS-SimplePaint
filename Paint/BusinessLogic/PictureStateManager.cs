/// <summary>
/// PictureStateManager.cs
/// Randolph Burt - June 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Picture State manager - undo, redo, save.
	/// </summary>
	public class PictureStateManager : IPictureStateManager
	{		
		/// <summary>
		/// The minimum time gap between changes before we will auto save (for undo/redo purposes)
		/// Thus, if a user makes lots of seperate changes (each less than MinTimeGapBeforeSave apart)
		/// then they are counted as one change for the purposes of the undo/redo buffer tracker
		/// </summary>
		private readonly TimeSpan MinTimeGapBeforeSave = new TimeSpan(TimeSpan.TicksPerSecond / 2);
		
		/// <summary>
		/// Is the undo option enabled.
		/// </summary>
		private bool undoEnabled = false;
		
		/// <summary>
		/// Is the redo option enabled.
		/// </summary>
		private bool redoEnabled = false;
		
		/// <summary>
		/// Have there been any changes made since the last time the image was saved.
		/// </summary>
		private bool changesMadeSinceLastSave = false;
		
		/// <summary>
		/// The last time a change (user drawing) was made
		/// </summary>
		private DateTime lastChangeMadeUTC = DateTime.MinValue;
				
		/// <summary>
		/// The canvas recorder.
		/// </summary>
		private ICanvasRecorder canvasRecorder = null;
		
		/// <summary>
		/// The renderTarget handler - tracks the undo/redo of changes in the image
		/// </summary>
		private IRenderTargertHandler renderTargetHandler;
		
		/// <summary>
		/// The filename resolver.
		/// </summary>
		private IFilenameResolver filenameResolver;

		/// <summary>
		/// Handles the saving of the imageStateData
		/// </summary>
		private IPictureIOManager pictureIOManager;
		
		/// <summary>
		/// Current, first and last save points - current status of the image
		/// </summary>
		public ImageStateData ImageStateData
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets a value indicating whether the undo option is enabled.
		/// </summary>
		public bool UndoEnabled
		{
			get
			{
				return this.undoEnabled;
			}
			
			private set
			{
				if (this.undoEnabled != value)
				{
					this.undoEnabled = value;
					this.OnUndoEnabledChanged(EventArgs.Empty);
				}
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the redo option is enabled.
		/// </summary>
		public bool RedoEnabled
		{
			get
			{
				return this.redoEnabled;
			}
			
			private set
			{
				if (this.redoEnabled != value)
				{
					this.redoEnabled = value;
					this.OnRedoEnabledChanged(EventArgs.Empty);
				}
			}
		}
		
		/// <summary>
		/// Occurs when the enabled state of the undo option has changed.
		/// </summary>
		public event EventHandler UndoEnabledChanged;

		/// <summary>
		/// Occurs when the enabled state of the redo option has changed.
		/// </summary>
		public event EventHandler RedoEnabledChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PictureStateManager"/> class.		
		/// <param name='filenameResolver' Determines the filename for all files saved/loaded />
		/// <param name='pictureIOManager' Handles the saving of the imageStateData />
		/// <param name='renderTargetHandler' Handles the rendering of the appropriate render target when undo/redoing a change />
		/// <param name='imageStateData' Current save point, max undo/redo count etc />
		/// </summary>
		public PictureStateManager(IFilenameResolver filenameResolver, IPictureIOManager pictureIOManager, IRenderTargertHandler renderTargetHandler, ImageStateData imageStateData)
		{
			this.canvasRecorder = new CanvasRecorder();
			this.renderTargetHandler = renderTargetHandler;
			this.filenameResolver = filenameResolver;
			this.pictureIOManager = pictureIOManager;
			this.ImageStateData = imageStateData;
		}
		
		/// <summary>
		/// Initialise the picture state manager
		/// <param name='newImage' Indicates if this is a new image or not />
		/// </summary>
		public void InitialisePictureState(bool newImage)
		{
			if (newImage == true)
			{
				this.StoreSavePointData();
				this.UndoEnabled = false;
				this.RedoEnabled = false;
			}
			else
			{
				// We are editing an existing picture
				this.LoadSavePointData();
				
				if (this.ImageStateData.CurrentSavePoint == this.ImageStateData.FirstSavePoint)
				{
					this.UndoEnabled = false;
				}
				else
				{
					this.UndoEnabled = true;
				}
				
				if (this.ImageStateData.CurrentSavePoint == this.ImageStateData.LastSavePoint)
				{
					this.RedoEnabled = false;
				}
				else
				{
					this.RedoEnabled = true;
				}				
			}
		}
		
		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// <param name='color' The color to use for the drawing />
		/// <param name='brush' The brush to use for the drawing />
		/// <param name='touchPointList'>
		/// The list of all gestures / locations touched by the user since the last update
		/// </param>		
		/// </summary>
		public void Draw(List<ITouchPointSizeColor> touchPoints)
		{
			DateTime now = DateTime.UtcNow;

			if (touchPoints.Count == 0)
			{				
				if (this.changesMadeSinceLastSave == true && now - this.lastChangeMadeUTC > this.MinTimeGapBeforeSave)
				{
					this.Save();	
				}
				
				return;
			}
			
			this.lastChangeMadeUTC = now;
			
			this.canvasRecorder.Draw(touchPoints);
			
			if (this.changesMadeSinceLastSave == false)
			{
				this.RedoEnabled = false;
				this.changesMadeSinceLastSave = true;
				
//				this.RemoveFutureSavePoints();
				this.ImageStateData.ResetLastSavePoint();
				this.StoreWorkingImageStateData();

				this.UndoEnabled = true;
			}
		}

		/// <summary>
		/// Undo the most recent change
		/// </summary>
		public void Undo()
		{
			if (this.UndoEnabled == false)
			{
				return;
			}
			
			if (this.changesMadeSinceLastSave == true)
			{
				// temporarily increase the current save point by one and save - then set it back as 
				// we are performing an undo.
				this.ImageStateData.IncrementSavePoint();
				this.StoreSavePointData();
				this.changesMadeSinceLastSave = false;
				this.ImageStateData.ResetLastSavePoint();
			}

			this.ImageStateData.DecrementSavePoint();
			this.StoreWorkingImageStateData();
			this.LoadSavePointData();

			this.RedoEnabled = true;
			
			if (this.ImageStateData.CurrentSavePoint == this.ImageStateData.FirstSavePoint)
			{
				// we are at the beginning - no more undo's available.
				this.UndoEnabled = false;
			}
			else
			{
				this.UndoEnabled = true;
			}
		}
		
		/// <summary>
		/// Redo the most recent undo.
		/// </summary>
		public void Redo()
		{
			if (this.RedoEnabled == false)
			{
				return;
			}
			
			this.UndoEnabled = true;
			this.ImageStateData.IncrementSavePoint();
			this.StoreWorkingImageStateData();
	
			if (this.ImageStateData.CurrentSavePoint == this.ImageStateData.LastSavePoint)
			{
				this.RedoEnabled = false;
			}
			
			this.LoadSavePointData();
		}
		
		/// <summary>
		/// Save the image in its current state
		/// </summary>
		public void Save()
		{			
			if (this.changesMadeSinceLastSave == true)
			{
				this.ImageStateData.IncrementSavePoint();
				this.StoreSavePointData();
			}
			
			this.RedoEnabled = false;
			this.changesMadeSinceLastSave = false;

//			this.RemoveFutureSavePoints();
			this.ImageStateData.ResetLastSavePoint();
			this.StoreWorkingImageStateData();
		}

		/// <summary>
		/// Raises the undo ebabled changed event.
		/// </summary>
		/// <param name='e'>
		/// General EventArgs
		/// </param>
		protected virtual void OnUndoEnabledChanged(EventArgs e)
		{
			if (this.UndoEnabledChanged != null)
			{
				this.UndoEnabledChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the redo ebabled changed event.
		/// </summary>
		/// <param name='e'>
		/// General EventArgs
		/// </param>
		protected virtual void OnRedoEnabledChanged(EventArgs e)
		{
			if (this.RedoEnabledChanged != null)
			{
				this.RedoEnabledChanged(this, EventArgs.Empty);
			}
		}

		/*
		private void RemoveFutureSavePoints()
		{
			for (int count = this.currentSavePoint + 1; count < this.lastSavePoint; count++)
			{
				File.Delete(this.Filename(FileExtensionCanvasRecorder, count));
				// TODO - other file extensions...
			}
			
			this.lastSavePoint = this.currentSavePoint;
		}*/

		/// <summary>
		/// Persists the working image state data to disk
		/// </summary>
		private void StoreWorkingImageStateData()
		{
			this.pictureIOManager.SaveImageStateData(this.filenameResolver.WorkingImageInfoFilename, this.ImageStateData);
		}

		/// <summary>
		/// Stores the save point data.
		/// Saves the CanvasRecorder file to disk in the working folder
		/// Saves the current image into an in memory RenderTarget
		/// </summary>
		private void StoreSavePointData()
		{
			var canvasRecorderFile = this.filenameResolver.WorkingCanvasRecorderFilename(this.ImageStateData.CurrentSavePoint);
			this.canvasRecorder.Save(canvasRecorderFile);
			
			this.renderTargetHandler.StoreSavePoint(this.ImageStateData.CurrentSavePoint);
		}

		/// <summary>
		/// Loads the save point data.
		/// Loads the CanvasRecorder file from the working folder
		/// Gets the image from the specific in memory rendertarget and draws that to the master canvas (rendertarget)
		/// </summary>
		private void LoadSavePointData()
		{
			var canvasRecorderFile = this.filenameResolver.WorkingCanvasRecorderFilename(this.ImageStateData.CurrentSavePoint);
			this.canvasRecorder.Load(canvasRecorderFile);
			
			this.renderTargetHandler.RestoreSavePoint(this.ImageStateData.CurrentSavePoint);
		}
	}
}

