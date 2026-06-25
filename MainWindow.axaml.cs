using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

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

        private bool _isInitialized = false;

        public string buttonText = "Initialize Database";
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            }
        }

        public string searchBar = "Search by title...";
        public string SearchBar
        {
            get { return searchBar; }
            set
            {
                searchBar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchBar)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void DBInit(object? sender, RoutedEventArgs e)
        {
            if (_isInitialized == false)
            {
                ButtonText = "Initializing...";
                DatabaseInitializer dbinit = new DatabaseInitializer();
                dbinit.InitTables(ConnectionString);
                dbinit.TestData(ConnectionString);
                ButtonText = "Database have been initialized";
                _isInitialized = true;
            }
            else
            {
                ButtonText = "Database is arleady initialized";
            }
        }
    }
}