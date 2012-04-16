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
		/// Draw a button to dock the toolbar at the top.
		/// </summary>
		DockTopButton,
		
		/// <summary>
		/// Draw a button to dock the toolbar at the bottom.
		/// </summary>
		DockBottomButton,
		
		/// <summary>
		/// Draw a button to maximize the toolbar.
		/// </summary>
		MaximizeToolbar,
		
		/// <summary>
		/// Draw a button to minimize the toolbar.
		/// </summary>
		MinimizeToolbar,
	}
}

