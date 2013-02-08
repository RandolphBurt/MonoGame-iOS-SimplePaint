/// <summary>
/// TwitterIOS5.cs
/// Randolph Burt - Feb 2013
/// </summary>
namespace Paint
{
	using MonoTouch.UIKit;
	using MonoTouch.Twitter;

	/// <summary>
	/// Twitter support for iOS5.
	/// </summary>
	public class TwitterIOS5
	{		
		/// <summary>
		/// The parent controller (owner of the social network form we will display)
		/// </summary>
		private UIViewController parentController;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.TwitterIOS5"/> class.
		/// </summary>
		/// <param name='parentController'>
		/// The parent controller (owner of the email form we will display)
		/// </param>
		public TwitterIOS5(UIViewController parentController)
		{
			this.parentController = parentController;
		}

		/// <summary>
		/// Posts a message to a social network with the image attached
		/// </summary>
		/// <param name='filename'>
		/// The image to post
		/// </param>
		public void PostImage(string filename)
		{
			if (TWTweetComposeViewController.CanSendTweet)
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
			var tweetComposerViewController = new TWTweetComposeViewController();
			tweetComposerViewController.SetInitialText(LanguageStrings.TweetMessage);

			var image = UIImage.FromFile(filename);
			tweetComposerViewController.AddImage(image);

			tweetComposerViewController.SetCompletionHandler((TWTweetComposeViewControllerResult result) => {
				this.parentController.InvokeOnMainThread(() => {
					this.parentController.DismissViewController(true, null);
				});
			});
			
			this.parentController.PresentViewController(tweetComposerViewController, true, null);
		}

		/// <summary>
		/// Informs the user that we are unable to interact with twitter
		/// </summary>
		private void DisplayCannotPostMessage()
		{
			UIAlertView alert = new UIAlertView()
			{
				Title = LanguageStrings.SocialNetworkDisabledAlertViewTitle,
				Message = LanguageStrings.TweetDisabledAlertViewMessage
			};
			
			alert.AddButton(LanguageStrings.OKButtonText);	
			
			alert.Show();
		}
	}
}

