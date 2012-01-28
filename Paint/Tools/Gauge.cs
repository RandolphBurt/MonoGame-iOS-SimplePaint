/// <summary>
/// Gauge.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Abstract Base class for all types of Guage
	/// </summary>
	public abstract class Gauge : CanvasToolTouchBase, IGauge
	{
		/// <summary>
		/// The size of the marker.
		/// </summary>
		protected int markerWidth;
		
		/// <summary>
		/// The color of the gauge.
		/// </summary>
		protected Color gaugeColor;
		
		/// <summary>
		/// The current position/value of the marker.
		/// </summary>
		protected float currentMarker;

		/// <summary>
		/// The previous position/value of the marker.
		/// </summary>
		protected float previousMarker;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.Gauge"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the gauge />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='markerWidth' The size of the marked (the bit the user drags) />
		/// <param name='gaugeColor' The color of the gauge />
		/// <param name='startMarker' The starting position/value of the marker />
		public Gauge (Color backgroundColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds, int markerWidth, Color gaugeColor, float startMarker) 
			: base(backgroundColor, backgroundColor, spriteBatch, transparentSquareTexture, bounds) 
		{
			this.currentMarker = this.previousMarker = startMarker;
			this.gaugeColor = gaugeColor;
			this.markerWidth = markerWidth;
		}
		
		/// <summary>
		/// Occurs when the position of the marker changes.
		/// </summary>
		public event EventHandler MarkerChanged;
		
		/// <summary>
		/// Gets or sets the position of the marker (0.0 -> 1.0)
		/// </summary>
		public float Marker 
		{
			get
			{
				return this.currentMarker;
			}
			
			set 
			{
				if (this.currentMarker != value) 
				{
					this.currentMarker = value;
					this.OnMarkerChanged(EventArgs.Empty);
				}
			}
		}
		
		/// <summary>
		/// Draw the gauge.
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should completely redraw the gauge 
		/// False = we should just draw the regions that have changed since we last drew the gauge
		/// </param>
		public override void Draw(bool refreshDisplay)
		{
			
			if (refreshDisplay == true || this.previousMarker != this.currentMarker) 
			{
				// Blank out everything 
				this.DrawRectangle(this.bounds, this.backgroundColor); 
								
				// Draw the gauge bar
				this.DrawRectangle(this.CreateGaugeRectangle(), this.gaugeColor);
				
				// Draw the marker
				this.DrawRectangle(this.CreateMarkerRectangle(), this.gaugeColor);			
				
				this.previousMarker = this.currentMarker;
			}
			
			return;
		}
		
		/// <summary>
		/// Simply creates a rectangle that defines the location and size of the gauge
		/// </summary>
		/// <returns>
		/// The gauge rectangle.
		/// </returns>
		protected abstract Rectangle CreateGaugeRectangle();
		
		/// <summary>
		/// Creates a rectangle that defines the location and size of the marker
		/// </summary>
		/// <returns>
		/// The marker rectangle.
		/// </returns>
		protected abstract Rectangle CreateMarkerRectangle();
			
		/// <summary>
		/// Raises the marker changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs - should be EventArgs.Empty
		/// </param>
		protected virtual void OnMarkerChanged(EventArgs e)
		{
			if (this.MarkerChanged != null) 
			{
				this.MarkerChanged(this, EventArgs.Empty);
			}
		}		
	}
}

