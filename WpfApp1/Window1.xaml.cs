using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public partial class Window1 : Window
    {
        private readonly SamokatEntities db = new SamokatEntities();

        public Window1()
        {
            InitializeComponent();
            ComboBoxRole.ItemsSource = db.Role.ToList();
            ComboBoxRole.DisplayMemberPath = "role1";
            ComboBoxRole.SelectedValuePath = "ID";

            db.Users.Load();
            UserGrid.ItemsSource = db.Users.Local.ToBindingList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (new[] { TextBoxLogin.Text, TextBoxPassword.Password, TextBoxSurname.Text, TextBoxName.Text, TextBoxPatronymic.Text }.Any(string.IsNullOrWhiteSpace))
                return;

            if (db.Users.Any(u => u.login == TextBoxLogin.Text))
                return;

            db.Users.Add(new Users
            {
                login = TextBoxLogin.Text,
                password = TextBoxPassword.Password,
                role2 = (int?)ComboBoxRole.SelectedValue ?? 0,
                surname = TextBoxSurname.Text,
                name = TextBoxName.Text,
                otchestvo = TextBoxPatronymic.Text,
                count = 0,
                active = true,
            });

            db.SaveChanges();
            UserGrid.ItemsSource = db.Users.ToList();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem is Users user)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                UserGrid.ItemsSource = db.Users.ToList();
            }
        }

        private void ButtonRedScoot(object sender, RoutedEventArgs e)
        {
            new AdminScooterWindow().Show();
            this.Close();
        }
        
    }
}
