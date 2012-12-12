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
	public class ColorSelector : ToolBoxToolTouchBase, IColorSelector
	{		
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
		/// The current color.
		/// </summary>
		private Color color;

		/// <summary>
		/// The color selector definition -layout information for this control
		/// </summary>
		private ColorSelectorDefinition colorSelectorDefinition;
		
		/// <summary>
		/// List of all the gauges
		/// </summary>
		private Gauge[] gaugeList;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSelector"/> class.
		/// </summary>
		/// <param name='graphicsDisplay'>
		/// Graphics display.
		/// </param>
		/// <param name='colorSelectorDefinition'>
		/// Color selector definition - layout of the color selector
		/// </param>
		public ColorSelector(IGraphicsDisplay graphicsDisplay, ColorSelectorDefinition colorSelectorDefinition)
			: base(
				colorSelectorDefinition.BackgroundColor, 
				colorSelectorDefinition.BorderColor, 
				colorSelectorDefinition.BorderWidth, 
				graphicsDisplay, 
				colorSelectorDefinition.Bounds) 
		{
			this.colorSelectorDefinition = colorSelectorDefinition;

			// Create all our gauge sub-controls
			List<Gauge> gauges = new List<Gauge>();
			
			Color[] colorList = new Color[] { Color.Red, Color.Lime, Color.Blue, Color.DarkBlue };

			byte[] markerValueList = new byte[] 
			{ 
				this.colorSelectorDefinition.StartColor.R, 
				this.colorSelectorDefinition.StartColor.G, 
				this.colorSelectorDefinition.StartColor.B, 
				this.colorSelectorDefinition.StartColor.A 
			}; 
			
			for (int i = 0; i < colorList.Length; i++)
			{
				Rectangle gaugeRectangle = new Rectangle(
					this.bounds.X + this.colorSelectorDefinition.GaugeHorizontalMargin,
					this.bounds.Y + this.bounds.Height - ((GaugeCount - i) * (this.colorSelectorDefinition.GaugeWidth + this.colorSelectorDefinition.GaugeVerticalMargin)),
					this.bounds.Width - this.colorSelectorDefinition.GaugeHorizontalMargin * 2,	
					this.colorSelectorDefinition.GaugeWidth);
				
				gauges.Add(
					new HorizontalGauge(
						this.colorSelectorDefinition.BackgroundColor, 
						graphicsDisplay, 
						gaugeRectangle, 
						this.colorSelectorDefinition.GaugeMarkerWidth, 
						colorList[i], 
						markerValueList[i] / 255.0f));
			}
			
			this.gaugeList = gauges.ToArray();
			this.color = this.colorSelectorDefinition.StartColor;
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
				this.BlankAndRedrawWithBorder();
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

