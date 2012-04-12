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
		/// The height of the toolbar and the width of the min/max and dock buttons inside it
		/// </summary>
		public const int ToolbarSize = 50;
		
		/// <summary>
		/// The brush control's width.
		/// </summary>
		private const int BrushControlWidth = 70;
		
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
		/// The SpriteBatch for all rendering
		/// </summary>
		private SpriteBatch spriteBatch;
		
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
		/// Initializes a new instance of the <see cref="Paint.ToolBox"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the toolbox />
		/// <param name='borderColor' The border color for all controls in the toolbox />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='graphicsDisplay' The graphics texture map - contains images for buttons and controls />
		/// <param name='colorList' List of pre-defined colors for the user to pick from  />
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='toolboxWidth' The width of the toolbox />
		/// <param name='minBrushSize' The minimum size brush we can use />
		/// <param name='maxBrushSize' The minimum size brush we can use />
		/// <param name='startBrushSize' The initial size brush we can use />
		public ToolBox (Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, 
		                IGraphicsDisplay graphicsDisplay, Color[] colorList, Color startColor, 
		                int toolboxWidth, int minBrushSize, int maxBrushSize, int startBrushSize)
		{
			this.spriteBatch = spriteBatch;
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
				return ToolbarSize;
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
		/// Occurs when the user has selected the Exit button
		/// </summary>
		public event EventHandler ExitSelected;
		
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
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

			this.colorSetter.Draw(refreshDisplay);
			
			foreach (var tool in this.canvasTools)
			{
				tool.Draw(refreshDisplay);
			}
			
			this.spriteBatch.End ();
		}
		
		/// <summary>
		/// Raises the exit selected changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnButtonPressed(EventArgs e)
		{
			if (this.ExitSelected != null) 
			{
				this.ExitSelected(this, EventArgs.Empty);
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

			float colorPickerWidth = (float)(toolboxWidth) / (float)(colorList.Length);

			// Bush size selector
			BrushSizeSelector brushSizeSelector = new BrushSizeSelector(
				this.backgroundColor,
				this.borderColor,
				this.graphicsDisplay,
				new Rectangle(0, (int)(ToolbarSize + colorPickerWidth), BrushControlWidth, BrushControlHeight),
				minBrushSize, maxBrushSize, startBrushSize, startColor);
			
			this.SetBrushSizeRectange(brushSizeSelector.BrushSize);

			brushSizeSelector.BrushSizeChanged += (sender, e) => { 
				this.SetBrushSizeRectange(brushSizeSelector.BrushSize);
			};

			this.canvasTools.Add(brushSizeSelector);			
			
			// ColorSetter - shows what colour the user has chosen
			this.colorSetter = new ColorSetter(
					this.borderColor,
					this.graphicsDisplay,
					new Rectangle(BrushControlWidth, 0, toolboxWidth - (2 * ToolbarSize) - BrushControlWidth, ToolbarSize),
					startColor);
			
			// Exit/save button
			Button exitButton = new Button(
				backgroundColor,
				borderColor,
				new Rectangle(0, 0, BrushControlWidth, ToolbarSize), // Ensure lines up with the brush control
				this.graphicsDisplay,
				ImageType.ExitButton);
			
			exitButton.ButtonPressed += (sender, e) => (this.OnButtonPressed(EventArgs.Empty));
			
			this.canvasTools.Add(exitButton);

			// min/max
			ToggleMinimizeMaximizeButton toggleMinMaxButton = new ToggleMinimizeMaximizeButton(
				backgroundColor, 
				borderColor, 
				this.graphicsDisplay,
				new Rectangle(toolboxWidth - (2 * ToolbarSize), 0, ToolbarSize, ToolbarSize),
				MinimizedMaximizedState.Maximized);
			
			toggleMinMaxButton.MinimizeMaximizeStateChanged += (sender, e) => 
			{
				if (toggleMinMaxButton.MinimizedMaximizedState == MinimizedMaximizedState.Maximized)
				{
					this.ToolboxHeight = this.toolboxMaximisedHeight;
				}
				else 
				{
					this.ToolboxHeight = ToolbarSize;
				}
			};

			this.canvasTools.Add(toggleMinMaxButton);
			
			// dock position
			ToggleDockButton toggleDock = new ToggleDockButton(
				backgroundColor, 
				borderColor, 
				this.graphicsDisplay,
				new Rectangle(toolboxWidth - ToolbarSize, 0, ToolbarSize, ToolbarSize),
				this.DockPosition);
				
			toggleDock.DockPositionChanged += (sender, e) => 
			{
				this.DockPosition = toggleDock.DockPosition;
			};
			
			this.canvasTools.Add(toggleDock);			

			// User defined color selector
			ColorSelector colorSelector = new ColorSelector(
				this.backgroundColor, 
				this.borderColor,
				this.graphicsDisplay,
				new Rectangle(BrushControlWidth, (int)(ToolbarSize + colorPickerWidth), toolboxWidth - BrushControlWidth, BrushControlHeight),
				startColor);
			
			colorSelector.ColorChanged += (sender, e) => { 
				this.colorSetter.Color = colorSelector.Color;
				brushSizeSelector.Color = colorSelector.Color;
			};
			
			this.canvasTools.Add(colorSelector);

			// Pre defined color pickers
			for (int i = 0; i < colorList.Length; i++)
			{
				ColorPicker colorPicker = new ColorPicker(
					colorList[i], 
					this.borderColor,
					this.graphicsDisplay,
					new Rectangle((int)(colorPickerWidth * i), ToolbarSize, (int)colorPickerWidth, (int)colorPickerWidth));
				
				colorPicker.ColorSelected += (sender, e) => 
				{
					colorSelector.Color = colorPicker.Color;
				};
				
				this.canvasTools.Add(colorPicker);
			}
			
			this.toolboxMaximisedHeight = ToolbarSize + (int)(colorPickerWidth) + BrushControlHeight;
			
			// we start maximised
			this.ToolboxHeight = toolboxMaximisedHeight;
		}		
	}
}

