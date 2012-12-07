/// <summary>
/// ButtonDefinition.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;
	
	using Paint.ToolboxLayout;
	
	/// <summary>
	/// Color picker definition - layout of the color picker
	/// </summary>
	public class ButtonDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ToolboxLayoutDefinitionControlsButton"/> class.
		/// </summary>
		/// <param name='buttonDefinition'>
		/// Layout of a button as defined within a xml file.
		/// </param>
		public ButtonDefinition(ToolboxLayoutDefinitionControlsButton buttonDefinition, ImageType[] imageList, ImageType? disabledImageType)
		{	
			this.Bounds = new Rectangle(
				(int)buttonDefinition.Region.Location.X,
				(int)buttonDefinition.Region.Location.Y, 
				buttonDefinition.Region.Size.Width,
				buttonDefinition.Region.Size.Height);
			
			this.BackgroundColor = new Color(
				buttonDefinition.Region.BackgroundColor.Red,
				buttonDefinition.Region.BackgroundColor.Green,
				buttonDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				buttonDefinition.Region.Border.Color.Red,
				buttonDefinition.Region.Border.Color.Green,
				buttonDefinition.Region.Border.Color.Blue);
			
			this.BorderWidth = buttonDefinition.Region.Border.Width;

			this.ImageList = imageList;
			this.DisabledImageType = disabledImageType;
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

		public ImageType[] ImageList
		{
			get;
			private set;
		}

		public ImageType? DisabledImageType
		{
			get;
		 	private set;
		}
	}
}

