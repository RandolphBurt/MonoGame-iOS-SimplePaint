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
		/// <param name='filename' The file we are saving to />
		/// </summary>
		void Save(string filename);

		/// <summary>
		/// Load an existing playback file - possibly ready to add more commands to
		/// <param name='filename' The file we are loading from />
		/// </summary>
		void Load(string filename);
	}
}

