using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignColors;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace NetManager
{
    /// <summary>
    /// Logique d'interaction pour Results.xaml
    /// </summary>
    public partial class Results : MaterialWindow
    {
        public Results()
        {
            InitializeComponent();
            LoadUserSettings();
        }

        private string configFilePath = "config.cfg";
        private MainWindow mainWindow;

        private void ExitResult(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExportResults(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.SaveFileDialog();
            var resources = Application.Current.Resources;
            if (resources.Contains("sfdTitle4"))
            {
                openFileDialog.Title = resources["sfdTitle4"].ToString();
            }
            if (resources.Contains("sfdFilter3"))
            {
                openFileDialog.Filter = resources["sfdFilter3"].ToString();

            }
            if (resources.Contains("sfdDefExt3"))
            {
                openFileDialog.DefaultExt = resources["sfdDefExt3"].ToString();
            }
            if (resources.Contains("sfdFilterIndex1"))
            {
                if (int.TryParse(resources["sfdFilterIndex1"].ToString(), out int defaultFilterIndex))
                {
                    openFileDialog.FilterIndex = defaultFilterIndex;
                }
            }
            if (openFileDialog.ShowDialog() == true)
            {
                // Le fichier a été sélectionné
                string selectedFilePath = openFileDialog.FileName;
                using (StreamWriter writer = new StreamWriter(openFileDialog.FileName))
                {
                    writer.WriteLine(textResults.Text + Environment.NewLine);
                }
                MessageBoxResult firstRequest = MessageBox.Show(resources["exportCmdResultsMessage1"].ToString() + openFileDialog.FileName,
                    resources["cmdResultsTitle"].ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (firstRequest == MessageBoxResult.Yes)
                {
                    Process.Start("notepad.exe", openFileDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show(resources["exportCmdResultsMessage2"].ToString(), resources["cmdResultsTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadUserSettings()
        {
            if (File.Exists(configFilePath))
            {
                string[] settings = File.ReadAllLines(configFilePath);

                if (settings.Length >= 3)
                {
                    string BaseTheme = settings[0];
                    string PrimaryColor = settings[1];
                    string TextColor = settings[2];

                    // Appliquer les valeurs lues depuis le fichier
                    ApplyUserSettings(BaseTheme, PrimaryColor, TextColor);
                }
            }
        }

        private void ApplyUserSettings(string BaseTheme, string PrimaryColor, string TextColor)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            if (BaseTheme == "Light")
            {
                theme.SetBaseTheme(Theme.Light);
            }

            else if (BaseTheme == "Dark")
            {
                theme.SetBaseTheme(Theme.Dark);
            }

            Color primaryColor = (Color)ColorConverter.ConvertFromString(PrimaryColor);
            theme.SetPrimaryColor(primaryColor);
            Color textColor = (Color)ColorConverter.ConvertFromString(TextColor);
            Border1.Background = new SolidColorBrush(primaryColor);
            WindowTitle.Foreground = new SolidColorBrush(textColor);

            paletteHelper.SetTheme(theme);
        }

        public void UpdateBorderColor(Color color)
        {
            Border1.Background = new SolidColorBrush(color);
        }
        public void UpdateTitleTextColor(Color color)
        {
            WindowTitle.Foreground = new SolidColorBrush(color);
        }
        public void UpdateButtonTextColor(Color color)
        {
            exitBtn.Foreground = new SolidColorBrush(color);
            exprtCmdRsltBtn.Foreground = new SolidColorBrush(color);
        }
        private void OnClosed (object sender, EventArgs e)
        {
            Close();
        }
    }
}
