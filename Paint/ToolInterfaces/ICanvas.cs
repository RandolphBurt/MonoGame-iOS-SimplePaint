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
		/// Handles any user interaction.
		/// </summary>
		/// <param name='touchPointList'>
		/// Teh list of all gestures / locations touched by the user since the last update
		/// </param>
		void HandleTouchPoints(List<ITouchPoint> touchPointList);
		
		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// <param name='color' The color to use for the drawing />
		/// <param name='brush' The brush to use for the drawing />
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		/// </summary>
		void Draw(Color color, Rectangle brush, bool refreshDisplay = false);
	}
}

