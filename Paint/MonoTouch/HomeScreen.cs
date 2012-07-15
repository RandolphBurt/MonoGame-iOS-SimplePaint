
using System;
using System.Drawing;
using System.IO;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.ComponentModel;

namespace Paint
{
	public partial class HomeScreen : UIViewController
	{
		private int currentFileIndex = 0;
		private string[] fileList = null;
		private int fileListLength = 0;
		private UIImageView[] imageViewList = null;
		private UIImageView animatedCircleImage;
		
		public HomeScreen() : base ("HomeScreen", null)
		{
		}
		
		public event EventHandler<PictureSelectedEventArgs> PaintSelected;

		public event EventHandler<PictureSelectedEventArgs> PlaybackSelected;
		
		public event EventHandler NewImagePortraitSelected;
		
		public event EventHandler NewImageLandscapeSelected;

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
				
			// an animating image
			animatedCircleImage = new UIImageView();
			animatedCircleImage.AnimationImages = new UIImage[] {
				  UIImage.FromBundle ("Content/Spinning Circle_1.png")
				, UIImage.FromBundle ("Content/Spinning Circle_2.png")
				, UIImage.FromBundle ("Content/Spinning Circle_3.png")
				, UIImage.FromBundle ("Content/Spinning Circle_4.png")
			} ;
			animatedCircleImage.AnimationRepeatCount = 0;
			animatedCircleImage.AnimationDuration = .5;
			animatedCircleImage.Frame = new RectangleF(110, 20, 100, 100);
			View.AddSubview(animatedCircleImage);
			
			this.btnPaint.SetBackgroundImage(UIImage.FromBundle("Content/graphics.png"),UIControlState.Normal);
		
			this.fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "*.PNG");
			this.fileListLength = this.fileList.Length;
			
			this.imageViewList = new UIImageView[] {
				new UIImageView(),
				new UIImageView(),
				new UIImageView()
			};
			
			int count = 0;
			foreach (var imageView in this.imageViewList)
			{
				imageView.Frame = new System.Drawing.RectangleF(count * this.scrollView.Frame.Width, 0, this.scrollView.Frame.Width, this.scrollView.Frame.Height);
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				this.scrollView.AddSubview(imageView);
				count++;
			}
			
			this.scrollView.ContentSize = new SizeF(this.scrollView.Frame.Width * this.imageViewList.Length, this.scrollView.Frame.Height);	
			
			this.scrollView.DecelerationEnded += this.scrollView_DecelerationEnded;
						
			this.LoadImageWithIndex(0, this.fileListLength - 1);
			this.LoadImageWithIndex(1, 0);
			this.LoadImageWithIndex(2, 1);
			
			// start in the middle
			this.scrollView.SetContentOffset(new PointF(this.imageViewList[1].Frame.X, 0), false);

			/*
			count = 0;
			foreach (var file in fileList)
			{
				this.AddImageWithName(file, count++);
			}

			this.scrollView.ContentSize = new SizeF(this.scrollView.Frame.Width * fileList.Length, this.scrollView.Frame.Height);
			*/
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();

			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;

			ReleaseDesignerOutlets();
		}
		

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
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
 
		private void scrollView_DecelerationEnded (object sender, EventArgs e)
		{
			/* Based on http://www.accella.net/objective-c-using-a-uiscrollview-for-infinite-page-loops/ */
			// All data for the documents are stored in an array (documentTitles).
		  	// We keep track of the index that we are scrolling to so that we
		  	// know what data to load for each page.
		  	if (this.scrollView.ContentOffset.X > this.scrollView.Frame.Size.Width) 
			{
	    		// We are moving forward. Load the current doc data on the first page.
				// this.LoadImageWithIndex(0, this.currentFileIndex);
				this.imageViewList[0].Image = this.imageViewList[1].Image;
 
			    // Add one to the currentIndex or reset to 0 if we have reached the end.
			    this.currentFileIndex = (this.currentFileIndex >= this.fileListLength -1) ? 0 : this.currentFileIndex + 1;
				// this.LoadImageWithIndex(1, this.currentFileIndex);
 				this.imageViewList[1].Image = this.imageViewList[2].Image;

			    // Load content on the last page. This is either from the next item in the array
			    // or the first if we have reached the end.
			    int nextIndex = (this.currentFileIndex >= this.fileListLength -1) ? 0 : this.currentFileIndex + 1;
				this.LoadImageWithIndex(2, nextIndex);
			}
			else 
			{
    			// We are moving backward. Load the current doc data on the last page.
				this.LoadImageWithIndex(2, this.currentFileIndex);
 				this.imageViewList[2].Image = this.imageViewList[1].Image;
 
			    // Subtract one from the currentIndex or go to the end if we have reached the beginning.
			    this.currentFileIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
//				this.LoadImageWithIndex(1, this.currentFileIndex);
 				this.imageViewList[1].Image = this.imageViewList[0].Image;
 
 			   	// Load content on the first page. This is either from the prev item in the array
			    // or the last if we have reached the beginning.
			    int prevIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
				this.LoadImageWithIndex(0, prevIndex);
			}
				
			// Reset offset back to middle page
			this.scrollView.SetContentOffset(new PointF(this.imageViewList[1].Frame.X, 0), false);
		}
		
		partial void btnNewLandscape_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			this.OnNewImageLandscapeSelected(EventArgs.Empty);
		}
		
		partial void btnNewPortrait_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			this.OnNewImagePortraitSelected(EventArgs.Empty);
		}
		
		partial void btnPaint_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			this.animatedCircleImage.StartAnimating();
			
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += (s2, e2) => {
				this.InvokeOnMainThread( delegate {
           			this.OnPaintSelected(new PictureSelectedEventArgs(this.PictureIdFromFile(currentFileIndex)));
				});
			};

        	backgroundWorker.RunWorkerAsync();

        	backgroundWorker.RunWorkerCompleted += (s3, e3) =>
        	{
				this.animatedCircleImage.StopAnimating();
        	};
			
			/*
			var a = this.InterfaceOrientation;
			
			this.View.Transform.Rotate(3.14159f * 0.5f);
			
			this.OnPaintSelected(EventArgs.Empty);
			*/
		}
		
		partial void btnPlayback_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			this.animatedCircleImage.StartAnimating();
			
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += (s2, e2) => {
				this.InvokeOnMainThread( delegate {
           			this.OnPlaybackSelected(new PictureSelectedEventArgs(this.PictureIdFromFile(currentFileIndex)));
				});
			};

        	backgroundWorker.RunWorkerAsync();

        	backgroundWorker.RunWorkerCompleted += (s3, e3) =>
        	{
				this.animatedCircleImage.StopAnimating();
        	};
		}
		
		private void AddImageWithName (string imageString, int position)
		{
			// add image to scroll view
			UIImage image = UIImage.FromFile (imageString);
			UIImageView imageView = new UIImageView (image);
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			
			imageView.Frame = new System.Drawing.RectangleF (position * this.scrollView.Frame.Width, 0, this.scrollView.Frame.Width, this.scrollView.Frame.Height);
			
			this.scrollView.AddSubview(imageView);
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
	}
}

