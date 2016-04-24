using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsServices.FileServices
{
    public static class FileManager
    {
        public static String GetSaveFileLocation(String filter)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = filter;
            dialog.Title = "Choose file to save";

            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        public static String GetOpenFileLocation(String filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = filter;
            dialog.Title = "Choose file to open";

            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        public static void SaveTextToFile(String filePath, IEnumerable<String> values)
        {
            File.WriteAllLines(filePath, values);

            if (MessageBox.Show(
                "Do you want to open saved file?",
                "Open file",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                System.Diagnostics.Process.Start(filePath);
        }

        public static String GetTextFromFile(String filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
