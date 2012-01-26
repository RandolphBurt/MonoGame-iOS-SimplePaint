/// <summary>
/// Canvas.cs
/// Randolph Burt - January 2012
/// </summary>
namespace Paint
{
	using System;
	using System.Collections.Generic;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	/// <summary>
	/// The main class - Canvas - handles all the rendering and user interaction
	/// </summary>
	public class Canvas : ICanvas
	{
		/// <summary>
		/// Defines the height/ location of the picture on our screen
		/// </summary>
		private const int PictureHeight = 715;
		
		/// <summary>
		/// The color we will set the brush to start with
		/// </summary>
		private readonly Color StartColor = Color.Green;
		
		/// <summary>
		/// The maximum size of the brush
		/// </summary>
		private const int MaxBrushSize = 50;

		/// <summary>
		/// The minimum size of the brush
		/// </summary>
		private const int MinBrushSize = 1;
		
		/// <summary>
		/// The initial size of the brush
		/// </summary>
		private const int StartBrushSize = 10;
		
		/// <summary>
		/// Simply tracks whether this is the very first time we are drawing the canvas - if so then we need to draw everything on each control/tool.
		/// </summary>
		private bool initialDraw = true;
		
		/// <summary>
		/// The list of touchpoints (gesture type and location) made since the last update
		/// </summary>
		private List<Vector2> touchPoints = new List<Vector2>();
		
		/// <summary>
		/// All the tools we can use to assist us with our drawing
		/// </summary>
		private List<ICanvasToolTouch> canvasTools;
		
		/// <summary>
		/// The color setter tool
		/// </summary>
		private ColorSetter colorSetter;

		/// <summary>
		/// The SpriteBatch for all rendering
		/// </summary>
		private SpriteBatch spriteBatch;
		
		/// <summary>
		/// A simple transparent texture used for all drawing - we simply set the appropriate color.
		/// </summary>
		private Texture2D transparentSquareTexture;
				
		/// <summary>
		/// Initializes a new instance of the <see cref="Paint.Canvas"/> class.
		/// </summary>
		/// <param name='backgroundColor' The background color of the canvas />
		/// <param name='borderColor' The border color for all controls on the canvas />
		/// <param name='spriteBatch' The SpriteBatch object used for any rendering />
		/// <param name='transparentSquareTexture' The transparent texture used for all drawing - we just specify the color we want at the time />
		/// <param name='bounds' The bounds of this control/tool />
		public Canvas(Color backgroundColor, Color borderColor, SpriteBatch spriteBatch, Texture2D transparentSquareTexture, Rectangle bounds)
		{
			this.Color = StartColor;
			this.Brush = new Rectangle(0, 0, StartBrushSize, StartBrushSize);
			this.spriteBatch = spriteBatch;
			this.transparentSquareTexture = transparentSquareTexture;
			this.CreateCanvasTools(backgroundColor, borderColor, bounds);
		}

		/// <summary>
		/// Gets the current color being used for drawing
		/// </summary>
		public Color Color 
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the current brush being used for drawing
		/// </summary>
		public Rectangle Brush 
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Handles any user interaction.
		/// </summary>
		/// <param name='touchPointList'>
		/// The list of all gestures / locations touched by the user since the last update
		/// </param>		
		public void HandleTouchPoints(List<ITouchPoint> touchPointList)
		{
			foreach (var touch in touchPointList)
			{
				if (this.HandleTouchInCanvasTools(touch) == false)
				{
					if (touch.Position.Y < PictureHeight)
					{
						this.HandleTouchPicture(touch);
					}
				}
			}
		}
		
		/// <summary>
		/// Draw the latest updates to our image/render target.
		/// </summary>
		public void Draw()
		{
			/*
			 * Draw the tools with an Opque BlendState to ensure we overwrite the previous color completely
			 */
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
			
			this.colorSetter.Draw(this.initialDraw);
			
			foreach (var tool in this.canvasTools)
			{
				tool.Draw(this.initialDraw);
			}

			this.spriteBatch.End();
			
			/* 
			 * We use alpha blending when drawing our picture - this allows the user to reduce the alpha value (transparency)
			 * and then build up the color by drawing over the top of it.
			 * Also if painting one color over another then they will erge slightly - e.g. painting a red over a green will make a yellow.
			 * Other alternatives are Additive, Opaque and NonPremultiplied.
			 * Additive will eventually turn everything white the more you colour it.
			 * Opaque simply ignores the alpha value - hence useful when drawing controls as we want to completely replace the previous image
			 */			
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			
			this.DrawPicture();
			
			this.spriteBatch.End();
			
			this.initialDraw = false;
		}
		
		/// <summary>
		/// Updates our picture with any new points on screen that the user has touched
		/// </summary>
		private void DrawPicture()
		{
			int brushOffset = this.Brush.Width / 2;
			
			foreach (var touch in touchPoints) 
			{
				Rectangle rectangle = new Rectangle(
					(int)touch.X - brushOffset,
					(int)touch.Y - brushOffset,
					this.Brush.Width,
					this.Brush.Height);
				
				this.spriteBatch.Draw(this.transparentSquareTexture, rectangle, this.colorSetter.Color);
			}
			
			touchPoints = new List<Vector2>();
		}
		
		/// <summary>
		/// Handles any user interaction with the actual picture element of the screen.
		/// This is a case of adding it to our list of touch points that will need drawing onto the image when it comes to drawing
		/// </summary>
		/// <param name='touch'>
		/// The position and type of touch/gesture being made
		/// </param>
		private void HandleTouchPicture(ITouchPoint touch)
		{
			this.touchPoints.Add(touch.Position);
		}
		
		/// <summary>
		/// Handles any user interaction with the controls/toolds
		/// </summary>
		/// <returns>
		/// Was this touchPoint consumed/handled by one of the controls
		/// </returns>
		/// <param name='touch'>
		/// The position and type of touch/gesture being made
		/// </param>
		private bool HandleTouchInCanvasTools(ITouchPoint touch)
		{
			foreach (var tool in this.canvasTools)
			{
				if (tool.CheckTouchCollision(touch) == true)
				{
					// no need to check the other tools so exit now
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// Creates all our canvas tools.
		/// </summary>
		/// <param name='backgroundColor' The background color for each tool />
		/// <param name='borderColor' The border color for each control />
		/// <param name='bounds' The size of the iPad screen />
		private void CreateCanvasTools(Color backgroundColor, Color borderColor, Rectangle bounds)
		{
			this.canvasTools = new List<ICanvasToolTouch>();
			
			// Bush size selector
			BrushSizeSelector brushSizeSelector = new BrushSizeSelector(
				backgroundColor,
				borderColor,
				this.spriteBatch,
				this.transparentSquareTexture,
				new Rectangle(bounds.X, bounds.Height - 230, 70, 230),
				MinBrushSize, MaxBrushSize, StartBrushSize, this.StartColor);
			
			brushSizeSelector.BrushSizeChanged += (sender, e) => { 
				this.Brush = new Rectangle(0, 0, brushSizeSelector.BrushSize, brushSizeSelector.BrushSize);
			};

			this.canvasTools.Add(brushSizeSelector);			

			// Pre defined color pickers
			Color[] colorList = new Color[] { 
				Color.White, 
				Color.Black, 
				Color.Red, 
				Color.Green, 
				Color.Blue,
				Color.Yellow,
				Color.Cyan,
				Color.Pink, 
				Color.Orange
			};
			
			float colorPickerWidth = (float)(bounds.Width) / (float)(colorList.Length + 1);
			
			// ColorSetter - shows what colour the user has chosen
			colorSetter = new ColorSetter(
					borderColor,
					this.spriteBatch,
					this.transparentSquareTexture,
					new Rectangle(bounds.X, (int)((bounds.Height - 230) - colorPickerWidth), (int)colorPickerWidth, (int)colorPickerWidth),
					this.StartColor);

			// User defined color selector
			ColorSelector colorSelector = new ColorSelector(
				backgroundColor, 
				borderColor,
				this.spriteBatch,
				this.transparentSquareTexture,
				new Rectangle(bounds.X + 70, bounds.Height - 230, bounds.Width - 70, 230),
				StartColor);
			
			colorSelector.ColorChanged += (sender, e) => { 
				this.colorSetter.Color = colorSelector.Color;
				brushSizeSelector.Color = colorSelector.Color;
			};
			
			this.canvasTools.Add(colorSelector);


			// Pre defined color pickers
			for (int i = 0; i < colorList.Length; i++)
			{
				ColorPicker colorPicker = new ColorPicker(
					backgroundColor, 
					borderColor,
					this.spriteBatch,
					this.transparentSquareTexture,
					new Rectangle((int)(bounds.X + (colorPickerWidth * (i + 1))), (int)((bounds.Height - 230) - colorPickerWidth), (int)colorPickerWidth, (int)colorPickerWidth),
					colorList[i]);
				
				colorPicker.ColorSelected += (sender, e) => {
					colorSelector.Color = colorPicker.Color;
				};
				
				this.canvasTools.Add(colorPicker);
			}
		}
	}
}

