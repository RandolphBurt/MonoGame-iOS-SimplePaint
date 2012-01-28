/// <summary>
/// ITouchPoint.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;
	
	/// <summary>
	/// ITouchPoint - interface for tracking a specific touch/gesture on screen
	/// </summary>
	public interface ITouchPoint
	{
		/// <summary>
		/// Gets the position on screen that the user touched (0, 0 is top left hand corner)
		/// </summary>
		Vector2 Position { get; } 
		
		/// <summary>
		/// Gets the type of the touch (FreeFrag, Tap etc)
		/// </summary>
		TouchType TouchType { get; } 
	}
}

