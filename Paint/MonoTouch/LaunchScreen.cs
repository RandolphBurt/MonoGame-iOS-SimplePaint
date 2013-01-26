/// <summary>
/// LanguageStrings.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using System;
	
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;

	/// <summary>
	/// The application launch screen.
	/// </summary>
	public partial class LaunchScreen : UIViewController
	{
		/// <summary>
		/// Has the exit process already started / been completed.  
		/// Ensures we don't raise the LAunchScreenComplete event twice (e.g. use touching the screen and the timer completing)
		/// </summary>
		private bool exitInitiated = false;

		/// <summary>
		/// The timer for making the background images appear
		/// </summary>
		private NSTimer timer = null;

		/// <summary>
		/// Gets or sets a value indicating whether the user is allowed to exit early 
		/// (ie. by touching the screen)
		/// </summary>
		public bool AllowUserExit
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.LaunchScreen"/> class.
		/// </summary>
		public LaunchScreen() : base ("LaunchScreen", null)
		{
		}

		/// <Docs>Called when the system is running low on memory.</Docs>
		/// <summary>
		/// Dids the receive memory warning.
		/// </summary>
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		/// <summary>
		/// Occurs when launch screen complete.
		/// </summary>
		public event EventHandler LaunchScreenComplete;

		/// <Docs>Called after the controller’s view is loaded into memory.</Docs>
		/// <summary>
		/// This view has loaded
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.titleImageView.Image = UIImage.FromBundle("Content/launchTitle.png");
			this.authorImageView.Image = UIImage.FromBundle("Content/launchAuthor.png");
			this.pictureImageView.Image = UIImage.FromBundle("Content/launchImage.jpg");
		}

		/// <Docs>Set containing the touches.</Docs>
		/// <summary>
		/// Sent when one or more fingers touches the screen.
		/// </summary>
		/// <param name="touches">Touches.</param>
		/// <param name="evt">Evt.</param>
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			if (this.AllowUserExit)
			{
				this.OnLaunchScreenComplete(EventArgs.Empty);
			}
		}

		/// <summary>
		/// This view is now on screen
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			this.titleImageView.Alpha = 0.0f;
			this.authorImageView.Alpha = 0.0f;
			this.pictureImageView.Alpha = 0.0f;

			this.InvokeOnMainThread(() => {
				UIView.Animate(5f, () => {
					this.titleImageView.Alpha = 1.0f;
					this.authorImageView.Alpha = 1.0f;
					this.pictureImageView.Alpha = 1.0f;
				});
			});

			this.timer = NSTimer.CreateScheduledTimer(6f, () => {
				this.InvokeOnMainThread(() => {
					this.timer.Dispose();
					this.timer = null;
					this.OnLaunchScreenComplete(EventArgs.Empty);
				});
			});
		}

		/// <summary>
		/// Raises the launch screen complete event.
		/// </summary>
		/// <param name="e">E.</param>
		protected virtual void OnLaunchScreenComplete(EventArgs e)
		{
			if (!this.exitInitiated)
			{
				this.exitInitiated = true;

				if (this.LaunchScreenComplete != null)
				{
					this.LaunchScreenComplete(this, EventArgs.Empty);
				}
			}
		}
	}
}

