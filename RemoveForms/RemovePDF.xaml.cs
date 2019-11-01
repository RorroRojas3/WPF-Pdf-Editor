using IronPdf;
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
            PdfDocument pdf = new PdfDocument(_openFileDialog.FileName);
            _pdf = pdf;
            ShowPDFInformation();

        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = 0;
                foreach (var item in CheckBoxList.Items.OfType<CheckBox>())
                {
                    if (item.IsChecked == true)
                    {
                        _pdf.RemovePage(i);
                    }
                    i++;
                }

                var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(_openFileDialog.FileName);
                filePath += $"\\{fileName}-Removed.pdf";
                _pdf.SaveAs(filePath);
                Close();
                MessageBox.Show("Deleted selected pages from PDF!");
                
            }
            catch(Exception)
            {
                Close();
                MessageBox.Show("Could not delete pages from PDF.");
            }
        }
    }
}
