/// <summary>
/// Device.cs
/// Randolph Burt - Feb 2013
/// </summary>
namespace Paint
{
	using MonoTouch.UIKit;

	/// <summary>
	/// Device information
	/// </summary>
	public static class Device
	{
		public const int OSVersion43 = 400300;
		public const int OSVersion50 = 500000;
		public const int OSVersion51 = 500100;
		public const int OSVersion60 = 600000;
		public const int OSVersion61 = 600100;

		/// <summary>
		/// Gets the OS version.
		/// </summary>
		/// <value>The OS version.</value>
		public static int OSVersion
		{
			get
			{
				switch (UIDevice.CurrentDevice.SystemVersion)
				{
					case "4.3":
						return OSVersion43;
					case "5.0":
						return OSVersion50;
					case "5.1":
						return OSVersion51;
					case "6.0":
						return OSVersion60;
					case "6.1":
					default:
						return OSVersion61;
				}
			}
		}
	}
}

