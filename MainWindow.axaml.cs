using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Tmds.DBus.Protocol;

namespace Desktop_FINAL2
{
    public partial class MainWindow : Window
    {

        static public string ConnectionString =
            @"Server=(localdb)\mssqllocaldb;
              Database=UniversityDB;
              Integrated Security=True;
              TrustServerCertificate=True;";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void DBInit(object? sender, RoutedEventArgs e)
        {
            DatabaseInitializer dbinit = new DatabaseInitializer();
            dbinit.InitTables(ConnectionString);
        }
    }
}