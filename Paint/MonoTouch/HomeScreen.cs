/// <summary>
/// HoemScreen.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	
	using MonoTouch.Foundation;
	using MonoTouch.Social;
	using MonoTouch.UIKit;
	
	/// <summary>
	/// Home screen.
	/// </summary>
	public partial class HomeScreen : UIViewController
	{
		/// <summary>
		/// The index (within the fileList array) of the currently selected file.
		/// </summary>
		private int currentFileIndex = 0;
		
		/// <summary>
		/// List of all the pictures we have drawn
		/// </summary>
		private List<string> fileList = null;
		
		/// <summary>
		/// Number of files that we have drawn
		/// </summary>
		private int fileListLength = 0;
		
		/// <summary>
		/// How far forward can the user scroll before we need to load the next image
		/// </summary>
		private float maxForwardScroll = 0;

		/// <summary>
		/// How far back can the user scroll before we need to load the next image
		/// </summary>
		private float minBackwardScroll = 0;
		
		/// <summary>
		/// Path to where all the image data (save points and info file) should be saved
		/// </summary>
		private string imageDataPath;

		/// <summary>
		/// Path to where all the master images are stored
		/// </summary>
		private string masterImagePath;

		/// <summary>
		/// Array of the images we will display in our scroll view
		/// </summary>
		private UIImageView[] imageViewList = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.HomeScreen"/> class.
		/// </summary>
		/// <param name='imageDataPath'>
		/// Path to where all the image data (save points and info file) should be saved
		/// </param>
		/// <param name='masterImagePath'>
		/// Path to where all the master images are stor
		/// </param>
		public HomeScreen(string imageDataPath, string masterImagePath) : base ("HomeScreen", null)
		{
			this.masterImagePath = masterImagePath;
			this.imageDataPath = imageDataPath;
		}

		/// <summary>
		/// Occurs when an image has been selected for editing/painting
		/// </summary>
		public event EventHandler<PictureSelectedEventArgs> PaintSelected;

		/// <summary>
		/// Occurs when an image has been selected for playback
		/// </summary>
		public event EventHandler<PictureSelectedEventArgs> PlaybackSelected;

		/// <summary>
		/// Occurs when a new portrait image needs to be created.
		/// </summary>
		public event EventHandler NewImagePortraitSelected;

		/// <summary>
		/// Occurs when a new landscape image needs to be created.
		/// </summary>
		public event EventHandler NewImageLandscapeSelected;

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		/// <Docs>
		/// Called after the controller’s view is loaded into memory.
		/// </Docs>
		/// <summary>
		/// Basic form initialization
		/// </summary>/
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (Device.OSVersion < Device.OSVersion60)
			{
				this.btnFacebook.Hidden = true;
			}

			this.btnPaint.SetImage(UIImage.FromBundle("Content/paint.png"), UIControlState.Normal);
			this.btnPlayback.SetImage(UIImage.FromBundle("Content/playback.png"), UIControlState.Normal);
			this.btnFacebook.SetImage(UIImage.FromBundle("Content/facebook.png"), UIControlState.Normal);
			this.btnTwitter.SetImage(UIImage.FromBundle("Content/twitter.png"), UIControlState.Normal);
			this.btnDelete.SetImage(UIImage.FromBundle("Content/delete.png"), UIControlState.Normal);
			this.btnNewPortrait.SetImage(UIImage.FromBundle("Content/portrait.png"), UIControlState.Normal);
			this.btnNewLandscape.SetImage(UIImage.FromBundle("Content/landscape.png"), UIControlState.Normal);
			this.btnCopy.SetImage(UIImage.FromBundle("Content/copy.png"), UIControlState.Normal);
			this.btnEmail.SetImage(UIImage.FromBundle("Content/email.png"), UIControlState.Normal);
			this.btnExportPhoto.SetImage(UIImage.FromBundle("Content/photo.png"), UIControlState.Normal);

			this.imageViewList = new UIImageView[] {
				new UIImageView(),
				new UIImageView(),
				new UIImageView()
			};

			foreach (var imageView in this.imageViewList)
			{
				this.scrollView.AddSubview(imageView);
			}

			this.PositionScrollViewContent();

			this.scrollView.Scrolled += this.scrollView_Scrolled;

			this.LoadAndDisplayImages();
		}

		/// <summary>
		/// Basic form initialization
		/// </summary>
		/// <param name='animated'>
		/// If set to <c>true</c> animated.
		/// </param>
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			// Since ios6 if we started in landscape mode then the scrollview did not resize properly befre ViewDidLoad
			// was called.  However, if we hook into ViewDidAppear and then resize/check everything then all is good!
			this.PositionScrollViewContent();
		}

		public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation(toInterfaceOrientation, duration);
			
			this.PositionScrollViewContent();
		}

		public override bool ShouldAutorotate()
		{
			return true;
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations (Required for iOS versions < 6.0)
			return true;
		}

		/// <summary>
		/// Loads all of our images and displays them on screen ready for editing
		/// </summary>
		public void LoadAndDisplayImages()
		{
			DirectoryInfo di = new DirectoryInfo(this.masterImagePath);
			FileSystemInfo[] files = di.GetFileSystemInfos("*.JPG");
			this.fileList = files.OrderByDescending(f => f.LastWriteTimeUtc).Select(x => x.FullName).ToList();
			
			this.fileListLength = this.fileList.Count;
			
			if (this.fileListLength > 0)
			{
				this.LoadImageWithIndex(0, this.fileListLength - 1);
				this.LoadImageWithIndex(1, 0);
				this.LoadImageWithIndex(2, Math.Min(this.fileListLength - 1, 1));
			}
			
			this.currentFileIndex = 0;
		}

		/// <summary>
		/// Sets the scroll view ContentSize to be the correct size along with all the images inside it.
		/// </summary>
		private void PositionScrollViewContent()
		{
			int count = 0;
			foreach (var imageView in this.imageViewList)
			{
				imageView.Frame = new System.Drawing.RectangleF(count * this.scrollView.Frame.Width, 0, this.scrollView.Frame.Width, this.scrollView.Frame.Height);
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				count++;
			}
			
			this.scrollView.ContentSize = new SizeF(this.scrollView.Frame.Width * this.imageViewList.Length, this.scrollView.Frame.Height);	
			
			// As soon as the user scrolls half way through the end image then we will reload the next image
			// [Note: We are comparing the x co-ord (left edge of the image) therefore it is only 'half a frame 
			// when scrolling left, but a 'frame and a half when scrolling right'
			this.maxForwardScroll = (this.scrollView.Frame.Width * (imageViewList.Length - 1)) - (this.scrollView.Frame.Width / 2);
			this.minBackwardScroll = this.scrollView.Frame.Width / 2;
			
			// start in the middle of the screen
			this.scrollView.SetContentOffset(new PointF(this.imageViewList[1].Frame.X, 0), false);
		}

		protected virtual void OnNewImageLandscapeSelected(EventArgs e)
		{
			if (this.NewImageLandscapeSelected != null)
			{
				this.NewImageLandscapeSelected(this, EventArgs.Empty);
			}
		}
		
		protected virtual void OnNewImagePortraitSelected(EventArgs e)
		{
			if (this.NewImagePortraitSelected != null)
			{
				this.NewImagePortraitSelected(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPlaybackSelected(PictureSelectedEventArgs e)
		{
			if (this.PlaybackSelected != null)
			{
				this.PlaybackSelected(this, e);
			}
		}
		
		protected virtual void OnPaintSelected(PictureSelectedEventArgs e)
		{
			if (this.PaintSelected != null)
			{
				this.PaintSelected(this, e);
			}
		}
		
		private void LoadImageWithIndex(int uiViewIndex, int fileListIndex)
		{
			this.imageViewList[uiViewIndex].Image = UIImage.FromFile(fileList[fileListIndex]);
		}
 
		private void scrollView_Scrolled(object sender, EventArgs e)
		{
			/* Based on http://www.accella.net/objective-c-using-a-uiscrollview-for-infinite-page-loops/ */
			// Code is improved to ensure no problems with delayed loading if the user scrolls too fast.
			// Thus we are using the Scrolled method rather than the DecelerationEnded method.

			if (this.fileListLength == 0)
			{
				return;
			}

			float xOffset = scrollView.ContentOffset.X;
			
			if (xOffset > this.maxForwardScroll)
			{
				// We are moving forward so move the images to the previous UIImageView
				this.imageViewList[0].Image = this.imageViewList[1].Image;
				this.imageViewList[1].Image = this.imageViewList[2].Image;
 
				// Add one to the currentIndex or reset to 0 if we have reached the end.
				this.currentFileIndex = (this.currentFileIndex >= this.fileListLength - 1) ? 0 : this.currentFileIndex + 1;

				// Load content on the last page. This is either from the next item in the array
				// or the first if we have reached the end.
				int nextIndex = (this.currentFileIndex >= this.fileListLength - 1) ? 0 : this.currentFileIndex + 1;
				this.LoadImageWithIndex(2, nextIndex);

				// reset the scroll position so the user does not realise we've moved the images around
				this.scrollView.ContentOffset = new PointF(xOffset - scrollView.Frame.Width, 0f);
			}
			else if (xOffset < this.minBackwardScroll)
			{
				// We are moving backwards so move the images to the next UIImageView
				this.imageViewList[2].Image = this.imageViewList[1].Image;
				this.imageViewList[1].Image = this.imageViewList[0].Image;
 
				// Subtract one from the currentIndex or go to the end if we have reached the beginning.
				this.currentFileIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
 
				// Load content on the first page. This is either from the prev item in the array
				// or the last if we have reached the beginning.
				int prevIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
				this.LoadImageWithIndex(0, prevIndex);

				// reset the scroll position so the user does not realise we've moved the images around
				this.scrollView.ContentOffset = new PointF(xOffset + scrollView.Frame.Width, 0f);
			}
		}
		
		partial void btnNewLandscape_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			this.InvokeCommandAndShowBusyIndicator(() =>
			{
				this.OnNewImageLandscapeSelected(EventArgs.Empty);
			});
		}

		partial void btnNewPortrait_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			this.InvokeCommandAndShowBusyIndicator(() =>
			{
				this.OnNewImagePortraitSelected(EventArgs.Empty);
			});
		}
		
		partial void btnPaint_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			this.InvokeCommandAndShowBusyIndicator(() =>
			{
				this.OnPaintSelected(this.SelectedPictureEventArgs());
			});
		}

		partial void btnPlayback_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			this.InvokeCommandAndShowBusyIndicator(() =>
			{
				this.OnPlaybackSelected(this.SelectedPictureEventArgs());
			});
		}
		
		partial void btnFacebook_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			this.PostImageToSocialNetwork(this.PictureIdFromFile(currentFileIndex), SLServiceKind.Facebook);
		}

		partial void btnTwitter_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			if (Device.OSVersion < Device.OSVersion60)
			{
				this.PostImageToTwitterIOS5(this.PictureIdFromFile(currentFileIndex));
			}
			else{
				this.PostImageToSocialNetwork(this.PictureIdFromFile(currentFileIndex), SLServiceKind.Twitter);
			}
		}

		partial void btnEmail_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			this.SendEmail(this.PictureIdFromFile(currentFileIndex));
		}

		partial void btnExportPhoto_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			this.ExportImageToPhotoGallery(this.PictureIdFromFile(currentFileIndex));
		}

		partial void btnCopy_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			this.InvokeCommandAndShowBusyIndicator(() => 
			{
				var newFile = this.CopyImage(this.PictureIdFromFile(currentFileIndex));
				this.AddImageToFileListAndScrollToDisplay(newFile);
			});
		}

		partial void btnDelete_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			if (this.fileListLength == 0)
			{
				return;
			}

			UIAlertView alert = new UIAlertView()
			{
				Title = LanguageStrings.DeleteAlertViewTitle,
				Message = LanguageStrings.DeleteAlertViewMessage,
				CancelButtonIndex = 0
			};
			
			alert.AddButton(LanguageStrings.CancelButtonText);
			alert.AddButton(LanguageStrings.OKButtonText);

			alert.Clicked += (s, e) => 
			{
				if (e.ButtonIndex == 1)
				{
					this.InvokeCommandAndShowBusyIndicator(() =>
					{
						this.DeleteCurrentImageFromFileListAndUpdateDisplay();
					});
				}
			};
			
			alert.Show();
		}
	
		private void InvokeCommandAndShowBusyIndicator(Action command)
		{
			this.activityIndicatorView.StartAnimating();
			
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += (s2, e2) => 
			{
				this.InvokeOnMainThread(() =>
				{
					command();
				});
			};
			
			backgroundWorker.RunWorkerAsync();
			
			backgroundWorker.RunWorkerCompleted += (s3, e3) =>
			{
				this.activityIndicatorView.StopAnimating();
			};
		}

		/// <summary>
		/// Adds the image to the file list and then scrolls the screen to display that image
		/// </summary>
		/// <param name='imageFilename'>
		/// Image to add and then display.
		/// </param>
		private void AddImageToFileListAndScrollToDisplay(string imageFilename)
		{
			int newFileLocation = this.currentFileIndex + 1;

			this.fileListLength++;
			this.fileList.Insert(newFileLocation, imageFilename);
			this.LoadImageWithIndex(2, newFileLocation);
			this.scrollView.SetContentOffset(new PointF(this.imageViewList[2].Frame.X, 0), true);
		}

		/// <summary>
		/// Deletes the current image from the file list and updates the display.
		/// </summary>
		private void DeleteCurrentImageFromFileListAndUpdateDisplay()
		{
			var filenameResolver = this.CreateFilenameResolver(this.PictureIdFromFile(this.currentFileIndex));
			var pictureIOManager = new PictureIOManager(filenameResolver);
			
			pictureIOManager.DeleteImage();

			this.fileList.RemoveAt(this.currentFileIndex);
			this.fileListLength--;

			if (this.fileListLength == 0)
			{
				// we've just deleted the last image!
				foreach (var image in this.imageViewList)
				{
					image.Image = null;
				}
			}
			else
			{
				// remove the current image from screen.
				// copy the first image into the middle (current) image
				// then scroll to the next image
				int previousImageIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
				this.LoadImageWithIndex(1, previousImageIndex);
				this.currentFileIndex = previousImageIndex;
				this.scrollView.SetContentOffset(new PointF(this.imageViewList[2].Frame.X, 0), true);
			}
		}

		/// <summary>
		/// Calculates the picture ID based on selected file
		/// </summary>
		/// <returns>
		/// PictureID
		/// </returns>
		/// <param name='selectedFile'>
		/// Selected file.
		/// </param>
		private Guid PictureIdFromFile(int selectedFile)
		{
			return new Guid(Path.GetFileNameWithoutExtension(this.fileList[selectedFile]));
		}

		/// <summary>
		/// Returns a PictureSelectedEventArgs containing a reference to the selected picture
		/// </summary>
		/// <returns>
		/// EventsArgs for the currently selected picture
		/// </returns>
		private PictureSelectedEventArgs SelectedPictureEventArgs()
		{
			return new PictureSelectedEventArgs(this.PictureIdFromFile(currentFileIndex));
		}

		/// <summary>
		/// Posts the image to twitter (when using iOS5).
		/// </summary>
		/// <param name="pictureId">Picture identifier.</param>
		private void PostImageToTwitterIOS5(Guid pictureId)
		{
			var filenameResolver = this.CreateFilenameResolver(pictureId);
			var twitterIOS5 = new TwitterIOS5(this);
			twitterIOS5.PostImage(filenameResolver.MasterImageFilename);
		}

		/// <summary>
		/// Offers the option to post the image to facebook or twitter
		/// </summary>
		/// <param name='pictureId'>
		/// ID of the picture we wish to post
		/// </param>
		/// <param name='socialNetwork'>
		/// Type of social network we wish to post to
		/// </param>
		private void PostImageToSocialNetwork(Guid pictureId, SLServiceKind socialNetwork)
		{
			var filenameResolver = this.CreateFilenameResolver(pictureId);
			var socialNetworkMessenger = new SocialNetworkMessenger(this, socialNetwork);
			socialNetworkMessenger.PostImage(filenameResolver.MasterImageFilename);
		}

		/// <summary>
		/// Offers the option to email the image to someone
		/// </summary>
		/// <param name='pictureId'>
		/// ID of the picture we wish to email
		/// </param>
		private void SendEmail(Guid pictureId)
		{
			var filenameResolver = this.CreateFilenameResolver(pictureId);
			var emailSender = new EmailSender(this);
			emailSender.SendImage(filenameResolver.MasterImageFilename);
		}

		/// <summary>
		/// Exports the image to the photo gallery.
		/// </summary>
		/// <param name='pictureId'>
		/// ID of the photo we wish to export
		/// </param>
		private void ExportImageToPhotoGallery(Guid pictureId)
		{
			var filenameResolver = this.CreateFilenameResolver(pictureId);
			var photoGallery = new PhotoGallery();
			photoGallery.SaveImage(filenameResolver.MasterImageFilename);
		}

		/// <summary>
		/// Copies the image and all associated data
		/// </summary>
		/// <returns>Filename for the newly created image</returns>
		/// <param name='pictureId'>
		/// Identifier of the image to copy
		/// </param>
		private string CopyImage(Guid pictureId)
		{
			var existingFileFilenameResolver = this.CreateFilenameResolver(pictureId);
			var pictureIOManager = new PictureIOManager(existingFileFilenameResolver);

			var newFileFilenameResolver = this.CreateFilenameResolver(Guid.NewGuid());

			pictureIOManager.CopyImage(newFileFilenameResolver);

			return newFileFilenameResolver.MasterImageFilename;
		}

		/// <summary>
		/// Creates the filename resolver.
		/// </summary>
		/// <returns>
		/// The filename resolver.
		/// </returns>
		/// <param name='pictureId'>
		/// Picture identifier.
		/// </param>
		private FilenameResolver CreateFilenameResolver(Guid pictureId)
		{
			return new FilenameResolver(pictureId, this.imageDataPath, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));	
		}
	}
}

