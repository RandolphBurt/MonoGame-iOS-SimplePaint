namespace ConvertImageDataFilesToRetina
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class MainClass
	{
		private static string DataFile = "DATA.INF";

		private static int PlayBackFileHeaderSize = 12;

		private static int PlayBackFileBytesPerCommand = 5;

		/// <summary>
		/// Constant for indicating we should paint at the given position.
		/// This is the beginning of the user dragging their finger to paint a line
		/// </summary>
		private const byte CanvasRecorderCommandStartDrag = 0;
		
		/// <summary>
		/// Constant for indicating we should paint at the given position.
		/// This is the end of the user dragging their finger to paint a line
		/// </summary>
		private const byte CanvasRecorderCommandDragComplete = 1;
		
		/// <summary>
		/// Constant for indicating we should paint at the given position.
		/// This is during the process of the user dragging their finger to paint a line
		/// </summary>
		private const byte CanvasRecorderCommandFreeDrag = 2;
		
		/// Constant for indicating we should paint at the given position.
		/// This is the user tapping their fingeron the screen to draw a single dot
		/// </summary>
		private const byte CanvasRecorderCommandTap = 3;
		
		/// <summary>
		/// Constant for indicating the colour has changed
		/// </summary>
		private const byte CanvasRecorderCommandSetColor = 4;
		
		/// <summary>
		/// Constant for indicating the brush size has changed.
		/// </summary>
		private const byte CanvasRecorderCommandSetBrushSize = 5;


		public static void Main(string[] args)
		{
			string dataFolder = "TestData";

			if (args.Length == 1)
			{
				dataFolder = args[0];
			}

			var backupFolder = Path.Combine(dataFolder, "BACKUP");
			Directory.CreateDirectory(backupFolder);

			ConvertDataFile(dataFolder, backupFolder);

			var playbackFiles = Directory.GetFiles(dataFolder, "*.REC");
			foreach (var file in playbackFiles)
			{
				var backupFile = Path.Combine(backupFolder, Path.GetFileName(file));
				File.Move(file, backupFile);
				ConvertPlaybackFile(backupFile, file);
			}
		}

		private static void ConvertPlaybackFile(string inFile, string outFile)
		{
			using (var inFileStream = File.OpenRead(inFile))
			{
				using (var outFileStream = File.OpenWrite(outFile))
				{
					byte[] header = new byte[PlayBackFileHeaderSize];
					inFileStream.Read(header, 0, PlayBackFileHeaderSize);
					outFileStream.Write(header, 0, PlayBackFileHeaderSize);

					byte[] command = new byte[PlayBackFileBytesPerCommand];
					while (inFileStream.Position < inFileStream.Length)
					{
						inFileStream.Read(command, 0, PlayBackFileBytesPerCommand);

						switch (command[0])
						{
							case CanvasRecorderCommandSetColor:
								outFileStream.Write(command, 0, PlayBackFileBytesPerCommand);
								break;

							case CanvasRecorderCommandSetBrushSize:
								outFileStream.Write(ConvertBrshSize(command), 0, PlayBackFileBytesPerCommand);
								break;
								
							case CanvasRecorderCommandTap:
							case CanvasRecorderCommandStartDrag:
							case CanvasRecorderCommandFreeDrag:
							case CanvasRecorderCommandDragComplete:
							default:
								outFileStream.Write(ConvertDraw(command), 0, PlayBackFileBytesPerCommand);
								break;
						}
					}
				}
			}
		}

		private static byte[] ConvertDraw(byte[] commandByteArray)
		{
			int positionX = commandByteArray[1] | commandByteArray[2] << 8;
			int positionY = commandByteArray[3] | commandByteArray[4] << 8;

			positionX *= 2;
			positionY *= 2;

			return new byte[] 
			{
				commandByteArray[0],
				(byte) positionX,
				(byte) (positionX >> 8),
				(byte) positionY,
				(byte) (positionY >> 8),
			};
		}

		private static byte[] ConvertBrshSize(byte[] commandByteArray)
		{
			int brushSize =  
				commandByteArray[1] |
				commandByteArray[2] << 8 |
				commandByteArray[3] << 16 |
				commandByteArray[4] << 24;

			brushSize *= 2;

			return new byte[] 
			{
				commandByteArray[0],
				(byte) brushSize,
				(byte) (brushSize >> 8),
				(byte) (brushSize >> 16),
				(byte) (brushSize >> 24),
			};
		}

		private static void ConvertDataFile(string dataFolder, string backupFolder)
		{
			// Read contents of the DATA.INF file
			var dataInfFile = Path.Combine(dataFolder, DataFile);
			var imageData = ReadImageData(dataInfFile);
			
			// Backup the existing DATA.INF file
			File.Move(dataInfFile, Path.Combine(backupFolder, DataFile));
			
			// Double the width and height
			imageData[0] *= 2;
			imageData[1] *= 2;
			
			// Write out the data file
			WriteImageData(dataInfFile, imageData);
		}

		private static void WriteImageData(string outFile, int[] data)
		{
			using (var stream = File.Open(outFile, FileMode.Create, FileAccess.Write))
			{
				foreach (int val in data)
				{
					stream.WriteByte((byte)val);
					stream.WriteByte((byte)(val >> 8));
					stream.WriteByte((byte)(val >> 16));
					stream.WriteByte((byte)(val >> 24));
				}
			}
		}

		private static int[] ReadImageData(string inFile)
		{
			var dataList = new List<int>();
			
			using (var stream = File.OpenRead(inFile))
			{
				for (short count = 0; count < 6; count++)
				{
					dataList.Add(
						stream.ReadByte() |
						(stream.ReadByte()) << 8 |
						(stream.ReadByte()) << 16 |
						(stream.ReadByte()) << 24
						);
				}
			}

			return dataList.ToArray();
		}
	}
}
