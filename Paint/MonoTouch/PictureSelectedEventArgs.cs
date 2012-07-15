/// <summary>
/// PictureSelectedEventArgs.cs
/// Randolph Burt - July 2012
/// </summary>
namespace Paint
{
	using System;

	/// <summary>
	/// Picture selected event arguments.
	/// </summary>
	public class PictureSelectedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PictureSelectedEventArgs"/> class.
		/// </summary>
		/// <param name='pictureId'>
		/// Picture identifier.
		/// </param>
		public PictureSelectedEventArgs(Guid pictureId)
		{
			this.PictureId = pictureId;
		}
		
		/// <summary>
		/// Gets the picture identifier.
		/// </summary>
		public Guid PictureId  
		{
			get;
			private set;
		}
	}
}

