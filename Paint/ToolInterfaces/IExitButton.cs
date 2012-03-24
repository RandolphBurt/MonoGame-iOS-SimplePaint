/// <summary>
/// IExitButton.cs
/// Randolph Burt - March 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for implementing an Exit button
	/// </summary>
	public interface IExitButton
	{
		/// <summary>
		/// Occurs when the user has selected the Exit button
		/// </summary>
		event EventHandler ExitSelected;
	}
}

