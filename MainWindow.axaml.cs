using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using static Desktop_FINAL2.DatabaseInitializer;

namespace Desktop_FINAL2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
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

            private void LoadTitlesOC()
            {
                var list = LoadTitles(ConnectionString);
                Titles = new ObservableCollection<Anime>(list);
            }
        }

        public async void DBInit(object? sender, RoutedEventArgs e)
        {
            if (Tests.IsDatabaseReady(ConnectionString) == false)
            {
                int a = 4;
                for (int i = 0; i < a; i++)
                {
                    string LoadingText = "Initializing";
                    string dots = new string('.', (i % 4) + 1);
                    ButtonText = LoadingText + dots;
                    await Task.Delay(400);
                }
                DatabaseInitializer dbinit = new DatabaseInitializer();
                dbinit.InitTables(ConnectionString);
                dbinit.TestData(ConnectionString);
                ButtonText = "Database have been initialized";
                await Task.Delay(3000);
                ButtonText = "Initialize Database";
            }
            else
            {
                ButtonText = "Database is already initialized";
                await Task.Delay(3000);
                ButtonText = "Initialize Database";
            }
        }
    }
}