using Microsoft.Win32;
using PDFEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PDFEditor.PDFToImageForms
{
    /// <summary>
    /// Interaction logic for PDFToImageSelection.xaml
    /// </summary>
    public partial class PDFToImage : Window
    {
        public PDFToImage()
        {
            InitializeComponent();
        }

        private void PDFToImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool result = false;

            if (openFileDialog.ShowDialog() == true)
            {
                if (PDFToPNG.IsChecked == true)
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ImageFormat.Png);
                }
                else if (PDFToJPEG.IsChecked == true)
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ImageFormat.Jpeg);
                }
                else if (PDFToGIF.IsChecked == true)
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ImageFormat.Gif);
                }
                else if (PDFToTIFF.IsChecked == true)
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ImageFormat.Tiff);
                }
                else
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ImageFormat.Bmp);
                }
            }

            if(result)
            {
                MessageBox.Show("Succesfully converted PDF to image");
            }
            else
            {
                MessageBox.Show("Could not convert PDF to image");
            }

            Close();
        }
    }
}
