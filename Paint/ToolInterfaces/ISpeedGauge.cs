/// <summary>
/// ISpeedGauge.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using System;

	/// <summary>
	/// Interface for the Speed Gauge toolbox item
	/// </summary>
	public interface ISpeedGauge
	{
		/// <summary>
		/// Gets or sets the position of the marker (0.0 -> 1.0)
		/// </summary>
		float Speed
		{ 
			get;
		}
		
		/// <summary>
		/// Occurs when the speed indicator changes
		/// </summary>
		event EventHandler SpeedChanged;
	}
}

