/// <summary>
/// SpeedGaugeDefinition.cs
/// Randolph Burt - January 2013
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	using Paint.ToolboxLayout;

	/// <summary>
	/// Speed gauge definition - layout of the speed gauge
	/// </summary>
	public class SpeedGaugeDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.SpeedGaugeDefinition"/> class.
		/// </summary>
		/// <param name='speedGaugeDefinition'>Layout of the speed gauge as defined within a xml file.</param>
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public SpeedGaugeDefinition(ToolboxLayoutDefinitionPlaybackToolsSpeedGauge speedGaugeDefinition, int scale)
		{
			this.Bounds = new Rectangle(
				(int)speedGaugeDefinition.Region.Location.X * scale,
				(int)speedGaugeDefinition.Region.Location.Y * scale, 
				speedGaugeDefinition.Region.Size.Width * scale,
				speedGaugeDefinition.Region.Size.Height * scale);
			
			this.BackgroundColor = new Color(
				speedGaugeDefinition.Region.BackgroundColor.Red,
				speedGaugeDefinition.Region.BackgroundColor.Green,
				speedGaugeDefinition.Region.BackgroundColor.Blue);

			this.BorderColor = new Color(
				speedGaugeDefinition.Region.Border.Color.Red,
				speedGaugeDefinition.Region.Border.Color.Green,
				speedGaugeDefinition.Region.Border.Color.Blue);
			
			this.BorderWidth = speedGaugeDefinition.Region.Border.Width * scale;

			this.GaugeWidth = speedGaugeDefinition.Gauge.Width * scale;
			this.GaugeMarkerWidth = speedGaugeDefinition.Gauge.MarkerWidth * scale;
			this.GaugeHorizontalMargin = speedGaugeDefinition.Gauge.HorizontalMargin * scale;
			this.GaugeVerticalMargin = speedGaugeDefinition.Gauge.VerticalMargin * scale;
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

		public int GaugeWidth
		{
			get;
			private set;
		}
		
		public int GaugeMarkerWidth
		{
			get;
			private set;
		}
		
		public int GaugeVerticalMargin
		{
			get;
			private set;
		}
		
		public int GaugeHorizontalMargin
		{
			get;
			private set;
		}
	}
}

