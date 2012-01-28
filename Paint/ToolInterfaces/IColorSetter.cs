/// <summary>
/// IColorSetter.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Interface for a ColorSetter tool - simply displays the currently selected color
	/// </summary>
	public interface IColorSetter
	{
		/// <summary>
		/// Gets or sets the color currently being used to draw
		/// </summary>
		Color Color	
		{ 
			get;
			set; 
		}
	}
}