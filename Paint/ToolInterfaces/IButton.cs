/// <summary>
/// IButton.cs
/// Randolph Burt - March 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for implementing a button
	/// </summary>
	public interface IButton
	{
		/// <summary>
		/// Occurs when the user has pressed the button
		/// </summary>
		event EventHandler ButtonPressed;
	}
}

