using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private SamokatEntities db = new SamokatEntities();
        private int userId;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на пустые поля
                if (string.IsNullOrWhiteSpace(TextBox1.Text) || string.IsNullOrWhiteSpace(PasswordBox1.Password))
                {
                    MessageBox.Show("Пожалуйста, заполните поля логина и пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Поиск пользователя в бд
                var users = db.Users.Where(u => u.login == TextBox1.Text).ToArray();

                if (users.Length == 0)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var currentUser = users[0];
                userId = currentUser.ID;

                // Сравнение пароля
                if (PasswordBox1.Password == currentUser.password)
                {
                    if (currentUser.active == true)
                    {
                        string message = $"Добро пожаловать, {currentUser.surname}. Вы вошли как ";

                        switch (currentUser.role2)
                        {
                            case 1:
                                MessageBox.Show(message + "пользователь.", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                                new Window2().Show();
                                this.Close();
                                break;

                            case 2:
                                MessageBox.Show(message + "администратор.", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                                new Window1().Show();
                                this.Close();
                                break;
                            default:
                                MessageBox.Show("Не удалось определить роль пользователя.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }

                        currentUser.count = 0;
                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Доступ заблокирован. Свяжитесь с администратором.", "Блокировка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    currentUser.count++;

                    if (currentUser.count > 2)
                    {
                        currentUser.active = false;
                        db.SaveChanges();
                        MessageBox.Show("Вы были заблокированы после трёх неудачных попыток.", "Блокировка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        db.SaveChanges();
                        MessageBox.Show("Неверный логин или пароль. Попробуйте снова.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла системная ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}