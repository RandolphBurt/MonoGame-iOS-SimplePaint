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
		/// The previous color used for drawing.
		/// </summary>
		private Color previousColor;
		
		/// <summary>
		/// The current color.
		/// </summary>
		private Color color;
		
		/// <summary>
		/// Contains all the graphics for rendering the tools
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;

		/// <summary>
		/// The color setter layout
		/// </summary>
		private ColorSetterDefinition colorSetterDefinition;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSetter"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the colorSetter />
		/// <param name='graphicsDisplay' Contains all the graphics for rendering the tools />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='startColor' The color we should start with />
		/// <param name='borderWidth' The border width />
		public ColorSetter (IGraphicsDisplay graphicsDisplay, ColorSetterDefinition colorSetterDefinition) 
		{
			this.graphicsDisplay = graphicsDisplay;
			this.colorSetterDefinition = colorSetterDefinition;
			this.previousColor = this.color = colorSetterDefinition.BackgroundColor;
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
				this.DrawRectangle(this.colorSetterDefinition.Bounds, this.colorSetterDefinition.BorderColor); 
				
				Rectangle inBorderRectangle = new Rectangle(
					this.colorSetterDefinition.Bounds.X + this.colorSetterDefinition.BorderWidth,
					this.colorSetterDefinition.Bounds.Y + this.colorSetterDefinition.BorderWidth,
					this.colorSetterDefinition.Bounds.Width - (2 * this.colorSetterDefinition.BorderWidth),
					this.colorSetterDefinition.Bounds.Height - (2 * this.colorSetterDefinition.BorderWidth));
				
				this.DrawRectangle(inBorderRectangle, this.Color); 
				
				this.previousColor = this.color;
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

