using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using PdfSharp;
using SautinSoft;
using ImageMagick;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using iTextSharp;


namespace PDFEditor.Helpers
{
    public static class ImagesAndPDFHelper
    {
        private static string[] ImagesExtensions = { ".jpg", ".png", ".gif", ".tiff", ".bpm" };

        private static bool IsFileAnImage(string fileExtension)
        {
            return ImagesExtensions.Contains(fileExtension);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openFileDialog"></param>
        /// <returns></returns>
        public static bool ImageToPDF(OpenFileDialog openFileDialog)
        {
            try
            {
                var fileExtension = Path.GetExtension(openFileDialog.FileName);
                if (IsFileAnImage(fileExtension))
                {
                    // Get the path for Dekstop
                    var dekstopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    var file = $"{dekstopPath}\\{fileName}.pdf";

                    // Convert image to PDF
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    PdfDocument pdf = new PdfDocument();
                    PdfPage page = pdf.Pages.Add();
                    XGraphics xGraphics = XGraphics.FromPdfPage(page);
                    XImage xImage = XImage.FromFile(openFileDialog.FileName);
                    page.Width = xImage.PixelWidth;
                    page.Height = xImage.PixelHeight;
                    xGraphics.DrawImage(xImage, 0, 0, xImage.PixelWidth, xImage.PixelHeight);
                    pdf.Save(file);
                    pdf.Close();
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openFileDialog"></param>
        /// <returns></returns>
        public static bool PDFToImage(OpenFileDialog openFileDialog, ImageFormat imageFormat)
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                var tempFilePath = $"{desktopPath}\\{fileName}" + "-{0}." + $"{imageFormat.ToString().ToLower()}";
                string filePath = "";
                int i = 1;
                MagickImageCollection image = new MagickImageCollection();
                MagickReadSettings readSettings = new MagickReadSettings();
                readSettings.Density = new Density(300);
                readSettings.BackgroundColor = MagickColors.White;
                image.Read(openFileDialog.FileName, readSettings);

                foreach(var item in image)
                {
                    filePath = string.Format(tempFilePath, i);
                    item.Write(filePath);
                    i++;
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
                return false;
            }
        }
    }
}
