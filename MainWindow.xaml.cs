using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using NetTools;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace NetManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        private string configFilePath = "config.cfg";
        private TrayIcon trayIcon;
        public static MainWindow? Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadUserSettings();
            Instance = this;
        }

        #region NavigationMenu

        public void ToggleDrawerAndAnimateIcon()
        {
            bool isDrawerOpen = DrawerHost.IsLeftDrawerOpen;
            DoubleAnimation rotationAnimation = new DoubleAnimation();

            if (isDrawerOpen)
            {
                // Rotation de 0 à 90 degrés (sens horaire)
                rotationAnimation.From = 90;
                rotationAnimation.To = 0;
            }
            else
            {
                // Rotation de 90 à 0 degré (sens antihoraire)
                rotationAnimation.From = 0;
                rotationAnimation.To = 90;
            }

            // Définir la durée de l'animation
            rotationAnimation.Duration = TimeSpan.FromSeconds(0.3);

            // Appliquer l'animation à la propriété RotateTransform.Angle de l'icône
            NavIcon.RenderTransformOrigin = new Point(0.5, 0.5);
            NavIcon.RenderTransform = new RotateTransform();
            NavIcon.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            // Ouvrir ou fermer le volet
            DrawerHost.IsLeftDrawerOpen = !isDrawerOpen;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            ToggleDrawerAndAnimateIcon();
        }

        private void NetConfigTab_Click(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 0;
            ToggleDrawerAndAnimateIcon();
        }

        private void ChangeConfigTab_Click(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 1;
            ToggleDrawerAndAnimateIcon();
        }

        private void NetCommandsTab_Click(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 2;
            ToggleDrawerAndAnimateIcon();
        }

        private void CalculatorTab_Click(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 4;
            ToggleDrawerAndAnimateIcon();
        }

        private void SettingsTab_Click(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 5;
            ToggleDrawerAndAnimateIcon();
        }

        private void RotateTo45Degrees(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = 90;
            rotationAnimation.To = 0;
            rotationAnimation.Duration = TimeSpan.FromSeconds(0.3);
            SettingsIcon.RenderTransformOrigin = new Point(0.5, 0.5);
            SettingsIcon.RenderTransform = new RotateTransform();
            SettingsIcon.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

        private void RotateTo0Degrees(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = 0;
            rotationAnimation.To = 90;
            rotationAnimation.Duration = TimeSpan.FromSeconds(0.3);
            SettingsIcon.RenderTransformOrigin = new Point(0.5, 0.5);
            SettingsIcon.RenderTransform = new RotateTransform();
            SettingsIcon.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

        #endregion

        #region NetSettings

        private void ShowNetSettings(object sender, RoutedEventArgs e)
        {
            string ipAddr = ipAddressTxtBx1.Text;
            string subnetMask = subnetMaskTxtBx1.Text;
            string gateway = gatewayTxtBx1.Text;
            string primDns = firstDnsTxtBx1.Text;
            string secDns = secondDnsTxtBx1.Text;

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface iface in interfaces)
            {
                // Vérifie si la carte réseau est en cours d'utilisation par Hyper-V, VMware ou VirtualBox
                if (iface.OperationalStatus == OperationalStatus.Up &&
                    iface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    !iface.Description.Contains("Hyper-V") &&
                    !iface.Description.Contains("VMware") &&
                    !iface.Description.Contains("VirtualBox"))
                {
                    IPInterfaceProperties ipProperties = iface.GetIPProperties();
                    foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddressTxtBx1.Text = ipAddr + ip.Address.ToString();
                            subnetMaskTxtBx1.Text = subnetMask + ip.IPv4Mask.ToString();
                        }
                    }
                    GatewayIPAddressInformationCollection gatewayAddresses = ipProperties.GatewayAddresses;
                    foreach (GatewayIPAddressInformation gw in gatewayAddresses)
                    {
                        if (gw.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            gatewayTxtBx1.Text = gateway + gw.Address.ToString();
                        }
                    }
                    System.Net.NetworkInformation.IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;
                    int dnsCount = dnsAddresses.Count;
                    if (dnsCount > 0)
                    {
                        // Affiche le premier serveur DNS dans la TextBox correspondante
                        firstDnsTxtBx1.Text = primDns + dnsAddresses[0].ToString();

                        // Si un deuxième serveur DNS est disponible, l'affiche dans la TextBox correspondante
                        if (dnsCount > 1)
                        {
                            secondDnsTxtBx1.Text = secDns + dnsAddresses[1].ToString();
                        }
                    }
                }
            }
        }

        private void ExportNetSettings(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.SaveFileDialog();
            var resources = Application.Current.Resources;
            if (resources.Contains("sfdTitle1"))
            {
                openFileDialog.Title = resources["sfdTitle1"].ToString();
            }
            if (resources.Contains("sfdFilter1"))
            {
                openFileDialog.Filter = resources["sfdFilter1"].ToString();

            }
            if (resources.Contains("sfdDefExt1"))
            {
                openFileDialog.DefaultExt = resources["sfdDefExt1"].ToString();
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
                    writer.WriteLine(ipAddressTxtBx1.Text + Environment.NewLine);
                    writer.WriteLine(subnetMaskTxtBx1.Text + Environment.NewLine);
                    writer.WriteLine(gatewayTxtBx1.Text + Environment.NewLine);
                    writer.WriteLine(firstDnsTxtBx1.Text + Environment.NewLine);
                    writer.WriteLine(secondDnsTxtBx1.Text + Environment.NewLine);
                }
                MessageBoxResult firstRequest = MessageBox.Show(resources["savingNetSettingsMessage1"].ToString() + openFileDialog.FileName + resources["netSettingsMessage"].ToString(),
                    resources["savingNetSettingsMessageTitle"].ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (firstRequest == MessageBoxResult.Yes)
                {
                    Process.Start("notepad.exe", openFileDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show(resources["savingNetSettingsMessage2"].ToString(), resources["netSettingsMessageTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region CustomNetConfig

        private void ExportConfig(object sender, RoutedEventArgs e)
        {
            var ExportCustomConfig = new Microsoft.Win32.SaveFileDialog();
            var resources = Application.Current.Resources;
            var ipStr = resources["IpAddressTxtBx"].ToString();
            var maskStr = resources["NetMaskTxtBx"].ToString();
            var gtwStr = resources["DefGatewayTxtBx"].ToString();
            var pdnsStr = resources["PrimDnsTxtBx"].ToString();
            var sdnsStr = resources["SecDnsTxtBx"].ToString();
            if (resources.Contains("sfdTitle2"))
            {
                ExportCustomConfig.Title = resources["sfdTitle2"].ToString();
            }
            if (resources.Contains("sfdFilter2"))
            {
                ExportCustomConfig.Filter = resources["sfdFilter2"].ToString();

            }
            if (resources.Contains("sfdDefExt2"))
            {
                ExportCustomConfig.DefaultExt = resources["sfdDefExt2"].ToString();
            }
            if (resources.Contains("sfdFilterIndex2"))
            {
                if (int.TryParse(resources["sfdFilterIndex2"].ToString(), out int defaultFilterIndex))
                {
                    ExportCustomConfig.FilterIndex = defaultFilterIndex;
                }
            }
            if (ExportCustomConfig.ShowDialog() == true)
            {
                // Le fichier a été sélectionné
                string selectedFilePath = ExportCustomConfig.FileName;
                using (StreamWriter writer = new StreamWriter(ExportCustomConfig.FileName))
                {
                    writer.WriteLine(ipStr.ToString() + ipAddrTxtBx.Text + Environment.NewLine);
                    writer.WriteLine(maskStr.ToString() + netMaskTxtBx.Text + Environment.NewLine);
                    writer.WriteLine(gtwStr.ToString() + defGatewayTxtBx.Text + Environment.NewLine);
                    writer.WriteLine(pdnsStr.ToString() + primDnsTxtBx.Text + Environment.NewLine);
                    writer.WriteLine(sdnsStr.ToString() + secDnsTxtBx.Text + Environment.NewLine);
                }
                MessageBox.Show(resources["exportNetSettingsMessage1"].ToString() + ExportCustomConfig.FileName, resources["netSettingsMessageTitle"].ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ipAddrTxtBx.Clear();
                netMaskTxtBx.Clear();
                defGatewayTxtBx.Clear();
                primDnsTxtBx.Clear();
                secDnsTxtBx.Clear();
            }
            else
            {
                MessageBox.Show(resources["exportNetSettingsMessage2"].ToString(), resources["netSettingsMessageTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void ImportConfig(object sender, RoutedEventArgs e)
        {
            var ImportCustomConfig = new Microsoft.Win32.OpenFileDialog();
            var resources = Application.Current.Resources;
            var ipStr = resources["IpAddressTxtBx"].ToString();
            var maskStr = resources["NetMaskTxtBx"].ToString();
            var gtwStr = resources["DefGatewayTxtBx"].ToString();
            var pdnsStr = resources["PrimDnsTxtBx"].ToString();
            var sdnsStr = resources["SecDnsTxtBx"].ToString();
            if (resources.Contains("sfdTitle3"))
            {
                ImportCustomConfig.Title = resources["sfdTitle2"].ToString();
            }
            if (resources.Contains("sfdFilter2"))
            {
                ImportCustomConfig.Filter = resources["sfdFilter2"].ToString();

            }
            if (resources.Contains("sfdDefExt2"))
            {
                ImportCustomConfig.DefaultExt = resources["sfdDefExt2"].ToString();
            }
            if (resources.Contains("sfdFilterIndex2"))
            {
                if (int.TryParse(resources["sfdFilterIndex2"].ToString(), out int defaultFilterIndex))
                {
                    ImportCustomConfig.FilterIndex = defaultFilterIndex;
                }
            }
            if (ImportCustomConfig.ShowDialog() == true)
            {
                if (new FileInfo(ImportCustomConfig.FileName).Length != 0)
                {
                    foreach (var line in File.ReadLines(ImportCustomConfig.FileName))
                    {
                        if (line.StartsWith(ipStr.ToString()))
                        {
                            string ipConf = line.Replace(ipStr, "");
                            ipAddrTxtBx.Text = ipConf;
                        }
                        else if (line.StartsWith(maskStr.ToString()))
                        {
                            string maskConf = line.Replace(maskStr, "");
                            netMaskTxtBx.Text = maskConf;

                        }
                        else if (line.StartsWith(gtwStr.ToString()))
                        {
                            string gtwConf = line.Replace(gtwStr, "");
                            defGatewayTxtBx.Text = gtwConf;

                        }
                        else if (line.StartsWith(pdnsStr.ToString()))
                        {
                            string pdnsConf = line.Replace(pdnsStr, "");
                            primDnsTxtBx.Text = pdnsConf;

                        }
                        else if (line.StartsWith(sdnsStr.ToString()))
                        {
                            string sdnsConf = line.Replace(sdnsStr, "");
                            secDnsTxtBx.Text = sdnsConf;

                        }
                    }
                    MessageBox.Show(resources["importNetSettingsMessage1"].ToString() + ImportCustomConfig.FileName, resources["netSettingsMessageTitle"].ToString(),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void SetStaticSettings(object sender, RoutedEventArgs e)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && adapter.OperationalStatus == OperationalStatus.Up
                    || adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && adapter.OperationalStatus == OperationalStatus.Up
                    || adapter.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    if (!adapter.Name.Contains("vEthernet") || !adapter.Description.Contains("VMware")
                        || !adapter.Description.Contains("VirtualBox") || !adapter.Description.Contains("Fortinet")
                        || adapter.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                        psi.UseShellExecute = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.Verb = "runas";
                        psi.Arguments = ("/c netsh interface ipv4 set address " + adapter.Name + " static " + ipAddrTxtBx.Text
                            + " " + netMaskTxtBx.Text + " " + defGatewayTxtBx.Text + " & netsh interface ipv4 set dnsservers " + adapter.Name + " static "
                            + primDnsTxtBx.Text + " primary & netsh interface ipv4 add dnsservers " + adapter.Name + " " + secDnsTxtBx.Text + " index=2");
                        Process.Start(psi);
                    }
                }
            }
            ipAddrTxtBx.Clear();
            netMaskTxtBx.Clear();
            defGatewayTxtBx.Clear();
            primDnsTxtBx.Clear();
            secDnsTxtBx.Clear();
        }

        private void SetDynamicSettings(object sender, RoutedEventArgs e)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && adapter.OperationalStatus == OperationalStatus.Up
                    || adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && adapter.OperationalStatus == OperationalStatus.Up
                    || adapter.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    if (!adapter.Name.Contains("vEthernet") || !adapter.Description.Contains("VMware")
                        || !adapter.Description.Contains("VirtualBox") || !adapter.Description.Contains("Fortinet")
                        || adapter.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                        psi.UseShellExecute = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.Verb = "runas";
                        psi.Arguments = ("/c netsh interface ipv4 set address " + adapter.Name + " source=dhcp " + " & netsh interface ipv4 set dnsservers " + adapter.Name + " source=dhcp ");
                        Process.Start(psi);
                    }
                }
            }
        }
        #endregion

        #region Cmd-Ping

        private async void StartPing(object sender, RoutedEventArgs e)
        {
            string pingHeader = Application.Current.Resources["pingHeader"].ToString();
            string pingSuccess = Application.Current.Resources["pingSuccess"].ToString();
            string pingNotSuccess = Application.Current.Resources["pingNotSuccess"].ToString();
            string ipError = Application.Current.Resources["ipError"].ToString();
            string ipAddress = ipAddrTxtBx1.Text;
            int timeout = int.Parse(timeoutTxtBx.Text);
            int repetition = int.TryParse(repetitionTxtBx.Text, out int parsedRepetition) ? parsedRepetition : 0;
            Results results = new Results();
            results.textResults.Text = pingHeader + Environment.NewLine;
            results.Show();
            bool isValidIp = IPAddress.TryParse(ipAddress, out IPAddress parsedIpAddress); // Vérification de l'adresse IP valide

            if (isValidIp)
            {
                bool isEndlessPing = repetition <= 0; // Vérifie si le ping doit être continu

                while (isEndlessPing || repetition > 0)
                {
                    Ping ping = new Ping();
                    PingReply reply = await ping.SendPingAsync(parsedIpAddress, timeout);

                    if (reply.Status == IPStatus.Success)
                    {
                        string message = pingSuccess + " " + parsedIpAddress.ToString() + Environment.NewLine;
                        results.textResults.Text += message;
                        await Task.Delay(1000); // Pause d'une seconde entre chaque ping
                    }
                    else
                    {
                        string message = pingNotSuccess + " " + parsedIpAddress.ToString() + Environment.NewLine;
                        results.textResults.Text += message;
                        await Task.Delay(1000); // Pause d'une seconde entre chaque ping
                    }

                    if (!isEndlessPing)
                    {
                        repetition--;
                    }
                }
            }
            else
            {
                results.Close();
                MessageBox.Show(ipError + " " + ipAddress);
            }
        }
        #endregion

        #region Cmd-ScanNetwork

        private async void StartScan(object sender, RoutedEventArgs e)
        {
            string scanHeader = Application.Current.Resources["scanHeader"].ToString();
            string scanSuccess = Application.Current.Resources["scanSuccess"].ToString();
            string scanNotSuccess = Application.Current.Resources["scanNotSuccess"].ToString();
            string scanStartIPError = Application.Current.Resources["scanStartIPError"].ToString();
            string ipFamilyError = Application.Current.Resources["ipFamilyError"].ToString();
            string scanEndIPError = Application.Current.Resources["scanEndIPError"].ToString();
            Results results = new Results();
            results.textResults.Text = scanHeader + Environment.NewLine;
            results.Show();

            if (!IPAddress.TryParse(firstHostTxtBx.Text, out IPAddress startingIP))
            {
                MessageBox.Show(scanStartIPError);
                return;
            }

            // Vérification de l'adresse IP de début saisie
            if (startingIP.AddressFamily != AddressFamily.InterNetwork)
            {
                results.Close();
                MessageBox.Show(ipFamilyError);
                return;
            }

            // Vérification de l'adresse IP de fin saisie
            if (!IPAddress.TryParse(lastHostTxtBx.Text, out IPAddress endingIP))
            {
                results.Close();
                MessageBox.Show(scanEndIPError);
                return;
            }

            // Vérification de l'adresse IP de fin saisie
            if (endingIP.AddressFamily != AddressFamily.InterNetwork)
            {
                results.Close();
                MessageBox.Show(ipFamilyError);
                return;
            }

            IPAddressRange ipRange = new IPAddressRange(startingIP, endingIP);

            foreach (IPAddress iPAddress in ipRange)
            {
                string ipString = iPAddress.ToString();
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(iPAddress, 1000);

                if (reply.Status == IPStatus.Success)
                {
                    string message = ipString + " " + scanSuccess + Environment.NewLine;
                    results.textResults.Text += message;
                    await Task.Delay(1000); // Pause d'une seconde entre chaque ping
                }
                else
                {
                    string message = ipString + " " + scanNotSuccess + Environment.NewLine;
                    results.textResults.Text += message;
                    await Task.Delay(1000); // Pause d'une seconde entre chaque ping
                }

            }

        }
        #endregion

        #region Cmd-MtuChecker

        private async void StartMTUCheck(object sender, RoutedEventArgs e)
        {
            string mtuHeader = Application.Current.Resources["mtuHeader"].ToString();
            string mtuSuccess = Application.Current.Resources["mtuSuccess"].ToString();
            string mtuNotSuccess = Application.Current.Resources["mtuNotSuccess"].ToString();
            string ipError = Application.Current.Resources["ipError"].ToString();
            string ipFamilyError = Application.Current.Resources["ipFamilyError"].ToString();
            string ipAddress = ipAddrTxtBx2.Text;
            int startSize = int.Parse(firstValueTxtBx.Text);
            int endSize = int.Parse(lastValueTxtBx.Text);
            Results results = new Results();
            results.textResults.Text = mtuHeader + Environment.NewLine;
            results.Show();

            // Vérification de l'adresse IP saisie
            IPAddress parsedIpAddress;
            if (!IPAddress.TryParse(ipAddress, out parsedIpAddress))
            {
                results.Close();
                MessageBox.Show(ipError + " " + ipAddress);
                return;
            }

            // Vérification de l'adresse IP saisie
            if (parsedIpAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                results.Close();
                MessageBox.Show(ipFamilyError);
                return;
            }

            for (int size = startSize; size <= endSize; size++)
            {
                Ping ping = new Ping();
                PingOptions pingOptions = new PingOptions();
                byte[] buffer = new byte[size];

                PingReply reply = await ping.SendPingAsync(ipAddress, 1000, buffer, pingOptions);

                if (reply.Status == IPStatus.Success)
                {
                    string message = ipAddress + " " + mtuSuccess + " " + size + Environment.NewLine;
                    results.textResults.Text += message;
                    await Task.Delay(1000); // Pause d'une seconde entre chaque ping
                }
                else
                {
                    string message = ipAddress + " " + mtuNotSuccess + " " + size + Environment.NewLine;
                    results.textResults.Text += message;
                    await Task.Delay(1000); // Pause d'une seconde entre chaque ping
                }
            }
        }

        #endregion

        #region Cmd-ARPCache

        private void ShowARPCache(object sender, RoutedEventArgs e)
        {
            string arpHeader = Application.Current.Resources["arpHeader"].ToString();
            Results results = new Results();
            results.textResults.Text = arpHeader + Environment.NewLine;
            results.Show();
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "arp",
                Arguments = "-a",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            output = output.Replace("ÿ", " ");
            process.WaitForExit();
            results.textResults.Text += output;
        }
        #endregion

        #region Cmd-Tracert

        private async void StartTraceroute(object sender, RoutedEventArgs e)
        {
            string tracertHeader = Application.Current.Resources["tracertHeader"].ToString();
            string tracertError = Application.Current.Resources["tracertError"].ToString();
            string ipError = Application.Current.Resources["ipError"].ToString();
            string ipFamilyError = Application.Current.Resources["ipFamilyError"].ToString();
            Results results = new Results();
            results.textResults.Text = tracertHeader + Environment.NewLine;
            results.Show();

            if (!IPAddress.TryParse(ipAddrTxtBx3.Text, out IPAddress ipAddress))
            {
                results.Close();
                MessageBox.Show(ipError + " " + ipAddrTxtBx3.Text);
                return;
            }

            // Vérification de l'adresse IP de début saisie
            if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                results.Close();
                MessageBox.Show(ipFamilyError);
                return;
            }

            await Task.Run(() =>
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "tracert",
                    Arguments = ipAddress.ToString(),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true

                };

                process.StartInfo = startInfo;
                process.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        byte[] bytes = Encoding.Unicode.GetBytes(args.Data);
                        string output = Encoding.Unicode.GetString(bytes);
                        output = output.Replace("Š", "\u00e8");
                        results.textResults.Text = (tracertError + " : " + output);
                    }
                };

                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            byte[] bytes = Encoding.Unicode.GetBytes(args.Data);
                            string output = Encoding.Unicode.GetString(bytes);

                            output = output.Replace("‚", "\u00E9");
                            output = output.Replace("ÿ", " ");
                            results.textResults.AppendText(output + Environment.NewLine);
                        });
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            });
        }


        #endregion

        #region Cmd-RouteTable

        private async void ShowRouteTable(object sender, RoutedEventArgs e)
        {
            string routetableHeader = Application.Current.Resources["routetableHeader"].ToString();
            Results results = new Results();
            results.textResults.Text = routetableHeader + Environment.NewLine;
            results.Show();

            await Task.Run(() =>
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c route print",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.StartInfo = startInfo;
                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            byte[] bytes = Encoding.Unicode.GetBytes(args.Data);
                            string output = Encoding.Unicode.GetString(bytes);

                            output = output.Replace("‚", "\u00E9");
                            output = output.Replace("ÿ", " ");
                            results.textResults.Text += output + Environment.NewLine;

                        });
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            });
        }
        #endregion

        #region NetmaskCalc

        private void StartMaskCalc(object sender, RoutedEventArgs e)
        {
            string input = ntwrkMaskTxtBx1.Text; // Obtenez la valeur saisie par l'utilisateur à partir de votre TextBox
            string maskcalcHeader = Application.Current.Resources["maskcalcHeader"].ToString();
            string maskcalcCIDR = Application.Current.Resources["maskcalcCIDR"].ToString();
            string maskcalcDecimal = Application.Current.Resources["maskcalcDecimal"].ToString();
            string maskcalLength = Application.Current.Resources["maskcalLength"].ToString();
            string maskcalValue = Application.Current.Resources["maskcalValue"].ToString();
            string maskcalcError = Application.Current.Resources["maskcalcError"].ToString();
            Results results = new Results();
            results.textResults.Text = maskcalcHeader + Environment.NewLine;
            results.Show();

            if (!string.IsNullOrEmpty(input))
            {
                if (IsValidDecimalSubnetMask(input))
                {
                    IPAddress subnetMask = IPAddress.Parse(input);
                    int prefixLength = GetPrefixLengthFromSubnetMask(subnetMask);
                    results.textResults.Text += input + " " + maskcalcCIDR + " /" + prefixLength.ToString() + Environment.NewLine;
                }
                else if (int.TryParse(input, out int prefixLength))
                {
                    if (prefixLength >= 0 && prefixLength <= 32)
                    {
                        IPAddress subnetMask = GetSubnetMaskFromPrefixLength(prefixLength);
                        results.textResults.Text += "/" + input + " " + maskcalcDecimal + " " + subnetMask.ToString();
                    }
                    else
                    {
                        results.Close();
                        MessageBox.Show(maskcalLength);
                    }
                }
                else
                {
                    results.Close();
                    MessageBox.Show(maskcalValue);
                }
            }
            else
            {
                results.Close();
                MessageBox.Show(maskcalcError);
            }
        }

        private bool IsValidDecimalSubnetMask(string input)
        {
            if (IPAddress.TryParse(input, out IPAddress ipAddress))
            {
                byte[] addressBytes = ipAddress.GetAddressBytes();

                // Vérifie si le masque de sous-réseau est valide
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(addressBytes);
                }

                uint mask = BitConverter.ToUInt32(addressBytes, 0);

                // La valeur du masque de sous-réseau valide doit être de la forme 2^x - 1
                uint invertedMask = ~mask;
                uint check = invertedMask + 1;

                return (check & (check - 1)) == 0;
            }

            return false;
        }

        private int GetPrefixLengthFromSubnetMask(IPAddress subnetMask)
        {
            byte[] addressBytes = subnetMask.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(addressBytes);
            }

            uint mask = BitConverter.ToUInt32(addressBytes, 0);

            int prefixLength = 0;
            while (mask != 0)
            {
                mask <<= 1;
                prefixLength++;
            }

            return prefixLength;
        }

        private IPAddress GetSubnetMaskFromPrefixLength(int prefixLength)
        {
            int hostBits = 32 - prefixLength;
            int mask = (int)(uint.MaxValue << hostBits);
            byte[] bytes = BitConverter.GetBytes(mask);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            IPAddress ipAddress = new IPAddress(bytes);
            return ipAddress;
        }
        #endregion

        #region SubnetSupernet

        private void StartSubnetSupernet(object sender, RoutedEventArgs e)
        {
            string oldNetworkAddressString = ntwrkTxtBx.Text;
            string oldSubnetMask = oldNtwrkMaskTxtBx.Text;
            string newSubnetMask = newNtwrkMaskTxtBx.Text;
            string subnetHeader = Application.Current.Resources["subnetHeader"].ToString();
            string subnetMessage1 = Application.Current.Resources["subnetMessage1"].ToString();
            string subnetMessage2 = Application.Current.Resources["subnetMessage2"].ToString();
            string subnetMessage3 = Application.Current.Resources["subnetMessage3"].ToString();
            string subnetAddress = Application.Current.Resources["subnetAddress"].ToString();
            string subnetMask = Application.Current.Resources["subnetMask"].ToString();
            string subnetWildcard = Application.Current.Resources["subnetWildcard"].ToString();
            string subnetFirstUsable = Application.Current.Resources["subnetFirstUsable"].ToString();
            string subnetLastUsable = Application.Current.Resources["subnetLastUsable"].ToString();
            string subnetHostPerSubnet = Application.Current.Resources["subnetHostPerSubnet"].ToString();
            string subnetBroadcast = Application.Current.Resources["subnetBroadcast"].ToString();
            string subnetTotalHost = Application.Current.Resources["subnetTotalHost"].ToString();
            string subnetMaskError = Application.Current.Resources["subnetMaskError"].ToString();
            string ipError = Application.Current.Resources["ipError"].ToString();
            string supernetHeader = Application.Current.Resources["supernetHeader"].ToString();
            string supernetAddress = Application.Current.Resources["supernetAddress"].ToString();
            string supernetMask = Application.Current.Resources["supernetMask"].ToString();
            string supernetWildcard = Application.Current.Resources["supernetWildcard"].ToString();
            string supernetFirstUsable = Application.Current.Resources["supernetFirstUsable"].ToString();
            string supernetLastUsable = Application.Current.Resources["supernetLastUsable"].ToString();
            string supernetBroadcast = Application.Current.Resources["supernetBroadcast"].ToString();
            string supernetTotalHost = Application.Current.Resources["supernetTotalHost"].ToString();
            string subnetHostPerSubnetString = string.Empty;
            Results results = new Results();
            results.Show();

            byte newCidr = (byte)MaskToCidr(newSubnetMask);
            byte oldCidr = (byte)MaskToCidr(oldSubnetMask);
            IPAddress oldNetworkAddress;
            if (!IPAddress.TryParse(oldNetworkAddressString, out oldNetworkAddress))
            {
                results.Close();
                MessageBox.Show(ipError + " " + oldNetworkAddressString);
                return;
            }

            IPNetwork ipnetwork = IPNetwork.Parse(oldNetworkAddressString + "/" + oldCidr);

            if (newCidr > oldCidr)
            {
                results.textResults.Text = subnetHeader + Environment.NewLine;
                IPNetworkCollection subneted = ipnetwork.Subnet(newCidr);

                results.textResults.Text += ipnetwork + " " + subnetMessage1 + " " + subneted.Count + " " + subnetMessage2 + Environment.NewLine;
                results.textResults.Text += subnetMessage3 + " " + Environment.NewLine;

                foreach (IPNetwork ipNetwork in subneted)
                {
                    results.textResults.Text += subnetAddress + " " + ipNetwork.Network + Environment.NewLine;
                    results.textResults.Text += subnetMask + " " + ipnetwork.Netmask + Environment.NewLine;
                    results.textResults.Text += subnetWildcard + " " + ipNetwork.WildcardMask + Environment.NewLine;
                    results.textResults.Text += subnetFirstUsable + " " + ipNetwork.FirstUsable + Environment.NewLine;
                    results.textResults.Text += subnetLastUsable + " " + ipNetwork.LastUsable + Environment.NewLine;
                    results.textResults.Text += subnetBroadcast + " " + ipNetwork.Broadcast + Environment.NewLine;
                    results.textResults.Text += Environment.NewLine;
                    subnetHostPerSubnetString = subnetHostPerSubnet + " " + ipNetwork.Usable;
                }
                results.textResults.Text += subnetHostPerSubnetString + Environment.NewLine;
                results.textResults.Text += subnetTotalHost + " " + ipnetwork.Total + Environment.NewLine;
            }
            else if (newCidr < oldCidr)
            {
                results.textResults.Text = supernetHeader + Environment.NewLine;
                IPNetwork oldNetwork = IPNetwork.Parse(oldNetworkAddressString, oldCidr);
                IPNetwork newNetwork = IPNetwork.Parse(oldNetworkAddressString, newCidr);
                IPNetwork supernet = IPNetwork.Supernet(oldNetwork, newNetwork);

                results.textResults.Text += supernetAddress + " " + supernet.Network + Environment.NewLine;
                results.textResults.Text += supernetMask + " " + ipnetwork.Netmask + Environment.NewLine;
                results.textResults.Text += supernetWildcard + " " + supernet.WildcardMask + Environment.NewLine;
                results.textResults.Text += supernetFirstUsable + " " + supernet.FirstUsable + Environment.NewLine;
                results.textResults.Text += supernetLastUsable + " " + supernet.LastUsable + Environment.NewLine;
                results.textResults.Text += supernetBroadcast + " " + supernet.Broadcast + Environment.NewLine;
                results.textResults.Text += Environment.NewLine;
                results.textResults.Text += supernetTotalHost + " " + supernet.Total + Environment.NewLine;
            }
            else
            {
                results.Close();
                MessageBox.Show(subnetMaskError + " " + oldCidr + " / " + newCidr);
            }

        }
        #endregion

        #region NetworkCalc

        private void StartNetworkCalc(object sender, RoutedEventArgs e)
        {
            string networkAddressString = ipAddrTxtBx4.Text;
            string subnetMaskString = ntwrkMaskTxtBx2.Text;
            string netCalcAddress = Application.Current.Resources["netCalcAddress"].ToString();
            string netCalcMask = Application.Current.Resources["netCalcMask"].ToString();
            string netCalcWildcard = Application.Current.Resources["netCalcWildcard"].ToString();
            string netCalcFirstUsable = Application.Current.Resources["netCalcFirstUsable"].ToString();
            string netCalcLastUsable = Application.Current.Resources["netCalcLastUsable"].ToString();
            string netCalcBroadcast = Application.Current.Resources["netCalcBroadcast"].ToString();
            string netCalcTotalHost = Application.Current.Resources["netCalcTotalHost"].ToString();
            string ipError = Application.Current.Resources["ipError"].ToString();
            Results results = new Results();
            results.Show();

            byte Cidr = (byte)MaskToCidr(subnetMaskString);
            IPAddress networkAddress;
            if (!IPAddress.TryParse(networkAddressString, out networkAddress))
            {
                results.Close();
                MessageBox.Show(ipError + " " + networkAddressString);
                return;
            }

            IPNetwork ipnetwork = IPNetwork.Parse(networkAddressString + "/" + Cidr);
            results.textResults.Text += netCalcAddress + " " + ipnetwork.Network + Environment.NewLine;
            results.textResults.Text += netCalcMask + " " + ipnetwork.Netmask + Environment.NewLine;
            results.textResults.Text += netCalcWildcard + " " + ipnetwork.WildcardMask + Environment.NewLine;
            results.textResults.Text += netCalcFirstUsable + " " + ipnetwork.FirstUsable + Environment.NewLine;
            results.textResults.Text += netCalcLastUsable + " " + ipnetwork.LastUsable + Environment.NewLine;
            results.textResults.Text += netCalcBroadcast + " " + ipnetwork.Broadcast + Environment.NewLine;
            results.textResults.Text += Environment.NewLine;
            BigInteger totalHostUsable = ipnetwork.Total - 2;
            results.textResults.Text += netCalcTotalHost + " " + totalHostUsable + Environment.NewLine;
        }
        #endregion

        #region Settings

        private void StartLanguageWizard(object sender, RoutedEventArgs e)
        {
            Language language = new Language();
            language.Show();
        }

        private void StartThemeAndColorWizard(object sender, RoutedEventArgs e)
        {
            ThemeAndColor themeAndColor = new ThemeAndColor();
            themeAndColor.Show();
        }
        #endregion

        public static int MaskToCidr(string mask)
        {
            string maskError = Application.Current.Resources["maskError"].ToString();
            if (int.TryParse(mask, out int cidr))
            {
                if (cidr >= 0 && cidr <= 32)
                {
                    return cidr;
                }
            }

            if (!IPAddress.TryParse(mask, out IPAddress ipAddress))
            {
                throw new ArgumentException(maskError + " " + mask);
            }

            byte[] bytes = ipAddress.GetAddressBytes();
            int calculatedCidr = 0;

            foreach (byte b in bytes)
            {
                byte currentByte = b;
                while (currentByte > 0)
                {
                    calculatedCidr++;
                    currentByte = (byte)(currentByte << 1);
                }
            }

            return calculatedCidr;
        }

        #region ThemeAndColor

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
            NavIcon.Foreground = new SolidColorBrush(textColor);
            SettingsIcon.Foreground = new SolidColorBrush(textColor);
            WindowTitle.Foreground = new SolidColorBrush(textColor);

            netConfigBtn.Foreground = new SolidColorBrush(textColor);
            changeConfigBtn.Foreground = new SolidColorBrush(textColor);
            netCommandsBtn.Foreground = new SolidColorBrush(textColor);
            netCalcBtn.Foreground = new SolidColorBrush(textColor);

            ipAddressTxtBx1.Foreground = new SolidColorBrush(textColor);
            subnetMaskTxtBx1.Foreground = new SolidColorBrush(textColor);
            gatewayTxtBx1.Foreground = new SolidColorBrush(textColor);
            firstDnsTxtBx1.Foreground = new SolidColorBrush(textColor);
            secondDnsTxtBx1.Foreground = new SolidColorBrush(textColor);
            shSettings.Foreground = new SolidColorBrush(textColor);
            exprtSettings.Foreground = new SolidColorBrush(textColor);

            ipAddressTxtBx2.Foreground = new SolidColorBrush(textColor);
            subnetMaskTxtBx2.Foreground = new SolidColorBrush(textColor);
            gatewayTxtBx2.Foreground = new SolidColorBrush(textColor);
            firstDnsTxtBx2.Foreground = new SolidColorBrush(textColor);
            secondDnsTxtBx2.Foreground = new SolidColorBrush(textColor);
            setStaticSettings.Foreground = new SolidColorBrush(textColor);
            setDynamicSettings.Foreground = new SolidColorBrush(textColor);
            exportSettings.Foreground = new SolidColorBrush(textColor);
            importSettings.Foreground = new SolidColorBrush(textColor);

            pingTxtBx.Foreground = new SolidColorBrush(textColor);
            ipAddrTxtBx1.Foreground = new SolidColorBrush(textColor);
            timeoutTxtBx.Foreground = new SolidColorBrush(textColor);
            repetitionTxtBx.Foreground = new SolidColorBrush(textColor);
            pingBtn.Foreground = new SolidColorBrush(textColor);
            scanningTxtBx.Foreground = new SolidColorBrush(textColor);
            firstHostTxtBx.Foreground = new SolidColorBrush(textColor);
            lastHostTxtBx.Foreground = new SolidColorBrush(textColor);
            scanNetwrkBtn.Foreground = new SolidColorBrush(textColor);
            mtuCheckTxtBx.Foreground = new SolidColorBrush(textColor);
            ipAddrTxtBx2.Foreground = new SolidColorBrush(textColor);
            firstValueTxtBx.Foreground = new SolidColorBrush(textColor);
            lastValueTxtBx.Foreground = new SolidColorBrush(textColor);
            chkMTUBtn.Foreground = new SolidColorBrush(textColor);
            nxtMenuBtn.Foreground = new SolidColorBrush(textColor);

            arpCacheTxtBx.Foreground = new SolidColorBrush(textColor);
            showARPCacheBtn.Foreground = new SolidColorBrush(textColor);
            tracerouteTxtBx.Foreground = new SolidColorBrush(textColor);
            ipAddrTxtBx3.Foreground = new SolidColorBrush(textColor);
            tracerouteBtn.Foreground = new SolidColorBrush(textColor);
            routingTableTxtBx.Foreground = new SolidColorBrush(textColor);
            showRoutingTableBtn.Foreground = new SolidColorBrush(textColor);
            prvMenuBtn.Foreground = new SolidColorBrush(textColor);

            networkMaskCalcTxtBx.Foreground = new SolidColorBrush(textColor);
            ntwrkMaskTxtBx1.Foreground = new SolidColorBrush(textColor);
            ntwrkMaskCalcBtn.Foreground = new SolidColorBrush(textColor);
            nettingTxtBx.Foreground = new SolidColorBrush(textColor);
            ntwrkTxtBx.Foreground = new SolidColorBrush(textColor);
            oldNtwrkMaskTxtBx.Foreground = new SolidColorBrush(textColor);
            newNtwrkMaskTxtBx.Foreground = new SolidColorBrush(textColor);
            nettingCalcBtn.Foreground = new SolidColorBrush(textColor);
            ntwrkCalcBtn.Foreground = new SolidColorBrush(textColor);
            ntwrkCalcTxtBx.Foreground = new SolidColorBrush(textColor);
            ipAddrTxtBx4.Foreground = new SolidColorBrush(textColor);
            ntwrkMaskTxtBx2.Foreground = new SolidColorBrush(textColor);

            themeColorTxtBx.Foreground = new SolidColorBrush(textColor);
            themeColorWizBtn.Foreground = new SolidColorBrush(textColor);
            languageTxtBx.Foreground = new SolidColorBrush(textColor);
            languageWizBtn.Foreground = new SolidColorBrush(textColor);
            showTutorialBtn.Foreground = new SolidColorBrush(textColor);
            openProjectWebsiteBtn.Foreground = new SolidColorBrush(textColor);
            paletteHelper.SetTheme(theme);
        }

        public void UpdateBorderColor(Color color)
        {
            WindowBorder.Background = new SolidColorBrush(color);
        }

        public void UpdateNavIcon(Color color)
        {
            NavIcon.Foreground = new SolidColorBrush(color);
            SettingsIcon.Foreground = new SolidColorBrush(color);
        }

        public void UpdateTitleTextColor(Color color)
        {
            WindowTitle.Foreground = new SolidColorBrush(color);
        }

        public void UpdateTextBoxButtonTextColor(Color color)
        {
            netConfigBtn.Foreground = new SolidColorBrush(color);
            changeConfigBtn.Foreground = new SolidColorBrush(color);
            netCommandsBtn.Foreground = new SolidColorBrush(color);
            netCalcBtn.Foreground = new SolidColorBrush(color);

            ipAddressTxtBx1.Foreground = new SolidColorBrush(color);
            subnetMaskTxtBx1.Foreground = new SolidColorBrush(color);
            gatewayTxtBx1.Foreground = new SolidColorBrush(color);
            firstDnsTxtBx1.Foreground = new SolidColorBrush(color);
            secondDnsTxtBx1.Foreground = new SolidColorBrush(color);
            shSettings.Foreground = new SolidColorBrush(color);
            exprtSettings.Foreground = new SolidColorBrush(color);

            ipAddressTxtBx2.Foreground = new SolidColorBrush(color);
            subnetMaskTxtBx2.Foreground = new SolidColorBrush(color);
            gatewayTxtBx2.Foreground = new SolidColorBrush(color);
            firstDnsTxtBx2.Foreground = new SolidColorBrush(color);
            secondDnsTxtBx2.Foreground = new SolidColorBrush(color);
            setStaticSettings.Foreground = new SolidColorBrush(color);
            setDynamicSettings.Foreground = new SolidColorBrush(color);
            exportSettings.Foreground = new SolidColorBrush(color);
            importSettings.Foreground = new SolidColorBrush(color);

            pingTxtBx.Foreground = new SolidColorBrush(color);
            ipAddrTxtBx1.Foreground = new SolidColorBrush(color);
            timeoutTxtBx.Foreground = new SolidColorBrush(color);
            repetitionTxtBx.Foreground = new SolidColorBrush(color);
            pingBtn.Foreground = new SolidColorBrush(color);
            scanningTxtBx.Foreground = new SolidColorBrush(color);
            firstHostTxtBx.Foreground = new SolidColorBrush(color);
            lastHostTxtBx.Foreground = new SolidColorBrush(color);
            scanNetwrkBtn.Foreground = new SolidColorBrush(color);
            mtuCheckTxtBx.Foreground = new SolidColorBrush(color);
            ipAddrTxtBx2.Foreground = new SolidColorBrush(color);
            firstValueTxtBx.Foreground = new SolidColorBrush(color);
            lastValueTxtBx.Foreground = new SolidColorBrush(color);
            chkMTUBtn.Foreground = new SolidColorBrush(color);
            nxtMenuBtn.Foreground = new SolidColorBrush(color);

            arpCacheTxtBx.Foreground = new SolidColorBrush(color);
            showARPCacheBtn.Foreground = new SolidColorBrush(color);
            tracerouteTxtBx.Foreground = new SolidColorBrush(color);
            ipAddrTxtBx3.Foreground = new SolidColorBrush(color);
            tracerouteBtn.Foreground = new SolidColorBrush(color);
            routingTableTxtBx.Foreground = new SolidColorBrush(color);
            showRoutingTableBtn.Foreground = new SolidColorBrush(color);
            prvMenuBtn.Foreground = new SolidColorBrush(color);

            networkMaskCalcTxtBx.Foreground = new SolidColorBrush(color);
            ntwrkMaskTxtBx1.Foreground = new SolidColorBrush(color);
            ntwrkMaskCalcBtn.Foreground = new SolidColorBrush(color);
            nettingTxtBx.Foreground = new SolidColorBrush(color);
            ntwrkTxtBx.Foreground = new SolidColorBrush(color);
            oldNtwrkMaskTxtBx.Foreground = new SolidColorBrush(color);
            newNtwrkMaskTxtBx.Foreground = new SolidColorBrush(color);
            nettingCalcBtn.Foreground = new SolidColorBrush(color);
            ntwrkCalcBtn.Foreground = new SolidColorBrush(color);
            ntwrkCalcTxtBx.Foreground = new SolidColorBrush(color);
            ipAddrTxtBx4.Foreground = new SolidColorBrush(color);
            ntwrkMaskTxtBx2.Foreground = new SolidColorBrush(color);

            themeColorTxtBx.Foreground = new SolidColorBrush(color);
            themeColorWizBtn.Foreground = new SolidColorBrush(color);
            languageTxtBx.Foreground = new SolidColorBrush(color);
            languageWizBtn.Foreground = new SolidColorBrush(color);
            showTutorialBtn.Foreground = new SolidColorBrush(color);
            openProjectWebsiteBtn.Foreground = new SolidColorBrush(color);
        }

        #endregion

        private void NextPage(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 3;
        }

        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 2;
        }

        #region TrayIcon

        private void ShowAppFromTrayMenu(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
            Topmost = true;
            Topmost = false;
            ShowInTaskbar = true;
        }

        private void LoadedWindow(object sender, RoutedEventArgs e)
        {
            trayIcon = new TrayIcon();
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            trayIcon?.Dispose();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
            }

        }
        #endregion

        private void OpenProjectWebsite(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/L-Dlbcq/NetManager";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}

