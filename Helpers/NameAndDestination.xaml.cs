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
using Path = System.IO.Path;

namespace PDFEditor.Helpers
{
    /// <summary>
    /// Interaction logic for NameAndDestination.xaml
    /// </summary>
    public partial class NameAndDestination : Window
    {
        SaveFileDialog _saveFileDialog;
        string _destination;
        string _fileName;

        public NameAndDestination()
        {
            InitializeComponent();
            _saveFileDialog = new SaveFileDialog();
            _destination = string.Empty;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            _saveFileDialog.ShowDialog();
            _destination = _saveFileDialog.FileName;
            _fileName = Path.GetFileNameWithoutExtension(_saveFileDialog.FileName);
            Close();
        }

        public string GetDestination()
        {
            return _destination;
        }

        public string GetFileName()
        {
            return _fileName;
        }
    }
}
