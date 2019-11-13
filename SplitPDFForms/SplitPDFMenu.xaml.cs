using Microsoft.Win32;
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

namespace PDFEditor.SplitPDFForms
{
    /// <summary>
    /// Interaction logic for SplitPDF.xaml
    /// </summary>
    public partial class SplitPDFMenu : Window
    {
        public SplitPDFMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Extract PDF pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtractPDFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                ExtractPDF extractPDF = new ExtractPDF(openFileDialog);
                extractPDF.Show();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
                Close();
            }
        }

        /// <summary>
        ///     Split PDF by range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitPDFByRangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                SplitPDFByRange splitPDF = new SplitPDFByRange(openFileDialog);
                splitPDF.Show();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
                Close();
            }
        }
    }
}
