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
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;

	using Paint.ToolboxLayout;

	/// <summary>
	/// The main application class just inherits from the monogame:Game class
	/// Override a few key methods and away we go...
	/// </summary>
	public class PaintApp : BaseGame, IRenderTargertHandler
	{
		/// <summary>
		/// The render targets used for tracking undo/redo - The image in the previous (wrapping) entry in the array 
		/// should be shown when we undo a change
		/// </summary>
		private RenderTarget2D[] undoRedoRenderTargets = null;
		
		/// <summary>
		/// The picture state manager - handles playback and save/undo/redo
		/// </summary>
		private IPictureStateManager pictureStateManager;
		
		/// <summary>
		/// The tool box - contains all our color pickers and brush size controls
		/// More specialised version of the parent class IToolBox, but the same instance
		/// </summary>
		private IPaintToolBox paintToolBox;
		
		/// <summary>
		/// The picture IO manager - handles all reading / writing images and information file
		/// </summary>
		private IPictureIOManager pictureIOManager;

		/// <summary>
		/// The filename resolver.
		/// </summary>
		private IFilenameResolver filenameResolver;
		
		/// <summary>
		/// What mode is the app currently running in paint, pending shutdown or actually exiting
		/// </summary>
		private PaintMode paintMode = PaintMode.Paint;
		
		/// <summary>
		/// view/form to present when we about to save all the data
		/// </summary>
		private IUIBusyMessage saveBusyMessageDisplay = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PaintApp"/> class.
		/// Instantiate our GraphicsDeviceManager and establish where the content folder is
		/// </summary>
		/// <param name='pictureIOManager'>Picture IO Manager</param>
		/// <param name='filenameResolver'>Filename Resolver</param>
		/// <param name='imageStateData'>ImageSaveData</param>
		/// <param name='saveBusyMessageDisplay'>Class for dsplaying the 'Busy - saving' message</param>
		/// <param name='toolboxLayoutDefinition'>Layout of the toolbox</param>
		/// <param name='deviceScale'>The device scale/resolution. 1 = normal.  2 = retina.</param>
		public PaintApp(
			IPictureIOManager pictureIOManager, 
			IFilenameResolver filenameResolver, 
			ImageStateData imageStateData,
			IUIBusyMessage saveBusyMessageDisplay,
			ToolboxLayoutDefinition toolboxLayoutDefinition,
			int deviceScale)
			: base(imageStateData, toolboxLayoutDefinition, deviceScale)
		{
			this.filenameResolver = filenameResolver;
			this.pictureIOManager = pictureIOManager;
			this.saveBusyMessageDisplay = saveBusyMessageDisplay;
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
			this.RenderImage(this.InMemoryCanvasRenderTarget, this.undoRedoRenderTargets[savePoint]);
		}
		
		/// <summary>
		/// Save the canvas/display to the specified save-point (undo/redo render target).
		/// </summary>
		/// <param name='savePoint'>specific save point we wish to use to store the canvas</param>
		public void StoreSavePoint(int savePoint)
		{
			this.RenderImage(this.undoRedoRenderTargets[savePoint], this.InMemoryCanvasRenderTarget);
		}		

		/// <summary>
		/// We load any content we need at the beginning of the application life cycle.
		/// Also anything that needs initialising is done here
		/// </summary>
		protected override void LoadContent()
		{
			base.LoadContent();

			this.CreatePictureStateManager();
		}
		
		/// <summary>
		/// Called everytime we need to redraw the screen
		/// </summary>
		/// <param name='gameTime' >
		/// Allows you to monitor time passed since last draw
		/// </param>
		protected override void Draw(GameTime gameTime)
		{
			this.pictureStateManager.Draw(this.CanvasTouchPoints);

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
		/// Creates the render targets (including all the undo buffers).
		/// </summary>
		protected override void CreateRenderTargets()
		{
			var device = this.GraphicsDeviceManager.GraphicsDevice;
			var width = this.ImageStateData.Width;
			var height = this.ImageStateData.Height;
			
			List<RenderTarget2D> renderTargetList = new List<RenderTarget2D>();
			
			for (var count = 0; count < this.ImageStateData.MaxUndoRedoCount; count++)
			{
				renderTargetList.Add(new RenderTarget2D(device, width, height));
			}
						
			this.undoRedoRenderTargets = renderTargetList.ToArray();

			base.CreateRenderTargets();
		}

		/// <summary>
		/// Creates the toolbox.
		/// </summary>
		/// <returns>The toolbox.</returns>
		protected override IToolBox CreateToolbox()
		{
			this.paintToolBox = new PaintToolBox(this.ToolboxLayoutDefinition, this.GraphicsDisplay, this.DeviceScale);

			this.paintToolBox.ExitSelected += (sender, e) => 
			{
				this.InitiateShutdown();
			};
			
			this.paintToolBox.RedoSelected += (sender, e) => 
			{
				this.pictureStateManager.Redo();
			};
			
			this.paintToolBox.UndoSelected += (sender, e) => 
			{
				this.pictureStateManager.Undo();
			};

			return this.paintToolBox;
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
				
				TouchPointSizeColour touchPoint = new TouchPointSizeColour(
					gesture.Position, 
					touchType, 
					this.paintToolBox.Color, 
					this.paintToolBox.Brush);
				
				// First check if this can be handled by the toolbox - if not then we will keep for the canvas
				if (this.CheckToolboxCollision(touchPoint) == false)
				{
					this.CanvasTouchPoints.Add(this.ConvertScreenTouchToCanvasTouch(touchPoint));
				}				
			}
		}

		/// <summary>
		/// Renders a specific source RenderTarget on to a specific target RenderTarget
		/// </summary>
		/// <param name='target'>Where we want to render the image</param>
		/// <param name='source'>The source image we wish to render</param>
		private void RenderImage(RenderTarget2D target, RenderTarget2D source)
		{
			GraphicsDevice device = this.GraphicsDeviceManager.GraphicsDevice;	
			device.SetRenderTarget(target);
			this.SpriteBatch.Begin();
			device.Clear(this.BackgroundColor);
			this.SpriteBatch.Draw(source, Vector2.Zero, this.BackgroundColor);
			this.SpriteBatch.End();	                      
		}		

		/// <summary>
		/// Creates the picture state manager.
		/// </summary>
		private void CreatePictureStateManager()
		{
			bool newImage = true;

			if (File.Exists(this.filenameResolver.MasterImageInfoFilename) == true)
			{
				// existing image so we load the rendertargetlist from disk
				this.pictureIOManager.LoadData(this.GraphicsDeviceManager.GraphicsDevice, this.SpriteBatch, this.undoRedoRenderTargets, this.BackgroundColor);
				newImage = false;
			}
			
			this.pictureStateManager = new PictureStateManager(this.filenameResolver, this.pictureIOManager, this, this.ImageStateData);			
			this.pictureStateManager.RedoEnabledChanged += (sender, e) => 
			{
				this.paintToolBox.RedoEnabled = this.pictureStateManager.RedoEnabled;
			};
			
			this.pictureStateManager.UndoEnabledChanged += (sender, e) => 
			{
				this.paintToolBox.UndoEnabled = this.pictureStateManager.UndoEnabled;
			};
			
			this.pictureStateManager.InitialisePictureState(newImage);			
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
		/// Saves all the data and then exits.
		/// </summary>
		private void SaveAndExit()
		{
			this.pictureStateManager.Save();
			this.pictureIOManager.SaveData(this.pictureStateManager.ImageStateData, this.InMemoryCanvasRenderTarget, this.undoRedoRenderTargets, this.ToolBox.ToolboxMinimizedHeight);
			
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

