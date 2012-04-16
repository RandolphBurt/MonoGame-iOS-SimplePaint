/// <summary>
/// Button.cs
/// Randolph Burt - March 2012
/// </summary>
namespace Paint
{
	using System;

	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework;

	/// <summary>
	/// A button toolbox item
	/// </summary>
	public class Button : CanvasToolTouchBase, IButton
	{
		/// <summary>
		/// The images we are using to draw this button
		/// If more than one image in the list then the image will change with each press
		/// </summary>
		private ImageType[] imageTypeList;
		
		/// <summary>
		/// What is the state of the button - Which image are we currently displaying.
		/// </summary>
		private int currentButtonState = 0;
		
		/// <summary>
		/// What was the previous button state - Which image did we previously display
		/// </summary>
		private int previousButtonState = 0;
		
		/// <summary>
		/// Where do we want to draw the actual button image
		/// </summary>
		private Rectangle imageDestinationRectangle;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the button />
		/// <param name='buttonBounds' The bounds of this control/tool />
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageType' Reference to the texture map of the image we are using for rendering />
		public Button (Color backgroundColor, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, ImageType imageType)
			: base(backgroundColor, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, new ImageType[1] {imageType});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the button />
		/// <param name='borderColor' The border color of the button />
		/// <param name='borderWidth' Width of any border we want to display />
		/// <param name='buttonBounds' The bounds of this control/tool />
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageType' Reference to the texture map of the image we are using for rendering />
		public Button (Color backgroundColor, Color borderColor, int borderWidth, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, 
		               ImageType imageType)
			: base(backgroundColor, borderColor, borderWidth, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, new ImageType[1] {imageType});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the button />
		/// <param name='buttonBounds' The bounds of this control/tool />
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageTypeList' Reference to the texture map of the images we are using for rendering />
		public Button (Color backgroundColor, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, ImageType[] imageTypeList)
			: base(backgroundColor, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, imageTypeList);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the button />
		/// <param name='borderColor' The border color of the button />
		/// <param name='borderWidth' Width of any border we want to display />
		/// <param name='buttonBounds' The bounds of this control/tool />
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageTypeList' Reference to the texture map of the images we are using for rendering />
		public Button (Color backgroundColor, Color borderColor, int borderWidth, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, 
		               ImageType[] imageTypeList)
			: base(backgroundColor, borderColor, borderWidth, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, imageTypeList);
		}
		
		/// <summary>
		/// Gets the current Button State
		/// </summary>
		public int State
		{
			get
			{
				return this.currentButtonState;
			}
		}
		
		/// <summary>
		/// Occurs when the user has pressed the button
		/// </summary>
		public event EventHandler ButtonPressed;

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public override void Draw(bool refreshDisplay)
		{	
			if (refreshDisplay == true || this.previousButtonState != this.currentButtonState) 
			{
				// Blank out everything - and then draw our graphic
				this.BlankAndRedrawWithBorder();
				this.DrawGraphic(this.imageTypeList[this.currentButtonState], this.imageDestinationRectangle);
				
				this.previousButtonState = this.currentButtonState;
			}
		}
		
		/// <summary>
		/// Handle the user interaction for a particular touch/gesture type and position
		/// </summary>
		/// <param name='touchPosition'>
		/// Touch position and type of gesture 
		/// </param>
		protected override void HandleTouch(ITouchPoint touchPosition)
		{
			if (this.imageDestinationRectangle.Contains(touchPosition.Position))
			{
				if (++this.currentButtonState >= this.imageTypeList.Length)
				{
					// we've looped through all the images so go back to the first image
					this.currentButtonState = 0;
				}
				
				this.OnButtonPressed(EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises the button pressed changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnButtonPressed(EventArgs e)
		{
			if (this.ButtonPressed != null) 
			{
				this.ButtonPressed(this, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Initialise the button.
		/// </summary>
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageType' Reference to the texture map of the image we are using for rendering />
		private void Initialise(IGraphicsDisplay graphicsDisplay, ImageType[] imageTypeList)
		{
			this.imageTypeList = imageTypeList;
			
			// Assume all images for this button are the same size, then calculate where to position the button
			var graphicsRectangle = graphicsDisplay.SourceRectangleFromImageType(this.imageTypeList[0]);
			int xDiff = this.bounds.Width - graphicsRectangle.Width;
			int yDiff = this.bounds.Height - graphicsRectangle.Height;
			
			this.imageDestinationRectangle = new Rectangle(
				this.bounds.X + xDiff / 2, 
				this.bounds.Y + yDiff / 2,
				this.bounds.Width - xDiff,
				this.bounds.Height - yDiff);
		}		
	}
}

