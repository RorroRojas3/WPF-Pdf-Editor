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

namespace PDFEditor.Helpers
{
    public static class ImagesAndPDFHelper
    {
        private static string[] ImagesExtensions = { ".jpg", ".png", ".gif" };

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
            var fileExtension = Path.GetExtension(openFileDialog.FileName);
            if (IsFileAnImage(fileExtension))
            {
                var dekstopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                var file = $"{dekstopPath}\\{fileName}.pdf";
                ImageToPdfConverter.ImageToPdf(openFileDialog.FileName).SaveAs(file);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openFileDialog"></param>
        /// <returns></returns>
        public static bool PDFToImage(OpenFileDialog openFileDialog, string fileExtension)
        {
            #region WATERMARK
            //var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            //int result = 0;

            //PdfFocus pdf = new PdfFocus();
            //pdf.OpenPdf(openFileDialog.FileName);

            //pdf.ImageOptions.Dpi = 300;
            //result = pdf.ToImage(desktopPath, fileName + fileExtension);

            //return result != 0 ? true : false;
            #endregion

            #region NOTCLEAR
            //var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //var fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            //var file = $"{desktopPath}\\{fileName}{fileExtension}";
            //MagickImageCollection images = new MagickImageCollection();
            //images.Read(openFileDialog.FileName);
            //IMagickImage verticalImages = images.AppendVertically();
            //switch(fileExtension)
            //{
            //    case ".png":
            //        verticalImages.Format = MagickFormat.Png;
            //        break;
            //    case ".jpeg":
            //        verticalImages.Format = MagickFormat.Jpeg;
            //        break;
            //    case ".gif":
            //        verticalImages.Format = MagickFormat.Gif;
            //        break;
            //}
            //verticalImages.Density = new Density(100);
            //verticalImages.Write(file);
            #endregion

            return true;
        }


    }
}
