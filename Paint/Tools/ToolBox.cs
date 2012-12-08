/// <summary>
/// ToolBox.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	using Paint.ToolboxLayout;

	/// <summary>
	/// Tool box - container for all tools
	/// </summary>
	public class ToolBox : IToolBox
	{
		/// <summary>
		/// Border size for drawing the tool on screen.
		/// </summary>
		protected const int StandardBorderSize = 2;

		/// <summary>
		/// The height of the toolbar and the width of the min/max and dock buttons inside it
		/// </summary>
		public const int ToolbarHeight = 50;
		
		/// <summary>
		/// The width of the toolbox.
		/// </summary>
		private int toolboxWidth;
		
		/// <summary>
		/// All the tools we can use to assist us with our drawing
		/// </summary>
		private List<ICanvasToolTouch> canvasTools;

		/// <summary>
		/// The color setter tool
		/// </summary>
		private ColorSetter colorSetter;

		/// <summary>
		/// The color of the border of all controls
		/// </summary>
		Color borderColor;
		
		/// <summary>
		/// The color of the background of all controls
		/// </summary>
		Color backgroundColor;
		
		/// <summary>
		/// The graphics texture map - contains images for buttons and controls
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;

		/// <summary>
		/// The height of the toolbox when maximised.
		/// </summary>
		private int toolboxMaximisedHeight;
		
		/// <summary>
		/// The undo button.
		/// </summary>
		private Button undoButton;

		/// <summary>
		/// The redo button.
		/// </summary>
		private Button redoButton;

		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ToolBox"/> class.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		/// <param name='graphicsDisplay' The graphics texture map - contains images for buttons and controls />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public ToolBox (ToolboxLayoutDefinition toolboxLayoutDefinition, IGraphicsDisplay graphicsDisplay, int scale)
		{
			this.graphicsDisplay = graphicsDisplay;
			this.backgroundColor = this.TranslateToolboxLayoutColor(toolboxLayoutDefinition.BackgroundColor);
			this.borderColor = this.TranslateToolboxLayoutColor(toolboxLayoutDefinition.Border.Color);
			this.toolboxWidth = toolboxLayoutDefinition.Width * scale;
			this.DockPosition = DockPosition.Bottom; 
			
			this.CreateCanvasTools(toolboxLayoutDefinition, scale);
		}

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
		/// Gets the current brush being used for drawing
		/// </summary>
		public Rectangle Brush 
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the current color.
		/// </summary>
		public Color Color 
		{
			get
			{
				return colorSetter.Color;
			}
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether the undo button should be enabled or not.
		/// </summary>
		public bool UndoEnabled { 
			get
			{
				return this.undoButton.Enabled;
			}
			
			set 
			{
				this.undoButton.Enabled = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the redo button should be enabled or not.
		/// </summary>
		public bool RedoEnabled
		{
			get
			{
				return this.redoButton.Enabled;
			}
			
			set 
			{
				this.redoButton.Enabled = value;
			}
		}
		
		/// <summary>
		/// Occurs when the user has selected the Exit button
		/// </summary>
		public event EventHandler ExitSelected;

		/// <summary>
		/// Occurs when the user has pressed the undo button.
		/// </summary>
		public event EventHandler UndoSelected;
		
		/// <summary>
		/// Occurs when the user has pressed the redo button.
		/// </summary>
		public event EventHandler RedoSelected;
				
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
			foreach (var tool in this.canvasTools)
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
			
			this.colorSetter.Draw(refreshDisplay);
			
			foreach (var tool in this.canvasTools)
			{
				tool.Draw(refreshDisplay);
			}
			
			this.graphicsDisplay.EndRender();
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
		/// Raises the undo selected changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnUndoSelected(EventArgs e)
		{
			if (this.UndoSelected != null) 
			{
				this.UndoSelected(this, EventArgs.Empty);
			}
		}		
		
		/// <summary>
		/// Raises the redo selected changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnRedoSelected(EventArgs e)
		{
			if (this.RedoSelected != null) 
			{
				this.RedoSelected(this, EventArgs.Empty);
			}
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

		/// <summary>
		/// Sets the brush size rectange.
		/// </summary>
		/// <param name='brushWidth'>
		/// Brush width.
		/// </param>
		private void SetBrushSizeRectange(int brushWidth)
		{
			this.Brush = new Rectangle(0, 0, brushWidth, brushWidth);
		}
		
		/// <summary>
		/// Draws the background (and border)
		/// </summary>
		private void DrawBackground()
		{
			// First fill the entire region with the border colour
			Rectangle borderRectangle = new Rectangle(0, 0, this.toolboxWidth, this.toolboxMaximisedHeight);
			
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, borderRectangle, this.borderColor);
			
			// Then go over the tool bar area - we want to ensure there is a double thickness border because we won't draw 
			// the border on buttons
			Rectangle toolbarRectangle = new Rectangle(
				StandardBorderSize * 2, 
				StandardBorderSize * 2, 
				this.toolboxWidth - (4 * StandardBorderSize),
				ToolbarHeight - (2 * StandardBorderSize));
			
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, toolbarRectangle, this.backgroundColor);
			
			// We don't bother blanking out the area beneath the toolbar because we'll draw over that with our
			// controls anyway
		}

		/// <summary>
		/// Creates all our canvas tools.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		/// <param name='scale' Scale of this iPad = i.e. 2 is retina, 1 is normal />
		private void CreateCanvasTools(ToolboxLayoutDefinition toolboxLayoutDefinition, int scale)
		{
			Color startColor = new Color(
				toolboxLayoutDefinition.Controls.ColorSetter.Region.BackgroundColor.Red,
				toolboxLayoutDefinition.Controls.ColorSetter.Region.BackgroundColor.Green,
				toolboxLayoutDefinition.Controls.ColorSetter.Region.BackgroundColor.Blue);

			this.ToolboxMinimizedHeight = toolboxLayoutDefinition.MinimizedHeight * 2;
			this.toolboxMaximisedHeight = toolboxLayoutDefinition.MaximizedHeight * 2;

			// we start maximised
			this.ToolboxHeight = this.toolboxMaximisedHeight;

			this.canvasTools = new List<ICanvasToolTouch>();

			var brushSizeSelector = this.CreateBrushSizeSelector(startColor, toolboxLayoutDefinition.Controls.BrushSizeSelector, scale);

			this.canvasTools.Add(brushSizeSelector);

			// ColorSetter - shows what colour the user has chosen
			this.CreateColorSetter(startColor, toolboxLayoutDefinition.Controls.ColorSetter, scale);
			
			// Add all the buttons
			this.CreateButtons(toolboxLayoutDefinition.Controls.Button, scale);
	
			// User defined color selector			
			var colorSelector = this.CreateColorSelector(startColor, toolboxLayoutDefinition.Controls.ColorSelector, scale);

			colorSelector.ColorChanged += (sender, e) => { 
				this.colorSetter.Color = colorSelector.Color;
				brushSizeSelector.Color = colorSelector.Color;
			};
			
			this.canvasTools.Add(colorSelector);

			// Pre defined color pickers
			this.CreateColorPickers(colorSelector, toolboxLayoutDefinition.Controls.ColorPicker, scale);			
		}	
		
		/// <summary>
		/// Creates the color pickers.
		/// </summary>
		/// <param name='colorSelector' The colorSelector control we need to update when ever the user picks a color />
		/// <param name='layoutColorPickers' Layout information for each color picker control />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		private void CreateColorPickers(ColorSelector colorSelector, ToolboxLayoutDefinitionControlsColorPicker[] layoutColorPickers, int scale)
		{
			foreach (var layoutColorPicker in layoutColorPickers)
			{
				var colorPicker = new ColorPicker(this.graphicsDisplay, new ColorPickerDefinition(layoutColorPicker, scale));

				colorPicker.ColorSelected += (sender, e) => 
				{
					colorSelector.Color = colorPicker.Color;
				};
				
				this.canvasTools.Add(colorPicker);
			}
		}
		
		/// <summary>
		/// Creates all the buttons.
		/// </summary>
		/// <param name='buttons' All the buttons we need to display on screen />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		private void CreateButtons(ToolboxLayoutDefinitionControlsButton[] buttons, int scale)
		{
			foreach (var buttonLayout in buttons)
			{
				ImageType? disabledImageType = null;
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
					
					case ToolboxLayoutDefinitionControlsButtonButtonType.Undo:
						imageList.Add(ImageType.UndoButton);
						disabledImageType = ImageType.UndoButtonDisabled;
						break;
					
					case ToolboxLayoutDefinitionControlsButtonButtonType.Redo:
						imageList.Add(ImageType.RedoButton);
						disabledImageType = ImageType.RedoButtonDisabled;
						break;
				}

				var button = 
					new Button(
						this.graphicsDisplay, 
						new ButtonDefinition(buttonLayout, scale, imageList.ToArray(), disabledImageType));

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
								this.ToolboxHeight = ToolbarHeight + (2 * StandardBorderSize);
							}
						};

						break;
						
					case ToolboxLayoutDefinitionControlsButtonButtonType.Undo:
						this.undoButton = button;
						this.undoButton.Enabled = false;
						this.undoButton.ButtonPressed += (sender, e) => 
						{
							this.OnUndoSelected(EventArgs.Empty);
						};

						break;
						
					case ToolboxLayoutDefinitionControlsButtonButtonType.Redo:
						this.redoButton = button;
						this.redoButton.Enabled = false;
						this.redoButton.ButtonPressed += (sender, e) => 
						{
							this.OnRedoSelected(EventArgs.Empty);
						};

						break;
				}

				this.canvasTools.Add(button);
			}
		}
		
		/// <summary>
		/// Creates the color selector.
		/// </summary>
		/// <returns>
		/// The color selector.
		/// </returns>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='layoutColorSelector' The layout information for this color selector />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		private ColorSelector CreateColorSelector(Color startColor, ToolboxLayoutDefinitionControlsColorSelector layoutColorSelector, int scale)
		{
			return new ColorSelector(this.graphicsDisplay, new ColorSelectorDefinition(startColor, layoutColorSelector, scale));
		}

		/// <summary>
		/// Creates the color setter.
		/// </summary>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='layoutColorSetter' The layout information for this color setter />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		private void CreateColorSetter(Color startColor, ToolboxLayoutDefinitionControlsColorSetter layoutColorSetter, int scale)
		{
			this.colorSetter = new ColorSetter(this.graphicsDisplay, new ColorSetterDefinition(layoutColorSetter, scale));
		}

		/// <summary>
		/// Creates the brush size selector.
		/// </summary>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='layoutBrushSizeSelector' The layout information for this brush size selector />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		private BrushSizeSelector CreateBrushSizeSelector(Color startColor, ToolboxLayoutDefinitionControlsBrushSizeSelector layoutBrushSizeSelector, int scale)
		{
			var brushSizeSelector = new BrushSizeSelector(
				this.graphicsDisplay, 
				new BrushSizeSelectorDefinition(startColor, layoutBrushSizeSelector, scale));

			this.SetBrushSizeRectange(brushSizeSelector.BrushSize);

			brushSizeSelector.BrushSizeChanged += (sender, e) => { 
				this.SetBrushSizeRectange(brushSizeSelector.BrushSize);
			};
			
			return brushSizeSelector;
		}
	}
}

