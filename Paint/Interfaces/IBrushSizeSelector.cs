/// <summary>
/// IBrushSizeSelector.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;

	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Interface for Brush Size selector tools
	/// </summary>
	public interface IBrushSizeSelector
	{
		/// <summary>
		/// Gets the size of the brush.
		/// </summary>
		int BrushSize { get; }
		
		/// <summary>
		/// Gets or sets the color used by the brush
		/// </summary>
		Color Color 
		{
			get; 
			set; 
		}
		
		/// <summary>
		/// Occurs when the brush size is changed.
		/// </summary>
		event EventHandler BrushSizeChanged;
	}
}

