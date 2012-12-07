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
		/// <param name='colorPickerDefinition'>
		/// Layout of the color setter as defined within a xml file.
		/// </param>
		public ColorPickerDefinition(ToolboxLayoutDefinitionControlsColorPicker colorPickerDefinition)
		{	
			this.Bounds = new Rectangle(
				(int)colorPickerDefinition.Region.Location.X,
				(int)colorPickerDefinition.Region.Location.Y, 
				colorPickerDefinition.Region.Size.Width,
				colorPickerDefinition.Region.Size.Height);
			
			this.BackgroundColor = new Color(
				colorPickerDefinition.Region.BackgroundColor.Red,
				colorPickerDefinition.Region.BackgroundColor.Green,
				colorPickerDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				colorPickerDefinition.Region.Border.Color.Red,
				colorPickerDefinition.Region.Border.Color.Green,
				colorPickerDefinition.Region.Border.Color.Blue);
			
			this.BorderWidth = colorPickerDefinition.Region.Border.Width;
			
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

