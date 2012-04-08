/// <summary>
/// CanvasRecorder.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;	
	using System.Collections.Generic;
	using System.IO;
	
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Canvas recorder.
	/// </summary>
	public class CanvasRecorder : ICanvasRecorder
	{
		/// <summary>
		/// The curren color
		/// </summary>
		private Color currentColor = Color.White;
		
		/// <summary>
		/// The size of the brush.
		/// </summary>
		private int currentBrushSize = 0;
		
		/// <summary>
		/// The list of commands (set color, set brush size, paint at position x,y) that make up this picture.
		/// </summary>
		List<byte> commandList = new List<byte>();
		
		/// <summary>
		/// The name of the file where we will save the playback data.
		/// </summary>
		private string filename = null;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasRecorder"/> class.
		/// </summary>
		/// <param name='filename'>
		/// The name of the file where we should save the recording.
		/// </param>
		public CanvasRecorder(string filename)
		{
			this.filename = filename;
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
			if (touchPoints.Count == 0)
			{
				// no point recording anything (even if color change - e.g. user has just set the color) because we haven't drawn 
				// anything on screen anyway.  When they do draw on screen (maybe after a few more color changes) then we will pick
				// up the color change at the same time
				return;
			}
			
			if (color != this.currentColor)
			{
				RecordColorChange (color);
			}
			
			if (brush.Width != this.currentBrushSize)
			{
				RecordBrushSizeChange (brush);
			}
			
			foreach (var touch in touchPoints)
			{
				RecordTouchPoint(touch);
			}
		}
		
		/// <summary>
		/// Save the Canvas to file ready for playback.
		/// </summary>
		public void Save()
		{
			using (FileStream stream = File.Open(this.filename, FileMode.Create, FileAccess.Write))
			{
				// 5 bytes per command
				int commandCount = this.commandList.Count / 5;
				
				// Write the command length
				stream.WriteByte((byte)commandCount);
				stream.WriteByte((byte)(commandCount >> 8));
				stream.WriteByte((byte)(commandCount >> 16));
				stream.WriteByte((byte)(commandCount >> 24));
				
				stream.Write(this.commandList.ToArray(), 0, this.commandList.Count);
			}
		}
		
		/// <summary>
		/// Records the color change as a command
		/// </summary>
		/// <param name='color'>
		/// The color for subsequent dots
		/// </param>
		private void RecordColorChange(Color color)
		{
			this.currentColor = color;
			
			commandList.Add(CanvasRecorderCommand.SetColor);
			commandList.Add(color.A);
			commandList.Add(color.R);
			commandList.Add(color.G);
			commandList.Add(color.B);
		}		
		
		/// <summary>
		/// Records the brush size change as a command
		/// </summary>
		/// <param name='brush'>
		/// The size of the brush for subsequent dots
		/// </param>
		void RecordBrushSizeChange(Rectangle brush)
		{
			this.currentBrushSize = brush.Width;
			
			commandList.Add(CanvasRecorderCommand.SetBrushSize);
			commandList.Add((byte)this.currentBrushSize);
			commandList.Add((byte)(this.currentBrushSize >> 8));
			commandList.Add((byte)(this.currentBrushSize >> 16));
			commandList.Add((byte)(this.currentBrushSize >> 24));
		}
		
		/// <summary>
		/// Records the touch point as a command
		/// </summary>
		void RecordTouchPoint(ITouchPoint touch)
		{
			Vector2 position = touch.Position;
			
			commandList.Add(CanvasRecorderCommand.FromTouchType(touch.TouchType));
			
			// take advantage of the fact that the X and Y positions could actually be held within a short - therefore we can
			// still get both in a single int
			commandList.Add((byte) ((int)position.X));
			commandList.Add((byte) (((int)position.X) >> 8));
			commandList.Add((byte) ((int)position.Y));
			commandList.Add((byte) (((int)position.Y) >> 8));
		}
	}
}

