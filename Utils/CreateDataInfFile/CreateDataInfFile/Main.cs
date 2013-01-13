/// <summary>
/// Create a DATA.INF file (e.g. for default images included with the application)
/// Randolph Burt - Jan 2013
/// </summary>
namespace CreateDataInfFile
{
	using System;
	using System.IO;

	public class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length != 6)
			{
				Console.WriteLine("CreateDataInfFile <width> <height> <maxUndoRedoCount> <firstSavePoint> <lastSavePoint> <currentSavePoint>");
				return;
			}

			using (var stream = File.Open("DATA.INF", FileMode.Create, FileAccess.Write))
			{
				foreach (var arg in args)
				{
					int val = Convert.ToInt32(arg);

					stream.WriteByte((byte)val);
					stream.WriteByte((byte)(val >> 8));
					stream.WriteByte((byte)(val >> 16));
					stream.WriteByte((byte)(val >> 24));
				}
			}	
		}
	}
}
