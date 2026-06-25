using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
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

        static public int a;

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

        public string searchBar = "Search by title...";
        public string SearchBar
        {
            get { return searchBar; }
            set
            {
                searchBar = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SearchBar)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void DBInit(object? sender, RoutedEventArgs e)
        {
            if (a == 0)
            {
                ButtonText = "Initializing...";
                DatabaseInitializer dbinit = new DatabaseInitializer();
                dbinit.InitTables(ConnectionString);
                dbinit.TestData(ConnectionString);
                ButtonText = "Ready!";
                a++;
                ButtonText = buttonText;
            }
            else
            {
                ButtonText = "Database is arleady initialized";
                ButtonText = buttonText;
            }
        }
    }
}