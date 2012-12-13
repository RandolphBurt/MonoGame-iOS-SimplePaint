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
		/// <param name='buttonDefinition'>Layout of a button as defined within a xml file.</param>
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		/// <param name='imageList' List of images to use for this button (will rotate through them all as the user presses the button) />
		/// <param name='disabledImageType' Image to use if this button is disabled />
		public ButtonDefinition(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonDefinition, int scale, ImageType[] imageList, ImageType? disabledImageType)
		{	
			this.Bounds = new Rectangle(
				(int)buttonDefinition.Region.Location.X * scale,
				(int)buttonDefinition.Region.Location.Y * scale, 
				buttonDefinition.Region.Size.Width * scale,
				buttonDefinition.Region.Size.Height * scale);
			
			this.BackgroundColor = new Color(
				buttonDefinition.Region.BackgroundColor.Red,
				buttonDefinition.Region.BackgroundColor.Green,
				buttonDefinition.Region.BackgroundColor.Blue);
			
			this.BorderColor = new Color(
				buttonDefinition.Region.Border.Color.Red,
				buttonDefinition.Region.Border.Color.Green,
				buttonDefinition.Region.Border.Color.Blue);
			
			this.BorderWidth = buttonDefinition.Region.Border.Width * scale;

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

