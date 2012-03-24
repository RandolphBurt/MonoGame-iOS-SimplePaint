/// <summary>
/// PaintApp.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Audio;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Input.Touch;
	using Microsoft.Xna.Framework.Media;
	using Microsoft.Xna.Framework.Storage;
	
	/// <summary>
	/// The main application class just inherits from the monogame:Game class
	/// Override a few key methods and away we go...
	/// </summary>
	public class PaintApp : Game
	{
		/// <summary>
		/// The maximum size of the brush
		/// </summary>
		private readonly int MaxBrushSize = 50;

		/// <summary>
		/// The minimum size of the brush
		/// </summary>
		private readonly int MinBrushSize = 1;
		
		/// <summary>
		/// The initial size of the brush
		/// </summary>
		private readonly int StartBrushSize = 10;
		
		/// <summary>
		/// Background color for our app
		/// </summary>
		private readonly Color BackgroundColor = Color.White;
		
		/// <summary>
		/// The borders of all controls will be black
		/// </summary>
		private readonly Color BorderColor = Color.Black;

		/// <summary>
		/// The color we will set the brush to start with
		/// </summary>
		private readonly Color StartColor = Color.Green;

		/// <summary>
		/// Our monogame GrahicsDeviceManager handles all the rendering
		/// </summary>
		private GraphicsDeviceManager graphicsDeviceManager;

		/// <summary>
		/// We have a single transparent png file with which we draw everything - just simply specify the color we want to use whenever we use it - nice and simple.
		/// </summary>
		private Texture2D transparentSquareTexture;
		
		/// <summary>
		/// An in memory render target - as the user draws on the canvas then so we update this rendertarget.
		/// Then each 'draw cycle' we render this to the screen.
		/// </summary>
		private RenderTarget2D inMemoryCanvasRenderTarget;

		/// <summary>
		/// An in memory render target for holding the toolbox
		/// Then each 'draw cycle' we render this to the screen.
		/// </summary>
		private RenderTarget2D inMemoryToolboxRenderTarget;

		/// <summary>
		/// The sprite batch handles all the drawing to the render targets / screen
		/// </summary>
		private SpriteBatch spriteBatch;
				
		/// <summary>
		/// Where we draw our picture - the canvas/paper/drawing board
		/// </summary>
		private ICanvas canvas;
		
		/// <summary>
		/// The tool box - contains all our color pickers and brush size controls
		/// </summary>
		private IToolBox toolBox;
		
		/// <summary>
		/// Simply tracks whether this is the very first time we are drawing the canvas - if so then we need to draw everything on each control/tool.
		/// </summary>
		private bool initialDraw = true;
		
		/// <summary>
		/// Keep track of the previous gesture/touch-type that was made by the user.
		/// </summary>
		private TouchType previousTouchType = TouchType.DragComplete;
		
		/// <summary>
		/// The height of the screen.
		/// </summary>
		private int screenHeight;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PaintApp"/> class.
		/// Instantiate our GraphicsDeviceManager and establish where the content folder is
		/// </summary>
		public PaintApp()
		{
			this.graphicsDeviceManager = new GraphicsDeviceManager(this);
			this.graphicsDeviceManager.IsFullScreen = true;
			
			this.Content.RootDirectory = "Content";
		}
				
		/// <summary>
		/// We load any content we need at the beginning of the application life cycle.
		/// Also anything that needs initialising is done here
		/// </summary>
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch (graphicsDeviceManager.GraphicsDevice);
			this.transparentSquareTexture = Content.Load<Texture2D> ("transparent.png");
						
			this.CreateCanvas();
			this.CreateToolbox();
		}
		
		/// <summary>
		/// Enable the capturing of gestures on the device
		/// </summary>
		protected override void Initialize()
        {
            // Enable the gestures we care about. You must set EnabledGestures before
            // you can use any of the other gesture APIs.
            TouchPanel.EnabledGestures =
                GestureType.Tap | 
                GestureType.FreeDrag |
				GestureType.DragComplete;
			
			this.screenHeight = this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight;
			
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
			this.canvas.Draw(this.toolBox.Color, this.toolBox.Brush, this.initialDraw);
			
			// Then we draw the toolbox
			device.SetRenderTarget(inMemoryToolboxRenderTarget);
			this.toolBox.Draw(this.initialDraw);
			
			// Thes switch back to drawing onto the screen where we copy our image on to the screen
			device.SetRenderTarget (null);
			device.Clear (this.BackgroundColor);
			
			spriteBatch.Begin();
									
			this.DrawCanvasOnScreen();
			this.DrawToolboxOnScreen();
			
			spriteBatch.End ();
			
			this.initialDraw = false;
			
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
			this.HandleInput();
			
			base.Update (gameTime);
		}
		
		/// <summary>
		/// Draws the canvas on screen.
		/// </summary>
		private void DrawCanvasOnScreen()
		{
			Vector2 canvasPosition = Vector2.Zero;
			
			if (this.toolBox.DockPosition == DockPosition.Top)
			{
				canvasPosition = new Vector2(0, this.toolBox.ToolboxMinimizedHeight);
			}

			this.spriteBatch.Draw(this.inMemoryCanvasRenderTarget, canvasPosition, this.BackgroundColor);
		}
		
		/// <summary>
		/// Draws the toolbox on screen.
		/// </summary>
		private void DrawToolboxOnScreen()
		{
			Rectangle toolboxBounds = 
				new Rectangle(
					0, 
					0,
					this.inMemoryToolboxRenderTarget.Width, 
					this.toolBox.ToolboxHeight);

			Vector2 toolboxPosition = Vector2.Zero;
			if (this.toolBox.DockPosition == DockPosition.Bottom)
			{
				toolboxPosition = new Vector2(0, inMemoryToolboxRenderTarget.Height - this.toolBox.ToolboxHeight);
			}
				
			this.spriteBatch.Draw(this.inMemoryToolboxRenderTarget, toolboxPosition, toolboxBounds, this.BackgroundColor);
		}
		
		/// <summary>
		/// Creates the canvas.
		/// </summary>
		private void CreateCanvas()
		{
			this.canvas = new Canvas(
				this.BackgroundColor, 
				this.BorderColor, 
				this.spriteBatch, 
				this.transparentSquareTexture, 
				new Microsoft.Xna.Framework.Rectangle(0, 0, 
			                                      this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, 
			                                      this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight));

			this.inMemoryCanvasRenderTarget = new RenderTarget2D(
				this.graphicsDeviceManager.GraphicsDevice, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight);			
		}
		
		/// <summary>
		/// Creates the toolbox.
		/// </summary>
		private void CreateToolbox()
		{
			// Pre defined color pickers
			Color[] colorList = new Color[] { 
				Color.White, 
				Color.Black, 
				Color.Red, 
				Color.Lime, 
				Color.Blue,
				Color.Yellow,
				Color.Cyan,
				Color.Fuchsia,
				Color.Pink, 
				Color.Orange,
				Color.Green, 
			};
			
			this.toolBox = new ToolBox(
				this.BackgroundColor, 
				this.BorderColor, 
				this.spriteBatch, 
				this.transparentSquareTexture, 
				colorList, 
				this.StartColor,
				this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth,
				this.MinBrushSize, 
				this.MaxBrushSize, 
				this.StartBrushSize);
			
			this.inMemoryToolboxRenderTarget = new RenderTarget2D(
				this.graphicsDeviceManager.GraphicsDevice, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight);			
			
			this.toolBox.ExitSelected += (sender, e) => 
			{
				SaveAndExit();
			};
		}
		
		private void SaveAndExit()
		{
			// TODO - save image to disk
			this.Exit();
		}
		
		/// <summary>
		/// Handles any user input.
		/// Collect all gestures made since the last 'update' - then pass this through to our Canvas to handle
		/// </summary>
		private void HandleInput()
		{
			List<ITouchPoint> canvasTouchPoints = new List<ITouchPoint>();
	
			while (TouchPanel.IsGestureAvailable)
            {
                // read the next gesture from the queue
                GestureSample gesture = TouchPanel.ReadGesture();
				
				TouchType touchType = this.ConvertGestureType(gesture.GestureType);
				
				TouchPoint touchPoint = new TouchPoint(gesture.Position, touchType);
				
				// First check if this can be handled by the toolbox - if not then we will keep for the canvas
				if (this.CheckToolboxCollision(touchPoint) == false)
				{
					canvasTouchPoints.Add(this.ConvertScreenTouchToCanvasTouch(touchPoint));
				}
				
				previousTouchType = touchType;
			}
			
			this.canvas.HandleTouchPoints(canvasTouchPoints);
		}
		
		/// <summary>
		/// Converts the screen touchpoint to a canvas touchpoint - i.e. alters the position of the touch to take into account the position and size
		/// of the toolbox
		/// </summary>
		/// <returns>
		/// A TouchPoint in Canvas co-ordinates
		/// </returns>
		/// <param name='screenTouchPoint' The touch point in screen co-ordinates/>
		private TouchPoint ConvertScreenTouchToCanvasTouch(TouchPoint screenTouchPoint)
		{
			if (this.toolBox.DockPosition == DockPosition.Top)
			{
				Vector2 offsetPosition = new Vector2(screenTouchPoint.Position.X, screenTouchPoint.Position.Y - this.toolBox.ToolboxMinimizedHeight);
				return new TouchPoint(offsetPosition, screenTouchPoint.TouchType);
			}
			
			return screenTouchPoint;
		}
		
		/// <summary>
		/// Checks whether the user has touched inside the toolbox
		/// </summary>
		/// <returns>
		/// true if the touch is within the toolbox, false if not
		/// </returns>
		/// <param name='touchPoint' Where the user touched the screen />
		private bool CheckToolboxCollision(TouchPoint touchPoint)
		{
			bool touchInToolbox = false;			
			TouchPoint offsetCollisionPoint = touchPoint;
			
			if (this.toolBox.DockPosition == DockPosition.Bottom)
			{
				int toolboxPositionY = (this.screenHeight - this.toolBox.ToolboxHeight);
				Vector2 offsetPosition = new Vector2(touchPoint.Position.X, touchPoint.Position.Y - toolboxPositionY);
				offsetCollisionPoint = new TouchPoint(offsetPosition, touchPoint.TouchType);
				
				if (touchPoint.Position.Y >= toolboxPositionY)
				{
					touchInToolbox = true;
				}
			}
			else 
			{
				if (touchPoint.Position.Y <= this.toolBox.ToolboxHeight)
				{
					touchInToolbox = true;
				}				
			}
						
			if (touchInToolbox == false)
			{
				return false;
			}
			
			this.toolBox.CheckTouchCollision(offsetCollisionPoint);
			return true;
		}
		
		/// <summary>
		/// Converts a MonoGame.GestureType into our TouchType representation
		/// </summary>
		/// <returns>
		/// The converted TouchType
		/// </returns>
		/// <param name='gestureType'>
		/// The monogame gesture type.
		/// </param>
		private TouchType ConvertGestureType(GestureType gestureType)
		{
			/* If the user taps the screen then we get a single Tap
			 * If the user touches the screen and drags then we get several FreeDrag events and a final DragComplete event.
			 * We keep track of the previous GestureType so that we can characterise the first FreeDrag as a StartDrag.  This 
			 * allows the Canvas control to track where the drag started - I've decided that while a drag continues then all input
			 * should be handled by the control where the drag started, even if the user accidentally moves outside that control.
			 * Makes for a better user experience.
			 */
			switch (gestureType)
			{
				case GestureType.Tap:
					return TouchType.Tap;
				
				case GestureType.FreeDrag:
					if (previousTouchType != TouchType.FreeDrag && previousTouchType != TouchType.StartDrag) 
					{
						return TouchType.StartDrag;
					}
					else 
					{
						return TouchType.FreeDrag;
					}
				
				case GestureType.DragComplete:
					return TouchType.DragComplete;
			}
			
			return TouchType.DragComplete;
		}
	}
}

