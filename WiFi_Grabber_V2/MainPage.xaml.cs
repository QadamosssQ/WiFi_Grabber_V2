using System;
using System.Text;
using NativeWifi;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace WiFi_Grabber_V2
{
    public partial class MainPage : ContentPage
    {
        public class Network
        {
            public string NetworkName { get; set; }
        }

        ObservableCollection<Network> networks = new ObservableCollection<Network>();

        public ObservableCollection<Network> Networks
        {
            get { return networks; }
        }

        static string GetStringForSSID(Wlan.Dot11Ssid ssid) =>
            Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);

        public MainPage()
        {
            InitializeComponent();

            var client = new WlanClient();
            foreach (var wlanIface in client.Interfaces)
            {
                var availableNetworks = wlanIface.GetAvailableNetworkList(0);

                foreach (var profileInfo in wlanIface.GetProfiles())
                {
                    string name = profileInfo.profileName;
                    string xml = wlanIface.GetProfileXml(profileInfo.profileName);
                    networks.Add(new Network() { NetworkName = name });
                }
            }

            NetworkListView.ItemsSource = networks;

            NetworkListView.ItemTapped += OnNetworkListViewItemTapped;
        }

        private async void OnNetworkListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Network selectedNetwork)
            {
                PasswordLabel.Text = "Grabbed password: " + Show_wifi_password(selectedNetwork.NetworkName);
            }

            NetworkListView.SelectedItem = null;
        }

        static string Show_wifi_password(string wifiName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = "wlan show profile name=\"" + wifiName + "\" key=clear",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int a = 0;
            string[] sieci = new string[lines.Length];

            foreach (string line in lines)
            {
                if (line.Contains("    Key Content            : "))
                {
                    a++;
                    string linia_1 = line.Replace("    Key Content            : ", "");
                    sieci[0] = linia_1;
                }
            }

            return sieci[0];
        }
    }
}