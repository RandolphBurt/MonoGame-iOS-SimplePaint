/// <summary>
/// IUIBusyMessage.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;

	/// <summary>
	/// Handles displaying a 'busy' message before invoking an action
	/// </summary>
	public class BusyMessageDisplay : IUIBusyMessage
	{
		/// <summary>
		/// The busy alert view UI.
		/// </summary>
		private UIBusyAlertView uiBusyAlertView = null;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.BusyMessageDisplay"/> class.
		/// </summary>
		/// <param name='title'>
		/// Title of the 'busy' view
		/// </param>
		/// <param name='message'>
		/// Message to display on the 'busy' view
		/// </param>
		public BusyMessageDisplay(string title, string message)
		{
			this.uiBusyAlertView = new UIBusyAlertView(title, message);
		}
		
		/// <summary>
		/// Show the 'busy' screen 
		/// </summary>
		/// <param name='whenPresented'>
		/// The action to run once the form/view is presented on screen
		/// </param>
		public void Show(Action whenPresented)
		{
			this.uiBusyAlertView.Presented += (sender, e) => 
			{
				whenPresented();
				this.uiBusyAlertView.Hide();
			};

			this.uiBusyAlertView.Show();
		}
	}
}

