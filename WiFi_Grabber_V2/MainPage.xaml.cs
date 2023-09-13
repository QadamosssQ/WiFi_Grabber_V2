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

        public MainPage()
        {
            InitializeComponent();

            fruits.Add(new Fruit() { FruitName = "Apple" });
            fruits.Add(new Fruit() { FruitName = "Orange" });
            fruits.Add(new Fruit() { FruitName = "Banana" });
            fruits.Add(new Fruit() { FruitName = "Grape" });
            fruits.Add(new Fruit() { FruitName = "Mango" });

            FruitListView.ItemsSource = fruits;

            // Dodanie obsługi zdarzenia ItemTapped
            FruitListView.ItemTapped += OnFruitListViewItemTapped;
        }

        private async void OnFruitListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Fruit selectedFruit)
            {
                await DisplayAlert("Selected Fruit", selectedFruit.FruitName, "OK");
            }

            // Oznaczenie elementu jako niezaznaczonego, aby umożliwić ponowne kliknięcie
            FruitListView.SelectedItem = null;
        }
    }
}