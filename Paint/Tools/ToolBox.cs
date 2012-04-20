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
		/// The brush control's width.
		/// </summary>
		private const int BrushControlWidth = 63;
		
		/// <summary>
		/// The brush control's height.
		/// </summary>
		private const int BrushControlHeight = 230;
		
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
		/// <param name='backgroundColor' The background color of the toolbox />
		/// <param name='borderColor' The border color for all controls in the toolbox />
		/// <param name='graphicsDisplay' The graphics texture map - contains images for buttons and controls />
		/// <param name='colorList' List of pre-defined colors for the user to pick from  />
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='toolboxWidth' The width of the toolbox />
		/// <param name='minBrushSize' The minimum size brush we can use />
		/// <param name='maxBrushSize' The minimum size brush we can use />
		/// <param name='startBrushSize' The initial size brush we can use />
		public ToolBox (Color backgroundColor, Color borderColor, IGraphicsDisplay graphicsDisplay, 
		                Color[] colorList, Color startColor, int toolboxWidth, int minBrushSize, 
		                int maxBrushSize, int startBrushSize)
		{
			this.graphicsDisplay = graphicsDisplay;
			this.backgroundColor = backgroundColor;
			this.borderColor = borderColor;
			this.toolboxWidth = toolboxWidth;
			this.DockPosition = DockPosition.Bottom; 
			
			this.CreateCanvasTools(colorList, startColor, minBrushSize, maxBrushSize, startBrushSize);
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
			get 
			{
				return ToolbarHeight;
			}
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
		/// <param name='bounds' The size of the iPad screen />
		/// <param name='colorList' List of pre-defined colors for the user to pick from  />
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='minBrushSize' The minimum size brush we can use />
		/// <param name='maxBrushSize' The minimum size brush we can use />
		/// <param name='startBrushSize' The initial size brush we can use />
		private void CreateCanvasTools(Color[] colorList, Color startColor, int minBrushSize, int maxBrushSize, int startBrushSize)
		{
			this.canvasTools = new List<ICanvasToolTouch>();
			
			// Reduce by the "2 * StandardBorderSize" because that is the border round the edge of the toolbox
			float colorPickerSquareSize = (float)(this.toolboxWidth - (2 * StandardBorderSize)) / (float)(colorList.Length);
			
			var brushSizeSelector = this.CreateBrushSizeSelector(
				startColor, 
				minBrushSize, 
				maxBrushSize, 
				startBrushSize, 
				StandardBorderSize,
				ToolbarHeight + (int)colorPickerSquareSize + StandardBorderSize);			

			this.canvasTools.Add(brushSizeSelector);
			
			// Where should the buttons be positioned
			int buttonGroupXPos = toolboxWidth - (int)(4.5 * ToolbarHeight) - (StandardBorderSize * 2);
			
			// ColorSetter - shows what colour the user has chosen
			this.CreateColorSetter(
				startColor, 
				BrushControlWidth, 
				0, 
				buttonGroupXPos - BrushControlWidth, 
				ToolbarHeight + (StandardBorderSize * 2));
			
			// Add all the buttons
			this.CreateButtons(buttonGroupXPos);
	
			// User defined color selector			
			var colorSelector = this.CreateColorSelector(
				startColor, 
				BrushControlWidth + StandardBorderSize, 
				ToolbarHeight + (int)colorPickerSquareSize + StandardBorderSize, 
				(toolboxWidth - BrushControlWidth) - (2 * StandardBorderSize), 
				BrushControlHeight);

			colorSelector.ColorChanged += (sender, e) => { 
				this.colorSetter.Color = colorSelector.Color;
				brushSizeSelector.Color = colorSelector.Color;
			};
			
			this.canvasTools.Add(colorSelector);

			// Pre defined color pickers
			this.CreateColorPickers(colorSelector, colorList, colorPickerSquareSize);
			
			this.toolboxMaximisedHeight = ToolbarHeight + (int)(colorPickerSquareSize) + BrushControlHeight + (2 * StandardBorderSize);
			
			// we start maximised
			this.ToolboxHeight = toolboxMaximisedHeight;
		}	
		
		/// <summary>
		/// Creates the color pickers.
		/// </summary>
		/// <param name='colorSelector' The colorSelector control we need to update when ever the user picks a color />
		/// <param name='colorList' The list of colors the user can pick from />
		/// <param name='colorPickerSquareSize' The size of each color picker control />
		private void CreateColorPickers(ColorSelector colorSelector, Color[] colorList, float colorPickerSquareSize)
		{
			for (int i = 0; i < colorList.Length; i++)
			{
				// The adjustedPickerWidth ensures the final color lines up correctly with the edge of the tool box
				int adjustedPickerWidth = (int)colorPickerSquareSize;
				
				if (i == colorList.Length - 1) 
				{
					adjustedPickerWidth = this.toolboxWidth - ((int)(colorPickerSquareSize * i) + (2 * StandardBorderSize));
				}
				
				Rectangle colorPickerArea = new Rectangle(
					StandardBorderSize + (int)(colorPickerSquareSize * i),
					ToolbarHeight + StandardBorderSize,
					adjustedPickerWidth,
					(int)colorPickerSquareSize);
				
				ColorPicker colorPicker = new ColorPicker(
					colorList[i], 
					this.borderColor,
					StandardBorderSize,
					this.graphicsDisplay,
					colorPickerArea);
				
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
		/// <param name='buttonGroupXPos' Where should the buttons be positioned on screen />
		private void CreateButtons(int buttonGroupXPos)
		{
			int xPos = buttonGroupXPos;
			int yPos = StandardBorderSize * 2;
			int width = ToolbarHeight;
			int height = ToolbarHeight - (StandardBorderSize * 2);
			/*
			// Save Button
			var saveButton = this.CreateButton(xPos, yPos, width, height, ImageType.SaveButton);
			this.canvasTools.Add(saveButton);
			xPos += (int)(1.5 * (float)width);
			
			saveButton.ButtonPressed += (sender, e) => 
			{
				this.OnSaveSelected(EventArgs.Empty);
			};*/			

			// Undo Button
			this.undoButton = this.CreateButton(xPos, yPos, width, height, ImageType.UndoButton, ImageType.UndoButtonDisabled);
			this.undoButton.Enabled = false;
			this.canvasTools.Add(this.undoButton);
			xPos += width;

			// Redo Button
			this.redoButton = this.CreateButton(xPos, yPos, width, height, ImageType.RedoButton, ImageType.RedoButtonDisabled);
			this.redoButton.Enabled = false;
			this.canvasTools.Add(this.redoButton);
			xPos += (int)(1.5 * (float)width);

			this.undoButton.ButtonPressed += (sender, e) => 
			{
				this.OnUndoSelected(EventArgs.Empty);
			};

			this.redoButton.ButtonPressed += (sender, e) => 
			{
				this.OnRedoSelected(EventArgs.Empty);
			};

			// Min/Max Button
			var minMaxImageList = new ImageType[2] { ImageType.MinimizeToolbar, ImageType.MaximizeToolbar };

			var minMaxButton = this.CreateButton(xPos, yPos, width, height, minMaxImageList);
			
			minMaxButton.ButtonPressed += (sender, e) => 
			{
				if (minMaxButton.State == 0)
				{
					this.ToolboxHeight = this.toolboxMaximisedHeight;
				}
				else 
				{
					this.ToolboxHeight = ToolbarHeight + (2 * StandardBorderSize);
				}
			};

			this.canvasTools.Add(minMaxButton);

			// dock Button	
			xPos += width;
			var toggleDockImageList = new ImageType[2] { ImageType.DockTopButton, ImageType.DockBottomButton };

			var toggleDockButton = this.CreateButton(xPos, yPos, width, height, toggleDockImageList);
			
			toggleDockButton.ButtonPressed += (sender, e) => 
			{
				if (toggleDockButton.State == 0)
				{
					this.DockPosition = DockPosition.Bottom;
				}
				else 
				{
					this.DockPosition = DockPosition.Top;
				}
			};
			
			this.canvasTools.Add(toggleDockButton);			

			// Exit button - positioned at the top left of the toolbox - not with the other buttons
			// Ensure it lines up with the brush control
			int exitButtonWidth = BrushControlWidth - (StandardBorderSize * 2);
			int exitButtonXPos = StandardBorderSize * 2;
			
			var exitButton = this.CreateButton(exitButtonXPos, yPos, exitButtonWidth, height, ImageType.ExitButton);
			
			exitButton.ButtonPressed += (sender, e) => (this.OnExitButtonPressed(EventArgs.Empty));
			
			this.canvasTools.Add(exitButton);
		}

		/// <summary>
		/// Creates a button.
		/// </summary>
		/// <returns>
		/// The button.
		/// </returns>
		/// <param name='xPos' X offset for placing the control/>
		/// <param name='yPos' Y offset for placing the control/>
		/// <param name='width' width of the control/>
		/// <param name='height' height of the control/>
		/// <param name='imageType' image to use for the button/>
		/// <param name='disabledImageType' The image to use when the button is disabled />
		private Button CreateButton(int xPos, int yPos, int width, int height, ImageType imageType, ImageType? disabledImageType = null)
		{
			return this.CreateButton(xPos, yPos, width, height, new ImageType[1] { imageType}, disabledImageType);
		}
		
		/// <summary>
		/// Creates a button.
		/// </summary>
		/// <returns>
		/// The button.
		/// </returns>
		/// <param name='xPos' X offset for placing the control/>
		/// <param name='yPos' Y offset for placing the control/>
		/// <param name='width' width of the control/>
		/// <param name='height' height of the control/>
		/// <param name='imageTypeList' list of images to use for the button/>
		/// <param name='disabledImageType' The image to use when the button is disabled />
		private Button CreateButton(int xPos, int yPos, int width, int height, ImageType[] imageTypeList, ImageType? disabledImageType = null)
		{
			Rectangle buttonArea = new Rectangle(xPos, yPos, width, height);
			
			return new Button(backgroundColor, buttonArea, this.graphicsDisplay, imageTypeList, disabledImageType);
		}
		
		/// <summary>
		/// Creates the color selector.
		/// </summary>
		/// <returns>
		/// The color selector.
		/// </returns>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='xPos' X offset for placing the control/>
		/// <param name='yPos' Y offset for placing the control/>
		/// <param name='width' width of the control/>
		/// <param name='height' height of the control/>
		private ColorSelector CreateColorSelector(Color startColor, int xPos, int yPos, int width, int height)
		{
			Rectangle colorSelectorArea = new Rectangle(xPos, yPos, width, height);
			
			ColorSelector colorSelector = new ColorSelector(
				this.backgroundColor, 
				this.borderColor,
				StandardBorderSize,
				this.graphicsDisplay,
				colorSelectorArea,
				startColor);
						
			return colorSelector;
		}

		/// <summary>
		/// Creates the color setter.
		/// </summary>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='xPos' X offset for placing the control/>
		/// <param name='yPos' Y offset for placing the control/>
		/// <param name='width' width of the control/>
		/// <param name='height' height of the control/>
		private void CreateColorSetter (Color startColor, int xPos, int yPos, int width, int height)
		{
			Rectangle colorSetterArea = new Rectangle(xPos, yPos, width, height);
			
			this.colorSetter = new ColorSetter(
					this.borderColor,
					this.graphicsDisplay,
					colorSetterArea,
					startColor,
					StandardBorderSize * 2);
		}

		/// <summary>
		/// Creates the brush size selector.
		/// </summary>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='minBrushSize' The minimum size brush we can use />
		/// <param name='maxBrushSize' The minimum size brush we can use />
		/// <param name='startBrushSize' The initial size brush we can use />
		/// <param name='xPos' X offset for placing the control/>
		/// <param name='yPos' Y offset for placing the control/>
		private BrushSizeSelector CreateBrushSizeSelector (Color startColor, int minBrushSize, int maxBrushSize, int startBrushSize, int xPos, int yPos)
		{
			Rectangle brushArea = new Rectangle(
					xPos, 
					yPos, 
					BrushControlWidth, 
					BrushControlHeight);
			
			BrushSizeSelector brushSizeSelector = new BrushSizeSelector(
				this.backgroundColor,
				this.borderColor,
				StandardBorderSize,
				this.graphicsDisplay,
				brushArea,
				minBrushSize, 
				maxBrushSize, 
				startBrushSize, 
				startColor);
			
			this.SetBrushSizeRectange(brushSizeSelector.BrushSize);

			brushSizeSelector.BrushSizeChanged += (sender, e) => { 
				this.SetBrushSizeRectange(brushSizeSelector.BrushSize);
			};
			
			return brushSizeSelector;
		}
	}
}

