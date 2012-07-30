/// <summary>
/// TweetSender.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;	
	using MonoTouch.Twitter;
	using MonoTouch.UIKit;

	/// <summary>
	/// Email sender - allow user to fill in destination email address and send a picture to someone
	/// </summary>
	public class TweetSender
	{
		/// <summary>
		/// The email composer view.
		/// </summary>
		private TWTweetComposeViewController tweetComposerView = null;

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
		public TweetSender(UIViewController parentController)
		{
			this.parentController = parentController;
		}

		/// <summary>
		/// Tweets a message to twitter with the image attached
		/// </summary>
		/// <param name='filename'>
		/// The image to tweet
		/// </param>
		public void TweetImage(string filename)
		{
			if (TWTweetComposeViewController.CanSendTweet)
			{
				this.SendTweet(filename);
			}
			else
			{
				this.DisplayCannotTweetMessage();
			}
		}

		/// <summary>
		/// Sends the tweet to twitter
		/// </summary>
		/// <param name='filename'>
		/// Filename of the image we wish to tweet
		/// </param>
		private void SendTweet(string filename)
		{
			this.tweetComposerView = new TWTweetComposeViewController();
			this.tweetComposerView.SetInitialText(LanguageStrings.TweetMessage);

			UIImage img = UIImage.FromFile(filename);
			this.tweetComposerView.AddImage(img);

			this.tweetComposerView.SetCompletionHandler(result => {
				this.parentController.DismissModalViewControllerAnimated(true);
			}
			);

			this.parentController.PresentModalViewController(this.tweetComposerView, true);
		}

		/// <summary>
		/// Informs the user that we are unable to interact with Twitter
		/// </summary>
		private void DisplayCannotTweetMessage()
		{
			UIAlertView alert = new UIAlertView()
			{
				Title = LanguageStrings.TweetDisabledAlertViewTitle,
				Message = LanguageStrings.TweetDisabledAlertViewMessage
			};
			
			alert.AddButton(LanguageStrings.OKButtonText);	

			alert.Show();
		}
	}
}

