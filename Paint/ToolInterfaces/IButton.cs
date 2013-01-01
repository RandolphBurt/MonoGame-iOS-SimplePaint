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
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Paint.IButton"/> is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		bool Enabled 
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the current Button State
		/// </summary>
		int State
		{
			get;
			set;
		}
	}
}

