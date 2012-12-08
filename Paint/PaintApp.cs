/// <summary>
/// PaintApp.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Audio;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Input.Touch;
	using Microsoft.Xna.Framework.Media;
	using Microsoft.Xna.Framework.Storage;

	using Paint.ToolboxLayout;

	/// <summary>
	/// The main application class just inherits from the monogame:Game class
	/// Override a few key methods and away we go...
	/// </summary>
	public class PaintApp : Game, IRenderTargertHandler
	{
		/// <summary>
		/// Background color for our app
		/// </summary>
		private readonly Color BackgroundColor = Color.White;

		/// <summary>
		/// Our monogame GrahicsDeviceManager handles all the rendering
		/// </summary>
		private GraphicsDeviceManager graphicsDeviceManager;

		/// <summary>
		/// The graphics texture map - contains all our graphics for buttons etc
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;
		
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
		/// The render targets used for tracking undo/redo - The image in the previous (wrapping) entry in the array 
		/// should be shown when we undo a change
		/// </summary>
		private RenderTarget2D[] undoRedoRenderTargets = null;
		
		/// <summary>
		/// The sprite batch handles all the drawing to the render targets / screen
		/// </summary>
		private SpriteBatch spriteBatch;
				
		/// <summary>
		/// Where we draw our picture - the canvas/paper/drawing board
		/// </summary>
		private ICanvas canvas;
		
		/// <summary>
		/// The picture state manager - handles playback and save/undo/redo
		/// </summary>
		private IPictureStateManager pictureStateManager;
		
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
		/// Keeps track of all touch/gestures made on the canvas since the last Draw command- is then reset after handled by the Draw.
		/// </summary>
		private List<ITouchPoint> canvasTouchPoints = new List<ITouchPoint>();
		
		/// <summary>
		/// The picture IO manager - handles all reading / writing images and information file
		/// </summary>
		private IPictureIOManager pictureIOManager;

		/// <summary>
		/// The filename resolver.
		/// </summary>
		private IFilenameResolver filenameResolver;
		
		/// <summary>
		/// The image state data. height/width of image and details of save points (undo/redo state)
		/// </summary>
		private ImageStateData imageStateData = null;

		/// <summary>
		/// What mode is the app currently running in paint, pending shutdown or actually exiting
		/// </summary>
		private PaintMode paintMode = PaintMode.Paint;
		
		/// <summary>
		/// view/form to present when we about to save all the data
		/// </summary>
		private IUIBusyMessage saveBusyMessageDisplay = null;

		/// <summary>
		/// Defines the layout of all the controls in the toolbox
		/// </summary>
		private ToolboxLayoutDefinition toolboxLayoutDefinition = null;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PaintApp"/> class.
		/// Instantiate our GraphicsDeviceManager and establish where the content folder is
		/// <param name='pictureIOManager'>Picture IO Manager</param>
		/// <param name='filenameResolver'>Filename Resolver</param>
		/// <param name='imageStateData'>ImageSaveData</param>
		/// <param name='saveBusyMessageDisplay'>Class for dsplaying the 'Busy - saving' message</param>
		/// <param name='toolboxLayoutDefinition'>Layout of the toolbox</param>
		/// </summary>
		public PaintApp(
			IPictureIOManager pictureIOManager, 
			IFilenameResolver filenameResolver, 
			ImageStateData imageStateData,
			IUIBusyMessage saveBusyMessageDisplay,
			ToolboxLayoutDefinition toolboxLayoutDefinition)
		{
			this.graphicsDeviceManager = new GraphicsDeviceManager(this);
			this.graphicsDeviceManager.IsFullScreen = true;
			this.filenameResolver = filenameResolver;
			this.pictureIOManager = pictureIOManager;
			this.imageStateData = imageStateData;
			this.saveBusyMessageDisplay = saveBusyMessageDisplay;
			this.toolboxLayoutDefinition = toolboxLayoutDefinition;
			
			if (imageStateData.Width > imageStateData.Height)
			{
				this.graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
			}
			else
			{
				this.graphicsDeviceManager.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitUpsideDown;
			}
			
			this.Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Forces the immediate Shutdown and saving of state on this thread
		/// We must not change thread because we are probably going into the background and therefore the main UI thread will not
		/// come alive again until the app is back in teh foreground again
		/// </summary>
		public void ForceSaveAndExit()
		{
			if (this.paintMode != PaintMode.Exiting)
			{
				this.paintMode = PaintMode.Exiting;
				this.SaveAndExit();
			}
		}
	
		/// <summary>
		/// Restore the image held in the specified save point to the canvas/display
		/// </summary>
		/// <param name='savePoint'>specific save point we wish to restore</param>
		public void RestoreSavePoint(int savePoint)
		{
			this.RenderImage(this.inMemoryCanvasRenderTarget, this.undoRedoRenderTargets[savePoint]);
		}
		
		/// <summary>
		/// Save the canvas/display to the specified save-point (undo/redo render target).
		/// </summary>
		/// <param name='savePoint'>specific save point we wish to use to store the canvas</param>
		public void StoreSavePoint(int savePoint)
		{
			this.RenderImage(this.undoRedoRenderTargets[savePoint], this.inMemoryCanvasRenderTarget);
		}		
		
		/// <summary>
		/// We load any content we need at the beginning of the application life cycle.
		/// Also anything that needs initialising is done here
		/// </summary>
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(graphicsDeviceManager.GraphicsDevice);
			
			Texture2D graphicsTextureMap = null;
			bool highResolution = Math.Max(this.imageStateData.Height, this.imageStateData.Width) > 1024;

			if (highResolution)
			{
				graphicsTextureMap = Content.Load<Texture2D>("graphics@2x.png");
			}
			else
			{
				graphicsTextureMap = Content.Load<Texture2D>("graphics.png");
			}

			this.graphicsDisplay = new GraphicsDisplay(graphicsTextureMap, this.spriteBatch, highResolution);
			
			this.CreateCanvas();
			this.CreateToolbox();
			this.CreatePictureStateManager();
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
			GraphicsDevice device = this.graphicsDeviceManager.GraphicsDevice;
									
			// First set the render target to be our image that we update as we go along...
			device.SetRenderTarget(inMemoryCanvasRenderTarget);
		
			this.canvas.Draw(this.toolBox.Color, this.toolBox.Brush, this.canvasTouchPoints);
			this.pictureStateManager.Draw(this.toolBox.Color, this.toolBox.Brush, this.canvasTouchPoints);
			
			// Then we draw the toolbox
			device.SetRenderTarget(inMemoryToolboxRenderTarget);
			this.toolBox.Draw(this.initialDraw);
			
			// Thes switch back to drawing onto the screen where we copy our image on to the screen
			device.SetRenderTarget(null);
			device.Clear(this.BackgroundColor);
			
			this.graphicsDisplay.BeginRender();
									
			this.DrawCanvasOnScreen();
			this.DrawToolboxOnScreen();
			
			this.graphicsDisplay.EndRender();
			
			this.initialDraw = false;
			
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
		protected override void Update(GameTime gameTime)
		{
			switch (this.paintMode)
			{
				case PaintMode.Paint:
					this.HandleInput();
					break;
					
				case PaintMode.Exiting:
					// We are in the process of exiting
					break;
			}
			
			base.Update(gameTime);
		}

		/// <summary>
		/// Saves all the data and then exits.
		/// </summary>
		private void SaveAndExit()
		{
			this.pictureStateManager.Save();
			this.pictureIOManager.SaveData(this.pictureStateManager.ImageStateData, this.inMemoryCanvasRenderTarget, this.undoRedoRenderTargets);
			
			foreach (var renderTarget in this.undoRedoRenderTargets)
			{
				if (renderTarget != null && renderTarget.IsDisposed == false)
				{
					renderTarget.Dispose();
				}
			}
			
			this.undoRedoRenderTargets = null;
			
			this.Exit();
		}

		/// <summary>
		/// Renders a specific source RenderTarget on to a specific target RenderTarget
		/// </summary>
		/// <param name='target'>Where we want to render the image</param>
		/// <param name='source'>The source image we wish to render</param>
		private void RenderImage(RenderTarget2D target, RenderTarget2D source)
		{
			GraphicsDevice device = this.graphicsDeviceManager.GraphicsDevice;	
			device.SetRenderTarget(target);
			this.spriteBatch.Begin();
			device.Clear(this.BackgroundColor);
			this.spriteBatch.Draw(source, Vector2.Zero, this.BackgroundColor);
			this.spriteBatch.End();	                      
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
			
			// Blank the square where the toolbox will go - this ensures that none of the canvas shows through where
			// there is transparency.
			this.graphicsDisplay.DrawGraphic(
				ImageType.EmptySquare, 
				new Rectangle((int)toolboxPosition.X, (int)toolboxPosition.Y, toolboxBounds.Width, toolboxBounds.Height), 
				this.BackgroundColor);
			
			this.spriteBatch.Draw(this.inMemoryToolboxRenderTarget, toolboxPosition, toolboxBounds, this.BackgroundColor);
		}
				
		/// <summary>
		/// Creates the canvas.
		/// </summary>
		private void CreateCanvas()
		{
			this.canvas = new Canvas(this.graphicsDisplay);

			var device = this.graphicsDeviceManager.GraphicsDevice;
			var width = this.imageStateData.Width;
			var height = this.imageStateData.Height;
			
			List<RenderTarget2D> renderTargetList = new List<RenderTarget2D>();
			
			for (short count = 0; count < this.imageStateData.MaxUndoRedoCount; count++)
			{
				renderTargetList.Add(new RenderTarget2D(device, width, height));
			}
						
			this.undoRedoRenderTargets = renderTargetList.ToArray();
			this.inMemoryCanvasRenderTarget = new RenderTarget2D(device, width, height);					

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
		
		/// <summary>
		/// Creates the picture state manager.
		/// </summary>
		private void CreatePictureStateManager()
		{
			if (File.Exists(this.filenameResolver.MasterImageInfoFilename) == true)
			{
				// existing image so we load the rendertargetlist from disk
				this.pictureIOManager.LoadData(this.graphicsDeviceManager.GraphicsDevice, this.spriteBatch, this.undoRedoRenderTargets, this.BackgroundColor);
			}
			
			this.pictureStateManager = new PictureStateManager(this.filenameResolver, this.pictureIOManager, this, this.imageStateData);			
			this.pictureStateManager.RedoEnabledChanged += (sender, e) => 
			{
				this.toolBox.RedoEnabled = this.pictureStateManager.RedoEnabled;
			};
			
			this.pictureStateManager.UndoEnabledChanged += (sender, e) => 
			{
				this.toolBox.UndoEnabled = this.pictureStateManager.UndoEnabled;
			};

			this.pictureStateManager.InitialisePictureState();			
		}
		
		/// <summary>
		/// Creates the toolbox.
		/// </summary>
		private void CreateToolbox()
		{
			// determine if we are a retina or not - if so then we'll need to double (scale = 2) our layout locations/sizes
			int scale = Math.Max(this.imageStateData.Height, this.imageStateData.Width) / 1024;

			this.toolBox = new ToolBox(this.toolboxLayoutDefinition, this.graphicsDisplay, scale);
			
			this.inMemoryToolboxRenderTarget = new RenderTarget2D(
				this.graphicsDeviceManager.GraphicsDevice, 
				this.imageStateData.Width,
				this.imageStateData.Height);
			
			this.toolBox.ExitSelected += (sender, e) => 
			{
				this.InitiateShutdown();
			};
			
			this.toolBox.RedoSelected += (sender, e) => 
			{
				this.pictureStateManager.Redo();
			};
			
			this.toolBox.UndoSelected += (sender, e) => 
			{
				this.pictureStateManager.Undo();
			};
		}

		/// <summary>
		/// Initiates the shutdown process.
		/// We will display a 'busy' form/window -- we pass in a delegate/action to be run once the form 
		/// has displayed
		/// </summary>
		private void InitiateShutdown()
		{
			if (this.paintMode != PaintMode.Exiting)
			{
				this.paintMode = PaintMode.Exiting;			
				this.saveBusyMessageDisplay.Show(new Action(this.SaveAndExit));
			}
		}

		/// <summary>
		/// Handles any user input.
		/// Collect all gestures made since the last 'update' - stores these ready to be handled by the Canvas for drawing
		/// </summary>
		private void HandleInput()
		{	
			while (TouchPanel.IsGestureAvailable)
			{
				// read the next gesture from the queue
				GestureSample gesture = TouchPanel.ReadGesture();
				
				TouchType touchType = this.ConvertGestureType(gesture.GestureType);
				
				TouchPoint touchPoint = new TouchPoint(gesture.Position, touchType);
				
				// First check if this can be handled by the toolbox - if not then we will keep for the canvas
				if (this.CheckToolboxCollision(touchPoint) == false)
				{
					this.canvasTouchPoints.Add(this.ConvertScreenTouchToCanvasTouch(touchPoint));
				}
				
				this.previousTouchType = touchType;
			}
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
				int toolboxPositionY = (this.imageStateData.Height - this.toolBox.ToolboxHeight);
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
					if (this.previousTouchType != TouchType.FreeDrag && this.previousTouchType != TouchType.StartDrag)
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
		
		/// <summary>
		/// What mode is the app currently in
		/// </summary>
		private enum PaintMode
		{
			/// <summary>
			/// The user us able to paint
			/// </summary>
			Paint,
			
			/// <summary>
			/// The app is in the process of exiting
			/// </summary>
			Exiting
		}
	}
}

