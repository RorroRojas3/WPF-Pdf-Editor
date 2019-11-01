using IronPdf;
using Microsoft.Win32;
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
            _pdf = new PdfDocument(openFileDialog.FileName);
            InitializeComponent();
            ShowPDFInformation();
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtractPDFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = 0;
                foreach (var item in CheckBoxList.Items.OfType<CheckBox>())
                {
                    if (item.IsChecked == false)
                    {
                        index = CheckBoxList.Items.IndexOf(item);
                        _pdf.RemovePage(index);
                    }
                }

                var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                filePath += $"\\extracted.pdf";
                var newPDF = _pdf.SaveAs(filePath);
                Close();

                if (newPDF != null)
                {
                    MessageBox.Show("Extracted pages from PDF succesfully!");
                }
                else
                {
                    MessageBox.Show("Could not extract pages from PDF.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
                Close();
            }
        }
    }
}
