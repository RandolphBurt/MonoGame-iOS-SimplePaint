/// <summary>
/// IToolBox.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Interface for ToolBox container controls.
	/// </summary>
	public interface IToolBox 
	{
		/// <summary>
		/// Gets the current height of the toolbox.
		/// </summary>
		int ToolboxHeight { get; }
	
		/// <summary>
		/// Gets the height of the toolbox when minimised.
		/// </summary>
		int ToolboxMinimizedHeight { get; }

		/// <summary>
		/// Gets the current dock position.
		/// </summary>
		DockPosition DockPosition { get; }
		
		/// <summary>
		/// Gets the current brush.
		/// </summary>
		Rectangle Brush { get; }

		/// <summary>
		/// Gets the current color 
		/// </summary>
		Color Color	{ get; }

		/// <summary>
		/// Checks wheter a particular touch point (user pressing the screen) is within the bounds of this control.
		/// </summary>
		/// <returns>
		/// The location where the user touched the screen (and type of touch)
		/// </returns>
		/// <param name='touchPosition'>
		/// True = The touchPosition was handled by this control
		/// False = The touchPosition was not handled by this control
		/// </param>
		bool CheckTouchCollision(ITouchPoint touchPosition);
		
		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		void Draw(bool refreshDisplay = false);		
	}
}

