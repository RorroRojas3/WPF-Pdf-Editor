using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using IronPdf;
using PdfSharp;
using SautinSoft;
using ImageMagick;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using PdfSharpClass = PdfSharp.Pdf.PdfDocument;
using PDFSharpPdfPage = PdfSharp.Pdf.PdfPage;
using PdfSharp.Drawing;

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
                    PdfSharpClass pdf = new PdfSharpClass();
                    PDFSharpPdfPage page = pdf.Pages.Add();
                    XGraphics xGraphics = XGraphics.FromPdfPage(page);
                    XImage xImage = XImage.FromFile(openFileDialog.FileName);
                    xGraphics.DrawImage(xImage, 0, 0, page.Width, page.Height);
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
                PdfDocument pdf = new PdfDocument(openFileDialog.FileName);

                for (var i = 1; i <= pdf.PageCount; i++)
                {
                    var newBit = pdf.PageToBitmap(i);
                    var filePath = String.Format(tempFilePath, i);
                    newBit.Save(filePath, imageFormat);
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
