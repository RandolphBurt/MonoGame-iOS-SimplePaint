/// <summary>
/// IUIBusyMessage.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Interface for displaying busy message whilst we undertake a long running task
	/// </summary>
	public interface IUIBusyMessage
	{
		/// <summary>
		/// Show the busy message screen
		/// </summary>
		/// <param name='whenPresented'>The action to run once the form/view is presented on screen</param>
		void Show(Action whenPresented);
	}
}

