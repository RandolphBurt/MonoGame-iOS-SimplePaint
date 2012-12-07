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
		/// <param name='startColor'>
		/// Theinitial colour we are drawing with.
		/// </param>
		/// <param name='colorSelectorDefinition'>
		/// Layout of the color selector as defined within a xml file.
		/// </param>
		public ColorSelectorDefinition(Color startColor, ToolboxLayoutDefinitionControlsColorSelector colorSelectorDefinition)
		{
			this.Bounds = new Rectangle(
				(int)colorSelectorDefinition.Region.Location.X,
				(int)colorSelectorDefinition.Region.Location.Y, 
				colorSelectorDefinition.Region.Size.Width,
				colorSelectorDefinition.Region.Size.Height);

			this.StartColor = startColor;

			this.BackgroundColor = new Color(
				colorSelectorDefinition.Region.BackgroundColor.Red,
				colorSelectorDefinition.Region.BackgroundColor.Green,
				colorSelectorDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				colorSelectorDefinition.Region.Border.Color.Red,
				colorSelectorDefinition.Region.Border.Color.Green,
				colorSelectorDefinition.Region.Border.Color.Blue);

			this.GaugeWidth = colorSelectorDefinition.Gauge.Width;
			this.GaugeMarkerWidth = colorSelectorDefinition.Gauge.MarkerWidth;
			this.GaugeHorizontalMargin = colorSelectorDefinition.Gauge.HorizontalMargin;
			this.GaugeVerticalMargin = colorSelectorDefinition.Gauge.VerticalMargin;
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

