/// <summary>
/// PhotoGallery.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;
	using MonoTouch.UIKit;

	/// <summary>
	/// Photo gallery - Class allowing interaction with the Photo Gallery
	/// </summary>
	public class PhotoGallery
	{
		/// <summary>
		/// The image file we wish to export to the gallery.
		/// </summary>
		private string imageFile = null;

		/// <summary>
		/// Exports a picture to the photo gallery.
		/// </summary>
		/// <param name='imageFile'>
		/// File to export
		/// </param>
		public void SaveImage(string imageFile)
		{
			this.imageFile = imageFile;
			BusyMessageDisplay busyMessageDisplay = new BusyMessageDisplay("Saving", "Please wait...");
			busyMessageDisplay.Show(new Action(this.ExportPhoto));
		}

		/// <summary>
		/// Exports the photo to the gallery
		/// </summary>
		private void ExportPhoto()
		{
			var saveImage = UIImage.FromFile(this.imageFile);

			saveImage.SaveToPhotosAlbum(null);
		}
	}
}

