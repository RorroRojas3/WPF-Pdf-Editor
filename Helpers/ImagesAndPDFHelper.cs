using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using PdfSharp;
using ImageMagick;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using iTextSharp;
using Ghostscript.NET.Rasterizer;
using System.Runtime.InteropServices;
using System.Reflection;
using Ghostscript.NET;
using Ghostscript.NET.Viewer;

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
        ///     Converts Image to a PDF
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
        ///     Converts Image to PDF
        /// </summary>
        /// <param name="openFileDialog"></param>
        /// <returns></returns>
        public static  bool PDFToImage(OpenFileDialog openFileDialog, ImageFormat imageFormat)
        {
            try
            {
                // Gets DLL of GhostScritp
                string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");

                // Get PDF file information
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                var tempFilePath = $"{desktopPath}\\{fileName}" + "-{0}." + $"{imageFormat.ToString().ToLower()}";
                string filePath = "";

                // Set DPI (User will eventually chose)
                int xDPI = 1200;
                int yDPI = 1200;

                // PDF to desired image(s)
                GhostscriptVersionInfo gsVersion = new GhostscriptVersionInfo(gsDllPath);
                using (GhostscriptRasterizer rasterizer = new GhostscriptRasterizer())
                {
                    rasterizer.Open(openFileDialog.FileName, gsVersion, false);

                    for (var i = 1; i <= rasterizer.PageCount; i++)
                    {
                        filePath = string.Format(tempFilePath, i);
                        Image img = rasterizer.GetPage(xDPI, yDPI, i);
                        img.Save(filePath, imageFormat);
                    }
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
