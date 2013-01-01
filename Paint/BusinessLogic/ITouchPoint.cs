/// <summary>
/// ITouchPoint.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;

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

