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
		/// The image to use when the button is disabled.
		/// </summary>
		private ImageType? disabledImageType;
		
		/// <summary>
		/// Is the button enabled or not.
		/// </summary>
		private bool enabled = true;
		
		/// <summary>
		/// Is a refresh required on the next 'draw'
		/// </summary>
		private bool refreshRequired = false;
		
		/// <summary>
		/// What is the state of the button - Which image are we currently displaying.
		/// </summary>
		private int currentButtonState = 0;
		
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
		public Button (Color backgroundColor, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, ImageType imageType, ImageType? disabledImageType = null)
			: base(backgroundColor, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, new ImageType[1] {imageType}, disabledImageType);
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
		               ImageType imageType, ImageType? disabledImageType = null)
			: base(backgroundColor, borderColor, borderWidth, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, new ImageType[1] {imageType}, disabledImageType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the button />
		/// <param name='buttonBounds' The bounds of this control/tool />
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageTypeList' Reference to the texture map of the images we are using for rendering />
		public Button (Color backgroundColor, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, ImageType[] imageTypeList, ImageType? disabledImageType = null)
			: base(backgroundColor, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, imageTypeList, disabledImageType);
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
		               ImageType[] imageTypeList, ImageType? disabledImageType = null)
			: base(backgroundColor, borderColor, borderWidth, graphicsDisplay, buttonBounds) 
		{
			this.Initialise(graphicsDisplay, imageTypeList, disabledImageType);
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Paint.Button"/> is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			
			set
			{
				if (this.enabled != value)
				{
					this.enabled = value;
					this.refreshRequired = true;
				}
			}
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
			if (refreshDisplay == true || this.refreshRequired == true) 
			{
				// Blank out everything - and then draw our graphic
				this.BlankAndRedrawWithBorder();
				
				ImageType imageType = this.imageTypeList[this.currentButtonState];
				
				if (this.Enabled == false && this.disabledImageType.HasValue)
				{
					imageType = this.disabledImageType.Value;
				}
				
				this.DrawGraphic(imageType, this.imageDestinationRectangle);
				
				this.refreshRequired = false;
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
			if (this.Enabled == true && touchPosition.TouchType == TouchType.Tap)
			{
				if (this.imageDestinationRectangle.Contains(touchPosition.Position))
				{
					int previousState = this.currentButtonState;
					
					if (++this.currentButtonState >= this.imageTypeList.Length)
					{
						// we've looped through all the images so go back to the first image
						this.currentButtonState = 0;						
					}
					
					if (previousState != this.currentButtonState)
					{
						this.refreshRequired = true;
					}
					
					this.OnButtonPressed(EventArgs.Empty);
				}
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
		private void Initialise(IGraphicsDisplay graphicsDisplay, ImageType[] imageTypeList, ImageType? disabledImageType = null)
		{
			this.Enabled = true;
			this.imageTypeList = imageTypeList;
			this.disabledImageType = disabledImageType;
			
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

