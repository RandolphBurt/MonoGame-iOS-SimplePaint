/// <summary>
/// IToggleMinimizeMaximize.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for controls with a minimize/maximize option.
	/// </summary>
	public interface IToggleMinimizeMaximize 
	{
		/// <summary>
		/// Gets the minimized/maximized state of the tool
		/// </summary>
		MinimizedMaximizedState MinimizedMaximizedState { get; }
		
		/// <summary>
		/// Occurs when the state is changed to/from minimized from/to maximized
		/// </summary>
		event EventHandler MinimizeMaximizeStateChanged;
	}
}

