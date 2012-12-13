/// <summary>
/// IToolboxLayoutManager.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Paint.ToolboxLayout;

	public class ToolboxLayoutManager : IToolboxLayoutManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ToolboxLayoutManager"/> class.
		/// </summary>
		public ToolboxLayoutManager()
		{
			this.LoadToolboxLayouts();
		}

		/// <summary>
		/// Gets the portrait layout for the Paint Toolbox.
		/// </summary>
		public ToolboxLayoutDefinition PaintPortraitToolboxLayout { get; private set; }
		
		/// <summary>
		/// Gets the landscape layout for the Paint Toolbox.
		/// </summary>
		public ToolboxLayoutDefinition PaintLandscapeToolboxLayout { get; private set; }
		
		/// <summary>
		/// Gets the portrait layout for the Playback Toolbox.
		/// </summary>
		public ToolboxLayoutDefinition PlaybackPortraitToolboxLayout { get; private set; }
		
		/// <summary>
		/// Gets the landscape layout for the Playback Toolbox.
		/// </summary>
		public ToolboxLayoutDefinition PlaybackLandscapeToolboxLayout { get; private set; }

		/// <summary>
		/// Loads the toolbox layouts - from xml configuraiton files
		/// </summary>
		private void LoadToolboxLayouts()
		{
			this.PaintPortraitToolboxLayout = 
				ObjectDeserializer.DeserialiseFromXmlFile<ToolboxLayoutDefinition>("Content/PaintToolboxPortraitLayout.xml");
			
			this.PaintLandscapeToolboxLayout = 
				ObjectDeserializer.DeserialiseFromXmlFile<ToolboxLayoutDefinition>("Content/PaintToolboxLandscapeLayout.xml");
			
			this.PlaybackPortraitToolboxLayout = 
				ObjectDeserializer.DeserialiseFromXmlFile<ToolboxLayoutDefinition>("Content/PlaybackToolboxPortraitLayout.xml");
			
			this.PlaybackLandscapeToolboxLayout = 
				ObjectDeserializer.DeserialiseFromXmlFile<ToolboxLayoutDefinition>("Content/PlaybackToolboxLandscapeLayout.xml");
		}
	}
}

