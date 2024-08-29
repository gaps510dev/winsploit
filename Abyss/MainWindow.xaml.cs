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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit;
using Microsoft.Win32;
using SynapseZAPI;

namespace Abyss
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string abyssScriptsFolder;
        public SynapseZAPI.SynapseZAPI synapseZAPI = new SynapseZAPI.SynapseZAPI();
        public MainWindow()
        {
            InitializeComponent();
            abyssScriptsFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "abyssScripts");
            PopulateTreeView();
            string randomTitle = GenerateRandomString(12);
            this.Title = randomTitle;
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EditBTN.IsChecked = true;
            max.IsEnabled = false;
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NEW", "DEBUG");
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLOSE", "DEBUG");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void EditBTN_Checked(object sender, RoutedEventArgs e)
        {
            Storyboard s = (Storyboard)TryFindResource("EditSlide");
            s.Begin();

        }

        private void CloudBTN_Checked(object sender, RoutedEventArgs e)
        {
            Storyboard s = (Storyboard)TryFindResource("CloudSlide");
            s.Begin();
        }

        private void SetBTN_Checked(object sender, RoutedEventArgs e)
        {
            Storyboard s = (Storyboard)TryFindResource("SetSlide");
            s.Begin();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            synapseZAPI.Inject(Directory.GetCurrentDirectory());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string text = myTextEditor.Text;
            synapseZAPI.Execute(Directory.GetCurrentDirectory(), text);
        }

        private void topmost_c_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void topmost_c_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void opacity_c_Checked(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.8;
        }

        private void opacity_c_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Opacity = 1.0;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // un-needed since tab system doesnt exist yet
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            myTextEditor.Text = string.Empty;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt",
                Title = "Abyss | Save"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, myTextEditor.Text);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt",
                Title = "Abyss | Open"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                myTextEditor.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void PopulateTreeView()
        {
            FavoritesItem.Items.Clear();

            if (Directory.Exists(abyssScriptsFolder))
            {
                string[] files = Directory.GetFiles(abyssScriptsFolder, "*.*")
                                          .Where(f => f.EndsWith(".lua") || f.EndsWith(".txt"))
                                          .ToArray();

                foreach (string file in files)
                {
                    TreeViewItem item = new TreeViewItem
                    {
                        Header = System.IO.Path.GetFileName(file),
                        FontFamily = new FontFamily("Inter"),
                        FontSize = 11,
                        Foreground = new SolidColorBrush(Color.FromRgb(109, 109, 109)),
                        Tag = file
                    };

                    item.MouseLeftButtonUp += TreeViewItem_MouseLeftButtonUp;
                    FavoritesItem.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show($"{abyssScriptsFolder} wasn't found", "Abyss", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // gpt generated (i tried so hard to get it to work myself but i wasted 30 minutes for it to not work)
        private void TreeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                string filePath = item.Tag as string;
                if (File.Exists(filePath))
                {
                    string fileContents = File.ReadAllText(filePath);
                    myTextEditor.Text = fileContents;
                }
            }
        }

    }
}
