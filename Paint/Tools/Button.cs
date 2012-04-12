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
		/// The image we are using to draw this button
		/// </summary>
		private ImageType imageType;
		
		/// <summary>
		/// Where do we want to draw the actual button image
		/// </summary>
		private Rectangle imageDestinationRectangle;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ExitButton"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the button />
		/// <param name='borderColor' The border color of the button />
		/// <param name='buttonBounds' The bounds of this control/tool />
		/// <param name='graphicsDisplay' The graphics texture map - contains the image for this button />
		/// <param name='imageType' Reference to the texture map of the image we are using for rendering />
		public Button (Color backgroundColor, Color borderColor, Rectangle buttonBounds, IGraphicsDisplay graphicsDisplay, ImageType imageType)
			: base(backgroundColor, borderColor, graphicsDisplay, buttonBounds) 
		{
			this.imageType = imageType;
			
			var graphicsRectangle = graphicsDisplay.SourceRectangleFromImageType(imageType);
			int xDiff = buttonBounds.Width - graphicsRectangle.Width;
			int yDiff = buttonBounds.Height - graphicsRectangle.Height;
			
			this.imageDestinationRectangle = new Rectangle(
				buttonBounds.X + xDiff / 2, 
				buttonBounds.Y + yDiff / 2,
				buttonBounds.Width - xDiff,
				buttonBounds.Height - yDiff);
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
			if (refreshDisplay == true) 
			{
				// Blank out everything - and then draw our graphic
				this.Blank();
				this.DrawGraphic(this.imageType, this.imageDestinationRectangle);
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
			this.OnButtonPressed(EventArgs.Empty);
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
	}
}

