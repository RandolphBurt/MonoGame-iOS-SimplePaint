/// <summary>
/// IColorPicker.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;

	/// <summary>
	/// Interface for 'color picker' - a tool with a predefined color that the user can choose to use
	/// </summary>
	public interface IColorPicker
	{
		/// <summary>
		/// Gets the color this color picker is associated with
		/// </summary>
		Color Color	{ get; }
		
		/// <summary>
		/// Occurs when color the user picks this color.
		/// </summary>
		event EventHandler ColorSelected;
	}
}