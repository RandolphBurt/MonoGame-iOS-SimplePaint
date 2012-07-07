/// <summary>
/// ImageStateData.cs
/// Randolph Burt - June 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Image state data.
	/// </summary>
	public class ImageStateData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ImageStateData"/> class.
		/// </summary>
		/// <param name='firstSavePoint'>Which save point is the first one - ie. what is the furthest we can go back
		/// if we continually undo</param>
		/// <param name='lastSavePoint'>Which save point is the last one - ie. indicates how many times we can press
		/// redo</param>
		/// <param name='currentSavePoint'>The current save point</param>
		/// <param name='maxUndoRedoCount'>How big is the undo/redo list</param>
		/// <param name='width'>Width of the image</param>
		/// <param name='height'>Height of the image</param>
		public ImageStateData(
			int firstSavePoint, 
		    int lastSavePoint, 
			int currentSavePoint, 
			int maxUndoRedoCount,
			int width,
			int height)
		{
			this.MaxUndoRedoCount = maxUndoRedoCount;
			this.FirstSavePoint = firstSavePoint;
			this.LastSavePoint = lastSavePoint;
			this.CurrentSavePoint = currentSavePoint;
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// The first save point - starts as zero (beginning of buffer) but that will change as we make more changes
		/// because we can only undoo x times where x is the buffer size
		/// </summary>
		public int FirstSavePoint 
		{
			get;
			private set;
		}

		/// <summary>
		/// The last save point - i.e. max number of times we can select redo from a blank image.
		/// </summary>
		public int LastSavePoint 
		{
			get;
			private set;
		}
					
		/// <summary>
		/// The current save point - i.e. number of times we can press undo before we reach a blank image.
		/// </summary>
		public int CurrentSavePoint
		{
			get;
			private set;
		}
		
		/// <summary>
		/// The maximum number of times we can undo/redo a change.
		/// </summary>
		public int MaxUndoRedoCount
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the Image width.
		/// </summary>
		public int Width  
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the image height.
		/// </summary>
		public int Height  
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Determines whether this instance is new image.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is new image; otherwise, <c>false</c>.
		/// </returns>
		public bool IsNewImage()
		{
			if (this.CurrentSavePoint == 0 &&
			    this.FirstSavePoint == 0 &&
			    this.LastSavePoint == 0)
			{
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Resets the last save point to be the same as the current save point
		/// e.g. if user presses undo and then alters the image then we need to throw away future redo images
		/// - i.e. reset the lastSavePoint that we can go back to as the current save point
		/// </summary>
		public void ResetLastSavePoint()
		{
			this.LastSavePoint = this.CurrentSavePoint;
		}
		
		/// <summary>
		/// Increments the save point by one - but taking into account wrapping back to zero
		/// </summary>
		public void IncrementSavePoint()
		{
			if (++this.CurrentSavePoint == this.MaxUndoRedoCount)
			{
				this.CurrentSavePoint = 0;
			}
			
			if (this.CurrentSavePoint == this.FirstSavePoint)
			{
				// we've wrapped round to start re-using the buffer space so we need to move the firstSavePoint on one 
				// as well to ensure we don't loop too far when undoing the changes
				if (++this.FirstSavePoint == this.MaxUndoRedoCount)
				{
					this.FirstSavePoint = 0;
				}
			}
		}

		/// <summary>
		/// Decrements the save point by one - but taking into account wrapping past zero
		/// </summary>
		public void DecrementSavePoint()
		{
			if (--this.CurrentSavePoint == -1)
			{
				this.CurrentSavePoint = this.MaxUndoRedoCount - 1;
			}
		}		
	}
}

