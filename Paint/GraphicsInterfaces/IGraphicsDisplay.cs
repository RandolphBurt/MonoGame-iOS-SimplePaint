/// <summary>
/// IGraphicsDisplay.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Interface for graphics display - drawing images to screen
	/// </summary>
	public interface IGraphicsDisplay
	{
		/// <summary>
		/// Begins the drawing process.
		/// </summary>
		void BeginRender();
		
		/// <summary>
		/// Begins the drawing process - using opaque as the color merging option
		/// </summary>
		void BeginRenderOpaque();
		
		/// <summary>
		/// Begins the drawing process - using NonPremultiplied as the color merging option
		/// </summary>
		void BeginRenderNonPremultiplied();
		
		/// <summary>
		/// End the current rendering process
		/// </summary>
		void EndRender();
		
		// Calculates the area of the texturemap that we need to use for rendering a specific image
		Rectangle SourceRectangleFromImageType(ImageType imageType);
		                                      
		/// <summary>
		/// Draws a particular graphics image
		/// </summary>
		/// <param name='imageType'>The type of image to draw </param>
		/// <param name='paintRegion'>The area to display the image </param>
		/// <param name='color'>The color to render the  image in </param>
		void DrawGraphic(ImageType imageType, Rectangle paintRegion, Color color);
	}
}

