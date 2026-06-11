using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace Desktop_FINAL2
{
    public partial class MainWindow : Window
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        static public string ConnectionString =
            @"Server=(localdb)\mssqllocaldb;
              Database=UniversityDB;
              Integrated Security=True;
              TrustServerCertificate=True;";

        public string buttonText = "Initialize Database";
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void DBInit(object? sender, RoutedEventArgs e)
        {
            ButtonText = "Initializing Database";
            DatabaseInitializer dbinit = new DatabaseInitializer();
            ButtonText = "Initializing Database.";
            dbinit.InitTables(ConnectionString);
            ButtonText = "Initializing Database..";
            dbinit.TestData(ConnectionString);
            ButtonText = "Initializing Database...";
            ButtonText = "Done!";
            ButtonText = buttonText;
        }
    }
}