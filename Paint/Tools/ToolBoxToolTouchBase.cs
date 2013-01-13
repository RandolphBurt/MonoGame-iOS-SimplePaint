/// <summary>
/// TooBoxToolTouchBase.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using Microsoft.Xna.Framework;

	/// <summary>
	/// Base (Abstract) class for any tools that allow user interaction.
	/// </summary>
	public abstract class ToolBoxToolTouchBase : IToolBoxToolTouch
	{
		/// <summary>
		/// Border size for drawing the tool on screen.
		/// </summary>
		protected int BorderWidth;
		
		/// <summary>
		/// The background color of the tool
		/// </summary>
		protected Color BackgroundColor;
		
		/// <summary>
		/// The color of the borders around the control.
		/// </summary>
		protected Color BorderColor;	

		/// <summary>
		/// Contains all the images for the application
		/// </summary>
		private IGraphicsDisplay graphicsDisplay;
		
		/// <summary>
		/// Small bit of state - determines whether the user is currently interacting with this control by dragging their finger across the iPad
		/// </summary>
		private bool inDragMode = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasToolTouchBase"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of this tool />
		/// <param name='graphicsDisplay' Contains all the images for the application />
		/// <param name='bounds' The bounds of this control/tool />
		public ToolBoxToolTouchBase(Color backgroundColor, IGraphicsDisplay graphicsDisplay, Rectangle bounds)
		{
			this.Bounds = bounds;
			this.graphicsDisplay = graphicsDisplay;
			this.BackgroundColor = backgroundColor;
			this.BorderColor = backgroundColor;
			this.BorderWidth = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasToolTouchBase"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of this tool />
		/// <param name='borderColor' The border color of this tool />
		/// <param name='borderWidth' The border width of this tool />
		/// <param name='graphicsDisplay' Contains all the images for the application />
		/// <param name='bounds' The bounds of this control/tool />
		public ToolBoxToolTouchBase(Color backgroundColor, Color borderColor, int borderWidth, IGraphicsDisplay graphicsDisplay, Rectangle bounds)
		{
			this.Bounds = bounds;
			this.graphicsDisplay = graphicsDisplay;
			this.BackgroundColor = backgroundColor;
			this.BorderColor = borderColor;
			this.BorderWidth = borderWidth;
		}

		/// <summary>
		/// The location and size of the control.
		/// </summary>
		public Rectangle Bounds
		{
			get;
			private set;
		}

		/// <summary>
		/// Checks wheter a particular touch point (user pressing the screen) is within the bounds of this control.
		/// </summary>
		/// <returns>
		/// True = The touchPosition was handled by this control
		/// False = The touchPosition was not handled by this control
		/// </returns>
		/// <param name='touchPosition' The location where the user touched the screen (and type of touch) />
		public bool CheckTouchCollision (ITouchPoint touch)
		{
			if (this.Bounds.Contains(touch.Position)) 
			{
				if (touch.TouchType == TouchType.FreeDrag && inDragMode == false)
				{
					// although the drag is in this control, it did not start here so we are ignoring it
					return false;
				}
				
				switch (touch.TouchType)
				{
					case TouchType.StartDrag:
						this.inDragMode = true;
						break;
					
					case TouchType.DragComplete:
						this.inDragMode = false;
						break;
				}
			}
			else if (this.inDragMode == true) 
			{
				// we are in drag mode, although the user has dragged outside this control
				if (touch.TouchType == TouchType.DragComplete) 
				{
					this.inDragMode = false;
				}
			}
			else 
			{
				return false;
			}
			
			this.HandleTouch(touch);
			
			return true;
		}

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public abstract void Draw (bool refreshDisplay);
		
		/// <summary>
		/// Handles a particular touch by the user
		/// </summary>
		/// <param name='touch'>
		/// The position and type of gesture/touch made
		/// </param>
		protected abstract void HandleTouch(ITouchPoint touchPosition);
		
		/// <summary>
		/// Draws the rectangle.
		/// </summary>
		/// <param name='paintRegion' The rectangluar region to paint />
		/// <param name='color' The color we wish to paint the rectangle/>
		protected void DrawRectangle(Rectangle paintRegion, Color color)
		{
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, paintRegion, color);
		}
		
		/// <summary>
		/// Draws the graphic.
		/// </summary>
		/// <param name='paintRegion' The rectangluar region to paint />
		/// <param name='graphicsSourceRegion' The region of the graphics texture map to get the image to paint from/>
		protected void DrawGraphic(ImageType imageType, Rectangle paintRegion)
		{
			this.graphicsDisplay.DrawGraphic(imageType, paintRegion, this.BackgroundColor); 
		}

		/// <summary>
		/// Blanks the entire control
		/// </summary>
		protected void Blank()
		{
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, this.Bounds, this.BackgroundColor);
		}
		
		/// <summary>
		/// Blanks this tool and then redraws the border round the edge.
		/// </summary>
		protected void BlankAndRedrawWithBorder()
		{
			if (this.BorderWidth == 0)
			{
				this.Blank();
			}
			else 
			{
				this.BlankAndRedrawWithBorder(this.Bounds);
			}
		}
		
		/// <summary>
		/// Blanks the specific rectangle and then redraws the border round the edge.
		/// </summary>
		/// <param name='redrawRectangle' The rectangular region that should be blanked and redrawn with the border/>
		protected void BlankAndRedrawWithBorder(Rectangle redrawRectangle)
		{
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, redrawRectangle, this.BorderColor); 
			
			Rectangle inBorderRectangle = new Rectangle(
				this.Bounds.X + this.BorderWidth,
				this.Bounds.Y + this.BorderWidth,
				this.Bounds.Width - (2 * this.BorderWidth),
				this.Bounds.Height - (2 * this.BorderWidth));
			
			this.graphicsDisplay.DrawGraphic(ImageType.EmptySquare, inBorderRectangle, this.BackgroundColor); 
		}
	}
}

