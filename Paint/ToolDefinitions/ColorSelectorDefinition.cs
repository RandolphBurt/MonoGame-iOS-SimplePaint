/// <summary>
/// ColorSelectorDefinition.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	using Paint.ToolboxLayout;

	/// <summary>
	/// Color selector definition -layout of the color selector
	/// </summary>
	public class ColorSelectorDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSelectorDefinition"/> class.
		/// </summary>
		/// <param name='startColor'> The initial colour we are drawing with. </param>
		/// <param name='colorSelectorDefinition'> Layout of the color selector as defined within a xml file. </param>
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public ColorSelectorDefinition(Color startColor, ToolboxLayoutDefinitionPaintToolsColorSelector colorSelectorDefinition, int scale)
		{
			this.Bounds = new Rectangle(
				(int)colorSelectorDefinition.Region.Location.X * scale,
				(int)colorSelectorDefinition.Region.Location.Y * scale, 
				colorSelectorDefinition.Region.Size.Width * scale,
				colorSelectorDefinition.Region.Size.Height * scale);

			this.StartColor = startColor;

			this.BackgroundColor = new Color(
				colorSelectorDefinition.Region.BackgroundColor.Red,
				colorSelectorDefinition.Region.BackgroundColor.Green,
				colorSelectorDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				colorSelectorDefinition.Region.Border.Color.Red,
				colorSelectorDefinition.Region.Border.Color.Green,
				colorSelectorDefinition.Region.Border.Color.Blue);

			this.GaugeWidth = colorSelectorDefinition.Gauge.Width * scale;
			this.GaugeMarkerWidth = colorSelectorDefinition.Gauge.MarkerWidth * scale;
			this.GaugeHorizontalMargin = colorSelectorDefinition.Gauge.HorizontalMargin * scale;
			this.GaugeVerticalMargin = colorSelectorDefinition.Gauge.VerticalMargin * scale;
		}

		public Rectangle Bounds
		{
			get;
			private set;
		}

		public Color StartColor
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

