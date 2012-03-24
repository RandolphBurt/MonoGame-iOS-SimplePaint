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
	public class BrushSizeSelector : CanvasToolTouchBase, IBrushSizeSelector
	{		
		/// <summary>
		/// The color of the gauge.
		/// </summary>
		private readonly Color GaugeColor = Color.Black;
		
		/// <summary>
		/// The vertical margin used around each gauge.
		/// </summary>
		private const int GaugeVerticalMargin = 20;

		/// <summary>
		/// The horizontal margin used around each gauge.
		/// </summary>
		private const int GaugeHorizontalMargin = 20;
		
		/// <summary>
		/// The width of each gauge.
		/// </summary>
		private const int GaugeWidth = 30;

		/// <summary>
		/// The size of the marker used in the gauge
		/// </summary>
		private const int GaugeMarkerWidth = 10;
		
		/// <summary>
		/// The minimum size of the brush allowed.
		/// </summary>
		private int minBrushSize;
		
		/// <summary>
		/// The maximum size of the brush allowed.
		/// </summary>
		private int maxBrushSize;
		
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
		/// Initializes a new instance of the <see cref="Paint.BrushSizeSelector"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the colorSelector />
		/// <param name='borderColor' The border color of the colorSelector />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='minBrushSize' The minimum size allowed for the brush />
		/// <param name='maxBrushSize' The maximum size allowed for the brush />
		/// <param name='startBrushSize' The starting size of the brush />
		/// <param name='startColor' The color we should start with />
		public BrushSizeSelector(Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds,
		                         int minBrushSize, int maxBrushSize, int startBrushSize, Color startColor) 
			: base(backgroundColor, borderColor, spriteBatch, transparentSquareTexture, bounds) 
		{
			this.color = startColor;
			this.minBrushSize = minBrushSize;
			this.maxBrushSize = maxBrushSize;
			this.BrushSize = startBrushSize;
			
			this.gaugeYPosition = bounds.Y + GaugeVerticalMargin + maxBrushSize;
			
			Rectangle gaugeRectangle = new Rectangle(
				bounds.X + GaugeHorizontalMargin,
				this.gaugeYPosition,
				GaugeWidth,
				bounds.Height - (maxBrushSize + (GaugeVerticalMargin * 2)));
			
			float startMarkerValue = (float)(startBrushSize - minBrushSize) / (float)(maxBrushSize - minBrushSize);
			this.brushSizeGauge = new VerticalGauge(backgroundColor, spriteBatch, transparentSquareTexture, gaugeRectangle, GaugeMarkerWidth, GaugeColor, startMarkerValue);
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
				this.bounds.X + ((this.bounds.Width - this.maxBrushSize) / 2 ),
				this.bounds.Y + (GaugeVerticalMargin / 2),
				this.maxBrushSize,
				this.maxBrushSize);
			
			this.DrawRectangle(blankRectangle, this.backgroundColor); 							
			
			// draw new brush
			Rectangle brushRectangle = new Rectangle(
				this.bounds.X + ((this.bounds.Width - this.BrushSize) / 2 ),
				this.bounds.Y + (GaugeVerticalMargin + this.maxBrushSize - this.BrushSize) / 2,
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
			this.BrushSize = this.minBrushSize + (int)(this.brushSizeGauge.Marker * (this.maxBrushSize - this.minBrushSize));
			this.OnBrushSizeChanged(EventArgs.Empty);
		}	
	}
}

