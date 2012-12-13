/// <summary>
/// IToolboxLayoutManager.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using Paint.ToolboxLayout;

	/// <summary>
	/// Interface for the ToolboxLayout Manager
	/// </summary>
	public interface IToolboxLayoutManager
	{
		/// <summary>
		/// Gets the portrait layout for the Paint Toolbox.
		/// </summary>
		ToolboxLayoutDefinition PaintPortraitToolboxLayout { get; }

		/// <summary>
		/// Gets the landscape layout for the Paint Toolbox.
		/// </summary>
		ToolboxLayoutDefinition PaintLandscapeToolboxLayout { get; }

		/// <summary>
		/// Gets the portrait layout for the Playback Toolbox.
		/// </summary>
		ToolboxLayoutDefinition PlaybackPortraitToolboxLayout { get; }
	
		/// <summary>
		/// Gets the landscape layout for the Playback Toolbox.
		/// </summary>
		ToolboxLayoutDefinition PlaybackLandscapeToolboxLayout { get; }
	}
}

