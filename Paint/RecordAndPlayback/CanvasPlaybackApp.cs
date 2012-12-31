/// <summary>
/// CanvasPlaybackApp.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;

	using Paint.ToolboxLayout;

	/// <summary>
	/// Canvas playback app.
	/// </summary>
	public class CanvasPlaybackApp : BaseGame
	{
		/// <summary>
		/// The canvas playback.
		/// </summary>
		private ICanvasPlayback canvasPlayback;

		/// <summary>
		/// The current color we are using to draw
		/// </summary>
		private Color currentColor = Color.White;
		
		/// <summary>
		/// The brush 
		/// </summary>
		private Rectangle brush;

		/// <summary>
		/// Indicates whether we need to draw all outstanding touch points before we retrieve any more from the playback
		/// </summary>
		private bool drawRequiredBeforeNextUpdate = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasPlaybackApp"/> class.
		/// </summary>
		/// <param name='canvasPlayback'>Canvas Playback data</param>
		/// <param name='imageStateData'>ImageSaveData</param>
		/// <param name='toolboxLayoutDefinition'>Layout of the toolbox</param>
		public CanvasPlaybackApp(
			ICanvasPlayback canvasPlayback, 
			ImageStateData imageStateData, 
			ToolboxLayoutDefinition toolboxLayoutDefinition)
			: base(imageStateData, toolboxLayoutDefinition)
		{
			this.canvasPlayback = canvasPlayback;
		}

		/// <summary>
		/// Returns the current brush size
		/// </summary>
		/// <returns>The brush size.</returns>
		protected override Rectangle CurrentBrushSize()
		{
			return this.brush;
		}
		
		/// <summary>
		/// Returns the Ccrrent color.
		/// </summary>
		/// <returns>The color.</returns>
		protected override Color CurrentColor()
		{
			return this.currentColor;
		}

		/// <summary>
		/// Creates the toolbox.
		/// </summary>
		/// <returns>The toolbox.</returns>
		/// <param name='scale'>
		/// determine if we are a retina or not - if so then we'll need to double (scale = 2) our layout locations/sizes
		/// </param>
		protected override IToolBox CreateToolbox(int scale)
		{
			// TODO
			return new PaintToolBox(this.ToolboxLayoutDefinition, this.GraphicsDisplay, scale);
		}

		/// <summary>
		/// Called often. Allows us to handle any user input (gestures on screen) and/or game time related work - e.g. moving an animation character based on
		/// elapsed time since last called etc
		/// </summary>
		/// <param name='gameTime'>
		/// Allows you to monitor time passed since last draw
		/// </param>
		protected override void Update (GameTime gameTime)
		{
			if (this.canvasPlayback.DataAvailable == false)
			{
				// TODO - sort out exit
				System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
				this.Exit();
			}
			
			if (this.drawRequiredBeforeNextUpdate == true)
			{
				if (this.CanvasTouchPoints.Count > 0)
				{
					// we still have touchpoints to draw!
					return;
				}
				
				this.drawRequiredBeforeNextUpdate = false;
			}
			
			// We need to cache the current color and brush size here to avoid the situation where the color/brush
			// has been updated on the playback class ready for the next set of touch points but we have not yet 
			// drawn the current touch points.  If we did not track the color/brush size ourselves then we would end
			// up using the wrong color/brush size ahead of time
			this.currentColor = this.canvasPlayback.Color;
			this.brush = this.canvasPlayback.Brush;
			
			var touchPoint = this.canvasPlayback.GetNextTouchPoint();
			
			if (touchPoint != null)
			{
				this.CanvasTouchPoints.Add(touchPoint);
			}
			else 
			{
				this.drawRequiredBeforeNextUpdate = true;
			}
			                           
			base.Update (gameTime);
		}		
	}
}

