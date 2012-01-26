/// <summary>
/// PaintApp.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	
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
		/// Background color for our app
		/// </summary>
		private readonly Color BackgroundColor = Color.White;
		
		/// <summary>
		/// The borders of all controls will be black
		/// </summary>
		private readonly Color BorderColor = Color.Black;
		
		/// <summary>
		/// Our monogame GrahicsDeviceManager handles all the rendering
		/// </summary>
		private GraphicsDeviceManager graphicsDeviceManager;

		/// <summary>
		/// We have a single transparent png file with which we draw everything - just simply specify the color we want to use whenever we use it - nice and simple.
		/// </summary>
		private Texture2D transparentSquareTexture;
		
		/// <summary>
		/// An in memory render target - as the user draws on screen then so we update this rendertarget.
		/// Then each 'draw cycle' we render this to the screen.
		/// </summary>
		private RenderTarget2D inMemoryRenderTarget;
		
		/// <summary>
		/// The sprite batch handles all the drawing to the render targets / screen
		/// </summary>
		private SpriteBatch spriteBatch;
		
		/// <summary>
		/// We pass all user interaction and commands to update/draw to our Canvas clas
		/// </summary>
		private ICanvas canvas;
		
		/// <summary>
		/// Keep track of the previous gesture/touch-type that was made by the user.
		/// </summary>
		private TouchType previousTouchType = TouchType.DragComplete;

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
			this.transparentSquareTexture = Content.Load<Texture2D> ("Transparent.png");
			
			this.inMemoryRenderTarget = new RenderTarget2D(
				this.graphicsDeviceManager.GraphicsDevice, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, 
			    this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight);
			
			this.canvas = new Canvas(
				this.BackgroundColor, 
				this.BorderColor, 
				this.spriteBatch, 
				this.transparentSquareTexture, 
				new Microsoft.Xna.Framework.Rectangle(0, 0, 
			                                      this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, 
			                                      this.graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight));
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
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice device = this.graphicsDeviceManager.GraphicsDevice;

			// First set the render target to be our image that we update as we go along...
			device.SetRenderTarget(inMemoryRenderTarget);
			
			// Update the image
			this.canvas.Draw();
			
			// Thes switch back to drawing onto the screen where we copy our image on to the screen
			GraphicsDevice.SetRenderTarget (null);
			GraphicsDevice.Clear (this.BackgroundColor);
			spriteBatch.Begin();
			this.spriteBatch.Draw(this.inMemoryRenderTarget, new Vector2(0, 0), this.BackgroundColor);
			spriteBatch.End ();
			
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
		/// Handles any user input.
		/// Collect all gestures made since the last 'update' - then pass this through to our Canvas to handle
		/// </summary>
		private void HandleInput()
		{
			List<ITouchPoint> touchPoints = new List<ITouchPoint>();
	
			while (TouchPanel.IsGestureAvailable)
            {
                // read the next gesture from the queue
                GestureSample gesture = TouchPanel.ReadGesture();
				
				TouchType touchType = this.ConvertGestureType(gesture.GestureType);
				
				touchPoints.Add(new TouchPoint(gesture.Position, touchType));
				
				previousTouchType = touchType;
			}
			
			this.canvas.HandleTouchPoints(touchPoints);
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

