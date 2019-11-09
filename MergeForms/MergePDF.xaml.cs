using Microsoft.Win32;
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
using System.Linq;
using Path = System.IO.Path;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PDFEditor.MergeForms
{
    /// <summary>
    /// Interaction logic for MergePDF.xaml
    /// </summary>
    public partial class MergePDF : Window
    {
        private Dictionary<string, string> _pdf = new Dictionary<string, string>();
        public MergePDF()
        {
            InitializeComponent();
        }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PDFNameListBox.Items.Clear();

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.ShowDialog();

                foreach (var item in openFileDialog.FileNames)
                {
                    var fileName = Path.GetFileNameWithoutExtension(item);
                    RadioButton radioButton = new RadioButton
                    {
                        Content = fileName,
                    };
                    PDFNameListBox.Items.Add(radioButton);
                    _pdf.Add(fileName, item);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton selectedRadioBtn = PDFNameListBox.Items.OfType<RadioButton>().Where(x => x.IsChecked == true).FirstOrDefault();
                List<RadioButton> radioButtons = new List<RadioButton>();
                int index = PDFNameListBox.Items.IndexOf(selectedRadioBtn);
                if (index > 0)
                {
                    foreach (var item in PDFNameListBox.Items.OfType<RadioButton>())
                    {
                        radioButtons.Add(item);
                    }

                    RadioButton tempRadioBtn = new RadioButton();
                    tempRadioBtn = radioButtons[index - 1];
                    radioButtons[index - 1] = selectedRadioBtn;
                    radioButtons[index] = tempRadioBtn;

                    var selectedPDF = _pdf[selectedRadioBtn.Content.ToString()];
                    var tempPDF = _pdf[tempRadioBtn.Content.ToString()];
                    _pdf[tempRadioBtn.Content.ToString()] = selectedPDF;
                    _pdf[selectedRadioBtn.Content.ToString()] = tempPDF;

                    PDFNameListBox.Items.Clear();

                    foreach (var item in radioButtons)
                    {
                        PDFNameListBox.Items.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton selectedRadioBtn = PDFNameListBox.Items.OfType<RadioButton>().Where(x => x.IsChecked == true).FirstOrDefault();
                List<RadioButton> radioButtons = new List<RadioButton>();
                int index = PDFNameListBox.Items.IndexOf(selectedRadioBtn);
                if ((index != (PDFNameListBox.Items.Count - 1)) && (index < PDFNameListBox.Items.Count - 1))
                {
                    foreach (var item in PDFNameListBox.Items.OfType<RadioButton>())
                    {
                        radioButtons.Add(item);
                    }

                    RadioButton tempRadioBtn = new RadioButton();
                    tempRadioBtn = radioButtons[index + 1];
                    radioButtons[index + 1] = selectedRadioBtn;
                    radioButtons[index] = tempRadioBtn;
                    PDFNameListBox.Items.Clear();

                    var selectedPDF = _pdf[selectedRadioBtn.Content.ToString()];
                    var tempPDF = _pdf[tempRadioBtn.Content.ToString()];
                    _pdf[tempRadioBtn.Content.ToString()] = selectedPDF;
                    _pdf[selectedRadioBtn.Content.ToString()] = tempPDF;


                    foreach (var item in radioButtons)
                    {
                        PDFNameListBox.Items.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton selectedRadioBtn = PDFNameListBox.Items.OfType<RadioButton>().Where(x => x.IsChecked == true).FirstOrDefault();
                if (selectedRadioBtn != null)
                {
                    int index = PDFNameListBox.Items.IndexOf(selectedRadioBtn);
                    PDFNameListBox.Items.RemoveAt(index);
                    _pdf.Remove(selectedRadioBtn.Content.ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergePDFs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDocument pdf = new PdfDocument();
                foreach (var item in _pdf)
                {
                    var tempPDF = PdfReader.Open(item.Value, PdfDocumentOpenMode.Import);
                    foreach(var page in tempPDF.Pages)
                    {
                        pdf.AddPage(page);
                    }
                    tempPDF.Close();
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                desktopPath += "\\merged.pdf";
                pdf.Save(desktopPath);
                Close();
                MessageBox.Show("PDF merge was successfull!");
            }
            catch(Exception ex)
            {
                Close();
                MessageBox.Show($"Error occured: {ex.Message}");
            }
        }
    }
}
