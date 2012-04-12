/// <summary>
/// GraphicsDisplay.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// GraphicsDisplay class - defines the rectangles for the different images used in the app
	/// </summary>
	public class GraphicsDisplay : IGraphicsDisplay
	{
		/// <summary>
		/// The rectangle for the location of the empty square within the graphics texture
		/// Can be used to draw blocks in specific colours just be setting the colour
		/// </summary>
		private Rectangle emptySquare = new Rectangle(0 ,0 , 0, 0);
		
		/// <summary>
		/// The rectangle for the exit-button within the graphics texture
		/// </summary>
		private Rectangle exitButton;

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
			this.graphicsTexture = graphicsTexture;
			this.spriteBatch = spriteBatch;

			if (highResolution == true)
			{
				this.exitButton = new Rectangle(1 ,0 , 100, 100);
			}
			else
			{
				this.exitButton = new Rectangle(1 ,0 , 50, 50);
			}
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
			switch (imageType)
			{				
				case ImageType.ExitButton:	
					return this.exitButton;

				case ImageType.EmptySquare:
				default:
					return this.emptySquare;
			}
		}		
	}
}

