// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Paint
{
	[Register ("LaunchScreen")]
	partial class LaunchScreen
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView titleImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView authorImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView pictureImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (titleImageView != null) {
				titleImageView.Dispose ();
				titleImageView = null;
			}

			if (authorImageView != null) {
				authorImageView.Dispose ();
				authorImageView = null;
			}

			if (pictureImageView != null) {
				pictureImageView.Dispose ();
				pictureImageView = null;
			}
		}
	}
}
