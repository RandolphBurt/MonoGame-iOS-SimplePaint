/// <summary>
/// ImageType.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Image type.
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// Draw an empty square - can be any colour
		/// </summary>
		EmptySquare,
		
		/// <summary>
		/// Draw an exit button
		/// </summary>
		ExitButton,
		
		/// <summary>
		/// Draw a button to minimize the toolbar.
		/// </summary>
		MinimizeToolbar,
		
		/// <summary>
		/// Draw a button to maximize the toolbar.
		/// </summary>
		MaximizeToolbar,
		
		/// <summary>
		/// Draw a button to dock the toolbar at the bottom.
		/// </summary>
		DockBottomButton,
		
		/// <summary>
		/// Draw a button to dock the toolbar at the top.
		/// </summary>
		DockTopButton,
		
		/// <summary>
		/// Draw a save button
		/// </summary>
		SaveButton,
		
		/// <summary>
		/// Draw an undo button
		/// </summary>
		UndoButton,
		
		/// <summary>
		/// Draw a disabled undo button
		/// </summary>
		UndoButtonDisabled,
		
		/// <summary>
		/// Draw a redo button
		/// </summary>
		RedoButton,
		
		/// <summary>
		/// Draw a disabled redo button
		/// </summary>
		RedoButtonDisabled,
		
		/// <summary>
		/// Draw a go-slow icon.
		/// </summary>
		SlowIcon,
		
		/// <summary>
		/// Draw the background for speed gauge
		/// </summary>
		SpeedGaugeBackground,
		
		/// <summary>
		/// Far a go-fast icon.
		/// </summary>
		SpeedIcon,
		
		/// <summary>
		/// Draw a play button.
		/// </summary>
		PlayButton,
		
		/// <summary>
		/// Draw a pause button.
		/// </summary>
		PauseButton,
		
		/// <summary>
		/// Draw the left hand side of the progress bar
		/// </summary>
		ProgressBarLeft,
		
		/// <summary>
		/// Draw the middle of a progress bar.
		/// </summary>
		ProgressBarMiddle,
		
		/// <summary>
		/// Draw the right hand side of the progress bar
		/// </summary>
		ProgressBarRight,
		
		/// <summary>
		/// Draw a restart button.
		/// </summary>
		RestartButton,

		/// <summary>
		/// Draw a disabled play button
		/// </summary>
		PlayButtonDisabled
	}
}

