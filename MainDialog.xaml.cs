using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace NetManager
{
    /// <summary>
    /// Logique d'interaction pour MainDialog.xaml
    /// </summary>
    public partial class MainDialog : MaterialWindow
    {
        private string selectedInterfaceId;
        private string configFilePath = "config.cfg";
        public event EventHandler<InterfaceSelectedEventArgs> InterfaceSelected;

        public MainDialog()
        {
            InitializeComponent();
            LoadUserSettings();
            LoadNetworkInterfaces();
        }
        private void LoadNetworkInterfaces()
        {
            // Récupérer toutes les interfaces réseau
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Utiliser le StackPanel existant
            StackPanel stackPanel = stackPanel1;

            foreach (NetworkInterface netInterface in networkInterfaces)
            {
                // Filtrer les interfaces virtuelles, les interfaces de boucle logicielle et Bluetooth
                if (!IsVirtualInterface(netInterface) && !IsLoopbackInterface(netInterface) && !IsBluetoothInterface(netInterface))
                {
                    string formattedMacAddress = BitConverter.ToString(netInterface.GetPhysicalAddress().GetAddressBytes()).Replace("-", ":");

                    // Créer un bouton radio pour chaque interface non filtrée
                    RadioButton radioButton = new RadioButton
                    {
                        Content = $"{netInterface.Description} (MAC: {formattedMacAddress})",
                        Tag = netInterface.Id,
                        GroupName = "NetworkInterfaces",
                        Margin = new Thickness(0, 5, 0, 5), // Ajustez la marge pour l'espacement
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                    };

                    // Gérer l'événement Checked pour savoir quel bouton radio a été sélectionné
                    radioButton.Checked += RadioButton_Checked;

                    // Ajouter le bouton radio au StackPanel existant
                    stackPanel.Children.Add(radioButton);
                }
            }
        }

        private bool IsVirtualInterface(NetworkInterface networkInterface)
        {
            // Ajoutez des termes spécifiques pour identifier les interfaces virtuelles
            string[] virtualInterfaceKeywords = { "VirtualBox", "VMware", "Hyper-V", "Microsoft Wi-Fi Direct Virtual Adapter", "Fortinet" };

            foreach (string keyword in virtualInterfaceKeywords)
            {
                if (networkInterface.Description.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsLoopbackInterface(NetworkInterface networkInterface)
        {
            return networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback;
        }

        private bool IsBluetoothInterface(NetworkInterface networkInterface)
        {
            // Ajoutez des termes spécifiques pour identifier les interfaces Bluetooth dans la description
            string[] bluetoothInterfaceKeywords = { "Bluetooth", "BT" };

            foreach (string keyword in bluetoothInterfaceKeywords)
            {
                if (networkInterface.Description.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Logique à exécuter lorsque le bouton radio est sélectionné
            RadioButton selectedRadioButton = (RadioButton)sender;
            selectedInterfaceId = selectedRadioButton.Tag.ToString();

            // Émettez l'événement pour informer la MainWindow de la sélection
            InterfaceSelected?.Invoke(this, new InterfaceSelectedEventArgs(selectedRadioButton.Content.ToString()));

            this.Close();
        }

        public string GetSelectedInterfaceId()
        {
            return selectedInterfaceId;
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
            SelectInterfaceTxtBx.Foreground = new SolidColorBrush(textColor);
        }

        public void UpdateTitleTextColor(Color color)
        {
            WindowTitle.Foreground = new SolidColorBrush(color);
        }

        public void UpdateBorderColor(Color color)
        {
            WindowBorder.Background = new SolidColorBrush(color);
        }
    }
}
