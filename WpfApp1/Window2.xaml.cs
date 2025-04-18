using System.Linq;
using System.Windows;
using System.Data.Entity;
using System;

namespace WpfApp1
{
    public partial class Window2 : Window
    {
        private readonly SamokatEntities db = new SamokatEntities();

        public Window2()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            db.Somokati.Load();
            UserGrid.ItemsSource = db.Somokati.Local.ToBindingList();
        }
    }
}
