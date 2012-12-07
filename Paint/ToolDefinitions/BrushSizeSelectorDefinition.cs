/// <summary>
/// BrushSizeSelectorDefinition.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;

	using Paint.ToolboxLayout;

	/// <summary>
	/// Brush size selector definition - defines the layout for a BrushSize selector toolbox control
	/// </summary>
	public class BrushSizeSelectorDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.BrushSizeSelectorDefinition"/> class.
		/// </summary>
		/// <param name='startColor'>
		/// Initial color to use for the brush
		/// </param>
		/// <param name='layoutBrushSizeSelector'>
		/// Layout of the brush size as defined within a xml file.
		/// </param>
		public BrushSizeSelectorDefinition(Color startColor, ToolboxLayoutDefinitionControlsBrushSizeSelector layoutBrushSizeSelector)
		{			
			this.Bounds = new Rectangle(
				(int)layoutBrushSizeSelector.Region.Location.X,
				(int)layoutBrushSizeSelector.Region.Location.Y, 
				layoutBrushSizeSelector.Region.Size.Width,
				layoutBrushSizeSelector.Region.Size.Height);

			this.StartColor = startColor;

			this.BackgroundColor = new Color(
				layoutBrushSizeSelector.Region.BackgroundColor.Red,
				layoutBrushSizeSelector.Region.BackgroundColor.Green,
				layoutBrushSizeSelector.Region.BackgroundColor.Blue);

			this.BorderColor = new Color(
				layoutBrushSizeSelector.Region.Border.Color.Red,
				layoutBrushSizeSelector.Region.Border.Color.Green,
				layoutBrushSizeSelector.Region.Border.Color.Blue);

			this.BorderWidth = layoutBrushSizeSelector.Region.Border.Width;

			this.BrushSizeInitial = layoutBrushSizeSelector.BrushSize.Initial;
			this.BrushSizeMinimum = layoutBrushSizeSelector.BrushSize.Minimum;
			this.BrushSizeMaximum = layoutBrushSizeSelector.BrushSize.Maximum;

			this.GaugeWidth = layoutBrushSizeSelector.Gauge.Width;
			this.GaugeMarkerWidth = layoutBrushSizeSelector.Gauge.MarkerWidth;
			this.GaugeVerticalMargin = layoutBrushSizeSelector.Gauge.VerticalMargin;
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

		public int BrushSizeInitial
		{
			get;
			private set;
		}

		public int BrushSizeMinimum
		{
			get;
			private set;
		}
	
		public int BrushSizeMaximum
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
	}
}

