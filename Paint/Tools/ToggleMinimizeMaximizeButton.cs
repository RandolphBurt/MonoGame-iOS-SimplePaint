/// <summary>
/// ToggleMinimizeMaximizeButton.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Toggle minimize maximize button.
	/// </summary>
	public class ToggleMinimizeMaximizeButton : CanvasToolTouchBase, IToggleMinimizeMaximizeButton, IToggleMinimizeMaximize
	{
		/// <summary>
		/// The width of the plus/minus signs
		/// </summary>
		private const int SymbolWidth = 5;

		/// <summary>
		/// Gap between the border and the plus/minus sign.
		/// </summary>
		private const int SymbolMargin = 5;

		/// <summary>
		/// The horizontal rectangle to draw in the minMax square representing the 'minus' for the minimize
		/// </summary>
		private Rectangle horizontalMinMaxRectangle;
		
		/// <summary>
		/// The vertical rectangle to draw in the minMax square representing the 'plus' for the maximize
		/// </summary>
		private Rectangle verticalMinMaxRectangle;

		/// <summary>
		/// Keep track of the previous min/max state so we know when to redraw
		/// </summary>
		private MinimizedMaximizedState previousMinimizedMaximizedState;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.DockingBar"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the gauge />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='startDockPosition' The initial DockPosition />
		/// <param name='startMinimizedMaximizedState' The initial minimized/maximized state />
		public ToggleMinimizeMaximizeButton (Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds,
		                   MinimizedMaximizedState startMinimizedMaximizedState) 
			: base(backgroundColor, borderColor, spriteBatch, transparentSquareTexture, bounds) 
		{
			this.MinimizedMaximizedState = previousMinimizedMaximizedState = startMinimizedMaximizedState;
			
			this.verticalMinMaxRectangle = new Rectangle(
				bounds.X + (bounds.Width - SymbolWidth) / 2,
				bounds.Y + StandardBorderWidth + SymbolMargin,
				SymbolWidth,
				bounds.Height - ((StandardBorderWidth + SymbolMargin) * 2));

			this.horizontalMinMaxRectangle = new Rectangle(
				bounds.X + StandardBorderWidth + SymbolMargin,
				bounds.Y + (bounds.Height - SymbolWidth) / 2,
				bounds.Width - ((StandardBorderWidth + SymbolMargin) * 2),
				SymbolWidth);
		}

		/// <summary>
		/// Gets the minimized/maximized state of the tool
		/// </summary>
		public MinimizedMaximizedState MinimizedMaximizedState 
		{
			get;
			private set;
		}

		/// <summary>
		/// Occurs when the state is changed to/from minimized from/to maximized
		/// </summary>
		public event EventHandler MinimizeMaximizeStateChanged;

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public override void Draw(bool refreshDisplay)
		{		
			if (refreshDisplay == true || this.MinimizedMaximizedState != this.previousMinimizedMaximizedState) 
			{
				// Blank out everything 
				this.BlankAndRedrawWithBorder();
				
				/* If we are currently minimized then we will draw the maximise 'plus' to signify that the user can press this to
				 * maximise the contorl.
				 * Likewise if the state is currently maximized then we will draw the minus sign
				 */
				
				if (this.MinimizedMaximizedState == MinimizedMaximizedState.Minimized)
				{
					// Draw the vertical part of the plus
					this.DrawRectangle(this.verticalMinMaxRectangle, this.borderColor);
				}
				
				// Draw the minus sign (or of course horizontal part of the plus)
				this.DrawRectangle(this.horizontalMinMaxRectangle, this.borderColor);
				
				this.previousMinimizedMaximizedState = this.MinimizedMaximizedState;
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
			 * between each min/max position before the screen is refreshed
			 */
			if (touchPosition.TouchType == TouchType.StartDrag || touchPosition.TouchType == TouchType.Tap)
			{
				if (this.MinimizedMaximizedState == MinimizedMaximizedState.Maximized) 
				{
					this.MinimizedMaximizedState = MinimizedMaximizedState.Minimized;
				}
				else 
				{
					this.MinimizedMaximizedState = MinimizedMaximizedState.Maximized;
				}

				this.OnMinimizeMaximizeStateChanged(EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises the minimize maximize state changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (Should be EventArgs.Empty)
		/// </param>
		protected virtual void OnMinimizeMaximizeStateChanged(EventArgs e)
		{
			if (this.MinimizeMaximizeStateChanged != null) 
			{
				this.MinimizeMaximizeStateChanged(this, EventArgs.Empty);
			}
		}	
	}
}

