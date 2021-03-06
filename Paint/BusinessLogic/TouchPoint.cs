/// <summary>
/// TouchPoint.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Touch point.
	/// Immutable class for tracking the location and type of 'touch'/gesture made by the user
	/// </summary>
	public class TouchPoint : ITouchPoint
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.TouchPoint"/> class.
		/// </summary>
		/// <param name='position'>The position on screen that the user touched.</param>
		/// <param name='touchType'>Touch type (FreeFrag, Tap etc)</param>
		public TouchPoint(Vector2 position, TouchType touchType)
		{
			this.Position = position;
			this.TouchType = touchType;
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
	}
}

