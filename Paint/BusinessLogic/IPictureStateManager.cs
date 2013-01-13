/// <summary>
/// IPictureStateManager.cs
/// Randolph Burt - June 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
		
	/// <summary>
	/// Interface for IPictureStateManager.
	/// </summary>
	public interface IPictureStateManager
	{	
		/// <summary>
		/// Current, first and last save points - current status of the image
		/// </summary>
		ImageStateData ImageStateData { get; }
		
		/// <summary>
		/// Gets a value indicating whether the undo option is enabled.
		/// </summary>
		bool UndoEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether the redo option is enabled.
		/// </summary>
		bool RedoEnabled { get; }
		
		/// <summary>
		/// Occurs when the enabled state of the undo option has changed.
		/// </summary>
		event EventHandler UndoEnabledChanged;

		/// <summary>
		/// Occurs when the enabled state of the redo option has changed.
		/// </summary>
		event EventHandler RedoEnabledChanged;

		/// <summary>
		/// Initialise the picture state manager
		/// <param name='newImage' Indicates if this is a new image or not />
		/// </summary>
		void InitialisePictureState(bool newImage);

		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// <param name='touchPointList'>
		/// The list of all gestures / locations touched by the user since the last update
		/// </param>		
		/// </summary>
		void Draw(List<ITouchPointSizeColor> touchPoints);
		
		/// <summary>
		/// Undo the most recent change
		/// </summary>
		void Undo();
		
		/// <summary>
		/// Redo the most recent undo.
		/// </summary>
		void Redo();
		
		/// <summary>
		/// Save the image in its current state
		/// </summary>
		void Save();
	}
}

