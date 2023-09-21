using System;
using System.Text;
using NativeWifi;

using System.Collections.ObjectModel;


namespace WiFi_Grabber_V2
{
    public partial class MainPage : ContentPage
    {
        public class Fruit
        {
            public string FruitName { get; set; }
        }

        ObservableCollection<Fruit> fruits = new ObservableCollection<Fruit>();
        public ObservableCollection<Fruit> Fruits { get { return fruits; } }
        
        static string GetStringForSSID(Wlan.Dot11Ssid ssid) =>
            Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);

        public MainPage()
        {
            InitializeComponent();
            
            var client = new WlanClient();
            foreach (var wlanIface in client.Interfaces)
            {
                var networks = wlanIface.GetAvailableNetworkList(0);
                
                foreach (var profileInfo in wlanIface.GetProfiles())
                {
                    string name = profileInfo.profileName;
                    string xml = wlanIface.GetProfileXml(profileInfo.profileName);
                    fruits.Add(new Fruit() { FruitName = name });
                }
            }

            NetworkListView.ItemsSource = fruits;

            // Dodanie obsługi zdarzenia ItemTapped
            NetworkListView.ItemTapped += OnFruitListViewItemTapped;
        }

        private async void OnFruitListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Fruit selectedFruit)
            {
                await DisplayAlert("Selected Fruit", selectedFruit.FruitName, "OK");
            }

            // Oznaczenie elementu jako niezaznaczonego, aby umożliwić ponowne kliknięcie
            NetworkListView.SelectedItem = null;
        }
    }
}