/// <summary>
/// ColorPickerDefinition.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	using Paint.ToolboxLayout;
	
	/// <summary>
	/// Color picker definition - layout of the color picker
	/// </summary>
	public class ColorPickerDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorPickerDefinition"/> class.
		/// </summary>
		/// <param name='colorPickerDefinition'> Layout of the color setter as defined within a xml file. </param>
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public ColorPickerDefinition(ToolboxLayoutDefinitionPaintToolsColorPickersColorPicker colorPickerDefinition, int scale)
		{	
			this.Bounds = new Rectangle(
				(int)colorPickerDefinition.Region.Location.X * scale,
				(int)colorPickerDefinition.Region.Location.Y * scale, 
				colorPickerDefinition.Region.Size.Width * scale,
				colorPickerDefinition.Region.Size.Height * scale);
			
			this.BackgroundColor = new Color(
				colorPickerDefinition.Region.BackgroundColor.Red,
				colorPickerDefinition.Region.BackgroundColor.Green,
				colorPickerDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				colorPickerDefinition.Region.Border.Color.Red,
				colorPickerDefinition.Region.Border.Color.Green,
				colorPickerDefinition.Region.Border.Color.Blue);
			
			this.BorderWidth = colorPickerDefinition.Region.Border.Width * scale;
			
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

