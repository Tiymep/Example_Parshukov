using System.Linq;
using System.Windows;
using System.Data.Entity;
using System;

namespace WpfApp1
{
    public partial class AdminScooterWindow : Window
    {
        private readonly SamokatEntities db = new SamokatEntities();

        public AdminScooterWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            db.Somokati.Load();
            ScooterGrid.ItemsSource = db.Somokati.Local.ToBindingList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newScooter = new Somokati
                {
                    model = ModelBox.Text,
                    opisanie = OpisanieBox.Text,
                    price = PriceBox.Text,
                    adress_mest = AdressBox.Text,
                    nalichie = int.Parse(NalichieBox.Text),
                    arenduet = int.Parse(ArenduetBox.Text)
                };

                db.Somokati.Add(newScooter);
                db.SaveChanges();
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScooterGrid.SelectedItem is Somokati scooter)
            {
                db.Somokati.Remove(scooter);
                db.SaveChanges();
                LoadData();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.SaveChanges();
                MessageBox.Show("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            ModelBox.Text = "";
            OpisanieBox.Text = "";
            PriceBox.Text = "";
            AdressBox.Text = "";
            NalichieBox.Text = "";
            ArenduetBox.Text = "";
        }
    }
}
