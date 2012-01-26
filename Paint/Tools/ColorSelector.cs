/// <summary>
/// ColorSelector.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Color selector - a tool allowing the user to specify the exact color to use by specifying individual RGBA values
	/// </summary>
	public class ColorSelector : CanvasToolTouchBase, IColorSelector
	{
		/// <summary>
		/// Border size for drawing the tool on screen.
		/// </summary>
		private const int BorderWidth = 5;
		
		/// <summary>
		/// Relative order of the Red Gauge on screen and in the internal array of gauges.
		/// </summary>
		private const int RedGaugePosn = 0;

		/// <summary>
		/// Relative order of the Green Gauge on screen and in the internal array of gauges.
		/// </summary>
		private const int GreenGaugePosn = 1;

		/// <summary>
		/// Relative order of the Blue Gauge on screen and in the internal array of gauges.
		/// </summary>
		private const int BlueGaugePosn = 2;

		/// <summary>
		/// Relative order of the Alpha (Transparency) Gauge on screen and in the internal array of gauges.
		/// </summary>
		private const int AlphaGaugePosn = 3;
		
		/// <summary>
		/// Number of gauges used by this control
		/// </summary>
		private const int GaugeCount = 4;
		
		/// <summary>
		/// The vertical margin used around each gauge.
		/// </summary>
		private const int GaugeVerticalMargin = 20;

		/// <summary>
		/// The horizontal margin used around each gauge.
		/// </summary>
		private const int GaugeHorizontalMargin = 20;
		
		/// <summary>
		/// The height of each gauge.
		/// </summary>
		private const int GaugeHeight = 30;

		/// <summary>
		/// The size of the marker used in the gauge
		/// </summary>
		private const int GaugeMarkerWidth = 10;
		
		/// <summary>
		/// The current color.
		/// </summary>
		private Color color;
		
		/// <summary>
		/// List of all the gauges
		/// </summary>
		private Gauge[] gaugeList;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSelector"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the colorSelector />
		/// <param name='borderColor' The border color of the colorSelector />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='startColor' The color we should start with />
		public ColorSelector (Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds, Color startColor) 
			: base(backgroundColor, borderColor, spriteBatch, transparentSquareTexture, bounds) 
		{
			// Create all our gauge sub-controls
			List<Gauge> gauges = new List<Gauge>();
			
			Color[] colorList = new Color[] { Color.Red, Color.Lime, Color.Blue, Color.DarkBlue };
			byte[] markerValueList = new byte[] { startColor.R, startColor.G, startColor.B, startColor.A }; 
			
			for (int i = 0; i < colorList.Length; i++)
			{
				Rectangle gaugeRectangle = new Rectangle(
					bounds.X + GaugeHorizontalMargin,
					bounds.Y + bounds.Height - ((GaugeCount - i) * (GaugeHeight + GaugeVerticalMargin)),
					bounds.Width - GaugeHorizontalMargin * 2,	
					GaugeHeight);

				gauges.Add(new HorizontalGauge(backgroundColor, spriteBatch, transparentSquareTexture, gaugeRectangle, GaugeMarkerWidth, colorList[i], markerValueList[i] / 255.0f));
			}
			
			gaugeList = gauges.ToArray();
			this.color = startColor;
			this.HookGaugeEvents();
		}
		
		/// <summary>
		/// Occurs when the color is changed
		/// </summary>
		public event EventHandler ColorChanged;
		
		/// <summary>
		/// Gets or sets the color we are currently drawing with
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
					
					this.UnHookGaugeEvents();
					
					this.gaugeList[RedGaugePosn].Marker = value.R / 255.0f;
					this.gaugeList[GreenGaugePosn].Marker = value.G / 255.0f;
					this.gaugeList[BlueGaugePosn].Marker = value.B / 255.0f;
					this.gaugeList[AlphaGaugePosn].Marker = value.A / 255.0f;
					
					this.HookGaugeEvents();
					
					this.OnColorChanged(EventArgs.Empty);
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
				this.spriteBatch.Draw(this.transparentSquareTexture, this.bounds, this.borderColor); 
				
				Rectangle inBorderRectangle = new Rectangle(
					this.bounds.X + BorderWidth,
					this.bounds.Y + BorderWidth,
					this.bounds.Width - (2 * BorderWidth),
					this.bounds.Height - (2 * BorderWidth));
				
				this.spriteBatch.Draw(this.transparentSquareTexture, inBorderRectangle, this.backgroundColor); 
			}
			
			for (int i = 0; i < GaugeCount; i++)			
			{
				this.gaugeList[i].Draw(refreshDisplay);
			}
		}
		
		/// <summary>
		/// Handles a particular touch by the user
		/// </summary>
		/// <param name='touch'>
		/// The position and type of gesture/touch made
		/// </param>
		protected override void HandleTouch(ITouchPoint touchPosition)
		{
			for (int i = 0; i < GaugeCount; i++)			
			{
				if (this.gaugeList[i].CheckTouchCollision(touchPosition) == true)
				{
					break;
				}
			}
		}
		
		/// <summary>
		/// Raises the color changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnColorChanged(EventArgs e)
		{
			if (this.ColorChanged != null) 
			{
				this.ColorChanged(this, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Hooks on to all events for each gauge
		/// </summary>
		private void HookGaugeEvents()
		{
			for (int i = 0; i < GaugeCount; i++)
			{
				this.gaugeList[i].MarkerChanged += gauge_MarkerChanged;
			}
		}
		
		/// <summary>
		/// Unhook all events for the Gauge sub-controls
		/// </summary>
		private void UnHookGaugeEvents()
		{
			for (int i = 0; i < GaugeCount; i++)
			{
				this.gaugeList[i].MarkerChanged -= gauge_MarkerChanged;
			}
		}
		
		/// <summary>
		/// Event occurrs whenever the gauge's marked position/value changes
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Any EventArgs
		/// </param>
		private void gauge_MarkerChanged (object sender, EventArgs e)
		{
			this.color = new Color(this.gaugeList[RedGaugePosn].Marker, this.gaugeList[GreenGaugePosn].Marker, this.gaugeList[BlueGaugePosn].Marker,  this.gaugeList[AlphaGaugePosn].Marker);
			this.OnColorChanged(EventArgs.Empty);
		}
	}
}

