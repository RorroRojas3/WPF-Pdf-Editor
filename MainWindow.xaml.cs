using Microsoft.Win32;
using PDFEditor.Helpers;
using PDFEditor.MergeForms;
using PDFEditor.PDFToImageForms;
using PDFEditor.RemoveForms;
using PDFEditor.SplitPDFForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;


namespace PDFEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageToPDF_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool result = false;
            
            if (openFileDialog.ShowDialog() == true)
            {
                result = ImagesAndPDFHelper.ImageToPDF(openFileDialog);
            }

            if(result)
            {
                MessageBox.Show("Image succesfully converted to PDF!");
            }
            else
            {
                MessageBox.Show("Image could not be converted to PDF, formats supported are JPEG, PNG, and GIF.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PDFToImage_Click(object sender, RoutedEventArgs e)
        {
            PDFToImage imageSelection = new PDFToImage();
            imageSelection.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePages_Click(object sender, RoutedEventArgs e)
        {
            RemovePDF removePage = new RemovePDF();
            removePage.Show();
        }

        private void MergePDFs(object sender, RoutedEventArgs e)
        {
            MergePDF merge = new MergePDF();
            merge.Show();
        }

        private void SplitPDF_Click(object sender, RoutedEventArgs e)
        {
            SplitPDFMenu split = new SplitPDFMenu();
            split.Show();
            
        }
    }
}
