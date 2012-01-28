/// <summary>
/// ICanvasTool.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for any tools used in the paint app
	/// </summary>
	public interface ICanvasTool
	{
		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		void Draw(bool refreshDisplay = false);
	}
}

