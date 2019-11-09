using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PDFEditor.SplitPDFForms
{
    /// <summary>
    /// Interaction logic for ExtractPDF.xaml
    /// </summary>
    public partial class ExtractPDF : Window
    {
        private OpenFileDialog _openFileDialog;
        private PdfDocument _pdf;
        public ExtractPDF(OpenFileDialog openFileDialog)
        {
            _openFileDialog = openFileDialog;
            _pdf = PdfReader.Open(_openFileDialog.FileName, PdfDocumentOpenMode.Import);
            InitializeComponent();
            ShowPDFInformation();
        }

        /// <summary>
        ///     Loads information into Window
        /// </summary>
        private void ShowPDFInformation()
        {
            NumOfPages.Content = _pdf.PageCount;
            
            for(var i = 0; i < _pdf.PageCount; i++)
            {
                CheckBox checkBox = new CheckBox
                {
                    Content = $"Page {i + 1}"
                };
                CheckBoxList.Items.Add(checkBox);
            }
        }

        /// <summary>
        ///  Once extract PDF button clicked, it will extract selected pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtractPDFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Remove pages that not wanted 
                int i = 0;
                foreach (var item in CheckBoxList.Items.OfType<CheckBox>())
                {
                    if (item.IsChecked == false)
                    {
                        _pdf.Pages.RemoveAt(i);
                        i--;
                    }
                    i++;
                }

                // Save PDF
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                filePath += $"\\extracted.pdf";
                _pdf.Save(filePath);
                Close();
                MessageBox.Show("Extracted pages from PDF succesfully!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not extract pages from PDF.");
                MessageBox.Show($"Error occured: {ex.Message}");
                Close();
            }
        }
    }
}
