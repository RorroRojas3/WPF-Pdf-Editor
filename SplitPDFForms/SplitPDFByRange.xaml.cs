using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Linq;

namespace PDFEditor.SplitPDFForms
{
    /// <summary>
    /// Interaction logic for SplitPDF.xaml
    /// </summary>
    public partial class SplitPDFByRange : Window
    {
        private OpenFileDialog _openFileDialog;
        private PdfDocument _pdf;
        public SplitPDFByRange(OpenFileDialog openFileDialog)
        {
            _openFileDialog = openFileDialog;
            _pdf = PdfReader.Open(openFileDialog.FileName, PdfDocumentOpenMode.Import);
            InitializeComponent();
        }

        /// <summary>
        ///     Adds range to RangeTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRangeButton_Click(object sender, RoutedEventArgs e)
        {
            int fromPage = 0;
            int toPage = 0;
            if (string.IsNullOrEmpty(FromPageTextBox.Text) || string.IsNullOrEmpty(ToPageTextBox.Text))
            {
                MessageBox.Show("Please enter ranges!");
                return;
            }

            fromPage = int.Parse(FromPageTextBox.Text);
            toPage = int.Parse(ToPageTextBox.Text);

            if ((fromPage >= toPage) || (toPage > _pdf.PageCount) || (fromPage > _pdf.PageCount) || (toPage > _pdf.PageCount))
            {
                MessageBox.Show("Invalid input for starting/ending page!");
                return;
            }

            CheckBox checkBox = new CheckBox
            {
                Content = $"{fromPage}-To-{toPage}"
            };
            RangesListBox.Items.Add(checkBox);
        }

        /// <summary>
        ///     Deletes selected ranges
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRangeButton_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            
            foreach(var item in RangesListBox.Items.OfType<CheckBox>())
            {
                if (item.IsChecked == false)
                {
                    checkBoxes.Add(item);
                }
            }

            RangesListBox.Items.Clear();

            foreach(var item in checkBoxes)
            {
                RangesListBox.Items.Add(item);
            }
        }

        /// <summary>
        ///     Split PDF in all ranges
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitPDFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int fromPage = 0;
                int toPage = 0;
                int i = 0;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var tempfilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(_openFileDialog.FileName);
                tempfilePath += $"\\{fileName}-Removed-" + "{0}.pdf";
                string filePath = "";

                foreach (var item in RangesListBox.Items.OfType<CheckBox>())
                {
                    var value = item.Content.ToString().Split("-");
                    fromPage = int.Parse(value[0]) - 1;
                    toPage = int.Parse(value[2]) - 1;
                    PdfDocument newPdf = new PdfDocument();
                    filePath = string.Format(tempfilePath, i);
                    for (var j = fromPage; j <= toPage; j++)
                    {
                        newPdf.AddPage(_pdf.Pages[j]);
                    }
                    newPdf.Save(filePath);
                    i++;
                }

                Close();
                MessageBox.Show("PDF succesfully splitted into wanted ranges!");
            }
            catch(Exception ex)
            {
                Close();
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }
    }
}
