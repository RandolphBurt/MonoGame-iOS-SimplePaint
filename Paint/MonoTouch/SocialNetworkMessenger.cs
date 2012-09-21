/// <summary>
/// SocialNetworkMessenger.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;	
	using MonoTouch.Social;
	using MonoTouch.UIKit;

	/// <summary>
	/// SocialNetworkMessenger - allow user to post a message to a social network
	/// </summary>
	public class SocialNetworkMessenger
	{
		/// <summary>
		/// The social network composer view.
		/// </summary>
		private SLComposeViewController socialNetworkMessageView = null;

		/// <summary>
		/// The parent controller (owner of the social network form we will display)
		/// </summary>
		private UIViewController parentController;

		/// <summary>
		/// The type of social network we want to interact with.
		/// </summary>
		private SLServiceKind socialNetworkType;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.EmailSender"/> class.
		/// </summary>
		/// <param name='parentController'>
		/// The parent controller (owner of the email form we will display)
		/// </param>
		public SocialNetworkMessenger(UIViewController parentController, SLServiceKind socialNetworkType)
		{
			this.parentController = parentController;
			this.socialNetworkType = socialNetworkType;
		}

		/// <summary>
		/// Posts a message to a social network with the image attached
		/// </summary>
		/// <param name='filename'>
		/// The image to post
		/// </param>
		public void PostImage(string filename)
		{
			if (SLComposeViewController.IsAvailable(this.socialNetworkType))
			{
				this.PostMessage(filename);
			}
			else
			{
				this.DisplayCannotPostMessage();
			}
		}

		/// <summary>
		/// Posts the message to the social network
		/// </summary>
		/// <param name='filename'>
		/// Filename of the image we wish to post
		/// </param>
		private void PostMessage(string filename)
		{
			this.socialNetworkMessageView = SLComposeViewController.FromService(this.socialNetworkType);

			if (this.socialNetworkType == SLServiceKind.Facebook)
			{
				this.socialNetworkMessageView.SetInitialText(LanguageStrings.PostFacebookMessage);
			}
			else
			{
				this.socialNetworkMessageView.SetInitialText(LanguageStrings.TweetMessage);
			}

			UIImage img = UIImage.FromFile(filename);
			this.socialNetworkMessageView.AddImage(img);

			this.socialNetworkMessageView.CompletionHandler += result => {
				this.parentController.InvokeOnMainThread(() => {
					this.parentController.DismissViewController(true, null);
				});
			};

			this.parentController.PresentViewController(this.socialNetworkMessageView, true, null);
		}

		/// <summary>
		/// Informs the user that we are unable to interact with the social network
		/// </summary>
		private void DisplayCannotPostMessage()
		{
			string message = null;

			if (this.socialNetworkType == SLServiceKind.Facebook)
			{
				message = LanguageStrings.FacebookDisabledAlertViewMessage;
			}
			else
			{
				message = LanguageStrings.TweetDisabledAlertViewMessage;
			}

			UIAlertView alert = new UIAlertView()
			{
				Title = LanguageStrings.SocialNetworkDisabledAlertViewTitle,
				Message = message
			};
			
			alert.AddButton(LanguageStrings.OKButtonText);	

			alert.Show();
		}
	}
}

