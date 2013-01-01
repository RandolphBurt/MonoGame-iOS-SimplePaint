/// <summary>
/// ICanvasPlayback.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;

	/// <summary>
	/// ICanvas playback.
	/// </summary>
	public interface ICanvasPlayback
	{
		/// <summary>
		/// Gets the current color used for drawing
		/// </summary>
		Color Color { get; }
		
		/// <summary>
		/// Gets the current brush size used for drawing
		/// </summary>
		Rectangle Brush { get; }

		/// <summary>
		/// Gets a value indicating whether there are still any touchpoints left to playback
		/// </summary>
		bool DataAvailable { get; }

		/// <summary>
		/// Gets the next touch point.
		/// </summary>
		/// <returns>
		/// The next touch point.
		/// </returns>
		ITouchPointSizeColor GetNextTouchPoint();

		/// <summary>
		/// Reset the input stream to the beginning ready for starting the playback all over again
		/// </summary>
		void Restart();
	}
}

