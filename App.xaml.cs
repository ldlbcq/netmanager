using MaterialDesignThemes.Wpf;
using NetManager;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetManager
{
    public partial class App : Application
    {
        private string configFilePath = "config.cfg";
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeConfigFile();
            if (File.Exists(configFilePath))
            {
                string[] settings = File.ReadAllLines(configFilePath);

                if (settings.Length > 3)
                {
                    string culture = settings[3];
                    if (culture == "en-EN")
                    {
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
                    else if (culture == "fr-FR")
                    {
                        var enENDictionary = Application.Current.Resources.MergedDictionaries
    .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.EndsWith("en-EN.xaml"));

                        if (enENDictionary != null)
                        {
                            Application.Current.Resources.MergedDictionaries.Remove(enENDictionary);
                        }

                        // Charge le dictionnaire de ressources pour la langue anglaise (en-EN.xaml)
                        ResourceDictionary frenchResourceDictionary = new ResourceDictionary();
                        frenchResourceDictionary.Source = new Uri("fr-FR.xaml", UriKind.Relative);
                        Application.Current.Resources.MergedDictionaries.Add(frenchResourceDictionary);
                    }
                }
            }


        }
        private void InitializeConfigFile()
        {
            if (!File.Exists(configFilePath))
            {
                // Créer le fichier avec les valeurs par défaut
                string[] defaultSettings = new string[] { "Light", "#FF9C27B0", "#FF000000", "en-EN" };
                File.WriteAllLines(configFilePath, defaultSettings);
            }
        }

    }

}
