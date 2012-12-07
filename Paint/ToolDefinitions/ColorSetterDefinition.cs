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
		/// <param name='colorSetterDefinition'>
		/// Layout of the color setter as defined within a xml file.
		/// </param>
		public ColorSetterDefinition(ToolboxLayoutDefinitionControlsColorSetter colorSetterDefinition)
		{	
			this.Bounds = new Rectangle(
				(int)colorSetterDefinition.Region.Location.X,
				(int)colorSetterDefinition.Region.Location.Y, 
				colorSetterDefinition.Region.Size.Width,
				colorSetterDefinition.Region.Size.Height);

			this.BackgroundColor = new Color(
				colorSetterDefinition.Region.BackgroundColor.Red,
				colorSetterDefinition.Region.BackgroundColor.Green,
				colorSetterDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				colorSetterDefinition.Region.Border.Color.Red,
				colorSetterDefinition.Region.Border.Color.Green,
				colorSetterDefinition.Region.Border.Color.Blue);

			this.BorderWidth = colorSetterDefinition.Region.Border.Width;

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

