
using System;
using System.Drawing;
using System.IO;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Paint
{
	public partial class HomeScreen : UIViewController
	{
		private int currentFileIndex = 0;
		private string[] fileList = null;
		private int fileListLength = 0;
		private UIImageView[] imageViewList = null;
		
		public HomeScreen() : base ("HomeScreen", null)
		{
		}
		
		public event EventHandler PaintSelected;

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
				
			this.btnPaint.SetBackgroundImage(UIImage.FromBundle("Content/graphics.png"),UIControlState.Normal);
		
			var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "0fc348d2-83a2-4487-9536-98887c42aa8d");			
			this.fileList = Directory.GetFiles(folder, "*.PNG");
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
			
			this.scrollView.ContentSize = new SizeF(this.scrollView.Frame.Width * 3, this.scrollView.Frame.Height);	
			
			this.scrollView.DecelerationEnded += this.scrollView_DecelerationEnded;
						
			this.LoadImageWithIndex(0, this.fileListLength - 1);
			this.LoadImageWithIndex(1, 0);
			this.LoadImageWithIndex(2, 1);
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
		
		protected virtual void OnPaintSelected(EventArgs e)
		{
			if (this.PaintSelected != null)
			{
				this.PaintSelected(this, EventArgs.Empty);
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
				this.LoadImageWithIndex(0, this.currentFileIndex);
 
			    // Add one to the currentIndex or reset to 0 if we have reached the end.
			    this.currentFileIndex = (this.currentFileIndex >= this.fileListLength -1) ? 0 : this.currentFileIndex + 1;
				this.LoadImageWithIndex(1, this.currentFileIndex);
 
			    // Load content on the last page. This is either from the next item in the array
			    // or the first if we have reached the end.
			    int nextIndex = (this.currentFileIndex >= this.fileListLength -1) ? 0 : this.currentFileIndex + 1;
				this.LoadImageWithIndex(2, nextIndex);
			}
			else 
			{
    			// We are moving backward. Load the current doc data on the last page.
				this.LoadImageWithIndex(2, this.currentFileIndex);
 
			    // Subtract one from the currentIndex or go to the end if we have reached the beginning.
			    this.currentFileIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
				this.LoadImageWithIndex(1, this.currentFileIndex);
 
 			   	// Load content on the first page. This is either from the prev item in the array
			    // or the last if we have reached the beginning.
			    int prevIndex = (this.currentFileIndex == 0) ? this.fileListLength - 1 : this.currentFileIndex - 1;
				this.LoadImageWithIndex(0, prevIndex);
			}
				
			// Reset offset back to middle page
			this.scrollView.SetContentOffset(new PointF(this.imageViewList[1].Frame.X, 0), false);
		}

		partial void btnPaint_TouchUpInside(MonoTouch.UIKit.UIButton sender)
		{
			var a = this.InterfaceOrientation;
			
			this.View.Transform.Rotate(3.14159f * 0.5f);
			
			//this.OnPaintSelected(EventArgs.Empty);
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

	}
}

