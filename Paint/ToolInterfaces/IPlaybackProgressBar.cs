/// <summary>
/// IPlaybackProgressBar.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	/// <summary>
	/// Interface for the ProgressBar toolbox item
	/// </summary>
	public interface IPlaybackProgressBar
	{
		/// <summary>
		/// Gets or sets the percentage through the progress bar
		/// </summary>
		float Percentage
		{
			get;
			set;
		}
	}
}

