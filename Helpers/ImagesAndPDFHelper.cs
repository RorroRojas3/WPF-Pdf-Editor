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
using Path = System.IO.Path;

namespace PDFEditor.Helpers
{
    
    public static class ImagesAndPDFHelper
    {
        private static string[] ImagesExtensions = { ".jpg", ".png", ".gif", ".tiff", ".bpm" };

        private static bool IsFileAnImage(string fileExtension)
        {
            return ImagesExtensions.Contains(fileExtension.ToLower());
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
                PdfDocument pdf = new PdfDocument();
                foreach (var item in openFileDialog.FileNames)
                {
                    var fileExtension = Path.GetExtension(item);
                    if (IsFileAnImage(fileExtension))
                    {
                        // Convert image to PDF
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);      
                        PdfPage page = pdf.Pages.Add();
                        XGraphics xGraphics = XGraphics.FromPdfPage(page);
                        XImage xImage = XImage.FromFile(item);
                        xGraphics.DrawImage(xImage, 0, 0, 612, 792);
                    }
                    else
                    {
                        return false;
                    }
                }
                
                // Get path where file will be saved
                NameAndDestination nameAndDestination = new NameAndDestination();
                nameAndDestination.ShowDialog();
                var destination = nameAndDestination.GetDestination();

                if (!string.IsNullOrEmpty(destination))
                {
                    destination += ".pdf";
                    pdf.Save(destination);
                    pdf.Close();
                    return true;
                }

                return false ;
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
                string filePath = string.Empty;
                NameAndDestination nameAndDestination = new NameAndDestination();
                nameAndDestination.ShowDialog();
                var destination = nameAndDestination.GetDestination();
                destination = Path.GetDirectoryName(destination);
                var fileName = nameAndDestination.GetFileName();
                fileName = destination + "\\" + fileName + "-{0}";

                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(destination))
                {
                    return false;
                }

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
                        filePath = string.Format(fileName, i) + $".{imageFormat.ToString().ToLower()}";
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
