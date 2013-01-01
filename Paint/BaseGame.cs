/// <summary>
/// BaseGame.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;

	using Paint.ToolboxLayout;

	/// <summary>
	/// Base game - derived from the MonoGame Game class and acts as a base class for our separate 
	/// 'paint' and 'playback' apps
	/// </summary>
	public abstract class BaseGame : Game
	{
		/// <summary>
		/// Background color for our app
		/// </summary>
		protected readonly Color BackgroundColor = Color.White;
		
		/// <summary>
		/// Our monogame GrahicsDeviceManager handles all the rendering
		/// </summary>
		protected GraphicsDeviceManager GraphicsDeviceManager;
		
		/// <summary>
		/// The graphics texture map - contains all our graphics for buttons etc
		/// </summary>
		protected IGraphicsDisplay GraphicsDisplay;
		
		/// <summary>
		/// The sprite batch handles all the drawing to the render targets / screen
		/// </summary>
		protected SpriteBatch SpriteBatch;
		
		/// <summary>
		/// Where we draw our picture - the canvas/paper/drawing board
		/// </summary>
		protected ICanvas Canvas;
		
		/// <summary>
		/// An in memory render target - as the user draws on the canvas then so we update this rendertarget.
		/// Then each 'draw cycle' we render this to the screen.
		/// </summary>
		protected RenderTarget2D InMemoryCanvasRenderTarget;
		
		/// <summary>
		/// An in memory render target for holding the toolbox
		/// Then each 'draw cycle' we render this to the screen.
		/// </summary>
		protected RenderTarget2D InMemoryToolboxRenderTarget;
		
		/// <summary>
		/// Defines the layout of all the controls in the toolbox
		/// </summary>
		protected ToolboxLayoutDefinition ToolboxLayoutDefinition = null;
		
		/// <summary>
		/// The image state data. height/width of image and details of save points (undo/redo state)
		/// </summary>
		protected ImageStateData ImageStateData = null;

		/// <summary>
		/// Keeps track of all touch/gestures made on the canvas since the last Draw command- is then reset after handled by the Draw.
		/// </summary>
		protected List<ITouchPoint> CanvasTouchPoints = new List<ITouchPoint>();

		/// <summary>
		/// Simply tracks whether this is the very first time we are drawing the canvas - if so then we need to draw everything on each control/tool.
		/// </summary>
		protected bool InitialDraw = true;		

		/// <summary>
		/// The tool box - contains all our controls/buttons for the user to interact with the app
		/// </summary>
		protected IToolBox ToolBox;

		/// <summary>
		/// Keep track of the previous gesture/touch-type that was made by the user.
		/// </summary>
		private TouchType previousTouchType = TouchType.DragComplete;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.BaseGame"/> class.
		/// </summary>
		/// <param name='imageStateData'>ImageSaveData</param>
		/// <param name='toolboxLayoutDefinition'>Layout of the toolbox</param>
		public BaseGame(ImageStateData imageStateData, ToolboxLayoutDefinition toolboxLayoutDefinition)
		{
			this.ImageStateData = imageStateData;
			this.ToolboxLayoutDefinition = toolboxLayoutDefinition;

			this.GraphicsDeviceManager = new GraphicsDeviceManager(this);
			this.GraphicsDeviceManager.IsFullScreen = true;

			if (imageStateData.Width > imageStateData.Height)
			{
				this.GraphicsDeviceManager.SupportedOrientations = 
					DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
			}
			else
			{
				this.GraphicsDeviceManager.SupportedOrientations = 
					DisplayOrientation.Portrait | DisplayOrientation.PortraitUpsideDown;
			}
			
			this.Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Creates the toolbox.
		/// </summary>
		/// <returns>The toolbox.</returns>
		/// <param name='scale'>
		/// determine if we are a retina or not - if so then we'll need to double (scale = 2) our layout locations/sizes
		/// </param>
		protected abstract IToolBox CreateToolbox(int scale);

		/// <summary>
		/// We load any content we need at the beginning of the application life cycle.
		/// Also anything that needs initialising is done here
		/// </summary>
		protected override void LoadContent()
		{
			this.SpriteBatch = new SpriteBatch (GraphicsDeviceManager.GraphicsDevice);

			// determine if we are a retina or not - 
			// if so then we'll need to double (scale = 2) our layout locations/sizes
			// and load a bigger spritemap
			int scale = Math.Max(this.ImageStateData.Height, this.ImageStateData.Width) / 1024;			
			bool highResolution = scale > 1;

			Texture2D graphicsTextureMap = null;
			if (highResolution)
			{
				graphicsTextureMap = Content.Load<Texture2D>("graphics@2x.png");
			}
			else
			{
				graphicsTextureMap = Content.Load<Texture2D>("graphics.png");
			}
			
			this.GraphicsDisplay = new GraphicsDisplay(graphicsTextureMap, this.SpriteBatch, highResolution);

			this.Canvas = new Canvas(this.GraphicsDisplay);

			this.ToolBox = CreateToolbox(scale);

			this.CreateRenderTargets();
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
			
			base.Initialize();
		}

		/// <summary>
		/// Called everytime we need to redraw the screen
		/// </summary>
		/// <param name='gameTime' >
		/// Allows you to monitor time passed since last draw
		/// </param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice device = this.GraphicsDeviceManager.GraphicsDevice;
			
			// First set the render target to be our image that we update as we go along...
			device.SetRenderTarget(InMemoryCanvasRenderTarget);
			
			this.Canvas.Draw(this.CanvasTouchPoints);

			// Then we draw the toolbox
			device.SetRenderTarget(InMemoryToolboxRenderTarget);
			this.ToolBox.Draw(this.InitialDraw);
			
			// Thes switch back to drawing onto the screen where we copy our image on to the screen
			device.SetRenderTarget(null);
			device.Clear(this.BackgroundColor);
			
			this.GraphicsDisplay.BeginRender();
			
			this.DrawCanvasOnScreen();
			this.DrawToolboxOnScreen();
			
			this.GraphicsDisplay.EndRender();
			
			this.InitialDraw = false;
			
			// We've dealt with all the touch points so now we can start with a new list
			this.CanvasTouchPoints = new List<ITouchPoint>();
			
			base.Draw(gameTime);
		}
		
		/// <summary>
		/// Creates the canvas.
		/// </summary>
		protected virtual void CreateRenderTargets()
		{
			this.InMemoryToolboxRenderTarget = new RenderTarget2D(
				this.GraphicsDeviceManager.GraphicsDevice, 
				this.ImageStateData.Width,
				this.ImageStateData.Height);

			this.InMemoryCanvasRenderTarget = new RenderTarget2D(
				this.GraphicsDeviceManager.GraphicsDevice, 
				this.ImageStateData.Width, 
				this.ImageStateData.Height);		
			
			// Strange behaviour where the image used by the previous 'Game' is left in the RenderTarget2D
			// therefore we blank the rendertarget first to ensure nothing left behind
			this.BlankRenderTarget(this.InMemoryCanvasRenderTarget);
		}

		/// <summary>
		/// Checks whether the user has touched inside the toolbox
		/// </summary>
		/// <returns>
		/// true if the touch is within the toolbox, false if not
		/// </returns>
		/// <param name='touchPoint' Where the user touched the screen />
		protected bool CheckToolboxCollision(TouchPoint touchPoint)
		{
			bool touchInToolbox = false;			
			TouchPoint offsetCollisionPoint = touchPoint;
			
			if (this.ToolBox.DockPosition == DockPosition.Bottom)
			{
				int toolboxPositionY = (this.ImageStateData.Height - this.ToolBox.ToolboxHeight);
				Vector2 offsetPosition = new Vector2(touchPoint.Position.X, touchPoint.Position.Y - toolboxPositionY);
				
				offsetCollisionPoint = new TouchPoint(
					offsetPosition, 
					touchPoint.TouchType,
					touchPoint.Color, 
					touchPoint.Size);
				
				if (touchPoint.Position.Y >= toolboxPositionY)
				{
					touchInToolbox = true;
				}
			}
			else
			{
				if (touchPoint.Position.Y <= this.ToolBox.ToolboxHeight)
				{
					touchInToolbox = true;
				}				
			}
			
			if (touchInToolbox == false)
			{
				return false;
			}
			
			this.ToolBox.CheckTouchCollision(offsetCollisionPoint);
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
		protected TouchType ConvertGestureType(GestureType gestureType)
		{
			/* If the user taps the screen then we get a single Tap
			 * If the user touches the screen and drags then we get several FreeDrag events and a final DragComplete event.
			 * We keep track of the previous GestureType so that we can characterise the first FreeDrag as a StartDrag.  This 
			 * allows the Canvas control to track where the drag started - I've decided that while a drag continues then all input
			 * should be handled by the control where the drag started, even if the user accidentally moves outside that control.
			 * Makes for a better user experience.
			 */

			TouchType returnValue;

			switch (gestureType)
			{
				case GestureType.Tap:
					returnValue = TouchType.Tap;
					break;
					
				case GestureType.FreeDrag:
					if (this.previousTouchType != TouchType.FreeDrag && this.previousTouchType != TouchType.StartDrag)
					{
						returnValue = TouchType.StartDrag;
					}
					else
					{
						returnValue = TouchType.FreeDrag;
					}

					break;
					
				case GestureType.DragComplete:
				default:
					returnValue = TouchType.DragComplete;
					break;
			}

			this.previousTouchType = returnValue;

			return returnValue;
		}
		
		/// <summary>
		/// Converts the screen touchpoint to a canvas touchpoint - i.e. alters the position of the touch to take into account the position and size
		/// of the toolbox
		/// </summary>
		/// <returns>
		/// A TouchPoint in Canvas co-ordinates
		/// </returns>
		/// <param name='screenTouchPoint' The touch point in screen co-ordinates/>
		protected TouchPoint ConvertScreenTouchToCanvasTouch(TouchPoint screenTouchPoint)
		{
			if (this.ToolBox.DockPosition == DockPosition.Top)
			{
				Vector2 offsetPosition = new Vector2(screenTouchPoint.Position.X, screenTouchPoint.Position.Y - this.ToolBox.ToolboxMinimizedHeight);
				return new TouchPoint(
					offsetPosition, 
					screenTouchPoint.TouchType,
					screenTouchPoint.Color,
					screenTouchPoint.Size);
			}
			
			return screenTouchPoint;
		}

		/// <summary>
		/// Blanks the render target.
		/// </summary>
		/// <param name='renderTarget'>
		/// Render target.
		/// </param>
		protected void BlankRenderTarget(RenderTarget2D renderTarget)
		{
			var device = this.GraphicsDeviceManager.GraphicsDevice;
			
			device.SetRenderTarget(renderTarget);
			
			this.GraphicsDisplay.BeginRender();
			
			this.GraphicsDisplay.DrawGraphic(ImageType.EmptySquare, renderTarget.Bounds, this.BackgroundColor);
			
			this.GraphicsDisplay.EndRender();
		}		

		/// <summary>
		/// Draws the canvas on screen.
		/// </summary>
		private void DrawCanvasOnScreen()
		{
			Vector2 canvasPosition = Vector2.Zero;
			
			if (this.ToolBox.DockPosition == DockPosition.Top)
			{
				canvasPosition = new Vector2(0, this.ToolBox.ToolboxMinimizedHeight);
			}
			
			this.SpriteBatch.Draw(this.InMemoryCanvasRenderTarget, canvasPosition, this.BackgroundColor);
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
					this.InMemoryToolboxRenderTarget.Width, 
					this.ToolBox.ToolboxHeight);
			
			Vector2 toolboxPosition = Vector2.Zero;
			if (this.ToolBox.DockPosition == DockPosition.Bottom)
			{
				toolboxPosition = new Vector2(0, InMemoryToolboxRenderTarget.Height - this.ToolBox.ToolboxHeight);
			}
			
			// Blank the square where the toolbox will go - this ensures that none of the canvas shows through where
			// there is transparency.
			this.GraphicsDisplay.DrawGraphic(
				ImageType.EmptySquare, 
				new Rectangle((int)toolboxPosition.X, (int)toolboxPosition.Y, toolboxBounds.Width, toolboxBounds.Height), 
				this.BackgroundColor);
			
			this.SpriteBatch.Draw(this.InMemoryToolboxRenderTarget, toolboxPosition, toolboxBounds, this.BackgroundColor);
		}
	}
}

