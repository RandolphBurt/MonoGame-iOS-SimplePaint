/// <summary>
/// PaintToolBox.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	
	using Paint.ToolboxLayout;

	/// <summary>
	/// Tool box - container for all tools
	/// </summary>
	public class PaintToolBox : ToolBox, IPaintToolBox
	{	
		/// <summary>
		/// The color setter tool
		/// </summary>
		private ColorSetter colorSetter;

		/// <summary>
		/// The undo button.
		/// </summary>
		private Button undoButton;

		/// <summary>
		/// The redo button.
		/// </summary>
		private Button redoButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.PaintToolBox"/> class.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		/// <param name='graphicsDisplay' The graphics texture map - contains images for buttons and controls />
		/// <param name='scale' iPad size scale - i.e.2 for retina and 1 for normal - allows us to multiply up the layout />
		public PaintToolBox (ToolboxLayoutDefinition toolboxLayoutDefinition, IGraphicsDisplay graphicsDisplay, int scale)
			: base (toolboxLayoutDefinition, graphicsDisplay, scale)
		{
			this.CreateTools(toolboxLayoutDefinition);
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
		/// Occurs when the user has pressed the undo button.
		/// </summary>
		public event EventHandler UndoSelected;
		
		/// <summary>
		/// Occurs when the user has pressed the redo button.
		/// </summary>
		public event EventHandler RedoSelected;
				
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
		/// Creates all the buttons and adds them to our list of controls
		/// </summary>
		/// <param name='buttons' All the buttons we need to display on screen />
		protected override void AddButton(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonLayout)
		{
			switch (buttonLayout.ButtonType)
			{
				case ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType.Undo:
					this.AddUndoButton(buttonLayout);
					break;
					
				case ToolboxLayoutDefinitionStandardToolsButtonsButtonButtonType.Redo:
					this.AddRedoButton(buttonLayout);
					break;
					
				default:
					base.AddButton(buttonLayout);
					break;
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
		/// Creates all our tools.
		/// </summary>
		/// <param name='toolboxLayoutDefinition' The layout of the toolbox />
		private void CreateTools(ToolboxLayoutDefinition toolboxLayoutDefinition)
		{
			Color startColor = new Color(
				toolboxLayoutDefinition.PaintTools.ColorSetter.Region.BackgroundColor.Red,
				toolboxLayoutDefinition.PaintTools.ColorSetter.Region.BackgroundColor.Green,
				toolboxLayoutDefinition.PaintTools.ColorSetter.Region.BackgroundColor.Blue);

			var brushSizeSelector = this.CreateBrushSizeSelector(startColor, toolboxLayoutDefinition.PaintTools.BrushSizeSelector);
			this.AddTool(brushSizeSelector);

			// ColorSetter - shows what colour the user has chosen
			this.colorSetter = this.CreateColorSetter(startColor, toolboxLayoutDefinition.PaintTools.ColorSetter);
			this.AddTool(colorSetter);

			// User defined color selector			
			var colorSelector = this.CreateColorSelector(startColor, toolboxLayoutDefinition.PaintTools.ColorSelector);

			colorSelector.ColorChanged += (sender, e) => { 
				this.colorSetter.Color = colorSelector.Color;
				brushSizeSelector.Color = colorSelector.Color;
			};
			
			this.AddTool(colorSelector);

			// Pre defined color pickers
			this.CreateColorPickers(colorSelector, toolboxLayoutDefinition.PaintTools.ColorPickers.ColorPicker);			
		}	
		
		/// <summary>
		/// Creates the color pickers.
		/// </summary>
		/// <param name='colorSelector' The colorSelector control we need to update when ever the user picks a color />
		/// <param name='layoutColorPickers' Layout information for each color picker control />
		private void CreateColorPickers(ColorSelector colorSelector, ToolboxLayoutDefinitionPaintToolsColorPickersColorPicker[] layoutColorPickers)
		{
			foreach (var layoutColorPicker in layoutColorPickers)
			{
				var colorPicker = new ColorPicker(this.GraphicsDisplay, new ColorPickerDefinition(layoutColorPicker, this.Scale));

				colorPicker.ColorSelected += (sender, e) => 
				{
					colorSelector.Color = colorPicker.Color;
				};
				
				this.AddTool(colorPicker);
			}
		}

		/// <summary>
		/// Adds the redo button.
		/// </summary>
		/// <param name='buttonLayout'>
		/// Button layout.
		/// </param>
		private void AddRedoButton(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonLayout)
		{
			this.redoButton = new Button(
				this.GraphicsDisplay, 
				new ButtonDefinition(buttonLayout, this.Scale, new ImageType[] { ImageType.RedoButton } , ImageType.RedoButtonDisabled));		

			this.redoButton.Enabled = false;
			this.redoButton.ButtonPressed += (sender, e) => 
			{
				this.OnRedoSelected(EventArgs.Empty);
			};
		
			this.AddTool(this.redoButton);
		}

		/// <summary>
		/// Adds the undo button.
		/// </summary>
		/// <param name='buttonLayout'>
		/// Button layout.
		/// </param>
		private void AddUndoButton(ToolboxLayoutDefinitionStandardToolsButtonsButton buttonLayout)
		{
			this.undoButton = new Button(
				this.GraphicsDisplay, 
				new ButtonDefinition(buttonLayout, this.Scale, new ImageType[] { ImageType.UndoButton } , ImageType.UndoButtonDisabled));		
			
			this.undoButton.Enabled = false;
			this.undoButton.ButtonPressed += (sender, e) => 
			{
				this.OnUndoSelected(EventArgs.Empty);
			};		

			this.AddTool(this.undoButton);
		}
		
		/// <summary>
		/// Creates the color selector.
		/// </summary>
		/// <returns>
		/// The color selector.
		/// </returns>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='layoutColorSelector' The layout information for this color selector />
		private ColorSelector CreateColorSelector(Color startColor, ToolboxLayoutDefinitionPaintToolsColorSelector layoutColorSelector)
		{
			return new ColorSelector(this.GraphicsDisplay, new ColorSelectorDefinition(startColor, layoutColorSelector, this.Scale));
		}

		/// <summary>
		/// Creates the color setter.
		/// </summary>
		/// <returns>
		/// The colorSetter.
		/// </returns>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='layoutColorSetter' The layout information for this color setter />
		private ColorSetter CreateColorSetter(Color startColor, ToolboxLayoutDefinitionPaintToolsColorSetter layoutColorSetter)
		{
			return new ColorSetter(this.GraphicsDisplay, new ColorSetterDefinition(layoutColorSetter, this.Scale));
		}

		/// <summary>
		/// Creates the brush size selector.
		/// </summary>
		/// <param name='startColor' The color we will start drawing with />
		/// <param name='layoutBrushSizeSelector' The layout information for this brush size selector />
		private BrushSizeSelector CreateBrushSizeSelector(Color startColor, ToolboxLayoutDefinitionPaintToolsBrushSizeSelector layoutBrushSizeSelector)
		{
			var brushSizeSelector = new BrushSizeSelector(
				this.GraphicsDisplay, 
				new BrushSizeSelectorDefinition(startColor, layoutBrushSizeSelector, this.Scale));

			this.SetBrushSizeRectange(brushSizeSelector.BrushSize);

			brushSizeSelector.BrushSizeChanged += (sender, e) => { 
				this.SetBrushSizeRectange(brushSizeSelector.BrushSize);
			};
			
			return brushSizeSelector;
		}
	}
}

