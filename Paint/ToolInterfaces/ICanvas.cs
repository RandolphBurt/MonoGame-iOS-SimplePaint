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
		/// Gets the current color being used for drawing
		/// </summary>
		Color Color { get; }
		
		/// <summary>
		/// Gets the current brush being used for drawing
		/// </summary>
		Rectangle Brush { get; }
		
		/// <summary>
		/// Handles any user interaction.
		/// </summary>
		/// <param name='touchPointList'>
		/// Teh list of all gestures / locations touched by the user since the last update
		/// </param>
		void HandleTouchPoints(List<ITouchPoint> touchPointList);
		
		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// </summary>
		void Draw();
	}
}

