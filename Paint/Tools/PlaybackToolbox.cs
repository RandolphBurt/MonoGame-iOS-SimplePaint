/// <summary>
/// PlaybackToolbox.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using System;

	using Paint.ToolboxLayout;

	/// <summary>
	/// Playback toolbox.
	/// </summary>
	public class PlaybackToolbox : ToolBox, IPlaybackToolBox
	{
		/// <summary>
		/// The play button.
		/// </summary>
		private Button playButton = null;

		/// <summary>
		/// The playback progress bar.
		/// </summary>
		private PlaybackProgressBar playbackProgressBar;

		/// <summary>
		/// The speed gauge.
		/// </summary>
		private SpeedGauge speedGauge;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PlaybackToolbox"/> class.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		/// <param name='graphicsDisplay' The graphics texture map - contains images for buttons and controls />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public PlaybackToolbox(ToolboxLayoutDefinition toolboxLayoutDefinition, IGraphicsDisplay graphicsDisplay, int scale)
			: base (toolboxLayoutDefinition, graphicsDisplay, scale)
		{
			this.CreateTools(toolboxLayoutDefinition);
		}

		/// <summary>
		/// Sets the playback progress percentage.
		/// </summary>
		public float PlaybackProgressPercentage
		{ 
			set
			{
				this.playbackProgressBar.Percentage = value;
			}
		}

		/// <summary>
		/// Gets the playback speed.
		/// </summary>
		public float PlaybackSpeed
		{ 
			get
			{
				return this.speedGauge.Speed;
			}
		}

		/// <summary>
		/// Occurs when the user has pressed the restart button.
		/// </summary>
		public event EventHandler RestartSelected;
		
		/// <summary>
		/// Occurs when the user has pressed the play button.
		/// </summary>
		public event EventHandler PlaySelected;
		
		/// <summary>
		/// Occurs when the user has pressed the pause button.
		/// </summary>
		public event EventHandler PauseSelected;

		/// <summary>
		/// Disables the playback button
		/// </summary>
		public void SetPlayButtonDisabled()
		{ 
			this.playButton.Enabled = false;
		}
				
		/// <summary>
		/// Sets the playback button to signify we are currently paused
		/// </summary>
		public void SetPlayButtonPaused()
		{ 
			this.playButton.State = 0;
		}

		/// <summary>
		/// Raises the restart selected event.
		/// </summary>
		/// <param name='e'>EventArgs for Restart Event</param>
		protected virtual void OnRestartSelected(EventArgs e)
		{
			if (this.RestartSelected != null)
			{
				this.RestartSelected(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the play selected event.
		/// </summary>
		/// <param name='e'>EventArgs for Play Event</param>
		protected virtual void OnPlaySelected(EventArgs e)
		{
			if (this.PlaySelected != null)
			{
				this.PlaySelected(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the pause selected event.
		/// </summary>
		/// <param name='e'>EventArgs for Pause Event</param>
		protected virtual void OnPauseSelected(EventArgs e)
		{
			if (this.PauseSelected != null)
			{
				this.PauseSelected(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Creates all the buttons and adds them to our list of controls
		/// </summary>
		/// <param name='buttons' All the buttons we need to display on screen />
		protected override void AddButton(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonLayout)
		{
			switch (buttonLayout.ButtonType)
			{
				case ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType.PlayPausePlayback:
					this.AddPlayPauseButton(buttonLayout);
					break;

				case ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType.Restart:
					this.AddRestartButton(buttonLayout);
					break;

				default:
					base.AddButton(buttonLayout);
					break;
			}
		}

		/// <summary>
		/// Adds the restart button.
		/// </summary>
		/// <param name='buttonLayout'>
		/// Button layout.
		/// </param>
		private void AddRestartButton(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonLayout)
		{
			var restartButton = new Button(
				this.GraphicsDisplay, 
				new ButtonDefinition(buttonLayout, this.Scale, new ImageType[] { ImageType.RestartButton }, null));
			
			restartButton.ButtonPressed += (sender, e) => 
			{
				// we are restarting so we if we were previosly paused or disabled then reset to playing...
				this.playButton.State = 1;
				this.playButton.Enabled = true;

				this.OnRestartSelected(EventArgs.Empty);
			};		
			
			this.AddTool(restartButton);
		}

		/// <summary>
		/// Adds the play pause button.
		/// </summary>
		/// <param name='buttonLayout'>
		/// Button layout.
		/// </param>
		private void AddPlayPauseButton(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonLayout)
		{
			this.playButton = new Button(
				this.GraphicsDisplay, 
				new ButtonDefinition(buttonLayout, this.Scale, new ImageType[] { ImageType.PlayButton, ImageType.PauseButton }, ImageType.PlayButtonDisabled));
			
			this.playButton.ButtonPressed += (sender, e) => 
			{
				if (this.playButton.State == 0)
				{
					this.OnPauseSelected(EventArgs.Empty);
				}
				else
				{
					this.OnPlaySelected(EventArgs.Empty);
				}
			};		
			
			this.AddTool(this.playButton);
		}
		
		/// <summary>
		/// Creates all our tools.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		private void CreateTools(ToolboxLayoutDefinition toolboxLayoutDefinition)
		{
			this.playbackProgressBar = this.CreateProgressBar(toolboxLayoutDefinition.PlaybackTools.ProgressBar);
			this.AddTool(this.playbackProgressBar);

			this.speedGauge = this.CreateSpeedGauge(toolboxLayoutDefinition.PlaybackTools.SpeedGauge);
			this.AddTool(this.speedGauge);
		}

		/// <summary>
		/// Creates the progress bar.
		/// </summary>
		/// <returns>The progress bar.</returns>
		/// <param name='progressBar'>Progress bar layout definition</param>
		private PlaybackProgressBar CreateProgressBar(ToolboxLayoutDefinitionPlaybackToolsProgressBar progressBar)
		{
			return new PlaybackProgressBar(this.GraphicsDisplay, new PlaybackProgressBarDefinition(progressBar, this.Scale));
		}

		/// <summary>
		/// Creates the speed gauge.
		/// </summary>
		/// <returns> The speed gauge.</returns>
		/// <param name='speedGauge'>Speed gauge layout definition</param>
		private SpeedGauge CreateSpeedGauge(ToolboxLayoutDefinitionPlaybackToolsSpeedGauge speedGauge)
		{
			return new SpeedGauge(this.GraphicsDisplay, new SpeedGaugeDefinition(speedGauge, this.Scale));
		}
	}
}

