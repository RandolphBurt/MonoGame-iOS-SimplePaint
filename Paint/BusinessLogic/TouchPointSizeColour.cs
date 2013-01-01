/// <summary>
/// TouchPointSizeColour.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Touch point.
	/// Immutable class for tracking the location and type of 'touch'/gesture made by the user
	/// along with colour and size information
	/// </summary>
	public class TouchPointSizeColour : ITouchPointSizeColor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.TouchPoint"/> class.
		/// </summary>
		/// <param name='position'>The position on screen that the user touched.</param>
		/// <param name='touchType'>Touch type (FreeFrag, Tap etc)</param>
		/// <param name='color'>The color for this point</param>
		/// <param name='size'>The size of the point</param>
		public TouchPointSizeColour(Vector2 position, TouchType touchType, Color color, Rectangle size)
		{
			this.Position = position;
			this.TouchType = touchType;
			this.Color = color;
			this.Size = size;
		}

		/// <summary>
		/// Gets the position on screen that the user touched (0, 0 is top left hand corner)
		/// </summary>
		public Vector2 Position
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the type of the touch (FreeFrag, Tap etc)
		/// </summary>
		public TouchType TouchType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the color for this point
		/// </summary>
		public Color Color 
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the size of the point
		/// </summary>
		public Rectangle Size 
		{
			get;
			private set;
		}
	}
}

