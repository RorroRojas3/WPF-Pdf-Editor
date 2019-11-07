using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Linq;
using Path = System.IO.Path;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PDFEditor.RemoveForms
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RemovePDF : Window
    {
        private readonly OpenFileDialog _openFileDialog;
        private readonly PdfDocument _pdf;
        public RemovePDF()
        {
            InitializeComponent();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            _openFileDialog = openFileDialog;
            PdfDocument pdf = PdfReader.Open(openFileDialog.FileName);
            _pdf = pdf;
            ShowPDFInformation();

        }

        /// <summary>
        ///     Shows PDF information on Window
        /// </summary>
        private void ShowPDFInformation()
        {
            NumberOfPagesNumLabel.Content = _pdf.PageCount;
            
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
        ///     Removes selected pages from PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Remove pages
                int i = 0;
                foreach (var item in CheckBoxList.Items.OfType<CheckBox>())
                {
                    if (item.IsChecked == true)
                    {
                        _pdf.Pages.RemoveAt(i);
                        i--;
                    }
                    i++;
                }

                // Save new PDF
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(_openFileDialog.FileName);
                filePath += $"\\{fileName}-Removed.pdf";
                _pdf.Save(filePath);
                Close();
                MessageBox.Show("Deleted selected pages from PDF!");
                
            }
            catch(Exception ex)
            {
                Close();
                MessageBox.Show($"Error ocurred: {ex.Message}");
            }
        }
    }
}
