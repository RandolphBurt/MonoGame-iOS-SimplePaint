/// <summary>
/// SpeedGauge.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using System;

	using Microsoft.Xna.Framework;

	/// <summary>
	/// Speed gauge toolbox item
	/// </summary>
	public class SpeedGauge : ToolBoxToolTouchBase, ISpeedGauge
	{
		/// <summary>
		/// The speed gauge definition - layout of this toolbox item
		/// </summary>
		private SpeedGaugeDefinition speedGaugeDefinition;

		/// <summary>
		/// The gauge.
		/// </summary>
		private Gauge gauge;

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
		/// The area for the gauge
		/// </summary>
		private Rectangle gaugeBounds;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.SpeedGauge"/> class.
		/// </summary>
		/// <param name='graphicsDisplay'>raphics display.</param>
		/// <param name='speedGaugeDefinition'>Speed gauge definition.</param>
		public SpeedGauge(IGraphicsDisplay graphicsDisplay, SpeedGaugeDefinition speedGaugeDefinition)
			: base(
				speedGaugeDefinition.BackgroundColor, 
				speedGaugeDefinition.BorderColor, 
				speedGaugeDefinition.BorderWidth, 
				graphicsDisplay, 
				speedGaugeDefinition.Bounds) 
		{
			this.speedGaugeDefinition = speedGaugeDefinition;

			this.CreateGauge(graphicsDisplay);
			this.CreateGraphicsRectangles(graphicsDisplay);
		}

		/// <summary>
		/// Occurs when the speed indicator changes
		/// </summary>
		public event EventHandler SpeedChanged;

		/// <summary>
		/// Gets or sets the position of the marker (0.0 -> 1.0)
		/// </summary>
		public float Speed
		{
			get
			{
				return this.gauge.Marker;
			}

			set
			{
				this.gauge.Marker = value;
			}
		}

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates
		/// </param>
		public override void Draw(bool refreshDisplay)
		{
			if (refreshDisplay)
			{
				this.BlankAndRedrawWithBorder();
				this.DrawGraphic(ImageType.SlowIcon, this.boundsLeftImage);
				this.DrawGraphic(ImageType.SpeedGaugeBackground, this.boundsMiddleImage);
				this.DrawGraphic(ImageType.SpeedIcon, this.boundsRightImage);
			}

			this.gauge.Draw(refreshDisplay);
		}

		/// <summary>
		/// Handles a particular touch by the user
		/// </summary>
		/// <param name='touch'>The position and type of gesture/touch made</param>
		/// <param name='touchPosition'>Touch position.</param>
		protected override void HandleTouch(ITouchPoint touchPosition)
		{
			var gaugeTouchPoint = touchPosition;

			if (this.boundsMiddleImage.Contains(touchPosition.Position))
			{
				// if the user starts to press the gauge but slightly misses it (as it is quite small) and hence
				// their Y is just above/below the gauge then we alter the Y position to ensure that the touch
				// will get picked up on
				gaugeTouchPoint = new TouchPoint(
					new Vector2(touchPosition.Position.X, this.gaugeBounds.Y), 
					touchPosition.TouchType);
			}
				
			this.gauge.CheckTouchCollision(gaugeTouchPoint);
		}

		/// <summary>
		/// Raises the speed changed event.
		/// </summary>
		/// <param name='e'>
		/// Empty EventArgs.
		/// </param>
		protected virtual void OnSpeedChanged(EventArgs e)
		{
			if (this.SpeedChanged != null)
			{
				this.SpeedChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Creates the gauge.
		/// </summary>
		/// <param name='graphicsDisplay'>Graphics display.</param>
		private void CreateGauge(IGraphicsDisplay graphicsDisplay)
		{
			this.gaugeBounds = new Rectangle(
				this.Bounds.X + this.speedGaugeDefinition.GaugeHorizontalMargin,
				this.Bounds.Y + this.speedGaugeDefinition.GaugeVerticalMargin,
				this.speedGaugeDefinition.GaugeWidth,
				this.speedGaugeDefinition.GaugeMarkerWidth * 3);

			this.gauge = new HorizontalGauge(
				this.speedGaugeDefinition.BackgroundColor, 
				graphicsDisplay, 
				this.gaugeBounds, 
				this.speedGaugeDefinition.GaugeMarkerWidth, 
				this.speedGaugeDefinition.BorderColor,
				0.5f);

			this.gauge.MarkerChanged += (sender, e) => 
			{
				this.OnSpeedChanged(EventArgs.Empty);
			};
		}

		/// <summary>
		/// Creates the rectangles for the background graphics/icons on the gauge
		/// </summary>
		/// <param name='graphicsDisplay'>Graphics display.</param>
		private void CreateGraphicsRectangles(IGraphicsDisplay graphicsDisplay)
		{
			var graphicsRectangleProgressBarLeft = graphicsDisplay.SourceRectangleFromImageType(ImageType.SlowIcon);
			var graphicsRectangleProgressBarRight = graphicsDisplay.SourceRectangleFromImageType(ImageType.SpeedIcon);
			
			var yDiff = this.speedGaugeDefinition.Bounds.Height - graphicsRectangleProgressBarLeft.Height;

			this.boundsLeftImage = new Rectangle(
				this.speedGaugeDefinition.Bounds.X, 
				this.speedGaugeDefinition.Bounds.Y + yDiff / 2,
				graphicsRectangleProgressBarLeft.Width,
				this.speedGaugeDefinition.Bounds.Height - yDiff);
			
			this.boundsRightImage = new Rectangle(
				this.speedGaugeDefinition.Bounds.X + (this.speedGaugeDefinition.Bounds.Width - graphicsRectangleProgressBarRight.Width), 
				this.speedGaugeDefinition.Bounds.Y + yDiff / 2,
				graphicsRectangleProgressBarRight.Width,
				this.speedGaugeDefinition.Bounds.Height - yDiff);
			
			this.boundsMiddleImage = new Rectangle(
				this.speedGaugeDefinition.Bounds.X + this.boundsLeftImage.Width, 
				this.speedGaugeDefinition.Bounds.Y + yDiff / 2,
				this.speedGaugeDefinition.Bounds.Width - (graphicsRectangleProgressBarRight.Width + graphicsRectangleProgressBarLeft.Width),
				this.speedGaugeDefinition.Bounds.Height - yDiff);
		}
	}
}

