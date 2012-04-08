/// <summary>
/// ICanvas.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// ICanvas - Interface for classes handling the rendering and user interaction
	/// </summary>
	public interface ICanvas
	{			
		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// <param name='color' The color to use for the drawing />
		/// <param name='brush' The brush to use for the drawing />
		/// <param name='touchPointList'>
		/// The list of all gestures / locations touched by the user since the last update
		/// </param>		
		/// </summary>
		void Draw(Color color, Rectangle brush, List<ITouchPoint> touchPoints);
	}
}

