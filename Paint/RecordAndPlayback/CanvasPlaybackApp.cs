/// <summary>
/// CanvasPlaybackApp.cs
/// Randolph Burt - April 2012
/// </summary>
using Microsoft.Xna.Framework.Input.Touch;


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
		/// The playback mode.
		/// </summary>
		private PlaybackMode playbackMode = PlaybackMode.NotStarted;

		/// <summary>
		/// The tool box - contains all our buttons (e.g. play/pause/restart)
		/// More specialised version of the parent class IToolBox, but the same instance
		/// </summary>
		private IPlaybackToolBox playbackToolbox;

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
		/// Forces the playback to pause.
		/// </summary>
		public void ForcePause()
		{
			if (this.playbackMode == PlaybackMode.Playing)
			{
				this.playbackMode = PlaybackMode.Paused;
				this.playbackToolbox.SetPlayButtonPaused();
			}
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
			this.playbackToolbox = new PlaybackToolbox(this.ToolboxLayoutDefinition, this.GraphicsDisplay, scale);

			this.playbackToolbox.ExitSelected += (sender, e) => 
			{
				if (this.playbackMode != PlaybackMode.Exiting)
				{
					this.playbackMode = PlaybackMode.Exiting;
					this.Exit();
				}
			};

			this.playbackToolbox.PauseSelected += (sender, e) => 
			{
				if (this.playbackMode != PlaybackMode.Exiting && this.playbackMode != PlaybackMode.Finished)
				{
					this.playbackMode = PlaybackMode.Paused;
				}
			};

			this.playbackToolbox.PlaySelected += (sender, e) => 
			{
				if (this.playbackMode != PlaybackMode.Exiting && this.playbackMode != PlaybackMode.Finished)
				{
					this.playbackMode = PlaybackMode.Playing;
				}
			};

			this.playbackToolbox.RestartSelected += (sender, e) => 
			{
				if (this.playbackMode != PlaybackMode.Exiting)
				{
					this.canvasPlayback.Restart();
					this.BlankRenderTarget(this.InMemoryCanvasRenderTarget);
					this.playbackMode = PlaybackMode.Playing;
				}
			};

			return this.playbackToolbox;
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
			if (this.playbackMode == PlaybackMode.Exiting)
			{
				return;
			}

			this.HandleInput();

			if (this.canvasPlayback.DataAvailable == false)
			{
				this.playbackMode = PlaybackMode.Finished;
				this.playbackToolbox.SetPlayButtonDisabled();
			}
			else if (this.playbackMode == PlaybackMode.Playing)
			{			
				var touchPoint = this.canvasPlayback.GetNextTouchPoint();
			
				if (touchPoint != null)
				{
					this.CanvasTouchPoints.Add(touchPoint);
				}

				this.playbackToolbox.PlaybackProgressPercentage =  this.canvasPlayback.PercentageRead;
			}
			                           
			base.Update(gameTime);
		}		

		/// <summary>
		/// Handles any user input.
		/// Collect all gestures made since the last 'update' - check if these need to be handled
		/// </summary>
		private void HandleInput()
		{	
			while (TouchPanel.IsGestureAvailable)
			{
				// read the next gesture from the queue
				GestureSample gesture = TouchPanel.ReadGesture();
				
				TouchType touchType = this.ConvertGestureType(gesture.GestureType);
				
				TouchPoint touchPoint = new TouchPoint(gesture.Position, touchType);
				
				this.CheckToolboxCollision(touchPoint);
			}
		}

		/// <summary>
		/// What mode is the app currently in
		/// </summary>
		private enum PlaybackMode
		{
			/// <summary>
			/// The user has not yet started the playback process
			/// </summary>
			NotStarted,

			/// <summary>
			/// The playback process has been paused
			/// </summary>
			Paused,

			/// <summary>
			/// The playback process is happening now
			/// </summary>
			Playing,

			/// <summary>
			/// The playback process has finished
			/// </summary>
			Finished,

			/// <summary>
			/// The user has selected to exit
			/// </summary>
			Exiting
		}
	}
}

