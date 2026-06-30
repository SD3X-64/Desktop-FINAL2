using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private int _currentTableIndex = 0;
        private object[] _tableArray = new object[3];

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

        public string tableText = "";
        public string TableText
        {
            get { return tableText; }
            set
            {
                tableText = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TableText)));
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

        public void LoadData()
        {
            Titles = new ObservableCollection<Anime>(Logic.LoadTitles(ConnectionString));
            Studios = new ObservableCollection<Studio>(Logic.LoadStudios(ConnectionString));
            Genres = new ObservableCollection<Genre>(Logic.LoadGenres(ConnectionString));

            _tableArray[0] = Titles;
            _tableArray[1] = Studios;
            _tableArray[2] = Genres;
            _currentTableIndex = 0;
            TableText = FormatCollection(Titles);
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
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
            if (_currentTableIndex == 0)
            {
                SearchBar = "Search in studios...";
                TableText = FormatCollection(Studios);
                _currentTableIndex = 1;
            }
            else if (_currentTableIndex == 1)
            {
                SearchBar = "Search in genres...";
                TableText = FormatCollection(Genres);
                _currentTableIndex = 2;
            }
            else if (_currentTableIndex == 2)
            {
                SearchBar = "Search by title...";
                TableText = FormatCollection(Titles);
                _currentTableIndex = 0;
            }

        }

        private void Prev(object? sender, RoutedEventArgs e)
        {
            if (_currentTableIndex == 0)
            {
                SearchBar = "Search in genres...";
                TableText = FormatCollection(Genres);
                _currentTableIndex = 2;
            }
            else if (_currentTableIndex == 2)
            {
                SearchBar = "Search in studios...";
                TableText = FormatCollection(Studios);
                _currentTableIndex = 1;
            }
            else if (_currentTableIndex == 1)
            {
                SearchBar = "Search by title...";
                TableText = FormatCollection(Titles);
                _currentTableIndex = 0;
            }
        }

        public string FormatCollection(object collection)
        {
            if (collection == null) return "Нет данных.";

            var sb = new System.Text.StringBuilder();
            int index = 1;

            if (collection is ObservableCollection<Anime> titleList)
            {
                foreach (var a in titleList)
                    sb.AppendLine($"{index++}. {a.Title} — {a.StudioName}: \n({a.GenreName}, {a.YearStarted?.Year ?? 0}, {a.Episodes ?? 0} ep.)    \nStatus: {a.Status},    Watch Status: {a.WatchStatus},  Last Watched Ep: {a.LastWatched ?? 0}\n");
            }
            else if (collection is ObservableCollection<Studio> studioList)
            {
                foreach (var s in studioList)
                    sb.AppendLine($"{index++}. {s.Name}\n");
            }
            else if (collection is ObservableCollection<Genre> genreList)
            {
                foreach (var g in genreList)
                    sb.AppendLine($"{index++}. {g.Name}\n");
            }
            else
            {
                return "Неизвестный тип данных.";
            }

            return sb.ToString();
        }

        public void Search(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserInput))
            {
                switch (_currentTableIndex)
                {
                    case 0: TableText = FormatCollection(Titles); break;
                    case 1: TableText = FormatCollection(Studios); break;
                    case 2: TableText = FormatCollection(Genres); break;
                }
                return;
            }

            if (_currentTableIndex == 0)
            {
                var filtered = Titles
                    .Where(a => a.Title.Contains(UserInput, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                TableText = FormatCollection(new ObservableCollection<Anime>(filtered));
            }
            else if (_currentTableIndex == 1)
            {
                var filtered = Studios
                    .Where(s => s.Name.Contains(UserInput, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                TableText = FormatCollection(new ObservableCollection<Studio>(filtered));
            }
            else if (_currentTableIndex == 2)
            {
                var filtered = Genres
                    .Where(g => g.Name.Contains(UserInput, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                TableText = FormatCollection(new ObservableCollection<Genre>(filtered));
            }
        }

        public void Add(object? sender, RoutedEventArgs e)
        {

        }

        public void Delete(object? sender, RoutedEventArgs e)
        {

        }

        public void Redact(object? sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
        }
    }
}