/// <summary>
/// HorizontalGauge.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Horizontal gauge.
	/// </summary>
	public class HorizontalGauge : Gauge
	{
		// we cache this for speed - as it never changes
		private Rectangle gaugeRectangle;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.HorizontalGauge"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the gauge />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='markerWidth' The size of the marked (the bit the user drags) />
		/// <param name='gaugeColor' The color of the gauge />
		/// <param name='startMarker' The starting position/value of the marker />
		public HorizontalGauge (Color backgroundColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds, int markerWidth, Color gaugeColor, float startMarker) 
			: base(backgroundColor, spriteBatch, transparentSquareTexture, bounds, markerWidth, gaugeColor, startMarker) 
		{
			this.gaugeRectangle = new Rectangle(bounds.X, bounds.Y + (bounds.Height / 3), bounds.Width, bounds.Height / 3);
		}
		
		/// <summary>
		/// Handles a particular touch/gesture made by the user
		/// </summary>
		/// <param name='touch'>
		/// The position and type of gesture/touch made
		/// </param>
		protected override void HandleTouch(ITouchPoint touch)
		{
			// Calculate the new value, but make sure still in bounds
			int xPos = Math.Max((int)touch.Position.X, this.bounds.X);
			xPos = Math.Min(xPos, this.bounds.X + this.bounds.Width);
			
			this.currentMarker = (float)(xPos - this.bounds.X) / (float)this.bounds.Width;
			
			if (previousMarker != currentMarker) 
			{
				this.OnMarkerChanged(EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Simply creates a rectangle that defines the location and size of the gauge
		/// </summary>
		/// <returns>
		/// The gauge rectangle.
		/// </returns>
		protected override Rectangle CreateGaugeRectangle()
		{
			return this.gaugeRectangle;
		}
		
		/// <summary>
		/// Creates a rectangle that defines the location and size of the marker
		/// </summary>
		/// <returns>
		/// The marker rectangle.
		/// </returns>
		protected override Rectangle CreateMarkerRectangle()
		{
			return new Rectangle(
				this.BoundedX((this.bounds.X + (int)(this.currentMarker * this.bounds.Width)) - (this.markerWidth / 2)),
				this.bounds.Y, 
				this.markerWidth,
				this.bounds.Height);
		}
		
		/// <summary>
		/// Ensures the specified x co-ordinate is inside the bounds of the gauage
		/// </summary>
		/// <returns>
		/// The x co-ordinate correctly positioned within the bounds of the gauge
		/// </returns>
		/// <param name='xPosition'>
		/// The calculated/desired x Position
		/// </param>
		private int BoundedX(int xPosition) 
		{
			if (xPosition < this.bounds.X)
			{
				return this.bounds.X;
			}
			else 
			{
				int maxX = this.bounds.X + this.bounds.Width - this.markerWidth;
				if (xPosition > maxX)
				{
					return maxX;
				}
			}
			
			return xPosition;
		}
	}
}

