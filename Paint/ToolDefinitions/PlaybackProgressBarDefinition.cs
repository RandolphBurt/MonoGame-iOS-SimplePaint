/// <summary>
/// PlaybackProgressBarDefinition.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;

	using Paint.ToolboxLayout;

	/// <summary>
	/// Playback progress bar definition - layout of the progress bar
	/// </summary>
	public class PlaybackProgressBarDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PlaybackProgressBarDefinition"/> class.
		/// </summary>
		/// <param name='progressBarDefinition'>Layout of the progress bar as defined within a xml file.</param>
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public PlaybackProgressBarDefinition(ToolboxLayoutDefinitionPlaybackToolsProgressBar progressBarDefinition, int scale)
		{
			this.Bounds = new Rectangle(
				(int)progressBarDefinition.Region.Location.X * scale,
				(int)progressBarDefinition.Region.Location.Y * scale, 
				progressBarDefinition.Region.Size.Width * scale,
				progressBarDefinition.Region.Size.Height * scale);
			
			this.BackgroundColor = new Color(
				progressBarDefinition.Region.BackgroundColor.Red,
				progressBarDefinition.Region.BackgroundColor.Green,
				progressBarDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				progressBarDefinition.Region.Border.Color.Red,
				progressBarDefinition.Region.Border.Color.Green,
				progressBarDefinition.Region.Border.Color.Blue);
			
			this.BorderWidth = progressBarDefinition.Region.Border.Width * scale;

			this.ProgresIndicatorWidth = progressBarDefinition.IndicatorBar.Width * scale;
			this.ProgressIndicatorHeight = progressBarDefinition.IndicatorBar.Height * scale;

			this.ProgressIndicatorColor = new Color(
				progressBarDefinition.IndicatorBar.Color.Red,
				progressBarDefinition.IndicatorBar.Color.Green,
				progressBarDefinition.IndicatorBar.Color.Blue);
		}

		public Rectangle Bounds
		{
			get;
			private set;
		}
		
		public Color BorderColor
		{
			get;
			private set;
		}
		
		public Color BackgroundColor
		{
			get;
			private set;
		}
		
		public int BorderWidth
		{
			get;
			private set;
		}

		public int ProgressIndicatorHeight
		{
			get;
			private set;
		}
		
		public int ProgresIndicatorWidth
		{
			get;
			private set;
		}

		public Color ProgressIndicatorColor 
		{
			get;
			private set;
		}
	}
}

