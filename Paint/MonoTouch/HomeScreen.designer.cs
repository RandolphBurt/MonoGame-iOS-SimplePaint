// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Paint
{
	[Register ("HomeScreen")]
	partial class HomeScreen
	{
		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnPaint { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnPlayback { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnNewLandscape { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnNewPortrait { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnCopy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnDelete { get; set; }

		[Action ("btnNewLandscape_TouchUpInside:")]
		partial void btnNewLandscape_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("btnNewPortrait_TouchUpInside:")]
		partial void btnNewPortrait_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("btnPaint_TouchUpInside:")]
		partial void btnPaint_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("btnPlayback_TouchUpInside:")]
		partial void btnPlayback_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("btnCopy_TouchUpInside:")]
		partial void btnCopy_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("btnDelete_TouchUpInside:")]
		partial void btnDelete_TouchUpInside (MonoTouch.UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (btnPaint != null) {
				btnPaint.Dispose ();
				btnPaint = null;
			}

			if (btnPlayback != null) {
				btnPlayback.Dispose ();
				btnPlayback = null;
			}

			if (btnNewLandscape != null) {
				btnNewLandscape.Dispose ();
				btnNewLandscape = null;
			}

			if (btnNewPortrait != null) {
				btnNewPortrait.Dispose ();
				btnNewPortrait = null;
			}

			if (btnCopy != null) {
				btnCopy.Dispose ();
				btnCopy = null;
			}

			if (btnDelete != null) {
				btnDelete.Dispose ();
				btnDelete = null;
			}
		}
	}
}
