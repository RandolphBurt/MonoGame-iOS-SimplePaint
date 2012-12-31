/// <summary>
/// CanvasPlayback.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	using System.IO;
	
	using Microsoft.Xna;
	using Microsoft.Xna.Framework;
	
	/// <summary>
	/// Canvas playback.
	/// </summary>
	public class CanvasPlayback : ICanvasPlayback, IDisposable
	{
		/// <summary>
		/// The number of bytes per command
		/// </summary>
		private const int BytesPerCommand = 5;

		/// <summary>
		/// The file we will read that contains all the playback commands
		/// </summary>
		private FileStream fileStream = null;

		// Track whether Dispose has been called.
        private bool disposed = false;
		
		/// <summary>
		/// How many commands in the file
		/// </summary>
		private int playbackCommandTotal = 0;
		
		/// <summary>
		/// The commands read from the file so far.
		/// </summary>
		private int commandsReadSoFar = 0;
		
		/// <summary>
		/// Array to hold the details of the next command - populated by reading the fileStream
		/// </summary>
		private byte[] commandByteArray = new byte[BytesPerCommand];

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasPlayback"/> class.
		/// </summary>
		/// <param name='filename'>Name of the file containing all the playback commands</param>
		public CanvasPlayback(string filename)
		{
			this.fileStream = File.OpenRead(filename);
						
			this.playbackCommandTotal = 
				fileStream.ReadByte() |
				(fileStream.ReadByte()) << 8 |
				(fileStream.ReadByte()) << 16 |
				(fileStream.ReadByte()) << 24;
			
			// skip past the final brush size and color as we don't need to know them
			fileStream.Seek(8, SeekOrigin.Current);
			
			this.Color = Color.White;
		}
		
		/// <summary>
		/// Gets the current color used for drawing
		/// </summary>
		public Color Color
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the current brush size used for drawing
		/// </summary>
		public Rectangle Brush
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets a value indicating whether there are still any touchpoints left to playback
		/// </summary>
		public bool DataAvailable
		{
			get
			{
				return this.commandsReadSoFar < this.playbackCommandTotal;
			}
		}
		
		/// <summary>
		/// Gets the next touch point.
		/// </summary>
		/// <returns>
		/// The next touch point.
		/// A return value of null means that we are setting the colour or brush size - so no drawing happening 
		/// </returns>
		public ITouchPoint GetNextTouchPoint()
		{
			while (this.commandsReadSoFar < this.playbackCommandTotal)
			{
				fileStream.Read(this.commandByteArray, 0, BytesPerCommand);
				
				this.commandsReadSoFar++;
				
				switch (this.commandByteArray[0])
				{
					case CanvasRecorderCommand.SetColor:
						this.SetColor();
						return null;
						
					case CanvasRecorderCommand.SetBrushSize:
						this.SetBrushSize();
						return null;
					
					case CanvasRecorderCommand.Tap:
					case CanvasRecorderCommand.StartDrag:
					case CanvasRecorderCommand.FreeDrag:
					case CanvasRecorderCommand.DragComplete:
					default:
						return this.CreateTouchPoint(this.commandByteArray[0]);
				}
			}
			
			// Nothing more to read so let's close the file now.
			this.CloseFileStream();
			
			return null;
		}
		
		/// <summary>
		/// Creates a TouchPoint based on the data in the commandByteArray
		/// </summary>
		/// <returns>
		/// The touch point oorresponding to the user's paint command
		/// </returns>
		/// <param name='canvasRecorderCommand'>
		/// Canvas recorder command.
		/// </param>
		private ITouchPoint CreateTouchPoint(byte canvasRecorderCommand)
		{
			var touchType = CanvasRecorderCommand.ToTouchType(canvasRecorderCommand);
			
			int positionX = this.commandByteArray[1] | this.commandByteArray[2] << 8;
			int positionY = this.commandByteArray[3] | this.commandByteArray[4] << 8;
			
			return new TouchPoint(
				new Vector2(positionX, positionY), 
				touchType,
				this.Color,
				this.Brush);
		}
		
		/// <summary>
		/// Sets the size of the brush based on the data in the commandByteArray
		/// </summary>
		private void SetBrushSize()
		{
			int brushSize =  
				this.commandByteArray[1] |
				this.commandByteArray[2] << 8 |
				this.commandByteArray[3] << 16 |
				this.commandByteArray[4] << 24;			
			
			this.Brush = new Rectangle(0, 0, brushSize, brushSize);
		}
		
		/// <summary>
		/// Sets the color based on the data in the commandByteArray
		/// </summary>
		private void SetColor()
		{
			Color newColor;
			newColor.A = this.commandByteArray[1];
			newColor.R = this.commandByteArray[2];
			newColor.G = this.commandByteArray[3];
			newColor.B = this.commandByteArray[4];
			
			this.Color = newColor;
		}
		
		/// <summary>
		/// Closes the file stream.
		/// </summary>
		private void CloseFileStream()
		{
			if (this.fileStream != null)
			{
				this.fileStream.Close();
				
				this.fileStream = null;
			}
		}

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~CanvasPlayback()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
           this.Dispose(false);
        }
		
		/// <summary>
		/// Releases all resource used by the <see cref="Paint.CanvasPlayback"/> object.
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="Paint.CanvasPlayback"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Paint.CanvasPlayback"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Paint.CanvasPlayback"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Paint.CanvasPlayback"/> was occupying.
		/// </remarks>
        public void Dispose()
        {
            this.Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
					        
		/// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
		/// </summary>
		/// <param name='disposing'>
		/// Disposing.
		/// </param>
		protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
				this.CloseFileStream();
				
                 // Note disposing has been done.
                this.disposed = true;
            }
        }       
	}
}

