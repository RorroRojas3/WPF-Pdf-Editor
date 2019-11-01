using IronPdf;
using Microsoft.Win32;
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

namespace PDFEditor.SplitPDFForms
{
    /// <summary>
    /// Interaction logic for SplitPDF.xaml
    /// </summary>
    public partial class SplitPDF : Window
    {
        private OpenFileDialog _openFileDialog;
        private PdfDocument _pdf;
        public SplitPDF(OpenFileDialog openFileDialog)
        {
            _openFileDialog = openFileDialog;
            _pdf = new PdfDocument(_openFileDialog.FileName);
            InitializeComponent();
            ShowInformation();
        }

        private void ShowInformation()
        {
            Bitmap[] bitmaps = _pdf.ToBitmap(300);
            MemoryStream ms = new MemoryStream();
            bitmaps[0].Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            tempImage.Source = image;
        }
    }
}
