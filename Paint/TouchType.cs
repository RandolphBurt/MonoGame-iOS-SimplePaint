/// <summary>
/// TouchType.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// TouchType enumeration
	/// The type of gesture made by the user
	/// </summary>
	public enum TouchType
	{
		/// <summary>
		/// User just tapped the screen
		/// </summary>
		Tap,
		
		/// <summary>
		/// User started to drag their finger across the screen
		/// </summary>
		StartDrag,
		
		/// <summary>
		/// User continued to drag their finger across the screen
		/// </summary>
		FreeDrag,
		
		/// <summary>
		/// User completed dragging their finger across the screen.
		/// </summary>
		DragComplete
	}
}

