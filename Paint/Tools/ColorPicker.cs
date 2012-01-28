/// <summary>
/// ColorPicker.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// Color picker - Simple tool that has a preset color - the user can select this tool in order to start using that color
	/// </summary>
	public class ColorPicker : CanvasToolTouchBase, IColorPicker
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.ColorPicker"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the ColorPicker />
		/// <param name='borderColor' The border color of the ColorPicker />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		/// <param name='color' The color this tool represents />
		public ColorPicker (Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds) 
			: base(backgroundColor, borderColor, spriteBatch, transparentSquareTexture, bounds) 
		{
		}
		
		/// <summary>
		/// Gets the color this tool represents
		/// </summary>
		public Color Color 
		{ 
			get 
			{
				return this.backgroundColor;
			}
		}
		
		/// <summary>
		/// Occurs when this color is selected.
		/// </summary>
		public event EventHandler ColorSelected;
	
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
			}
		}
		
		/// <summary>
		/// Handles a particular touch by the user
		/// </summary>
		/// <param name='touch'>
		/// The position and type of gesture/touch made
		/// </param>
		protected override void HandleTouch(ITouchPoint touchPosition)
		{
			this.OnColorSelected(EventArgs.Empty);
		}
		
		/// <summary>
		/// Raises the color selected event.
		/// </summary>
		/// <param name='e'>
		/// Any relevant EventArgs (Should be EventArgs.empty)
		/// </param>
		protected virtual void OnColorSelected(EventArgs e)
		{
			if (this.ColorSelected != null) 
			{
				this.ColorSelected(this, EventArgs.Empty);
			}
		}
	}
}

