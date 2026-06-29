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

        private string userInput = "";
        public string UserInput
        {
            get { return userInput; }
            set
            {
                userInput = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(UserInput)));
            }
        }

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

        public ObservableCollection<Anime> _titles;

        public ObservableCollection<Anime> Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Titles)));
            }
        }

        private ObservableCollection<Studio> _studios;
        public ObservableCollection<Studio> Studios
        {
            get { return _studios; }
            set 
            { 
                _studios = value; 
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Studios))); 
            }
        }

        public ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Genres)));
            }
        }

        private object? currentItems;
        public object? CurrentItems
        {
            get { return currentItems; }
            set 
            { 
                currentItems = value; 
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentItems))); 
            }
        }

        public void LoadData()
        {
            Titles = new ObservableCollection<Anime>(Logic.LoadTitles(ConnectionString));
            Studios = new ObservableCollection<Studio>(Logic.LoadStudios(ConnectionString));
            Genres = new ObservableCollection<Genre>(Logic.LoadGenres(ConnectionString));
            CurrentItems = Titles;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
                LoadData();
            }
            else
            {
                ButtonText = "Database is already initialized";
                await Task.Delay(3000);
                ButtonText = "Initialize Database";
                LoadData();
            }
        }

        private void Next(object? sender, RoutedEventArgs e)
        {
            if (CurrentItems == Titles)
            {
                SearchBar = "Search in studios...";
                CurrentItems = Studios;
            }
            else if (CurrentItems == Studios)
            {
                SearchBar = "Search in genres...";
                CurrentItems = Genres;
            }
            else if (CurrentItems == Genres) 
            {
                SearchBar = "Search by title...";
                CurrentItems = Titles;
            }
                
        }

        private void Prev(object? sender, RoutedEventArgs e)
        {
            if (CurrentItems == Titles)
            {
                SearchBar = "Search in genres...";
                CurrentItems = Genres;
            }
            else if (CurrentItems == Genres)
            {
                SearchBar = "Search in studios...";
                CurrentItems = Studios;
            }
            else if (CurrentItems == Studios)
            {
                SearchBar = "Search by title...";
                CurrentItems = Titles;
            }
        }



        public void Search(object? sender, RoutedEventArgs e)
        {

        }
    }
}