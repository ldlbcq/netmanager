using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;
using MaterialDesignExtensions.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime;
using System.Media;
using System.Windows.Automation;

namespace NetManager
{
    /// <summary>
    /// Logique d'interaction pour ThemeAndColor.xaml
    /// </summary>
    public partial class ThemeAndColor : MaterialWindow
    {
        public ThemeAndColor()
        {
            InitializeComponent();
            mainWindow = (MainWindow?)Application.Current.MainWindow;
            results = Application.Current.Windows.OfType<Results>().FirstOrDefault();
            language = Application.Current.Windows.OfType<Language>().FirstOrDefault();
            LoadUserSettings();
            mainWindow.IsEnabled = false;
        }

        private Results? results;
        private Language? language;
        private MainWindow? mainWindow;
        private string configFilePath = "config.cfg";


        private void LightTheme(object sender, RoutedEventArgs e)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            // Modifier la valeur de BaseTheme
            theme.SetBaseTheme(Theme.Light); // ou Theme.Light pour le thème clair
            paletteHelper.SetTheme(theme);
            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[0] = "Light";
            settings[2] = "#FF000000";

            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);

            mainWindow?.UpdateNavIcon((Color)ColorConverter.ConvertFromString(settings[2]));
            mainWindow?.UpdateTitleTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            mainWindow?.UpdateTextBoxButtonTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            results?.UpdateTitleTextColor((Color)(ColorConverter.ConvertFromString(settings[2])));
            results?.UpdateButtonTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            language?.UpdateTitleTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            language?.UpdateButtonsColor((Color)ColorConverter.ConvertFromString(settings[2]));
            WindowTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            exitPg1Btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            nextPageBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            previousPageBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            exitPg2Btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));

        }

        private void DarkTheme(object sender, RoutedEventArgs e)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            // Modifier la valeur de BaseTheme
            theme.SetBaseTheme(Theme.Dark); // ou Theme.Light pour le thème clair
            paletteHelper.SetTheme(theme);
            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[0] = "Dark";
            settings[2] = "#FFFFFFFF";

            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);

            mainWindow?.UpdateNavIcon((Color)ColorConverter.ConvertFromString(settings[2]));
            mainWindow?.UpdateTitleTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            mainWindow?.UpdateTextBoxButtonTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            results?.UpdateTitleTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            results?.UpdateButtonTextColor((Color)(ColorConverter.ConvertFromString(settings[2])));
            language?.UpdateTitleTextColor((Color)ColorConverter.ConvertFromString(settings[2]));
            language?.UpdateButtonsColor((Color)ColorConverter.ConvertFromString(settings[2]));
            WindowTitle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            exitPg1Btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            nextPageBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            previousPageBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
            exitPg2Btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(settings[2]));
        }

        private void AmberColor(object sender, RoutedEventArgs e)
        {
            Color amber = Color.FromRgb(255, 193, 7);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(amber);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = amber.ToString();

            mainWindow?.UpdateBorderColor(amber);
            results?.UpdateBorderColor(amber);
            language?.UpdateBorderColor(amber);
            UpdateBorderColor(amber);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void BlueColor(object sender, RoutedEventArgs e)
        {
            Color blue = Color.FromRgb(33, 150, 243);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(blue);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = blue.ToString();

            mainWindow?.UpdateBorderColor(blue);
            results?.UpdateBorderColor(blue);
            language?.UpdateBorderColor(blue);
            UpdateBorderColor(blue);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void BlueGrayColor(object sender, RoutedEventArgs e)
        {
            Color blueGray = Color.FromRgb(96, 125, 139);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(blueGray);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = blueGray.ToString();

            mainWindow?.UpdateBorderColor(blueGray);
            results?.UpdateBorderColor(blueGray);
            language?.UpdateBorderColor(blueGray);
            UpdateBorderColor(blueGray);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void BrownColor(object sender, RoutedEventArgs e)
        {
            Color brown = Color.FromRgb(121, 85, 72);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(brown);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = brown.ToString();

            mainWindow?.UpdateBorderColor(brown);
            language?.UpdateBorderColor(brown);
            results?.UpdateBorderColor(brown);
            UpdateBorderColor(brown);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void CyanColor(object sender, RoutedEventArgs e)
        {
            Color cyan = Color.FromRgb(0, 188, 212);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(cyan);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = cyan.ToString();

            mainWindow?.UpdateBorderColor(cyan);
            results?.UpdateBorderColor(cyan);
            language?.UpdateBorderColor(cyan);
            UpdateBorderColor(cyan);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void DeepOrangeColor(object sender, RoutedEventArgs e)
        {
            Color deepOrange = Color.FromRgb(255, 87, 34);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(deepOrange);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = deepOrange.ToString();

            mainWindow?.UpdateBorderColor(deepOrange);
            results?.UpdateBorderColor(deepOrange);
            language?.UpdateBorderColor(deepOrange);
            UpdateBorderColor(deepOrange);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void DeepPurpleColor(object sender, RoutedEventArgs e)
        {
            Color deepPurple = Color.FromRgb(103, 58, 183);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(deepPurple);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = deepPurple.ToString();

            mainWindow?.UpdateBorderColor(deepPurple);
            results?.UpdateBorderColor(deepPurple);
            language?.UpdateBorderColor(deepPurple);
            UpdateBorderColor(deepPurple);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void GreenColor(object sender, RoutedEventArgs e)
        {
            Color green = Color.FromRgb(76, 175, 80);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(green);
            language?.UpdateBorderColor(green);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = green.ToString();

            mainWindow?.UpdateBorderColor(green);
            results?.UpdateBorderColor(green);
            UpdateBorderColor(green);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void GrayColor(object sender, RoutedEventArgs e)
        {
            Color gray = Color.FromRgb(158, 158, 158);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(gray);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = gray.ToString();

            mainWindow?.UpdateBorderColor(gray);
            results?.UpdateBorderColor(gray);
            language?.UpdateBorderColor(gray);
            UpdateBorderColor(gray);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void IndigoColor(object sender, RoutedEventArgs e)
        {
            Color indigo = Color.FromRgb(63, 81, 181);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(indigo);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = indigo.ToString();

            mainWindow?.UpdateBorderColor(indigo);
            results?.UpdateBorderColor(indigo);
            language?.UpdateBorderColor(indigo);
            UpdateBorderColor(indigo);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void PinkColor(object sender, RoutedEventArgs e)
        {
            Color pink = Color.FromRgb(233, 30, 99);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(pink);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = pink.ToString();

            mainWindow?.UpdateBorderColor(pink);
            results?.UpdateBorderColor(pink);
            language?.UpdateBorderColor(pink);
            UpdateBorderColor(pink);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void PurpleColor(object sender, RoutedEventArgs e)
        {
            Color purple = Color.FromRgb(156, 39, 176);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(purple);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = purple.ToString();

            mainWindow?.UpdateBorderColor(purple);
            results?.UpdateBorderColor(purple);
            language?.UpdateBorderColor(purple);
            UpdateBorderColor(purple);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void RedColor(object sender, RoutedEventArgs e)
        {
            Color red = Color.FromRgb(244, 67, 54);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(red);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = red.ToString();

            mainWindow?.UpdateBorderColor(red);
            results?.UpdateBorderColor(red);
            language?.UpdateBorderColor(red);
            UpdateBorderColor(red);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        private void TealColor(object sender, RoutedEventArgs e)
        {
            Color teal = Color.FromRgb(0, 150, 136);
            var paletteHelper = new PaletteHelper();
            ITheme colorTheme = paletteHelper.GetTheme();
            colorTheme.SetPrimaryColor(teal);
            paletteHelper.SetTheme(colorTheme);

            // Récupérer les valeurs existantes du fichier
            string[] settings = File.ReadAllLines(configFilePath);

            // Modifier les valeurs de PrimaryColor et TextColor
            settings[1] = teal.ToString();

            mainWindow?.UpdateBorderColor(teal);
            results?.UpdateBorderColor(teal);
            language?.UpdateBorderColor(teal);
            UpdateBorderColor(teal);
            var baseTheme = colorTheme.GetBaseTheme();

            if (baseTheme == BaseTheme.Dark)
            {
                mainWindow?.UpdateNavIcon(Colors.White);
                settings[2] = Colors.White.ToString();
            }
            else
            {
                mainWindow?.UpdateNavIcon(Colors.Black);
                settings[2] = Colors.Black.ToString();
            }
            // Sauvegarder les valeurs modifiées dans le fichier
            File.WriteAllLines(configFilePath, settings);
        }

        public void UpdateBorderColor(Color color)
        {
            WindowBorder.Background = new SolidColorBrush(color);
        }

        public void UpdateTitleTextColor(Color color)
        {
            WindowTitle.Foreground = new SolidColorBrush(color);
        }

        private void ExitUIConfig(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.IsEnabled = true;
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 1;
        }

        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 0;
        }
        private void SaveUserSettings(string BaseTheme, string PrimaryColor, string TextColor)
        {
            string[] settings = File.ReadAllLines(configFilePath);
            settings[0] = BaseTheme;
            settings[1] = PrimaryColor;
            settings[2] = TextColor;
            File.WriteAllLines(configFilePath, settings);
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

            exitPg1Btn.Foreground = new SolidColorBrush(textColor);
            nextPageBtn.Foreground = new SolidColorBrush(textColor);

            previousPageBtn.Foreground = new SolidColorBrush(textColor);
            exitPg2Btn.Foreground = new SolidColorBrush(textColor);

            paletteHelper.SetTheme(theme);
        }

        private void MaterialWindow_Closed(object sender, EventArgs e)
        {

        }

        private void OnClosed(object sender, EventArgs e)
        {
            mainWindow.IsEnabled = true;
            Close();
        }
    }
}
