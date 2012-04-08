/// <summary>
/// CanvasRecorderCommand.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Canvas recorder command
	/// Static class for holding the command declarations
	/// </summary>
	public static class CanvasRecorderCommand
	{		
		/// <summary>
		/// Constant for indicating we should paint at the given position.
		/// This is the beginning of the user dragging their finger to paint a line
		/// </summary>
		public const byte StartDrag = 0;
		
		/// <summary>
		/// Constant for indicating we should paint at the given position.
		/// This is the end of the user dragging their finger to paint a line
		/// </summary>
		public const byte DragComplete = 1;

		/// <summary>
		/// Constant for indicating we should paint at the given position.
		/// This is during the process of the user dragging their finger to paint a line
		/// </summary>
		public const byte FreeDrag = 2;

		/// Constant for indicating we should paint at the given position.
		/// This is the user tapping their fingeron the screen to draw a single dot
		/// </summary>
		public const byte Tap = 3;

		/// <summary>
		/// Constant for indicating the colour has changed
		/// </summary>
		public const byte SetColor = 4;
		
		/// <summary>
		/// Constant for indicating the brush size has changed.
		/// </summary>
		public const byte SetBrushSize = 5;
		
		/// <summary>
		/// Converts a TouchType to a byte for the corresponding Canvas Recorder Command
		/// </summary>
		/// <returns>
		/// A byte representing the Canvas Recorder Command
		/// </returns>
		/// <param name='touchType'>
		/// The type of touch the user made
		/// </param>
		public static byte FromTouchType(TouchType touchType)
		{
			switch (touchType) 
			{
				case TouchType.StartDrag:
					return StartDrag;
				
				case TouchType.DragComplete:
					return DragComplete;
				
				case TouchType.FreeDrag:
					return FreeDrag;
				
				case TouchType.Tap:
				default:
					return Tap;
			}			
		}
		
		/// <summary>
		/// Converts a Canvas Recorder Command to a corresponding TouchType
		/// </summary>
		/// <returns>
		/// The type of touch the user made
		/// </returns>
		/// <param name='command'>
		///  byte representing the Canvas Recorder Command
		/// </param>
		public static TouchType ToTouchType(byte command)
		{
			switch (command) 
			{
				case StartDrag:
					return TouchType.StartDrag;
				
				case DragComplete:
					return TouchType.DragComplete;
				
				case FreeDrag:
					return TouchType.FreeDrag;
				
				case Tap:
				default:
					return TouchType.Tap;
			}			
		}
	}
}

