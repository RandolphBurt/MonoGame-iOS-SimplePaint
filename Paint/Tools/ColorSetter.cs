/// <summary>
/// ColorSetter.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Color setter - A tool used to indicate the currently selected color for drawing
	/// </summary>
	public class ColorSetter : IColorSetter, ICanvasTool
	{
		/// <summary>
		/// Border size for drawing the tool on screen.
		/// </summary>
		private int borderWidth;
		
		/// <summary>
		/// The previous color used for drawing.
		/// </summary>
		private Color previousColor;
		
		/// <summary>
		/// The current color.
		/// </summary>
		private Color color;
		
		/// <summary>
		/// The color of the border of this tool
		/// </summary>
		private Color borderColor;
		
		/// <summary>
		/// The location and size of this tool
		/// </summary>
		private Rectangle bounds;
		
		/// <summary>
		/// Contains all the graphics for rendering the tools
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSetter"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the colorSetter />
		/// <param name='graphicsDisplay' Contains all the graphics for rendering the tools />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='startColor' The color we should start with />
		/// <param name='borderWidth' The border width />
		public ColorSetter (Color borderColor, IGraphicsDisplay graphicsDisplay, Rectangle bounds, Color startColor, int borderWidth) 
		{
			this.previousColor = this.color = startColor;
			this.bounds = bounds;
			this.graphicsDisplay = graphicsDisplay;
			this.borderColor = borderColor;
			this.borderWidth = borderWidth;
		}

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public void Draw (bool refreshDisplay)
		{
			if (this.color != this.previousColor || refreshDisplay == true)
			{
				// Blank out everything 
				this.DrawRectangle(this.bounds, this.borderColor); 
				
				Rectangle inBorderRectangle = new Rectangle(
					this.bounds.X + borderWidth,
					this.bounds.Y + borderWidth,
					this.bounds.Width - (2 * borderWidth),
					this.bounds.Height - (2 * borderWidth));
				
				this.DrawRectangle(inBorderRectangle, this.Color); 
				
				this.previousColor = this.color;
			}
		}
		
		/// <summary>
		/// Gets or sets the color we should display
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
		/// Draws the rectangle.
		/// </summary>
		/// <param name='rectangle' The rectangluar region to paint />
		/// <param name='color' The color we wish to paint the rectangle/>
		protected void DrawRectangle(Rectangle rectangle, Color color)
		{
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, rectangle, color); 
		}
	}
}

