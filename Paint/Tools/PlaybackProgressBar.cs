/// <summary>
/// PlaybackProgressBar.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;

	/// <summary>
	/// Playback progress bar.
	/// </summary>
	public class PlaybackProgressBar : IPlaybackProgressBar, IToolBoxTool
	{
		/// <summary>
		/// The previous percentage through the progress bar
		/// </summary>
		private float previousPercentage;

		/// <summary>
		/// Contains all the graphics for rendering the tools
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;

		/// <summary>
		/// The playback progress bar definition.
		/// </summary>
		private PlaybackProgressBarDefinition playbackProgressBarDefinition;

		/// <summary>
		/// The area for the left graphic for this toolbox item
		/// </summary>
		private Rectangle boundsLeftImage;

		/// <summary>
		/// The area for the right graphic for this toolbox item
		/// </summary>
		private Rectangle boundsRightImage;

		/// <summary>
		/// The area for the middle graphic for this toolbox item
		/// </summary>
		private Rectangle boundsMiddleImage;

		/// <summary>
		/// The area for the progress indicator line
		/// </summary>
		private Rectangle boundsProgressIndicator;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PlaybackProgressBar"/> class.
		/// </summary>
		/// <param name='graphicsDisplay' Contains all the graphics for rendering the tools />
		/// <param name='colorSetterDefinition' The layout of the progress bar />
		public PlaybackProgressBar(IGraphicsDisplay graphicsDisplay, PlaybackProgressBarDefinition playbackProgressBarDefinition)
		{
			this.graphicsDisplay = graphicsDisplay;
			this.playbackProgressBarDefinition = playbackProgressBarDefinition;

			var graphicsRectangleProgressBarLeft = this.graphicsDisplay.SourceRectangleFromImageType(ImageType.ProgressBarLeft);
			var graphicsRectangleProgressBarRight = this.graphicsDisplay.SourceRectangleFromImageType(ImageType.ProgressBarRight);

			var yDiff = this.playbackProgressBarDefinition.Bounds.Height - graphicsRectangleProgressBarLeft.Height;
			
			this.boundsLeftImage = new Rectangle(
				this.playbackProgressBarDefinition.Bounds.X, 
				this.playbackProgressBarDefinition.Bounds.Y + yDiff / 2,
				graphicsRectangleProgressBarLeft.Width,
				this.playbackProgressBarDefinition.Bounds.Height - yDiff);

			this.boundsRightImage = new Rectangle(
				this.playbackProgressBarDefinition.Bounds.X + (this.playbackProgressBarDefinition.Bounds.Width - graphicsRectangleProgressBarRight.Width), 
				this.playbackProgressBarDefinition.Bounds.Y + yDiff / 2,
				graphicsRectangleProgressBarRight.Width,
				this.playbackProgressBarDefinition.Bounds.Height - yDiff);

			this.boundsMiddleImage = new Rectangle(
				this.playbackProgressBarDefinition.Bounds.X + this.boundsLeftImage.Width, 
				this.playbackProgressBarDefinition.Bounds.Y + yDiff / 2,
				this.playbackProgressBarDefinition.Bounds.Width - (graphicsRectangleProgressBarRight.Width + graphicsRectangleProgressBarLeft.Width),
				this.playbackProgressBarDefinition.Bounds.Height - yDiff);

			var indicatorXOffset = (this.playbackProgressBarDefinition.Bounds.Width - this.playbackProgressBarDefinition.ProgresIndicatorWidth) / 2;
			var indicatorYOffset = (this.playbackProgressBarDefinition.Bounds.Height - this.playbackProgressBarDefinition.ProgressIndicatorHeight) / 2;

			this.boundsProgressIndicator = new Rectangle(
				this.playbackProgressBarDefinition.Bounds.X + indicatorXOffset,
				this.playbackProgressBarDefinition.Bounds.Y + indicatorYOffset,
				this.playbackProgressBarDefinition.ProgresIndicatorWidth,
				this.playbackProgressBarDefinition.ProgressIndicatorHeight);
		}

		/// <summary>
		/// Gets or sets the percentage through the progress bar
		/// </summary>
		public float Percentage
		{
			get;
			set;
		}

		public void Draw(bool refreshDisplay)
		{
			if (refreshDisplay || this.Percentage != this.previousPercentage)
			{
				this.graphicsDisplay.DrawGraphic(ImageType.ProgressBarLeft, this.boundsLeftImage, this.playbackProgressBarDefinition.BackgroundColor);
				this.graphicsDisplay.DrawGraphic(ImageType.ProgressBarMiddle, this.boundsMiddleImage, this.playbackProgressBarDefinition.BackgroundColor);
				this.graphicsDisplay.DrawGraphic(ImageType.ProgressBarRight, this.boundsRightImage, this.playbackProgressBarDefinition.BackgroundColor);

				this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, this.boundsProgressIndicator, this.playbackProgressBarDefinition.BackgroundColor);

				var boundsIndicatorPercentage = new Rectangle(
					this.boundsProgressIndicator.X,
					this.boundsProgressIndicator.Y,
					(int) (this.boundsProgressIndicator.Width * (this.Percentage / 100f)),
					this.boundsProgressIndicator.Height);

				this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, boundsIndicatorPercentage, this.playbackProgressBarDefinition.ProgressIndicatorColor);

				this.previousPercentage = this.Percentage;
			}
		}
	}
}

