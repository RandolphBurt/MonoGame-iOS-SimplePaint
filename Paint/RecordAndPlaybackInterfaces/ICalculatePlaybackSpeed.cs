/// <summary>
/// ICalculatePlaybackSpeed.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	/// <summary>
	/// I calculate playback speed interface.
	/// </summary>
	public interface ICalculatePlaybackSpeed
	{
		/// <summary>
		/// Determines how many touch points should be rendered on this update
		/// based on the current speed
		/// </summary>
		/// <returns>The number of touch points to read this time</returns>
		/// <param name='currentSpeed'>The current playback speed (between 0 and 1).</param>
		int TouchPointsToRender(float currentSpeed);
	}
}

