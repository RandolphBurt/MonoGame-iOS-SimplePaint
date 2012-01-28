/// <summary>
/// IGauge.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	/// <summary>
	/// Inteface for a Guage control - where the user can drag a marker along a range
	/// </summary>
	public interface IGauge
	{
		/// <summary>
		/// Gets or sets the position of the marker (0.0 -> 1.0)
		/// </summary>
		float Marker 
		{ 
			get;
			set;
		}
		
		/// <summary>
		/// Occurs when the location of the marker changes.
		/// </summary>
		event EventHandler MarkerChanged;
	}
}

