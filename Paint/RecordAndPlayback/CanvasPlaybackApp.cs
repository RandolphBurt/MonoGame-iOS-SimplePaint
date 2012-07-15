/// <summary>
/// CanvasPlaybackApp.cs
/// Randolph Burt - April 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;
	
	/// <summary>
	/// Canvas playback app.
	/// </summary>
	public class CanvasPlaybackApp : Game
	{
		/// <summary>
		/// Our monogame GrahicsDeviceManager handles all the rendering
		/// Plays back a previous painting
		/// </summary>
		private GraphicsDeviceManager graphicsDeviceManager;
		
		/// <summary>
		/// The sprite batch handles all the drawing to the render targets / screen
		/// </summary>
		private SpriteBatch spriteBatch;

		/// <summary>
		/// Where we draw our picture - the canvas/paper/drawing board
		/// </summary>
		private ICanvas canvas;
		
		/// <summary>
		/// The canvas playback.
		/// </summary>
		private ICanvasPlayback canvasPlayback;
		
		/// <summary>
		/// The graphics texture map - contains all our graphics for buttons etc
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;

		/// <summary>
		/// Background color for our app
		/// </summary>
		private readonly Color BackgroundColor = Color.White;
		
		/// <summary>
		/// The current color we are using to draw
		/// </summary>
		private Color currentColor = Color.White;
		
		/// <summary>
		/// The brush 
		/// </summary>
		private Rectangle brush;
		
		/// <summary>
		/// An in memory render target - as the user draws on the canvas then so we update this rendertarget.
		/// Then each 'draw cycle' we render this to the screen.
		/// </summary>
		private RenderTarget2D inMemoryCanvasRenderTarget;
		
		/// <summary>
		/// Indicates whether we need to draw all outstanding touch points before we retrieve any more from the playback
		/// </summary>
		private bool drawRequiredBeforeNextUpdate = false;

		/// <summary>
		/// Keeps track of all touch/gestures made on the canvas since the last Draw command- is then reset after handled by the Draw.
		/// </summary>
		private List<ITouchPoint> canvasTouchPoints = new List<ITouchPoint>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasPlaybackApp"/> class.
		/// </summary>
		public CanvasPlaybackApp(ICanvasPlayback canvasPlayback)
		{
			this.graphicsDeviceManager = new GraphicsDeviceManager(this);
			this.graphicsDeviceManager.IsFullScreen = true;
			
			this.Content.RootDirectory = "Content";
			
			this.canvasPlayback = canvasPlayback;
		}

		/// <summary>
		/// We load any content we need at the beginning of the application life cycle.
		/// Also anything that needs initialising is done here
		/// </summary>
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch (graphicsDeviceManager.GraphicsDevice);
			
			var screenHeight = this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight;

			var graphicsTextureMap = Content.Load<Texture2D> ("graphics.png");
			this.graphicsDisplay = new GraphicsDisplay(graphicsTextureMap, this.spriteBatch, screenHeight > 1024); // TODO - check resolution!
						
			this.CreateCanvas();
		}

		/// <summary>
		/// Enable the capturing of gestures on the device
		/// </summary>
		protected override void Initialize()
        {
			// TODO
            // Enable the gestures we care about. You must set EnabledGestures before
            // you can use any of the other gesture APIs.
            TouchPanel.EnabledGestures =
                GestureType.Tap | 
                GestureType.FreeDrag |
				GestureType.DragComplete;
			
            base.Initialize();
        }

	
		/// <summary>
		/// Called everytime we need to redraw the screen
		/// </summary>
		/// <param name='gameTime' >
		/// Allows you to monitor time passed since last draw
		/// </param>
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice device = this.graphicsDeviceManager.GraphicsDevice;
			
			// First set the render target to be our image that we update as we go along...
			device.SetRenderTarget(inMemoryCanvasRenderTarget);
			this.canvas.Draw(this.currentColor, this.brush, this.canvasTouchPoints);
			
			// TOOD - we need to draw the pause/go/speed selector (playback controls)
//			device.SetRenderTarget(inMemoryToolboxRenderTarget);
//			this.toolBox.Draw(this.initialDraw);
			
			// Thes switch back to drawing onto the screen where we copy our image on to the screen
			device.SetRenderTarget (null);
			device.Clear (this.BackgroundColor);
			
			this.graphicsDisplay.BeginRender();
									
			this.DrawCanvasOnScreen();
//			this.DrawToolboxOnScreen(); // TODO - draw playback control on screen
			
			this.graphicsDisplay.EndRender();
			
			// We've dealt with all the touch points so now we can start with a new list
			this.canvasTouchPoints = new List<ITouchPoint>();
			
			base.Draw(gameTime);
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
				if (this.canvasTouchPoints.Count > 0)
				{
					// we still have touchpoints to draw!
					return;
				}
				
				this.drawRequiredBeforeNextUpdate = false;
			}
			
			// We need to cahce the current color and brush size here to avoid the situation where the color/brush
			// has been updated on the playback class ready for the next set of touch points but we have not yet 
			// drawn the current touch points.  If we did not track the color/brush size ourselves then we would end
			// up using the wrong color/brush size ahead of time
			this.currentColor = this.canvasPlayback.Color;
			this.brush = this.canvasPlayback.Brush;
			
			var touchPoint = this.canvasPlayback.GetNextTouchPoint();
			
			if (touchPoint != null)
			{
				this.canvasTouchPoints.Add(touchPoint);
			}
			else 
			{
				this.drawRequiredBeforeNextUpdate = true;
			}
			                           
			base.Update (gameTime);
		}		


		/// <summary>
		/// Draws the canvas on screen.
		/// </summary>
		private void DrawCanvasOnScreen()
		{
			this.spriteBatch.Draw(this.inMemoryCanvasRenderTarget, Vector2.Zero, this.BackgroundColor);
		}		

		/// <summary>
		/// Creates the canvas.
		/// </summary>
		private void CreateCanvas()
		{
			this.canvas = new Canvas(this.graphicsDisplay);
			
			this.inMemoryCanvasRenderTarget = new RenderTarget2D(
				this.graphicsDeviceManager.GraphicsDevice, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight);		

			// Strange behaviour where the image used by the previous 'Game' is left in the RenderTarget2D
			// therefore we blank the rendertarget first to ensure nothing left behind
			this.BlankRenderTarget(this.inMemoryCanvasRenderTarget);
		}
		
		/// <summary>
		/// Blanks the render target.
		/// </summary>
		/// <param name='renderTarget'>
		/// Render target.
		/// </param>
		private void BlankRenderTarget(RenderTarget2D renderTarget)
		{
			var device = this.graphicsDeviceManager.GraphicsDevice;
			
			device.SetRenderTarget(renderTarget);
			
			this.graphicsDisplay.BeginRender();
			
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, renderTarget.Bounds, this.BackgroundColor);
			
			this.graphicsDisplay.EndRender();
		}
	}
}

