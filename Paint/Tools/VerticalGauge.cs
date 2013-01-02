/// <summary>
/// VerticalGauge.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Vertical gauge.
	/// </summary>
	public class VerticalGauge : Gauge
	{
		// we cache this for speed - as it never changes
		private Rectangle gaugeRectangle;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.VerticalGauge"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the gauge />
		/// <param name='graphicsDisplay' Contains all the graphics for rendering the tools />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='markerWidth' The size of the marked (the bit the user drags) />
		/// <param name='gaugeColor' The color of the gauge />
		/// <param name='startMarker' The starting position/value of the marker />
		public VerticalGauge (Color backgroundColor, IGraphicsDisplay graphicsDisplay, Rectangle bounds, int markerWidth, Color gaugeColor, float startMarker) 
			: base(backgroundColor, graphicsDisplay, bounds, markerWidth, gaugeColor, startMarker) 
		{
			this.gaugeRectangle = new Rectangle(bounds.X + (bounds.Width / 3), bounds.Y, bounds.Width / 3, bounds.Height);
		}
		
		/// <summary>
		/// Handles a particular touch/gesture made by the user
		/// </summary>
		/// <param name='yPosition'>
		/// The y-position of the touch 
		/// (This is all we are interested in)
		/// </param>		
		public void HandleTouch(float yPosition)
		{
			// Calculate the new value, but make sure still in bounds
			int yPos = Math.Max((int)yPosition, this.Bounds.Y);
			yPos = Math.Min(yPos, this.Bounds.Y + this.Bounds.Height);
			
			this.CurrentMarker = (float)((this.Bounds.Y + this.Bounds.Height) - yPos) / (float)this.Bounds.Height;
			
			if (PreviousMarker != CurrentMarker) 
			{
				this.OnMarkerChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Handles a particular touch/gesture made by the user
		/// </summary>
		/// <param name='touch'>
		/// The position and type of gesture/touch made
		/// </param>
		protected override void HandleTouch(ITouchPoint touch)
		{
			this.HandleTouch(touch.Position.Y);
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
				this.Bounds.X, 
				this.BoundedY(((this.Bounds.Y + this.Bounds.Height) - (int)(this.CurrentMarker * this.Bounds.Height)) - (this.MarkerWidth / 2)),
				this.Bounds.Width,
				this.MarkerWidth);
		}
				
		/// <summary>
		/// Ensures the specified y co-ordinate is inside the bounds of the gauage
		/// </summary>
		/// <returns>
		/// The y co-ordinate correctly positioned within the bounds of the gauge
		/// </returns>
		/// <param name='yPosition'>
		/// The calculated/desired y Position
		/// </param>
		private int BoundedY(int yPosition) 
		{
			if (yPosition < this.Bounds.Y)
			{
				return this.Bounds.Y;
			}
			else 
			{
				int maxY = this.Bounds.Y + this.Bounds.Height - this.MarkerWidth;
				if (yPosition > maxY)
				{
					return maxY;
				}
			}
			
			return yPosition;
		}
	}
}

