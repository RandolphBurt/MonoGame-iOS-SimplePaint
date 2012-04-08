/// <summary>
/// ICanvasRecorder.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;

	public interface ICanvasRecorder : ICanvas
	{
		/// <summary>
		/// Save the Canvas to file ready for playback.
		/// </summary>
		void Save();
	}
}

