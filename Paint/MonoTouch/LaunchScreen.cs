
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Paint
{
	public partial class LaunchScreen : UIViewController
	{
		public LaunchScreen() : base ("LaunchScreen", null)
		{
		}
		
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public event EventHandler LaunchScreenComplete;
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// TODO - Consider @2x - also better path than going through DefaultImages...
			this.titleImageView.Image = UIImage.FromBundle("Content/launchTitle.png");
			this.authorImageView.Image = UIImage.FromBundle("Content/launchAuthor.png");
			this.pictureImageView.Image = UIImage.FromBundle("Content/DefaultImages/StandardResolution/e2a9c783-d72a-4681-b083-47fd32874933/e2a9c783-d72a-4681-b083-47fd32874933.JPG");

			// Perform any additional setup after loading the view, typically from a nib.
		}

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

			// TODO - make timer a local property so we can properly dispose of it
			var timer = NSTimer.CreateScheduledTimer(6f, () => {
				this.InvokeOnMainThread(() => {
					this.OnLaunchScreenComplete(EventArgs.Empty);
				});
			});
		}

		protected virtual void OnLaunchScreenComplete(EventArgs e)
		{
			if (this.LaunchScreenComplete != null)
			{
				this.LaunchScreenComplete(this, EventArgs.Empty);
			}
		}
	}
}

