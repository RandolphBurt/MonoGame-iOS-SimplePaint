/// <summary>
/// IToggleDock.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Inteface for a tool that can dock to the top or bottom of the screen
	/// </summary>
	public interface IToggleDock
	{
		/// <summary>
		/// Gets the dock position.
		/// </summary>
		DockPosition DockPosition { get; }
		
		/// <summary>
		/// Occurs when the dock position is changed.
		/// </summary>
		event EventHandler DockPositionChanged;
	}
}

