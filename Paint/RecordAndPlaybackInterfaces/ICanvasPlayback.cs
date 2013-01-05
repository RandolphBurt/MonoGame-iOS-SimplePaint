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
		/// Gets the percentage of what we've processed so far.
		/// </summary>
		float PercentageRead { get; }

		/// <summary>
		/// Gets the next set of touch points.
		/// </summary>
		/// <param name='maxTouchPoints'>Maximum number of touchpoints that should be retrieved</param>
		/// <returns>
		/// The next set of touch points.
		/// </returns>
		ITouchPointSizeColor[] GetNextTouchPoints(int maxTouchPoints);

		/// <summary>
		/// Reset the input stream to the beginning ready for starting the playback all over again
		/// </summary>
		void Restart();

		/// <summary>
		/// Close any resources (e.g. open files)
		/// </summary>
		void Close();
	}
}

