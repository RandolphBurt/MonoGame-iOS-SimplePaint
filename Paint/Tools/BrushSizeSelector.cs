/// <summary>
/// BrushSizeSelector.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// Brush size selector - tool allowing the user to pick the size of the brush for drawing
	/// </summary>
	public class BrushSizeSelector : ToolBoxToolTouchBase, IBrushSizeSelector
	{		
		/// <summary>
		/// The gauge used for representing the size of the brush.
		/// </summary>
		private VerticalGauge brushSizeGauge;
		
		/// <summary>
		/// The current color used when drawing with this brush.
		/// </summary>
		private Color color;
		
		/// <summary>
		/// Caches the Y position of the gauge so we can quickly check if and 'touch' was within the gauge
		/// </summary>
		private int gaugeYPosition = 0;

		/// <summary>
		/// The brush size definition - layout information
		/// </summary>
		private BrushSizeSelectorDefinition brushSizeDefinition;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.BrushSizeSelector"/> class.
		/// </summary>
		/// <param name='graphicsDisplay'>
		/// Graphics display.
		/// </param>
		/// <param name='brushSizeDefinition'>
		/// Brush size definition - layout of the control.
		/// </param>
		public BrushSizeSelector(IGraphicsDisplay graphicsDisplay, BrushSizeSelectorDefinition brushSizeDefinition) 
			: base (
				brushSizeDefinition.BackgroundColor,
				brushSizeDefinition.BorderColor,
				brushSizeDefinition.BorderWidth,
				graphicsDisplay,
				brushSizeDefinition.Bounds)
		{
			this.brushSizeDefinition = brushSizeDefinition;

			this.color = brushSizeDefinition.StartColor;
			this.BrushSize = brushSizeDefinition.BrushSizeInitial;
			this.gaugeYPosition = Bounds.Y + brushSizeDefinition.GaugeVerticalMargin + this.brushSizeDefinition.BrushSizeMaximum;
			
			Rectangle gaugeBounds = new Rectangle(
				this.Bounds.X + ((this.Bounds.Width - this.brushSizeDefinition.GaugeWidth) / 2),
				this.gaugeYPosition,
				this.brushSizeDefinition.GaugeWidth,
				this.Bounds.Height - (this.brushSizeDefinition.BrushSizeMaximum + (this.brushSizeDefinition.GaugeVerticalMargin * 2)));
			
			float startMarkerValue = 
				(float)(this.brushSizeDefinition.BrushSizeInitial - this.brushSizeDefinition.BrushSizeMinimum) / 
				(float)(this.brushSizeDefinition.BrushSizeMaximum - this.brushSizeDefinition.BrushSizeMinimum);

			this.brushSizeGauge = 
				new VerticalGauge(
					this.BackgroundColor, 
					graphicsDisplay, 
					gaugeBounds, 
					this.brushSizeDefinition.GaugeMarkerWidth, 
					this.brushSizeDefinition.BorderColor, 
					startMarkerValue);

			this.brushSizeGauge.MarkerChanged += brushSizeGauge_MarkerChanged;
		}
	
				
		/// <summary>
		/// Occurs when the brush size changes.
		/// </summary>
		public event EventHandler BrushSizeChanged;
		
		/// <summary>
		/// Gets the size of the brush.
		/// </summary>
		public int BrushSize 
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets or sets the color used by the brush.
		/// </summary>
		public Color Color 
		{
			get
			{
				return this.color;
			}
			
			set
			{
				if (this.color != value)
				{
					this.color = value;
				}
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
			if (refreshDisplay == true) 
			{
				// Blank out everything 
				this.BlankAndRedrawWithBorder();
			}
			
			this.DrawBrush();
			
			this.brushSizeGauge.Draw(refreshDisplay);
		}
		
		/// <summary>
		/// Handle the user interaction for a particular touch/gesture type and position
		/// </summary>
		/// <param name='touchPosition'>
		/// Touch position and type of gesture 
		/// </param>
		protected override void HandleTouch(ITouchPoint touchPosition)
		{
			if (touchPosition.Position.Y >= this.gaugeYPosition)
			{
				// We are only interested in the y-position - we know the touch is within this control so anywhere on the x axis is good enough
				// This ensures an improved user experience because the user can click just to the side of the gauge and now it will count as a hit.
				this.brushSizeGauge.HandleTouch(touchPosition.Position.Y);
			}
		}
		
		/// <summary>
		/// Raises the brush size changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnBrushSizeChanged(EventArgs e)
		{
			if (this.BrushSizeChanged != null) 
			{
				this.BrushSizeChanged(this, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Draws the brush size representation in the current color
		/// </summary>
		private void DrawBrush()
		{
			// Blank out previous brush first...
			Rectangle blankRectangle = new Rectangle(
				this.Bounds.X + ((this.Bounds.Width - this.brushSizeDefinition.BrushSizeMaximum) / 2 ),
				this.Bounds.Y + (brushSizeDefinition.GaugeVerticalMargin / 2),
				this.brushSizeDefinition.BrushSizeMaximum,
				this.brushSizeDefinition.BrushSizeMaximum);
			
			this.DrawRectangle(blankRectangle, this.BackgroundColor); 							
			
			// draw new brush
			Rectangle brushRectangle = new Rectangle(
				this.Bounds.X + ((this.Bounds.Width - this.BrushSize) / 2 ),
				this.Bounds.Y + (brushSizeDefinition.GaugeVerticalMargin + this.brushSizeDefinition.BrushSizeMaximum - this.BrushSize) / 2,
				this.BrushSize,
				this.BrushSize);
			
			this.DrawRectangle(brushRectangle, this.Color); 			
		}
	
		/// <summary>
		/// Fires when the gauge has been altered, indicating the brush size needs to change
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Any relevant EventArgs
		/// </param>
		private void brushSizeGauge_MarkerChanged (object sender, EventArgs e)
		{
			this.BrushSize = 
				this.brushSizeDefinition.BrushSizeMinimum + 
					(int)(this.brushSizeGauge.Marker * 
					      (this.brushSizeDefinition.BrushSizeMaximum - this.brushSizeDefinition.BrushSizeMinimum));

			this.OnBrushSizeChanged(EventArgs.Empty);
		}	
	}
}

