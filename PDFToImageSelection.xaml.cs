using Microsoft.Win32;
using PDFEditor.Helpers;
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

namespace PDFEditor
{
    /// <summary>
    /// Interaction logic for PDFToImageSelection.xaml
    /// </summary>
    public partial class PDFToImageSelection : Window
    {
        public PDFToImageSelection()
        {
            InitializeComponent();
        }

        private void PDFToImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool result = false;

            if (openFileDialog.ShowDialog() == true)
            {
                if (PDFToPNG != null)
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ".png");
                }
                else if (PDFToJPEG != null)
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ".jpeg");
                }
                else
                {
                    result = ImagesAndPDFHelper.PDFToImage(openFileDialog, ".gif");
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
