/// <summary>
/// CanvasToolTouchBase.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input.Touch;
	
	/// <summary>
	/// Base (Abstract) class for any tools that allow user interaction.
	/// </summary>
	public abstract class CanvasToolTouchBase : ICanvasToolTouch
	{
		/// <summary>
		/// Border size for drawing the tool on screen.
		/// </summary>
		protected const int StandardBorderWidth = 5;

		/// <summary>
		/// The background color of the tool
		/// </summary>
		protected Color backgroundColor;
		
		/// <summary>
		/// The color of the borders around the control.
		/// </summary>
		protected Color borderColor;	
		
		/// <summary>
		/// The location and size of the control.
		/// </summary>
		protected Rectangle bounds;
		
		/// <summary>
		/// A simple transparent texture used for all drawing - simply set the color required at the time of drawing
		/// </summary>
		protected Texture2D transparentSquareTexture;
		
		/// <summary>
		/// The SpriteBatch used for drawing
		/// </summary>
		private SpriteBatch spriteBatch;
		
		/// <summary>
		/// Small bit of state - determines whether the user is currently interacting with this control by dragging their finger across the iPad
		/// </summary>
		private bool inDragMode = false;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.CanvasToolTouchBase"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of this tool />
		/// <param name='borderColor' The border color of this tool />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		public CanvasToolTouchBase(Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds)
		{
			this.spriteBatch = spriteBatch;
			this.bounds = bounds;
			this.transparentSquareTexture = transparentSquareTexture;
			this.backgroundColor = backgroundColor;
			this.borderColor = borderColor;
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
			if (this.bounds.Contains(touch.Position)) 
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
		/// <param name='rectangle' The rectangluar region to paint />
		/// <param name='color' The color we wish to paint the rectangle/>
		protected void DrawRectangle(Rectangle rectangle, Color color)
		{
			this.spriteBatch.Draw(this.transparentSquareTexture, rectangle, color); 
		}
		
		/// <summary>
		/// Blanks this tool and then redraws the border round the edge.
		/// </summary>
		protected void BlankAndRedrawWithBorder()
		{
			this.BlankAndRedrawWithBorder(this.bounds);
		}
		
		/// <summary>
		/// Blanks the specific rectangle and then redraws the border round the edge.
		/// </summary>
		/// <param name='redrawRectangle' The rectangular region that should be blanked and redrawn with the border/>
		protected void BlankAndRedrawWithBorder(Rectangle redrawRectangle)
		{
			this.spriteBatch.Draw(this.transparentSquareTexture, redrawRectangle, this.borderColor); 
			
			Rectangle inBorderRectangle = new Rectangle(
				this.bounds.X + StandardBorderWidth,
				this.bounds.Y + StandardBorderWidth,
				this.bounds.Width - (2 * StandardBorderWidth),
				this.bounds.Height - (2 * StandardBorderWidth));
			
			this.spriteBatch.Draw(this.transparentSquareTexture, inBorderRectangle, this.backgroundColor); 
		}
	}
}

