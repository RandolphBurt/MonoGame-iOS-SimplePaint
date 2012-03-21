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
		private const int BorderWidth = 5;
		
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
		/// The SpriteBatch object used for any rendering
		/// </summary>
		private SpriteBatch spriteBatch;
		
		/// <summary>
		/// A simple transparent texture used for the basis of all drwaing - we just set the color as appropriate.
		/// </summary>
		private Texture2D transparentSquareTexture;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSetter"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the colorSetter />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='startColor' The color we should start with />
		public ColorSetter (Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds, Color startColor) 
		{
			this.previousColor = this.color = startColor;
			this.spriteBatch = spriteBatch;
			this.bounds = bounds;
			this.transparentSquareTexture = transparentSquareTexture;
			this.borderColor = borderColor;
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
					this.bounds.X + BorderWidth,
					this.bounds.Y + BorderWidth,
					this.bounds.Width - (2 * BorderWidth),
					this.bounds.Height - (2 * BorderWidth));
				
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
			this.spriteBatch.Draw(this.transparentSquareTexture, rectangle, color); 
		}
	}
}

