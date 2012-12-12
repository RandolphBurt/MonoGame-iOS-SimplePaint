/// <summary>
/// ToolBox.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;

	using Paint.ToolboxLayout;

	public abstract class ToolBox : IToolBox
	{
		/// <summary>
		/// The toolbar inner bounds (i.e. inside the borders)
		/// </summary>
		private Rectangle toolbarInnerBounds;

		/// <summary>
		/// The height of the toolbox when maximised.
		/// </summary>
		private int toolboxMaximisedHeight;

		/// <summary>
		/// The width of the toolbox.
		/// </summary>
		private int toolboxWidth;
		
		/// <summary>
		/// All the tools in our toolbox that the user can manipulate/interact with (e.g. buttons)
		/// </summary>
		private List<IToolBoxToolTouch> interactiveTools;

		/// <summary>
		/// All the tools in our toolbox which the user cannot interact with (e.g. labels)
		/// </summary>
		private List<IToolBoxTool> nonInteractiveTools;

		/// <summary>
		/// The toolbox bounds.
		/// </summary>
		private Rectangle toolboxBounds;

		/// <summary>
		/// The color of the border of all controls
		/// </summary>
		protected Color borderColor;
		
		/// <summary>
		/// The color of the background of all controls
		/// </summary>
		protected Color backgroundColor;
		
		/// <summary>
		/// Scale of this iPad = i.e. 2 is retina, 1 is normal
		/// </summary>
		protected int scale;

		/// <summary>
		/// The graphics texture map - contains images for buttons and controls
		/// </summary>
		protected IGraphicsDisplay graphicsDisplay;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ToolBox"/> class.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		/// <param name='graphicsDisplay' The graphics texture map - contains images for buttons and controls />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public ToolBox(ToolboxLayoutDefinition toolboxLayoutDefinition, IGraphicsDisplay graphicsDisplay, int scale)
		{
			this.backgroundColor = this.TranslateToolboxLayoutColor(toolboxLayoutDefinition.BackgroundColor);
			this.borderColor = this.TranslateToolboxLayoutColor(toolboxLayoutDefinition.Border.Color);
			this.graphicsDisplay = graphicsDisplay;
			this.scale = scale;
			this.toolboxWidth = toolboxLayoutDefinition.Width * scale;
			this.ToolboxMinimizedHeight = toolboxLayoutDefinition.MinimizedHeight * scale;
			this.toolboxMaximisedHeight = toolboxLayoutDefinition.MaximizedHeight * scale;
			
			int toolboxBorderWidth = toolboxLayoutDefinition.Border.Width * scale;

			// we start maximised and docked at the bottom
			this.ToolboxHeight = toolboxLayoutDefinition.MaximizedHeight * scale;
			this.DockPosition = DockPosition.Bottom; 

			this.toolboxBounds = new Rectangle(0, 0, this.toolboxWidth, this.ToolboxHeight);
	
			this.toolbarInnerBounds = new Rectangle(
				toolboxBorderWidth, 
				toolboxBorderWidth, 
				this.toolboxWidth - (2 * toolboxBorderWidth),
				this.ToolboxMinimizedHeight - (2 * toolboxBorderWidth));

			this.interactiveTools = new List<IToolBoxToolTouch>();
			this.nonInteractiveTools = new List<IToolBoxTool>();
		}

		/// <summary>
		/// Occurs when the user has selected to exit
		/// </summary>
		public event EventHandler ExitSelected;

		/// <summary>
		/// Gets the current height of the toolbox.
		/// </summary>
		public int ToolboxHeight
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the height of the toolbox when minimised.
		/// </summary>
		public int ToolboxMinimizedHeight
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the current dock position of the toolbox
		/// </summary>
		public DockPosition DockPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// Checks wheter a particular touch point (user pressing the screen) is within the bounds of one of the tools.
		/// </summary>
		/// <returns>
		/// True = The touchPosition was handled by this control
		/// False = The touchPosition was not handled by this control
		/// </returns>
		/// <param name='touchPosition' The location where the user touched the screen (and type of touch) />
		public bool CheckTouchCollision (ITouchPoint touchPosition)
		{
			foreach (var tool in this.interactiveTools)
			{
				if (tool.CheckTouchCollision(touchPosition) == true)
				{
					// no need to check the other tools so exit now
					return true;
				}
			}
			
			return false;
		}

		/// <summary>
		/// Draw the toolbox (and all containing tools) on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public void Draw (bool refreshDisplay)
		{
			/*
			 * Draw the tools with an Opque BlendState to ensure we overwrite the previous color completely
			 */
			this.graphicsDisplay.BeginRenderOpaque();
			
			if (refreshDisplay == true)
			{
				this.DrawBackground();
			}

			this.DrawTools(refreshDisplay);

			this.graphicsDisplay.EndRender();
		}

		/// <summary>
		/// Draws the tools.
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		protected void DrawTools(bool refreshDisplay)
		{
			foreach (var tool in this.interactiveTools)
			{
				tool.Draw(refreshDisplay);
			}

			foreach (var tool in this.nonInteractiveTools)
			{
				tool.Draw(refreshDisplay);
			}
		}
		
		/// <summary>
		/// Draws the background (and border)
		/// </summary>
		protected virtual void DrawBackground()
		{
			// First fill the entire region with the border colour...
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, this.toolboxBounds, this.borderColor);
			
			// Then blank out the space for the toolbar...
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, this.toolbarInnerBounds, this.backgroundColor);

			// Classes ingeriting from us may override this and do more
		}

		/// <summary>
		/// Raises the exit selected changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnExitButtonPressed(EventArgs e)
		{
			if (this.ExitSelected != null) 
			{
				this.ExitSelected(this, EventArgs.Empty);
			}
		}		

		/// <summary>
		/// Adds the tool to our toolbox
		/// </summary>
		/// <param name='tool'>
		/// The tool to add to the toolbox
		/// </param>
		protected void AddTool(IToolBoxTool tool)
		{
			if (tool is IToolBoxToolTouch)
			{
				this.interactiveTools.Add(tool as IToolBoxToolTouch);
			}
			else
			{
				this.nonInteractiveTools.Add(tool);
			}
		}

		/// <summary>
		/// Creates all the buttons and adds them to our list of controls
		/// </summary>
		/// <param name='buttons' All the buttons we need to display on screen />
		protected void AddButtons(ToolboxLayoutDefinitionControlsButton[] buttons)
		{
			foreach (var buttonLayout in buttons)
			{
				this.AddButton(buttonLayout);
			}
		}

		/// <summary>
		/// Creates a button and adds it.
		/// </summary>
		/// <returns>
		/// The button.
		/// </returns>
		/// <param name='buttonLayout'>
		/// Button layout.
		/// </param>
		protected virtual void AddButton(ToolboxLayoutDefinitionControlsButton buttonLayout)
		{
			List<ImageType> imageList = new List<ImageType>();
			
			switch (buttonLayout.ButtonType)
			{
				case ToolboxLayoutDefinitionControlsButtonButtonType.Exit:
					imageList.Add(ImageType.ExitButton);
					break;
					
				case ToolboxLayoutDefinitionControlsButtonButtonType.ToggleDock:
					imageList.Add(ImageType.DockTopButton);
					imageList.Add(ImageType.DockBottomButton);
					break;
					
				case ToolboxLayoutDefinitionControlsButtonButtonType.ToggleMaxMin:
					imageList.Add(ImageType.MinimizeToolbar);
					imageList.Add(ImageType.MaximizeToolbar);
					break;					
			}
			
			var button = new Button(
					this.graphicsDisplay, 
					new ButtonDefinition(buttonLayout, this.scale, imageList.ToArray(), null));		

			switch (buttonLayout.ButtonType)
			{
				case ToolboxLayoutDefinitionControlsButtonButtonType.Exit:
					button.ButtonPressed += (sender, e) => (this.OnExitButtonPressed(EventArgs.Empty));
					break;
					
				case ToolboxLayoutDefinitionControlsButtonButtonType.ToggleDock:
					button.ButtonPressed += (sender, e) => 
					{
						if (button.State == 0)
						{
							this.DockPosition = DockPosition.Bottom;
						}
						else
						{
							this.DockPosition = DockPosition.Top;
						}
					};
					break;
					
				case ToolboxLayoutDefinitionControlsButtonButtonType.ToggleMaxMin:
					button.ButtonPressed += (sender, e) => 
					{
						if (button.State == 0)
						{
							this.ToolboxHeight = this.toolboxMaximisedHeight;
						}
						else
						{
							this.ToolboxHeight = this.ToolboxMinimizedHeight;
						}
					};
					
					break;
			}

			this.AddTool(button);
		}


		/// <summary>
		/// Translates the toolbox layout color type to a Color.
		/// </summary>
		/// <returns>The toolbox layout color type.</returns>
		/// <param name='color'>The converted Color object.</param>
		private Color TranslateToolboxLayoutColor(ColorType color)
		{
			return new Color(color.Red, color.Green, color.Blue);
		}
	}
}

