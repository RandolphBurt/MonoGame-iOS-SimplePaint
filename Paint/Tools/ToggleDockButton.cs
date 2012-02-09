/// <summary>
/// ToggleDockButton.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Toggle dock button.
	/// </summary>
	public class ToggleDockButton : CanvasToolTouchBase, IToggleDockButton, IToggleDock
	{
		/// <summary>
		/// The width of the arrow rectangle shapes
		/// </summary>
		private const int ArrowWidth = 5;
		
		/// <summary>
		/// Gap between the border and the arrow.
		/// </summary>
		private const int ArrowTopBottomMargin = 5;

		/// <summary>
		/// The rectangle where the middle of the arraw appears
		/// </summary>
		private Rectangle middleOfArrawRectangle;

		/// <summary>
		/// The rectangle to draw for the arrow point when the arrow is pointing up.
		/// </summary>
		private Rectangle arrowPointingUpRectangle;
		
		/// <summary>
		/// The rectangle to draw for the arrow point when the arrow is pointing down.
		/// </summary>
		private Rectangle arrowPointingDownRectangle;

		/// <summary>
		/// Keep track of the previous dock position so we know when to redraw
		/// </summary>
		private DockPosition previousDockPosition;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.DockingBar"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the gauge />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='startDockPosition' The initial DockPosition />
		public ToggleDockButton (Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds,
		                   DockPosition startDockPosition) 
			: base(backgroundColor, borderColor, spriteBatch, transparentSquareTexture, bounds)
		{
			this.DockPosition = previousDockPosition = startDockPosition;

			this.middleOfArrawRectangle = new Rectangle(
				bounds.X + (bounds.Width - ArrowWidth) / 2,
				bounds.Y + StandardBorderWidth + ArrowTopBottomMargin,
				ArrowWidth,
				bounds.Height - ((StandardBorderWidth + ArrowTopBottomMargin) * 2));
			
			this.arrowPointingUpRectangle = new Rectangle(
				this.middleOfArrawRectangle.X - ArrowWidth,
				this.middleOfArrawRectangle.Top + ArrowWidth,
				ArrowWidth * 3,
				ArrowWidth);

			this.arrowPointingDownRectangle = new Rectangle(
				this.middleOfArrawRectangle.X - ArrowWidth,
				(this.middleOfArrawRectangle.Top + this.middleOfArrawRectangle.Height) - (2 *ArrowWidth),
				ArrowWidth * 3,
				ArrowWidth);
		}

		/// <summary>
		/// Gets the dock position.
		/// </summary>
		public DockPosition DockPosition 
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Occurs when the dock position is changed.
		/// </summary>
		public event EventHandler DockPositionChanged;

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public override void Draw(bool refreshDisplay)
		{		
			if (refreshDisplay == true || this.DockPosition != this.previousDockPosition) 
			{
				// Blank the entire control
				this.BlankAndRedrawWithBorder();

				/* If the dockPosition is currently at the top then we will draw a down arrow to signify that the user can press this to 
				 * move the control to the bottom.
				 * Likewise if the dockPosition is currently at the bottom then we will draw an up arrow.
				 */
				
				// Draw the middle of the arrow which never changes
				this.DrawRectangle(middleOfArrawRectangle, this.borderColor);
				
				switch (this.DockPosition)
				{
					case Paint.DockPosition.Top:
						this.DrawRectangle(this.arrowPointingDownRectangle, this.borderColor);
						break;
	
					case Paint.DockPosition.Bottom:
						this.DrawRectangle(this.arrowPointingUpRectangle, this.borderColor);
						break;
				}
				
				this.previousDockPosition = this.DockPosition;
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
			/* We just check for the initial touch - not for subsequent drags otherwise we'll be continually toggling
			 * between each dock position before the screen is refreshed
			 */
			if (touchPosition.TouchType == TouchType.StartDrag || touchPosition.TouchType == TouchType.Tap)
			{
				if (this.DockPosition == DockPosition.Bottom) 
				{
					this.DockPosition = DockPosition.Top;
				}
				else 
				{
					this.DockPosition = DockPosition.Bottom;
				}

				this.OnDockPositionChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the dock position changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (Should be EventArgs.Empty)
		/// </param>
		protected virtual void OnDockPositionChanged(EventArgs e)
		{
			if (this.DockPositionChanged != null)
			{
				this.DockPositionChanged(this, EventArgs.Empty);
			}
		}
	}
}

