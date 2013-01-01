/// <summary>
/// IPlaybackToolBox.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using System;

	/// <summary>
	/// Interface for the playback toolbox.
	/// </summary>
	public interface IPlaybackToolBox: IToolBox
	{
		/// <summary>
		/// Occurs when the user has pressed the restart button.
		/// </summary>
		event EventHandler RestartSelected;

		/// <summary>
		/// Occurs when the user has pressed the play button.
		/// </summary>
		event EventHandler PlaySelected;

		/// <summary>
		/// Occurs when the user has pressed the pause button.
		/// </summary>
		event EventHandler PauseSelected;

		/// <summary>
		/// Sets the playback progress percentage.
		/// </summary>
		float PlaybackProgressPercentage{ set; }

		/// <summary>
		/// Disables the playback button
		/// </summary>
		void SetPlayButtonDisabled();

		/// <summary>
		/// Sets the playback button to signify we are currently paused
		/// </summary>
		void SetPlayButtonPaused();
	}
}

