using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WraptorPrinterTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> made;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            for(int i = 0; i < 100; i++)
            {
                list.Add(i.ToString());
            }
            cbNum.ItemsSource = list;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            made = new List<string>();
            for (int i = 0; i < cbNum.SelectedIndex; i++)
            {
                string x = getRandNum(12);
                made.Add(x.Trim());
                while (!print(tbName.Text, x)) { }
                DateTime dt = DateTime.Now.AddSeconds(3);
                while (dt > DateTime.Now) { }
            }
        }

        private bool print(string text, string num)
        {
            StreamWriter str = File.AppendText(@"C:\Users\bdill\Desktop\TempFile\label" + num+".csv");
            str.Write(text + "," + num + "," + num);
            str.Close();
            File.Copy(@"C:\Users\bdill\Desktop\TempFile\label" + num + ".csv", @"C:\Users\bdill\Desktop\PrintFile\label" + num + ".csv");
            DateTime dt = DateTime.Now.AddSeconds(5);
            while (dt > DateTime.Now) { }
            try
            {
                if (File.Exists(@"C:\Users\bdill\Desktop\label" + num + ".csv"))
                {
                    File.Delete(@"C:\Users\bdill\Desktop\label" + num + ".csv");
                }
                else
                {
                    if(File.Exists(@"C:\Users\bdill\Desktop\PrintFile\Error\label" + num + ".csv"))
                    {
                        MessageBox.Show("Printer returned error, please see EEPT for help", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return false;
                    }
                    else MessageBox.Show("Error deletig file, printer failed to print", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if (File.Exists(@"C:\Users\bdill\Desktop\TempFile\label" + num + ".csv"))
                {
                    File.Delete(@"C:\Users\bdill\Desktop\TempFile\label" + num + ".csv");
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Error deletig file, printer failed to print.\n" + e.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public string getRandNum(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        private void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            List<string> scanned = new List<string>();
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(@"C:\Users\bdill\Desktop\verify.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    scanned.Add(line.Trim());
                }
      
            }
            if(scanned.Count != made.Count)
            {
                MessageBox.Show("Not the same length", "", MessageBoxButton.OK);
                return;
            }
            for(int i = 0; i < scanned.Count; i++)
            {
                if (scanned[i] != made[i])
                {
                    MessageBox.Show("Not equal!\n"+ scanned[i] +"!="+ made[i], "", MessageBoxButton.OK);
                    return;
                }
            }
            MessageBox.Show("Same", "", MessageBoxButton.OK);
            return;
        }
    }
}
