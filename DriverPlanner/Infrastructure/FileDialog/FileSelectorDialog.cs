using System.Windows.Forms;

namespace DriverPlanner.Infrastructure.FileDialog
{
	public static class FileSelectorDialog
	{
		
		public static string FileDiaolog()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = @"D:\Desktop\Labs\DriverPlanner\DriverPlanner\Resources\Images";
				openFileDialog.Filter = "Image Files (*.png, *.jpg)|*.png; *.jpg";
				openFileDialog.FilterIndex = 2;
				openFileDialog.RestoreDirectory = true;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					return openFileDialog.FileName.ToString();
				}
				return null;
			}
		}

	}
}
