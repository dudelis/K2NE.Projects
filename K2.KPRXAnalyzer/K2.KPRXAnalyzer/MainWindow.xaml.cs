using System;
using System.Collections.Generic;
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
using System.Xml;
using KP = K2.KPRXAnalyzer.Process;

namespace K2.KPRXAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TrvProcess_Loaded()
        {
            TreeView TrvProcess = new TreeView();
            string Url = @"C:\!CustomDev\TestWorkflow.KPRX\TestWorkflow.KPRX\TestWorkflow.KPRX\Process1 - Copy.xml";
            List<KP.INode> ChildNodes = KP.Node.GetNodesWithExecItems(Url, "15");
        }

       
        //private void BtnPickFile_OnClick(object sender, RoutedEventArgs e)
        //{
        //    // Create OpenFileDialog 
        //    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        //    // Set filter for file extension and default file extension 
        //    dlg.DefaultExt = ".kprx";
        //    dlg.Filter = "KPRX Files (*.kprx)|*.kprx";
        //    //dlg.Filter = "KPRX Files (*.kprx)";


        //    // Display OpenFileDialog by calling ShowDialog method 
        //    Nullable<bool> result = dlg.ShowDialog();


        //    // Get the selected file name and display in a TextBox 
        //    if (result == true)
        //    {
        //        // Open document 
        //        string filename = dlg.FileName;
        //        lblFile.Content = filename;
        //    }
        //}


        private void BtnPickKPRX_OnClick(object sender, RoutedEventArgs e)
        {
             //Create OpenFileDialog 
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".kprx";
                dlg.Filter = "KPRX Files (*.kprx)|*.kprx";
                
                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();


                // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                tbxFileUrl.Text = filename;
            }
        }


        private void BtnSearchItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxFieldName.Text) || String.IsNullOrEmpty(tbxFileUrl.Text))
            {
                System.Windows.MessageBox.Show("Please, pick the Workflow File and/or enter the field name");
            }
            else
            {
                List<KP.INode> ChildNodes = KP.Node.GetNodesWithExecItems(tbxFileUrl.Text, tbxFieldName.Text);
                View.ItemsSource = ChildNodes;
            }
        }


        private void MnitemExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MnitemHelp_OnClick(object sender, RoutedEventArgs e)
        {
            wndAbout wnd = new wndAbout();
            wnd.Show();
           
        }
    }
}
