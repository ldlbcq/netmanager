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
using System.Windows.Shapes;
using MaterialDesignColors;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace NetManager
{
    /// <summary>
    /// Logique d'interaction pour Language.xaml
    /// </summary>
    public partial class Language : MaterialWindow
    {
        public Language()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            InitializeComponent();
            LoadUserSettings();
            mainWindow.IsEnabled = false;
        }

        private string configFilePath = "config.cfg";
        private MainWindow mainWindow;

        private void OnClosed(object sender, EventArgs e)
        {
            mainWindow.IsEnabled = true;
        }

        private void test(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SetLangToFrench(object sender, RoutedEventArgs e)
        {
            string[] settings = File.ReadAllLines(configFilePath);
            Array.Resize(ref settings, 4);
            settings[3] = "fr-FR";
            File.WriteAllLines(configFilePath, settings);
            // Supprime le dictionnaire lié à "en-EN.xaml"
            var enENDictionary = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.EndsWith("en-EN.xaml"));

            if (enENDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(enENDictionary);
            }

            // Charge le dictionnaire de ressources pour la langue française (fr-FR.xaml)
            ResourceDictionary frenchResourceDictionary = new ResourceDictionary();
            frenchResourceDictionary.Source = new Uri("fr-FR.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(frenchResourceDictionary);
        }

        private void SetLangToEnglish(object sender, RoutedEventArgs e)
        {
            string[] settings = File.ReadAllLines(configFilePath);
            Array.Resize(ref settings, 4);
            settings[3] = "en-EN";
            File.WriteAllLines(configFilePath, settings);
            // Supprime le dictionnaire lié à "fr-FR.xaml"
            var enENDictionary = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.EndsWith("fr-FR.xaml"));

            if (enENDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(enENDictionary);
            }

            // Charge le dictionnaire de ressources pour la langue anglaise (en-EN.xaml)
            ResourceDictionary frenchResourceDictionary = new ResourceDictionary();
            frenchResourceDictionary.Source = new Uri("en-EN.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(frenchResourceDictionary);

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
            WindowBorder.Background = new SolidColorBrush(primaryColor);
            WindowTitle.Foreground = new SolidColorBrush(textColor);

            setEnglishBtn.Foreground = new SolidColorBrush(textColor);
            setFrenchBtn.Foreground = new SolidColorBrush(textColor);
            exitBtn.Foreground = new SolidColorBrush(textColor);

            paletteHelper.SetTheme(theme);
        }

        public void UpdateBorderColor(Color color)
        {
            WindowBorder.Background = new SolidColorBrush(color);
        }

        public void UpdateTitleTextColor(Color color)
        {
            WindowTitle.Foreground = new SolidColorBrush(color);
        }

        public void UpdateButtonsColor(Color color)
        {
            setFrenchBtn.Foreground = new SolidColorBrush(color);
            setEnglishBtn.Foreground = new SolidColorBrush(color);
            exitBtn.Foreground = new SolidColorBrush (color);
        }
    }
}
