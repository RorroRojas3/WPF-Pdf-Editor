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
using PdfSharp.Pdf.IO;
using org.pdfclown.documents;
using org.pdfclown.files;
using org.pdfclown.tools;
using File = org.pdfclown.files.File;

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
                var tempFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                tempFilePath += $"\\{fileName}" + "-{0}" + $".{imageFormat.ToString().ToLower()}";

                File file = new File(openFileDialog.FileName);

                Document document = file.Document;
                Pages pages = document.Pages;


                for (var i = 0; i < pages.Count; i++)
                {
                    Page page = pages[i];
                    Renderer renderer = new Renderer();
                    var image = renderer.Render(page, new SizeF(1400, 850));
                    var filePath = string.Format(tempFilePath, i);
                    image.Save(filePath, imageFormat);
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
