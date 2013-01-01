/// <summary>
/// ITouchPointSizeColor.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// ITouchPointSizeColor - interface for tracking a specific touch/gesture on screen with size and color information
	/// </summary>
	public interface ITouchPointSizeColor : ITouchPoint
	{
		/// <summary>
		/// Gets the color for this point
		/// </summary>
		Color Color { get; }

		/// <summary>
		/// Gets the size of the point
		/// </summary>
		Rectangle Size { get; }
	}
}

