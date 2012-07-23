/// <summary>
/// Code by Macropus
/// http://macropus.net/modal-loading-dialog-in-monotouch/
/// </summary>
namespace Paint
{
	using System;
	using System.Drawing;
	using System.Linq;
	
	using MonoTouch.UIKit;
	
	// Original idea:   http://mymonotouch.wordpress.com/2011/01/27/modal-loading-dialog/
	// Based on:        http://mobiledevelopertips.com/user-interface/uialertview-without-buttons-please-wait-dialog.html

	public class UIBusyAlertView : UIAlertView
	{
		private UIActivityIndicatorView activityIndicatorView;
	 
		public new bool Visible 
		{
        	get;
        	set;
    	}
		
	    public UIBusyAlertView (string title, string message) : base (title, message, null, null, null)
	    {
			this.Visible = false;
	        this.activityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
	        this.AddSubview(this.activityIndicatorView);
	    }
	 
	    public new void Show()
	    {
	        base.Show();
	 
	        this.activityIndicatorView.Frame = new RectangleF((Bounds.Width / 2) - 15, Bounds.Height - 60, 30, 30);
	        this.activityIndicatorView.StartAnimating();
			this.Visible = true;
	    }
	 
	    public void Hide()
	    {
			this.Visible = false;
	        this.activityIndicatorView.StopAnimating();
	 
	        this.BeginInvokeOnMainThread(delegate () {
	            this.DismissWithClickedButtonIndex(0, true);
	        });
	    }
	}
}