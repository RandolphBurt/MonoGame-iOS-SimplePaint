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
		/// The number of bytes per command
		/// </summary>
		private const int BytesPerCommand = 5;
		
		/// <summary>
		/// The current color
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
		/// Draw the latest updates to our image/render target.
		/// <param name='touchPointList'>
		/// The list of all gestures / locations touched by the user since the last update
		/// </param>		
		/// </summary>
		public void Draw(List<ITouchPoint> touchPoints)
		{
			if (touchPoints.Count == 0)
			{
				// no point recording anything (even if color change - e.g. user has just set the color) because we haven't drawn 
				// anything on screen anyway.  When they do draw on screen (maybe after a few more color changes) then we will pick
				// up the color change at the same time
				return;
			}

			foreach (var touch in touchPoints)
			{
				if (touch.Color != this.currentColor)
				{
					RecordColorChange (touch.Color);
				}
				
				if (touch.Size.Width != this.currentBrushSize)
				{
					RecordBrushSizeChange (touch.Size);
				}

				this.RecordTouchPoint(touch);
			}
		}
		
		/// <summary>
		/// Save the Canvas to file ready for playback.
		/// <param name='filename' The file we are saving to />
		/// </summary>
		public void Save(string filename)
		{
			using (FileStream stream = File.Open(filename, FileMode.Create, FileAccess.Write))
			{
				int commandCount = this.commandList.Count / BytesPerCommand;
				
				// Write the command length
				stream.WriteByte((byte)commandCount);
				stream.WriteByte((byte)(commandCount >> 8));
				stream.WriteByte((byte)(commandCount >> 16));
				stream.WriteByte((byte)(commandCount >> 24));
				
				// Write the current brush size and color - this is required so that anyone continuing with this playback
				// file at a later time doesn't have to parse all the existing commands to know what color to continue
				// with
				stream.WriteByte((byte)this.currentBrushSize);
				stream.WriteByte((byte)(this.currentBrushSize >> 8));
				stream.WriteByte((byte)(this.currentBrushSize >> 16));
				stream.WriteByte((byte)(this.currentBrushSize >> 24));

				stream.WriteByte((byte)this.currentColor.A);
				stream.WriteByte((byte)this.currentColor.R);
				stream.WriteByte((byte)this.currentColor.G);
				stream.WriteByte((byte)this.currentColor.B);

				// Write all the commands
				stream.Write(this.commandList.ToArray(), 0, this.commandList.Count);
			}
		}
		
		/// <summary>
		/// Load an existing playback file - we will probably end up adding some more commands and resaving
		/// e.g. someone has pressed 'redo' so we are reloading an existing file.
		/// <param name='filename' The file we are loading from />
		/// </summary>
		public void Load(string filename)
		{
			using (FileStream stream = File.OpenRead(filename))
			{
				int playbackCommandTotal = 
					stream.ReadByte() |
					(stream.ReadByte()) << 8 |
					(stream.ReadByte()) << 16 |
					(stream.ReadByte()) << 24;
				
				this.currentBrushSize = 
					stream.ReadByte() |
					(stream.ReadByte()) << 8 |
					(stream.ReadByte()) << 16 |
					(stream.ReadByte()) << 24;
					
				this.currentColor.A = (byte)stream.ReadByte();
				this.currentColor.R = (byte)stream.ReadByte();
				this.currentColor.G = (byte)stream.ReadByte();
				this.currentColor.B = (byte)stream.ReadByte();

				var totalCommandBytes= playbackCommandTotal * BytesPerCommand;
				
				byte[] commands = new byte[totalCommandBytes];
				stream.Read(commands, 0, totalCommandBytes);
				
				this.commandList = new List<byte>(commands);
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

