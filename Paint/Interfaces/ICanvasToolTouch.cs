/// <summary>
/// ICanvasToolTouch.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for a tool that allows user interaction
	/// </summary>
	public interface ICanvasToolTouch : ICanvasTool
	{
		/// <summary>
		/// Checks wheter a particular touch point (user pressing the screen) is within the bounds of this control.
		/// </summary>
		/// <returns>
		/// The location where the user touched the screen (and type of touch)
		/// </returns>
		/// <param name='touchPosition'>
		/// True = The touchPosition was handled by this control
		/// False = The touchPosition was not handled by this control
		/// </param>
		bool CheckTouchCollision(ITouchPoint touchPosition);
	}
}

