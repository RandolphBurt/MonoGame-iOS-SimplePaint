/// <summary>
/// CalculatePlaybackSpeed.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	/// <summary>
	/// Calculate playback speed.
	/// </summary>
	public class CalculatePlaybackSpeed : ICalculatePlaybackSpeed
	{
		/// <summary>
		/// The number of calls to the 'TouchPointsToRender' method since we last returned that touch points could be rendered.
		/// </summary>
		private int noRenderCount = 0;

		/// <summary>
		/// Determines how many touch points should be rendered on this update
		/// based on the current speed
		/// </summary>
		/// <returns>The number of touch points to read this time</returns>
		/// <param name='currentSpeed'>The current playback speed (between 0 and 1).</param>
		public int TouchPointsToRender(float currentSpeed)
		{
			/*
			 * currentSpeed will be between 0 and 1
			 * 
			 * Convert to an int...
			 * ...speed will be between 0 and 10...
			 * 
			 * 0 = Read one every 6 updates
			 * 1 = Read one every 5 updates
			 * 2 = Read one every 4 updates
			 * 3 = Read one every 3 updates
			 * 4 = Read one every other update
			 * 
			 * 5  = Normal speed = read one touch point per update
			 * 
			 * 6  = 2 * normal speed
			 * 7  = 3 * normal speed
			 * 8  = 4 * normal speed
			 * 9  = 5 * normal speed
			 * 10 = 6 * normal speed
			 * 
			 */
			
			int touchPointsToRender = 0;
			
			var speed = (int)(currentSpeed * 10);
			
			if (speed >= 5)
			{
				touchPointsToRender = speed - 4;
			}
			else
			{
				var updateGap = 5 - speed;
				
				if (updateGap <= this.noRenderCount)
				{
					touchPointsToRender = 1;
				}
			}
			
			if (touchPointsToRender == 0)
			{
				this.noRenderCount++;
			}
			else
			{
				this.noRenderCount = 0;
			}
			
			return touchPointsToRender;
		}
	}
}

