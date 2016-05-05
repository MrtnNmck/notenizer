using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsServices.FileServices
{
    /// <summary>
    /// Managing files.
    /// </summary>
    public static class FileManager
    {

        #region Methods

        /// <summary>
        /// Gets loction to save file to.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static String GetSaveFileLocation(String filter)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = filter;
            dialog.Title = "Choose file to save";

            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        /// <summary>
        /// Gets location to open file from.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static String GetOpenFileLocation(String filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = filter;
            dialog.Title = "Choose file to open";

            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        /// <summary>
        /// Saves text to file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="values"></param>
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

        /// <summary>
        /// Gets text from file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String GetTextFromFile(String filePath)
        {
            return File.ReadAllText(filePath);
        }

        #endregion Methods

    }
}
