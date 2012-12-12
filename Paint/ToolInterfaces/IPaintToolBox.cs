/// <summary>
/// IPaintToolBox.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using System;

	using Microsoft.Xna.Framework;

	public interface IPaintToolBox : IToolBox
	{
		/// <summary>
		/// Occurs when the user has pressed the undo button.
		/// </summary>
		event EventHandler UndoSelected;
		
		/// <summary>
		/// Occurs when the user has pressed the redo button.
		/// </summary>
		event EventHandler RedoSelected;
		
		/// <summary>
		/// Gets or sets a value indicating whether the undo button should be enabled or not.
		/// </summary>
		bool UndoEnabled { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether the redo button should be enabled or not.
		/// </summary>
		bool RedoEnabled { get; set; }

		/// <summary>
		/// Gets the current brush.
		/// </summary>
		Rectangle Brush { get; }
		
		/// <summary>
		/// Gets the current color 
		/// </summary>
		Color Color	{ get; }
	}
}

