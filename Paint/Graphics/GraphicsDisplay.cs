/// <summary>
/// GraphicsDisplay.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// GraphicsDisplay class - defines the rectangles for the different images used in the app
	/// </summary>
	public class GraphicsDisplay : IGraphicsDisplay
	{
		/// <summary>
		/// Icon size for standard displays
		/// </summary>
		private const int IconSizeLowResolution = 36;
		
		/// <summary>
		/// Icon size for Retina displays
		/// </summary>
		private const int IconSizeRetina = 72;
		
		/// <summary>
		/// How many columns of icons are there in the sprite map
		/// </summary>
		private const int SpriteMapIconColumns = 5;
		
		/// <summary>
		/// How many rows of icons are there in the sprite map
		/// </summary>
		private const int SpriteMapIconRows = 4;
		
		/// <summary>
		/// Size/shape/location of eachimage in the spritemap
		/// </summary>
		private Rectangle[] imageRectangles = null;
		
		/// <summary>
		/// Image containing all graphics for the app.
		/// </summary>
		private Texture2D graphicsTexture;
		
		/// <summary>
		/// The sprite batch - for rendering
		/// </summary>
		private SpriteBatch spriteBatch;
			
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.GraphicsDisplay"/> class.
		/// </summary>
		/// <param name='graphicsTexture'>The sprite map - all images we need to render </param>
		/// <param name='spriteBatch'> Low level rendering class </param>
		/// <param name='highResolution'> Is the iPad running in High resolution. </param>
		public GraphicsDisplay(Texture2D graphicsTexture, SpriteBatch spriteBatch, bool highResolution)
		{
			int iconSize;
			int emptySquareOffset;
			
			if (highResolution)
			{
				iconSize = IconSizeRetina;
				emptySquareOffset = 2;
			}
			else
			{
				iconSize = IconSizeLowResolution;
				emptySquareOffset = 1;
			}

			this.graphicsTexture = graphicsTexture;
			this.spriteBatch = spriteBatch;
			
			List<Rectangle> imageList = new List<Rectangle>();			

			// This is the empty square image for all basic single color drawing
			imageList.Add(new Rectangle(0, 0, 0, 0));
			
			for (int y = 0; y < SpriteMapIconRows; y++)
			{
				for (int x = 0; x < SpriteMapIconColumns; x++)
				{
					imageList.Add(new Rectangle(emptySquareOffset + (x * iconSize), y * iconSize, iconSize, iconSize));
				}
			}
			
			this.imageRectangles = imageList.ToArray();
		}
		
		/// <summary>
		/// Begins the drawing process.
		/// </summary>
		public void BeginRender()
		{
			this.spriteBatch.Begin();
		}
		
		/// <summary>
		/// Begins the drawing process - using opaque as the color merging option
		/// </summary>
		public void BeginRenderOpaque()
		{
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
		}
		
		/// <summary>
		/// Begins the drawing process - using NonPremultiplied as the color merging option
		/// </summary>
		public void BeginRenderNonPremultiplied()
		{
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		}
		
		/// <summary>
		/// End the current rendering process
		/// </summary>
		public void EndRender()
		{
			this.spriteBatch.End();
		}
		
		/// <summary>
		/// Renders a specific image 
		/// </summary>
		/// <param name='imageType'> Type of image we want to render </param>
		/// <param name='paintRegion'> destination rectangle to render the graphic </param>
		/// <param name='color'>Colour to use for rendering the image</param>
		public void DrawGraphic(ImageType imageType, Rectangle paintRegion, Color color)
		{
			Rectangle sourceRegion = this.SourceRectangleFromImageType(imageType);
			
			this.spriteBatch.Draw(this.graphicsTexture, paintRegion, sourceRegion, color); 
		}
		
		/// <summary>
		/// Calculates the area of the texturemap that we need to use for rendering a specific image
		/// </summary>
		/// <returns>The rectangle to use as the source from the texture map</returns>
		/// <param name='imageType'> Type of image we want to render </param>
		public Rectangle SourceRectangleFromImageType(ImageType imageType)
		{
			return this.imageRectangles[(int)imageType];
		}		
	}
}

