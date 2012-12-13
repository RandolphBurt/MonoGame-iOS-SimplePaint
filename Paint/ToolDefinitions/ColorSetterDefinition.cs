/// <summary>
/// ColorSetterDefinition.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;

	using Paint.ToolboxLayout;

	/// <summary>
	/// Color setter definition - layout of the color setter
	/// </summary>
	public class ColorSetterDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorSetterDefinition"/> class.
		/// </summary>
		/// <param name='colorSetterDefinition'>Layout of the color setter as defined within a xml file.</param>
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public ColorSetterDefinition(ToolboxLayoutDefinitionPaintToolsColorSetter colorSetterDefinition, int scale)
		{	
			this.Bounds = new Rectangle(
				(int)colorSetterDefinition.Region.Location.X * scale,
				(int)colorSetterDefinition.Region.Location.Y * scale, 
				colorSetterDefinition.Region.Size.Width * scale,
				colorSetterDefinition.Region.Size.Height * scale);

			this.BackgroundColor = new Color(
				colorSetterDefinition.Region.BackgroundColor.Red,
				colorSetterDefinition.Region.BackgroundColor.Green,
				colorSetterDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				colorSetterDefinition.Region.Border.Color.Red,
				colorSetterDefinition.Region.Border.Color.Green,
				colorSetterDefinition.Region.Border.Color.Blue);

			this.BorderWidth = colorSetterDefinition.Region.Border.Width * scale;

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
	}
}

