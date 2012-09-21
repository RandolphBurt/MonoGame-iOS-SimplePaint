/// <summary>
/// EmailSender.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;	
	using MonoTouch.MessageUI;
	using MonoTouch.UIKit;

	/// <summary>
	/// Email sender - allow user to fill in destination email address and send a picture to someone
	/// </summary>
	public class EmailSender
	{
		/// <summary>
		/// The email composer view.
		/// </summary>
		private MFMailComposeViewController emailComposerView = null;

		/// <summary>
		/// The parent controller (owner of the email form we will display)
		/// </summary>
		private UIViewController parentController;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.EmailSender"/> class.
		/// </summary>
		/// <param name='parentController'>
		/// The parent controller (owner of the email form we will display)
		/// </param>
		public EmailSender(UIViewController parentController)
		{
			this.parentController = parentController;
		}

		public void SendImage(string filename)
		{
			this.emailComposerView = new MFMailComposeViewController();
			this.emailComposerView.SetSubject(LanguageStrings.EmailSenderMessage);
			this.emailComposerView.SetMessageBody(LanguageStrings.EmailSenderBody, false);

			UIImage img = UIImage.FromFile(filename);
			this.emailComposerView.AddAttachmentData(img.AsPNG(), "image/png", "image.png");

			this.emailComposerView.Finished += (sender, e) => {
				e.Controller.InvokeOnMainThread(() => {
					e.Controller.DismissViewController(true, null);
				});
			};

			this.parentController.PresentViewController(this.emailComposerView, true, null);
		}
	}
}

