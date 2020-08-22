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
using System.Diagnostics;

namespace futurerestore_gui
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        String latestPath = "";

        private void btnExitRecovery_Click(object sender, RoutedEventArgs e)
        {
            runCommand("--exit-recovery");
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            if(!File.Exists("futurerestore.exe"))
            {
                MessageBox.Show("Make sure the \"futurerestore_gui.exe\" is in the same path as \"futurerestore.exe\" ! Exiting App..", "\"futurerestore.exe\" not found!", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }

        public void runCommand(String command)
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = "cmd.exe";
            p.Start();
            p.StandardInput.WriteLine("futurerestore.exe " + command);
            p.StandardInput.Flush();
            p.StandardInput.Close();

            p.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                Console.WriteLine(e.Data);
            });

            p.BeginOutputReadLine();

        }

        public void fileDialog(String fileName, String defaultExt, String filter)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = fileName; // Default file name
            dlg.DefaultExt = defaultExt; // Default file extension
            dlg.Filter = filter; // Filter files by extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            dlg.ShowDialog();

            latestPath = dlg.FileName;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {

            if(pathBBbox.Text != "" && pathBMbox.Text != "" && pathIPSWbox.Text != "" & pathSEPbox.Text != "" && pathSHSH2box.Text != "")
            {
                String cmd = "";

                cmd += "-t \"" + pathSHSH2box.Text + "\" ";
                cmd += "-p \"" + pathBMbox.Text + "\" ";
                cmd += "-m \"" + pathBMbox.Text + "\" ";

                if (noBBcheckBox.IsChecked == true)
                {
                    cmd += "--no-baseband ";
                }
                else if (latestBBbox.IsChecked == true)
                {
                    cmd += "--latest-baseband ";
                }
                else
                {
                    cmd += "-b \"" + pathBBbox.Text + "\" ";
                }

                if (latestSEPbox.IsChecked == true)
                {
                    cmd += "--latest-sep ";
                }
                else
                {
                    cmd += "-s \"" + pathSEPbox.Text + "\" ";
                }

                if (updateCheckBox.IsChecked == true)
                {
                    cmd += "--update ";
                }

                cmd += "\"" + pathIPSWbox.Text + "\"";

                runCommand(cmd);
            }
            else
            {
                MessageBox.Show("Please make sure that you've filled any TextBox.", "Could not Restore / Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //runCommand("-t \"" + pathSHSH2box.Text + "\" -s \"" + pathSEPbox.Text + "\" -b \"" + pathBBbox.Text + "\" -p \"" + pathBMbox.Text + "\" -m \"" + pathBMbox.Text + "\" \"" + pathIPSWbox.Text + "\"");
        }

        private void noBBcheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            if(noBBcheckBox.IsChecked == true)
            {
                MessageBox.Show("Only use this if your iDevice has no BaseBand technology", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                labelBBpath.Visibility = Visibility.Hidden;
                pathBBbox.Visibility = Visibility.Hidden;
                latestBBbox.Visibility = Visibility.Hidden;
            } 
            else if(noBBcheckBox.IsChecked == false)
            {
                labelBBpath.Visibility = Visibility.Visible;
                pathBBbox.Visibility = Visibility.Visible;
                latestBBbox.Visibility = Visibility.Visible;
            }
        }

        private void updateCheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            if(updateCheckBox.IsChecked == true)
            {
                btnRestore.Content = "Update";
            }
            else if(updateCheckBox.IsChecked == false)
            {
                btnRestore.Content = "Restore";
            }
        }

        private void latestBBbox_Clicked(object sender, RoutedEventArgs e)
        {
            if(latestBBbox.IsChecked == true)
            {
                labelBBpath.Visibility = Visibility.Hidden;
                noBBcheckBox.Visibility = Visibility.Hidden;
                pathBBbox.Visibility = Visibility.Hidden;
            }
            else if(latestBBbox.IsChecked == false)
            {
                labelBBpath.Visibility = Visibility.Visible;
                noBBcheckBox.Visibility = Visibility.Visible;
                pathBBbox.Visibility = Visibility.Visible;
            }
        }

        private void latestSEPbox_Clicked(object sender, RoutedEventArgs e)
        {
            if (latestSEPbox.IsChecked == true)
            {
                labelSEPpath.Visibility = Visibility.Hidden;
                pathSEPbox.Visibility = Visibility.Hidden;
            }
            else if (latestSEPbox.IsChecked == false)
            {
                labelSEPpath.Visibility = Visibility.Visible;
                pathSEPbox.Visibility = Visibility.Visible;
            }
        }

        private void pathIPSWbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            fileDialog("", ".ipsw", "IPSW Files | *.ipsw");

            pathIPSWbox.Text = latestPath;
        }

        private void pathBMbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            fileDialog("BuildManifest", ".plist", "Plist Files | *.plist");

            pathBMbox.Text = latestPath;
        }

        private void pathBBbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            fileDialog("", ".bbfw", "BBFW Files | *.bbfw");

            pathBBbox.Text = latestPath;
        }

        private void pathSEPbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            fileDialog("", ".im4p", "IM4P Files | *.im4p");

            pathSEPbox.Text = latestPath;
        }

        private void pathSHSH2box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            fileDialog("", ".shsh2", "SHSH2 Files | *.shsh2");

            pathSHSH2box.Text = latestPath;
        }
    }
}
