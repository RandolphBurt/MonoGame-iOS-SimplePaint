/// <summary>
/// ExitButton.cs
/// Randolph Burt - March 2012
/// </summary>
namespace Paint
{
	using System;

	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework;

	/// <summary>
	/// An Exit button toolbox item
	/// </summary>
	public class ExitButton : CanvasToolTouchBase, IExitButton
	{
		/// <summary>
		/// The color of the cross.
		/// </summary>
		private readonly Color CrossColor = Color.Red;
		
		/// <summary>
		/// The width of the blocks that make up the cross
		/// </summary>
		private const int CrossWidth = 4;
		
		/// <summary>
		/// Margin around the cross
		/// </summary>
		private const int CrossMargin = 5;
		
		/// <summary>
		/// X Position of the left of the cross
		/// </summary>
		private int crossLeft = 0;
		
		/// <summary>
		/// Y Position of the top of the cross
		/// </summary>
		private int crossTop = 0;
		
		/// <summary>
		/// The widht (and height) of the cross.
		/// </summary>
		private int crossWidthHeight = 0;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the colorSelector />
		/// <param name='borderColor' The border color of the colorSelector />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		public ExitButton (Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds)
			: base(backgroundColor, borderColor, spriteBatch, transparentSquareTexture, bounds) 
		{
			if (bounds.Width > bounds.Height)
			{
				this.crossWidthHeight = bounds.Height - (2 * (CrossMargin + StandardBorderWidth));
				this.crossTop = bounds.Y + CrossMargin + StandardBorderWidth;
				this.crossLeft = ((bounds.Width - (2 * (CrossMargin + StandardBorderWidth))) - this.crossWidthHeight) / 2;
			}
			else if (bounds.Height > bounds.Width)
			{
				this.crossWidthHeight = bounds.Width - (2 * (CrossMargin + StandardBorderWidth));
				this.crossLeft = bounds.X + CrossMargin + StandardBorderWidth;
				this.crossTop = ((bounds.Height - (2 * (CrossMargin + StandardBorderWidth))) - this.crossWidthHeight) / 2;
			}
			else
			{
				this.crossWidthHeight = bounds.Width - (2 * (CrossMargin + StandardBorderWidth));
				this.crossLeft = bounds.X + CrossMargin + StandardBorderWidth;
				this.crossTop = bounds.Y + CrossMargin + StandardBorderWidth;
			}
		}

		/// <summary>
		/// Occurs when the user has selected the Exit button
		/// </summary>
		public event EventHandler ExitSelected;

		/// <summary>
		/// Draw this tool on to the image
		/// </summary>
		/// <param name='refreshDisplay'>
		/// True = we should redraw the entire control
		/// False = just draw any updates 
		/// </param>
		public override void Draw(bool refreshDisplay)
		{	
			if (refreshDisplay == true) 
			{
				// Blank out everything 
				this.BlankAndRedrawWithBorder();
				this.DrawExitButton();
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
			this.OnExitSelected(EventArgs.Empty);
		}
		
		/// <summary>
		/// Raises the exit selected changed event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (should be EventArgs.Empty)
		/// </param>
		protected virtual void OnExitSelected(EventArgs e)
		{
			if (this.ExitSelected != null) 
			{
				this.ExitSelected(this, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Draws the actual exit button.
		/// </summary>
		private void DrawExitButton()
		{
			// The lower (nearer to one) the value then the more details for the cross
			int detailLevel = 1;
			int maxOffset = this.crossWidthHeight - CrossWidth;
			
			Rectangle crossRectangle = new Rectangle(0, 0, CrossWidth, CrossWidth);
			
			for (int offset = 0; offset <= maxOffset; offset += detailLevel)
			{
				crossRectangle.X = this.crossLeft + offset;
				
				// Draw the squares that make up the top-left -> bottom-right of the cross
				crossRectangle.Y = this.crossTop + offset;				
				this.DrawRectangle(crossRectangle, CrossColor); 					

				// Draw the squares that make up the bottom-left -> top-right of the cross
				crossRectangle.Y = (this.crossTop + this.crossWidthHeight - CrossWidth) - offset;
				this.DrawRectangle(crossRectangle, CrossColor); 					
			}
		}
	}
}

