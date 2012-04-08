/// <summary>
/// Canvas.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// The main class - Canvas - handles all the rendering and user interaction
	/// </summary>
	public class Canvas : ICanvas
	{				
		/// <summary>
		/// The SpriteBatch for all rendering
		/// </summary>
		private SpriteBatch spriteBatch;
		
		/// <summary>
		/// A simple transparent texture used for all drawing - we simply set the appropriate color.
		/// </summary>
		private Texture2D transparentSquareTexture;
				
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.Canvas"/> class.
		/// </summary>
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		public Canvas(SpriteBatch spriteBatch, Texture2D transparentSquareTexture)
		{
			this.spriteBatch = spriteBatch;
			this.transparentSquareTexture = transparentSquareTexture;
		}
		
		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// <param name='color' The color to use for the drawing />
		/// <param name='brush' The brush to use for the drawing />
		/// <param name='touchPointList'>
		/// The list of all gestures / locations touched by the user since the last update
		/// </param>		
		/// </summary>
		public void Draw(Color color, Rectangle brush, List<ITouchPoint> touchPoints)
		{
			/* 
			 * We use NonPremultiplied when drawing our picture - this allows the user to reduce the alpha value (transparency)
			 * and then build up the color by drawing over the top of it.
			 * Also if painting one color over another then they will merge slightly - e.g. painting a red over a green will make a yellow.
			 * Other alternatives are Additive, Opaque and AlphaBlending.
			 * Additive will eventually turn everything white the more you colour it.
			 * Opaque simply ignores the alpha value - hence useful when drawing controls as we want to completely replace the previous image
			 * AlphaBlending and NonPremultiplied are very similar - NonPremultiplied uses Blend.SourceAlpha when mixing the source and 
			 * destination colours - BlendAlpha uses BlendOne.  (See Monogame/BlendState.cs)
			 * 
			 */			
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			
			this.DrawPicture(color, brush, touchPoints);
			
			this.spriteBatch.End();
		}
		
		/// <summary>
		/// Updates our picture with any new points on screen that the user has touched
		/// <param name='color' The color to use for the drawing />
		/// <param name='brush' The brush to use for the drawing />
		/// <param name='touchPoints'>
		/// The list of all gestures / locations that we need to paint
		/// </param>		
		/// </summary>
		private void DrawPicture(Color color, Rectangle brush, List<ITouchPoint> touchPoints)
		{
			int brushOffset = brush.Width / 2;
			
			foreach (var touch in touchPoints) 
			{
				Vector2 position = touch.Position;
				
				Rectangle rectangle = new Rectangle(
					(int)position.X - brushOffset,
					(int)position.Y - brushOffset,
					brush.Width,
					brush.Height);
				
				this.spriteBatch.Draw(this.transparentSquareTexture, rectangle, color);
			}
		}
	}
}

