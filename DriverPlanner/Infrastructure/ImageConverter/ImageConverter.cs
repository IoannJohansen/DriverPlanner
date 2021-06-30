using System.IO;
using System.Windows.Media.Imaging;

namespace DriverPlanner.Infrastructure.ImageConverter
{
	public static class ImageConverter
	{
		public static byte[] ImageToBytes(string filePath)
		{
			byte[] imgInBytes = null;
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			BinaryReader br = new BinaryReader(fs);
			imgInBytes = br.ReadBytes((int)fs.Length);
			return imgInBytes;
		}

	}
}
