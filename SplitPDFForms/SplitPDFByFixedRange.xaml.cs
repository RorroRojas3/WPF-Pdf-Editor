using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
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

namespace PDFEditor.SplitPDFForms
{
    /// <summary>
    /// Interaction logic for SplitPDFByFixedRange.xaml
    /// </summary>
    public partial class SplitPDFByFixedRange : Window
    {
        private OpenFileDialog _openFileDialog;
        private PdfDocument _pdf;

        public SplitPDFByFixedRange(OpenFileDialog openFileDialog)
        {
            InitializeComponent();
            _openFileDialog = openFileDialog;
            _pdf = PdfReader.Open(_openFileDialog.FileName, PdfDocumentOpenMode.Import);
            ShowInformation();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowInformation()
        {
            TotalNumOfPages.Content = _pdf.PageCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitPDFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int successParse;
                var range = SplitInRangesOfTextBox.Text;

                if (string.IsNullOrEmpty(range) || string.IsNullOrWhiteSpace(range))
                {
                    MessageBox.Show("Please enter a valid range.");
                    return;
                }

                var parsedRange = int.TryParse(range, out successParse);
                if (!parsedRange)
                {
                    MessageBox.Show("Only numbers can be entered in the range.");
                    return;
                }

                var totalRange = int.Parse(range);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var tempfilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(_openFileDialog.FileName);
                tempfilePath += $"\\{fileName}-FixedRange-" + "{0}.pdf";
                string filePath = "";

                int k = 0;
                for(var i = 1; i <= _pdf.PageCount / totalRange; i++)
                {
                    PdfDocument newPdf = new PdfDocument();
                    for (var j = k; j < (i * totalRange); j++)
                    {
                        newPdf.AddPage(_pdf.Pages[j]);
                    }
                    filePath = string.Format(tempfilePath, i);
                    newPdf.Save(filePath);
                    k = i * totalRange;
                }

                Close();
                MessageBox.Show("Succesfully split PDF by fixed range!");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
                Close();
            }
        }
    }
}
