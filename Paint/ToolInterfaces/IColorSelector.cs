/// <summary>
/// IColorSelector.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;

	/// <summary>
	/// Interface for a tool that allows the user to specify the exact colour (RGBA value)
	/// </summary>
	public interface IColorSelector
	{
		/// <summary>
		/// Gets or sets the current colour set in this ColorSelector
		/// </summary>
		Color Color 
		{ 
			get;
			set;
		}
		
		/// <summary>
		/// Occurs whenever the user changes the color 
		/// </summary>
		event EventHandler ColorChanged;
	}
}