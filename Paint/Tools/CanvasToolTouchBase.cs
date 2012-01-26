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
		/// The SpriteBatch used for drawing
		/// </summary>
		protected SpriteBatch spriteBatch;
		
		/// <summary>
		/// A simple transparent texture used for all drawing - simply set the color required at the time of drawing
		/// </summary>
		protected Texture2D transparentSquareTexture;
		
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
		/// The location where the user touched the screen (and type of touch)
		/// </returns>
		/// <param name='touchPosition'>
		/// True = The touchPosition was handled by this control
		/// False = The touchPosition was not handled by this control
		/// </param>
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
	}
}

